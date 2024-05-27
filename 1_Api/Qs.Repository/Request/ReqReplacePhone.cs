using Qs.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 更换手机号
    /// </summary>
    public class ReqReplacePhone
    {
        /// <summary>
        /// 手机号码(新)
        /// </summary>
        public string NewPhone { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        public string PhoneCode { get; set; }

        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(NewPhone,"新手机号"),
                new ValueTip(PhoneCode,"验证码")
            });
        }
    }
}
