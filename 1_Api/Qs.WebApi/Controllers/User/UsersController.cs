using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Qs.Comm;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Request.ReqApi;
using Qs.App.Response;
using Qs.App.UserManager;
using Qs.App.UserManager.Request;
using Qs.App.Wx;
using Qs.Repository.Base;
using Qs.Repository.Request;
using Qs.Repository.Response;
using WebApi.Model.ModelRes;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 用户操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppUserManager _app;
        private readonly IAuth _auth;
        private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="app"></param>
        public UserController(AppUserManager app, IAuth auth, IHostingEnvironment hostingEnvironment)
        {
            _app = app;
            _auth = auth;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 加载列表
        /// 获取当前登录用户可访问的一个部门及子部门全部用户
        /// </summary>
        [HttpGet]
        public  TableData Load([FromQuery] QueryUserListReq request)
        {
            return  _app.Load(request);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<ResUser> Get(string id)
        {
            var result = new Response<ResUser>();
            result.Result = _app.GetDetail(id);
            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<ResUserBusiness> GetMyBusinessUserInfo()
        {
            var result = new Response<ResUserBusiness>();
            result.Result = _app.GetMyBusinessUserInfo();
            return result;
        }

        // /// <summary>
        // /// 选择工地
        // /// </summary>
        // /// <returns></returns>
        // [HttpPost]
        // public Response<string> ChoiceBuilding(ResChoiceBuilding req)
        // {
        //     var result = new Response<string>();
        //     result.Result = _app.ChoiceBuilding(req);
        //     return result;
        // }



        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response ChangeName(ReqChangeName req)
        {
            var result = new Response();
            try
            {
                _app.ChangeNickName(req);
                result.Message = "修改成功，重新登录生效";
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response ChangeProfile(ChangeProfileReq req)
        {
            var result = new Response();  
            try
            {
                _app.ChangeProfile(req);
                result.Message = "修改成功，重新登录生效";
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }  
            return result;
        }

        /// <summary>
        /// 转移客户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response ChangeChildBusinessUser(ChangeChildBusinessUserReq req)
        {
            var result = new Response();
            try
            {
                _app.ChangeChildBusinessUser(req);
                result.Message = "修改成功";
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 修改用户类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response ChangeUserType(ReqChangeUserType req)
        {
            var result = new Response();
            try
            {
                _app.ChangeUserType(req);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response ChangeUserStatus(ReqChangeUserStatus req)
        {
            var result = new Response();
            try
            {
                _app.ChangeUserStatus(req);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 审核用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response CheckUser(ReqCheckUser req)
        {
            var result = new Response();
            try
            {
                _app.CheckUser(req);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 修改密码(PC)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public Response ChangePassword(ReqChangePwd request)
        {
            var result = new Response();
            try
            {
                _app.ChangePassword(request);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            } 
            return result;
        }
        /// <summary>
        /// 管理端添加或修改员工
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response AddOrUpdateEmp(UpdateUserReq obj)
        {
            var result = new Response();
            _app.AddOrUpdateEmp(obj);
            return result;
        }

        /// <summary>
        /// 管理端添加或修改用户(业务、司机、调度)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response AddOrUpdateManagement(UpdateUserReq obj)
        {
            var result = new Response();
            _app.AddOrUpdateManagement(obj);
            return result;
        }

        // /// <summary>
        // /// 绑定手机号码
        // /// </summary>
        // [HttpPost]
        // public Response<string> BindWxPhone([FromBody] ReqBindPhone req)
        // {
        //     var result = new Response<string>();
        //     req.Trim();
        //     var userId = _auth.GetCurrentContext().User.Id;
        //     var user = _app.Get(userId);
        //     Code2Session.ResWxPhone  resPhone= Code2Session.GetWxPhone(req.WxCode, req.AppKey);
        //     string phone = resPhone.phone_info.purePhoneNumber; //无区号手机号
        //     user.Phone = phone;
        //     user.NickName = $"微信用户_{phone.Substring(7)}";
        //     _app.AddOrUpdate(user);
        //     result.Result = user.Id;
        //     return result;
        // }

        [HttpPost]
        public Response Delete([FromBody] string[] ids)
        {
            var result = new Response();
            try
            {
                _app.Delete(ids);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 生成邀请海报(带二维码)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<ResInviteLink> GenInvitePoster([FromBody] ReqGenInvitePoster req)
        {
            var res = new Response<ResInviteLink>(); 
            res.Result = _app.GenInvitePoster(req, _hostingEnvironment);
            return res;
        }

        /// <summary>
        /// 充值余额
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response RechargeBalance([FromBody] ReqRechargeBalance req)
        {
            var result = new Response();
            try
            {
                _app.RechargeBalance(req);
                result.Message = "充值成功";
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 设置微信二维码
        /// </summary>
        [HttpPost]
        public Response<bool> SetWxQrCode([FromBody] ReqSetWxQrCode req)
        {
            var result = new Response<bool>();
            _app.SetWxQrCode(req);
            result.Result = true;
            return result;
        }
        /// <summary>
        /// 设置支付密码
        /// </summary>
        [HttpPost]
        public Response<bool> SetRechargePwd([FromBody] ReqSetRechargePwd req)
        {
            var result = new Response<bool>();
            _app.SetRechargePwd(req);
            result.Result =true ;
            return result;
        }

        /// <summary>
        /// 修改头像昵称
        /// </summary>
        [HttpPost]
        public Response<bool> ChangeUserInfo([FromBody] ReqChangeUserInfo req)
        {
            var result = new Response<bool>();
            _app.ChangeUserInfo(req);
            result.Result = true;
            return result;
        }

        /// <summary>
        /// 设置手机号
        /// </summary>
        [HttpPost]
        public Response<bool> SetReplacePhone([FromBody] ReqReplacePhone req)
        {
            var result = new Response<bool>();
            try
            {
                
                _app.SetReplacePhone(req);
                result.Result = true;
                result.Message = "修改成功";
            }
            catch (Exception e)
            {
                result.Result = false;
                result.Message = e.Message;
                result.Code = 500;
            }
            return result;
        }

        
        /// <summary>
        /// 加载指定角色的用户
        /// </summary>
        [HttpGet]
        public async Task<TableData> LoadByRole([FromQuery] QueryUserListByRoleReq req)
        {
            return await _app.LoadByRole(req);
        }

    }
}