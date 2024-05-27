using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 发送验证码
    /// </summary>
   public class ReqSendPhoneCode
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///   验证
        /// </summary>
        public  void Check()
        {
            xValidation.CheckPhone(Phone);
        }
    }
}
