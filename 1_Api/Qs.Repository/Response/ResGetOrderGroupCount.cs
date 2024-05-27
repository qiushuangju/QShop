using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 获取订单数量
    /// </summary>
    public class ResGetOrderGroupCount
    {
        /// <summary>
        /// 待支付订单数量
        /// </summary>
        public int CountNotPaid { get; set; }
        /// <summary>
        /// 待发货数量
        /// </summary>
        public int CountWaitDeliver { get; set; }
        /// <summary>
        /// 待收货数量
        /// </summary>
        public int CountWaitReceiving { get; set; }

        /// <summary>
        /// 待评价数量
        /// </summary>
        public int CountWaitComment { get; set; }

        /// <summary>
        /// 售后订单
        /// </summary>
        public int CountRefund { get; set; }
    }
}
