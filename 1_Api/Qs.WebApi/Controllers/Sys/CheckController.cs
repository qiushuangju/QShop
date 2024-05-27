// ***********************************************************************
// Assembly         : Qs.WebApi
// Author           : Qs
// Created          : 07-11-2016
//
// Last Modified By : Qs
// Last Modified On : 07-11-2016
// Contact :
// File: CheckController.cs
// 登录相关的操作
// ***********************************************************************

using Qs.Comm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Interface;
using Qs.App.Response;
using Qs.App.SSO;
using Qs.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Qs.App.AuthStrategies;
using Qs.App.Request.ReqApi;
using Qs.App.UserManager;
using Qs.App.Wx;
using Qs.Comm.Cache;
using Qs.Comm.Extensions;
using Qs.Comm.Helpers;
using Qs.Comm.VerifyCode;
using Qs.Repository.Base;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Vm;
using Qs.Repository.Wx;
using TencentCloud.Iot.V20180123.Models;

namespace Qs.WebApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// 登录及与登录信息获取相关的接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    // [ApiExplorerSettings(GroupName = "登录验证_Check")]
    public class CheckController : ControllerBase
    {
        private readonly IAuth _auth;
        private ILogger _logger;
        private AuthStrategyContext _authStrategyContext;
        private AppUserManager _appUser;
        private AppInviteLink _appInviteLink;
        private AppStoreSetting _appSetting;
        private ICacheContext _cacheContext;
        private AppSendSms _appSendSms;          
        public CheckController(IAuth authUtil, AppUserManager appUser,AppStoreSetting appSetting, AppInviteLink appInviteLink, AppSendSms appSendSms, ICacheContext cacheContext, ILogger<CheckController> logger)
        {
            _auth = authUtil;
            _logger = logger;
            _appUser = appUser;
            _appInviteLink = appInviteLink;
            _appSetting = appSetting;
            _authStrategyContext = _auth.GetCurrentContext();
            _cacheContext = cacheContext;
            _appSendSms = appSendSms;
        }
        
        /// <summary>
        /// 获取登录用户资料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<UserView> GetUserProfile()
        {
            var resp = new Response<UserView>();
            try
            {
                resp.Result = _authStrategyContext.User.MapTo<UserView>();
            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }

            return resp;
        }


        /// <summary>
        /// 获取登录用户的所有可访问的角色
        /// </summary>
        [HttpGet]
        public Response<List<Role>> GetRoles()
        {
            var result = new Response<List<Role>>();
            result.Result = _authStrategyContext.Roles;
            return result;
        }
       
        /// <summary>
        /// 获取登录用户的所有可访问的模块及菜单，以列表形式返回结果
        /// </summary>
        [HttpGet]
        public Response<List<ModuleView>> GetModules()
        {
            var result = new Response<List<ModuleView>>();
            result.Result = _authStrategyContext.Modules;
            return result;
        }

        /// <summary>
        /// 获取登录用户的所有可访问的模块及菜单，以树状结构返回
        /// </summary>
        [HttpGet]
        public Response<IEnumerable<TreeItem<ModuleView>>> GetModulesTree()
        {
            var result = new Response<IEnumerable<TreeItem<ModuleView>>>();

            result.Result = _authStrategyContext.Modules.GenerateTree(u => u.Id, u => u.ParentId);
            return result;
        }

        /// <summary>
        /// 根据token获取用户名称
        /// </summary>
        [HttpGet]
        public Response<string> GetUserName()
        {
            var result = new Response<string>();
            result.Result = _authStrategyContext.User.NickName;
            return result;
        }

        /// <summary>
        ///     微信小程序登录接口
        /// </summary>
        /// <param name="req">登录参数</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public Response<LoginResult>  LoginWx([FromBody] WxAppLoginRequest req)
        {
            Response<LoginResult> res = new Response<LoginResult>();
            res.Result= new LoginResult();
            req.Trim();
            VmSettingBasicWxApp wxConfig = _appSetting.GetDetail("basicWxApp");
            var resultWx = Code2Session.Get(req.WxCode, wxConfig);
            // Code2Session.ResultData resultWx = new Code2Session.ResultData()
            // {
            //     errcode = "0",
            //     openid = "o4sUj5OegqOc1DVXH5aFw8XY4NOM"
            // openid ="oacq15EjDFz-s8QQFWSvWhVfX7VQ" //曹冲 业务员openId
            // };

            if (xConv.ToInt(resultWx.errcode) == 0)
            {
                //新增修改用户
                ModelUser user = _appUser.AddOrUpdateWxUser(resultWx.openid,"", req.InviteCode);
                res.Result = _auth.LoginWx(Define.AppWxApp, resultWx.openid);
                res.Result.UserType = xConv.ToInt(user.UserType);
                res.Result.UserId = user.Id;
                res.Result.IsHavePayPwd = !string.IsNullOrEmpty(user.BalancePwd);
                res.Result.IsBindPhone = !string.IsNullOrEmpty(user.Phone);
               
            }
            return res;
        }

        /// <summary>
        /// 登录接口-短信验证码登录
        /// </summary>
        /// <param name="req">登录参数</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public Response<LoginResult> LoginByPhoneCode(ReqLoginByPhone req)
        {
            var res = new Response<LoginResult>();

            req.Check();
            //新增修改用户
            ModelUser user = _appUser.AddOrUpdateWxUser("", req.Mobile, req.InviteCode);
            res.Result = _auth.LoginWx(Define.AppWxApp, req.Mobile);
            ValidateCode(xEnum.CodeType.Sms, req.SmsVerifyCodeId, req.SmsCode);
            res.Result.UserType = xConv.ToInt(user.UserType);
            res.Result.UserId = user.Id;
            res.Result.IsHavePayPwd = !string.IsNullOrEmpty(user.BalancePwd);
            res.Result.IsBindPhone = !string.IsNullOrEmpty(user.Phone);
            return res;
        }

        /// <summary>
        /// 绑定手机号码
        /// </summary>
        [HttpPost]
        public Response<string> BindWxPhone([FromBody] ReqBindPhone req)
        {
            var result = new Response<string>();
            req.Trim();
            var userId = _auth.GetCurrentContext().User.Id;
            var user = _appUser.Get(userId);
            VmSettingBasicWxApp setting = _appSetting.GetDetail(DefineSetting.BasicWxApp);
            Code2Session.ResWxPhone resPhone = Code2Session.GetWxPhone(req.WxCode, req.AppKey, setting);
            string phone = resPhone.phone_info.purePhoneNumber; //无区号手机号
            user.Phone = phone;
            user.NickName = $"微信用户_{phone.Substring(7)}";
            _appUser.AddOrUpdate(user);
            result.Result = user.Id;
            return result;
        }



        /// <summary>
        /// 生成验证码 图片
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public Response<ResCaptcha> GetImageCaptcha()
        {
            Response<ResCaptcha> res = new Response<ResCaptcha>();
            ResCaptcha obj= GenVerCode(xEnum.VerifyCodeType.Arith);
            res.Result = obj;
            return res;
        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public Response<ResPhoneCode> SendSmsCaptcha(ReqSendSmsCaptcha req)
        {
            req.Check();
            Response<ResPhoneCode> res = new Response<ResPhoneCode>();
            ValidateCode(xEnum.CodeType.Image,req.VerifyCodeId, req.VerifyCode);
            ResPhoneCode objResult = _appSendSms.SendCode(req.Mobile);
            res.Result = objResult;
            return res;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="req">登录参数</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public Response<LoginResult> Login(PassportLoginRequest req)
        {
            var result = new Response<LoginResult>();
            try
            {
                req.Trim();
                ValidateCode(xEnum.CodeType.Sms,req.VerifyCodeId, req.VerifyCode);
                result.Result = _auth.Login(req.AppKey, req.Account, req.Password, req.PhoneCode);

                result.Code = result.Result.Code;
                result.Message = result.Result.Message;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        private ResCaptchaCode GenVerCode(xEnum.VerifyCodeType verifyCodeType)
        {
            ModelVerifyCode codeInfo = VerifyCodeHelper.Captcha(verifyCodeType);
            string verifyCodeId = xConv.NewGuid();
            _cacheContext.Set(verifyCodeId, codeInfo.Code, DateTime.Now.AddMinutes(5));
            ResCaptchaCode obj = new ResCaptchaCode()
            {
                Base64Str = codeInfo.Base64Str,
                VerifyCodeId = verifyCodeId ,
                Code = codeInfo.Code
            };
            return obj;
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="codeType">验证码类型</param>
        /// <param name="verifyCodeId">验证码Id</param>
        /// <param name="imageVerifyCode">验证码</param>
        private void ValidateCode(xEnum.CodeType codeType, string verifyCodeId, string imageVerifyCode)
        {
            var verifyCode = _cacheContext.Get<string>(verifyCodeId);
            _cacheContext.Remove(verifyCodeId);

            if (verifyCode == null)
            {
                if (codeType == xEnum.CodeType.Image)
                {
                    throw new Exception("验证码失效,请点击图片重新获取!");
                }

                else
                {
                    throw new Exception("验证码失效,请重新获取!");
                }
            }

            if (imageVerifyCode.ToLower() != verifyCode.ToLower())
            {
                throw new Exception("验证码错误!");
            }
        }

        /// <summary>
        /// 验证登录是否有效
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<bool> CheckToken()
        {
            var result = new Response<bool>();
            try
            {
                result.Result = _auth.CheckLogin();
            }
            catch (Exception ex)
            {
                result.Code = DefineErrCode.InvalidToken;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<bool> Logout()
        {
            var resp = new Response<bool>();
            try
            {
                resp.Result = _auth.Logout();
            }
            catch (Exception e)
            {
                resp.Result = false;
                resp.Message = e.Message;
            }   
            return resp;
        }
    }
}