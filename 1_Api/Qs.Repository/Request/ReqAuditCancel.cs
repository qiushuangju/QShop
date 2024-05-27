using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    public class ReqAuditCancel
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 审核状态  (-30:同意取消 20:拒绝)
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(OrderId, "订单Id"),
            });
            if (OrderStatus!=(int)xEnum.OrderStatus.Cancel && OrderStatus != (int)xEnum.OrderStatus.WaitDeliver)
            {
                throw new Exception("审核状态只能同意,或者拒绝");
            }

        }
    }
}
