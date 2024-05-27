using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    public class ReqSendSmsCaptcha
    {
        /// <summary>
        /// 图形验证码Id
        /// </summary>
        public string VerifyCodeId { get; set; }
        /// <summary>
        /// 图形验证码
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public void Check()
        {
               xValidation.CheckStrNull(new List<ValueTip>()
               {
                   new ValueTip(VerifyCodeId,"图形验证码ID"),
                   new ValueTip(VerifyCode,"图形验证码"),
                   new ValueTip(Mobile,"手机号")
               });
               xValidation.CheckPhone(Mobile);
        }
    }
}
