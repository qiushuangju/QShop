using System;
using System.Collections.Generic;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 充值
    /// </summary>
    public class ReqSetRechargePwd
    {
        /// <summary>
        /// 支付密码
        /// </summary>
        public string RechargePwd { get; set; }
        /// <summary>
        /// 第二此支付密码
        /// </summary>
        public string OtherRechargePwd { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        public string PhoneCode { get; set; }

        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(RechargePwd,"支付密码") ,
                new ValueTip(OtherRechargePwd,"第二次支付密码"),
                new ValueTip(PhoneCode,"验证码")
            });
            if (RechargePwd!= OtherRechargePwd)
            {
                throw new Exception("两次输入密码不一致!");
            }
        }
    }
}
