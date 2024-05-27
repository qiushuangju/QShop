using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    public class ReqPayOrderBefore
    {
        /// <summary>
        ///订单Id
        /// </summary>                                 
        public string OrderId { get; set; }

        /// <summary>
        ///支付方式 
        /// </summary>
        public int PayType { get; set; }
    }
}
