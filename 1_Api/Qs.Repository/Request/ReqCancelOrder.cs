using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    public class ReqCancelOrder
    {
        /// <summary>
        /// AppUser:用户端 AppBusiness:业务员 AppDriver:司机 AppDispatch:调度
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 退余额备注
        /// </summary>
        public string ReturnMoneyRemark { get; set; }

        /// <summary>
        /// 下单人ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {

                new ValueTip(OrderId, "订单Id"),
            });

        }
    }
}
