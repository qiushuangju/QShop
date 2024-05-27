using System;
using System.Collections.Generic;
using Qs.Comm;

namespace Qs.App.SSO
{

    /// <summary>
    /// 登录
    /// </summary>
    public class PassportLoginRequest
    {
        /// <summary>
        /// 账号(手机号码)
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码(密码/手机验证码必选一)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机验证码(密码/手机验证码必选一)
        /// </summary>           
        public string PhoneCode { get; set; }
        /// <summary>
        /// 验证码ID
        /// </summary>
        public string VerifyCodeId { get; set; }
        /// <summary>
        /// 图片验证码
        /// </summary>
        public string VerifyCode{ get; set; }
        /// <summary>
        /// 应用的AppKey，"AppUser":用户端 AppAnchor:主播端
        /// </summary>
        /// <example>AppUser</example>
        public string AppKey { get; set; }

        /// <summary>
        /// 检验
        /// </summary>
        public void Trim()
        {
            if (AppKey != Define.AppWxApp && AppKey != Define.AppWebAdmin && AppKey != Define.AppWebStore)
            {
                throw new Exception("请输入正确的AppKey,AppUser/AppAnchor");
            }

            if (string.IsNullOrEmpty(Account))
            {
                throw new Exception("用户名不能为空");
            }

            if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(PhoneCode))
            {
                throw new Exception("密码/手机验证码不能为空");
            }
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                 new ValueTip("VerifyCodeId","验证码") ,
                 new ValueTip("VerifyCode","验证码")
            });

            Account = Account.Trim();
            Password = xConv.Trim(Password);
            PhoneCode = xConv.Trim(PhoneCode);
            if (!string.IsNullOrEmpty(AppKey))
                AppKey = AppKey.Trim();
        }
    }
}