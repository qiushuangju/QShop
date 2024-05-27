using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 根据订单状态和日期统计订单
    /// </summary>
    public class VmStatisticOrderByStatus
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 实付金额总数
        /// </summary>
        public decimal PayPriceTotal { get; set; }

       
    }

    /// <summary>
    ///  根据订单状态和日期统计服务订单
    /// </summary>
    public class VmStatisticOrderServiceByStatus
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 支付方式(定金)
        /// </summary>
        public int PayDepositType { get; set; }

        /// <summary>
        /// 实付定金总额
        /// </summary>
        public decimal PayDepositPrice { get; set; }

        /// <summary>
        /// 支付方式(尾款)
        /// </summary>
        public int PayFinalType { get; set; }

        /// <summary>
        /// 实付尾款总额
        /// </summary>
        public decimal PayFinalPrice { get; set; }

        /// <summary>
        /// 实付总额
        /// </summary>
        public decimal PayPriceTotal { get; set; }
    }
}
