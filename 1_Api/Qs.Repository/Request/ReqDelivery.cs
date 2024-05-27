using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 订单发货
    /// </summary>
    public class ReqDelivery
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 物流公司
        /// </summary>
        public string ExpressId { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(OrderId, "订单Id"),
                new ValueTip(ExpressId, "物流公司"),
                new ValueTip(ExpressNo, "物流单号"),
            });
            

        }
    }
}
