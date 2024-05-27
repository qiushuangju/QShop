using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 验证码验证
    /// </summary>
   public class ReqCheckPhoneCode
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        ///   验证
        /// </summary>
        public  void Check()
        {
            xValidation.CheckPhone(Phone);
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(Code,"验证码")
            });
        }
    }
}
