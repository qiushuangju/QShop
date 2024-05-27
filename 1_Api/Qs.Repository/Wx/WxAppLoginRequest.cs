using System;
using System.ComponentModel;

namespace Qs.Repository.Wx
{
    /// <summary>
    /// 微信小程序登录
    /// </summary>
    public class WxAppLoginRequest
    {
        /// <summary>
        /// 微信小程序用户登录凭证（有效期五分钟）
        /// </summary>
        [Description("微信小程序用户登录凭证")]
        public string WxCode { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        public string InviteCode { get; set; }
       
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
       

        /// <summary>
        /// 应用Key (AppUser:用户 AppBusiness:业务员 AppDriver:司机 AppDispatch:调度)
        /// </summary>
        public string AppKey { get; set; }

        public void Trim()
        {
            if (string.IsNullOrEmpty(WxCode))
            {
                throw new Exception("Code不能为空");
            }

            WxCode = WxCode.Trim();

        }
    }
}