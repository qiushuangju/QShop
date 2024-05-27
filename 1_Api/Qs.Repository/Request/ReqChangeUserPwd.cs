using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 修改密码(App)
    /// </summary>
   public class ReqChangeUserPwd
    {
        /// <summary>
        /// 手机号码
        /// </summary>
       public string Phone { get; set; }
       /// <summary>
       /// 新密码
       /// </summary>
       public string Pwd { get; set; }
       /// <summary>
       /// 手机验证码
       /// </summary>
       public string PhoneCode { get; set; }    
    }
}
