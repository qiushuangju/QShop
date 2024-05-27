using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    public class ReqRefundMoney
    {
        /// <summary>
        /// Id
        /// </summary>
        public string OrderRefundSkuId { get; set; }

        /// <summary>
        /// 实际退款金额
        /// </summary>
        public decimal AmountRealRefund {  get; set; }
    }
}
