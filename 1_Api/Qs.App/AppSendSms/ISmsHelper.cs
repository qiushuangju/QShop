using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.App
{
   public interface ISmsHelper
   {
       ///  <summary>
       /// 发送验证码
       ///  </summary>
       ///  <param name="phone"></param>
       ///  <param name="code">验证码</param>
       ///  <returns></returns>
       ResPhoneCode SendPhoneCode(string phone,string code);

        /// <summary>
        /// 新订单(通知商家)
        /// </summary>
        /// <returns></returns>
        ResPhoneCode SendNewOrderToStore(string phone);
    }
}
