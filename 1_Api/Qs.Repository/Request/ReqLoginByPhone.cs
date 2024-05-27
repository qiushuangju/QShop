using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 手机号登录
    /// </summary>
    public class ReqLoginByPhone
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string InviteCode { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string SmsCode { get; set; }

        /// <summary>
        /// 短信验证码Id
        /// </summary>
        public string SmsVerifyCodeId { get; set; }
        /// <summary>
        /// 是否第三方
        /// </summary>
        public bool IsParty { get; set; }

        /// <summary>
        /// 第三方信息
        /// </summary>
        public object PartyData { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public void Check()
        {
            Mobile = Mobile.Trim();
            xValidation.CheckPhone(Mobile);
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(SmsVerifyCodeId,"短信验证码Id") ,
                new ValueTip(SmsCode,"验证码"),
            });
        }
    }
}
