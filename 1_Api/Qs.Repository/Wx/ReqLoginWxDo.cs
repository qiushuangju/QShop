using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace Qs.App.SSO
{
    /// <summary>
    /// 登录账号
    /// </summary>
    public class ReqLoginWx
    {
        /// <summary>
        /// 微信小程序用户登录凭证（有效期五分钟）
        /// </summary>
        [Description("微信小程序用户登录凭证")]
        public string Code { get; set; }

        /// <summary>
        /// 邀请链接Id(可选)
        /// </summary>
        public string InviteMemberId { get; set; }

        /// <summary>
        /// 应用Key
        /// </summary>
        public string AppKey { get; set; }

        public void Trim()
        {
            if (string.IsNullOrEmpty(Code))
            {
                throw new Exception("用户名不能为空");
            }

            Code = Code.Trim();
            InviteMemberId = InviteMemberId.Trim();
        }

        public class ReqLoginWxDo : ReqLoginWx
        {
            /// <summary>
            /// 微信小程序用户登录凭证（有效期五分钟）
            /// </summary>
            public string WxOpenIdOrPhone { get; set; }


            public void Trim()
            {
                if (string.IsNullOrEmpty(Code))
                {
                    throw new Exception("Code不能为空");
                }

                Code = Code.Trim();
                InviteMemberId = InviteMemberId.Trim();
            }
        }
    }
}
