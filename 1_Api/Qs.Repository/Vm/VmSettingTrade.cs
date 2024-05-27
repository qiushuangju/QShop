using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 交易设置
    /// </summary>
    public class VmSettingTrade
    {
        /// <summary>
        /// 运费策略(10:叠加 20:以最低运费结算 30:以最高运费结算)
        /// </summary>
        public int FreightRule { get; set; } =10;

        /// <summary>
        /// 订单交易设置
        /// </summary>
        public VmSettingTradeOrder Order { get; set; } = new VmSettingTradeOrder();
    }

    /// <summary>
    /// 订单设置
    /// </summary>
    public class VmSettingTradeOrder
    {
        /// <summary>
        /// 未支付订单关闭小时数
        /// </summary>
        public int CloseHours { get; set; } = 72;

        /// <summary>
        ///发货订单 自动收货天数
        /// </summary>
        public int ReceiveDays { get; set; } = 10;

        /// <summary>
        ///  已完成订单 允许售后天数
        /// </summary>
        public int RefundDays { get; set; } = 7;

        /// <summary>
        /// 库存计算方式 10:下单减库存 20:付款减库存
        /// </summary>
        public int DeductStockType { get; set; } = 20;
    }
}
