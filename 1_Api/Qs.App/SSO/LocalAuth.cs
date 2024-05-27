using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Qs.App.AuthStrategies;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Comm.Cache;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.App.SSO
{
    /// <summary>
    ///     使用本地登录。这个注入IAuth时，只需要Qs.Mvc一个项目即可，无需webapi的支持
    /// </summary>
    public class LocalAuth : IAuth
    {
        private readonly AuthContextFactory _appFactory;
        private readonly ICacheContext _cacheContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginParse _loginParse;
        private readonly IUnitWork<QsDBContext> _unitWork;

        public LocalAuth(IHttpContextAccessor httpContextAccessor, AuthContextFactory app, LoginParse loginParse,
            IUnitWork<QsDBContext> unitWork, ICacheContext cacheContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _appFactory = app;
            _loginParse = loginParse;
            _unitWork = unitWork;
            _cacheContext = cacheContext;
        }

        /// <summary>
        ///     是否已登录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        public bool CheckLogin(string token = "", string otherInfo = "")
        {
            if (string.IsNullOrEmpty(token))
            {
                token = GetToken();
            }

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var result = GetUserByToken(token) != null;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        ///     获取当前登录的用户信息
        ///     <para>通过URL中的Token参数或Cookie中的Token</para>
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>LoginUserVM.</returns>
        public AuthStrategyContext GetCurrentContext()
        {
            var token = GetToken();
            var appKey = GetPlatform();

            AuthStrategyContext context = null;
            if (string.IsNullOrEmpty(token))     //为空随便给个值避免与库内token为""的数据
            {
                token = xConv.NewGuid();
            }
            ModelUser userDb = GetUserByToken(token);
            if (userDb != null)
            {

                var nameOrPhone = !string.IsNullOrEmpty(userDb.Account) ? userDb.Account : userDb.Phone;
                context = _appFactory.GetAuthStrategyContext(nameOrPhone, appKey);
            }

            return context;
        }

        /// <summary>
        ///     获取当前登录的用户名
        ///     <para>通过URL中的Token参数或Cookie中的Token</para>
        /// </summary>
        /// <param name="otherInfo">The account.</param>
        /// <returns>System.String.</returns>
        public string GetUserName(string otherInfo = "")
        {
            var user = GetUserByToken(GetToken());
            if (user != null)
            {
                return user.Phone;
            }

            return "";
        }

        /// <summary>
        ///     登录接口
        /// </summary>
        /// <param name="appKey">应用程序key.</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="phoneCode">验证码</param>
        public LoginResult Login(string appKey, string username, string pwd, string phoneCode = "")
        {
            var result = _loginParse.Do(new PassportLoginRequest
            {
                AppKey = appKey,
                Account = username,
                Password = pwd,
                PhoneCode = phoneCode

            });
            return result;
        }
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="appKey">应用程序key.</param>
        /// <param name="wxOpenId"></param>
        /// <returns>System.String.</returns>
        public LoginResult LoginWx(string appKey, string wxOpenId)
        {
            var result = _loginParse.DoWxOrPhone(new ReqLoginWx.ReqLoginWxDo
            {
                AppKey = appKey,
                WxOpenIdOrPhone = wxOpenId
            });
            return result;
        }
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="appKey">应用程序key.</param>
        /// <param name="phone"></param>
        /// <returns>System.String.</returns>
        public LoginResult LoginByPhone(string appKey, string phone)
        {
            var result = _loginParse.DoWxOrPhone(new ReqLoginWx.ReqLoginWxDo
            {
                AppKey = appKey,
                WxOpenIdOrPhone = phone
            });
            return result;
        }


        /// <summary>
        ///     注销
        /// </summary>
        public bool Logout()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return true;
            }

            try
            {
                _cacheContext.Remove(token);
                _unitWork.Update<ModelUser>(p => p.Token == token, u => new ModelUser()
                {
                    Token = ""
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        private string GetToken()
        {
            string token = _httpContextAccessor == null ? "" : _httpContextAccessor.HttpContext.Request.Query[Define.TokenName];
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            token = _httpContextAccessor.HttpContext.Request.Headers[Define.TokenName];
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            token = _httpContextAccessor.HttpContext.Request.Cookies[Define.TokenName];
            return token ?? string.Empty;
        }

        /// <summary>
        ///获取登录用户店铺Id
        /// </summary>
        /// <returns></returns>
        public string GetStoreId()
        {

            var platform = GetPlatform();
            var storeId = _httpContextAccessor.HttpContext.Request.Headers["storeId"].ToString();
            if (platform == Define.AppWxApp && string.IsNullOrEmpty(storeId))
            {
                throw new Exception("小程序StoreId不能为空!");
            }
            if (platform == Define.AppWebStore)
            {
                var currentContext = GetCurrentContext();
                if (currentContext != null)
                {
                    storeId = GetCurrentContext().User.StoreId;
                }
            }
            return storeId;
        }
        /// <summary>
        ///获取当前平台来源
        /// </summary>
        /// <returns></returns>
        public string GetPlatform()
        {
            var platform = _httpContextAccessor.HttpContext.Request.Headers["platform"].ToString();
            return platform;
        }


        /// <summary>
        ///     根据Token获取用户
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ModelUser GetUserByToken(string token)
        {
            var user = _unitWork.FirstOrDefault<ModelUser>(p => p.Token == token && p.TokenTime > DateTime.Now.AddDays(-Define.TokenDays));
            return user;
        }
    }
}