using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    public class ResPcHomePage
    {
        /// <summary>
        /// 首页数据块是否显示
        /// </summary>
        public CardShow CardShow { get; set; } = new CardShow();

        /// <summary>
        /// 统计数量
        /// </summary>
        public Total Total { get; set; } = new Total();

        /// <summary>
        /// 今日数据
        /// </summary>
        public Today Today { get; set; } = new Today();

        public List<VmEchart> ListEchart { get; set; }
    }

    /// <summary>
    /// 是否显示数据块
    /// </summary>
    public class CardShow
    {
        /// <summary>
        /// 实时操作
        /// </summary>
        public bool? IsShowCardOperate { get; set; } = true;
        /// <summary>
        /// 实时概况
        /// </summary>
        public bool? IsShowCardProfile { get; set; } = true;
    }
    /// <summary>
    /// 统计总数
    /// </summary>
    public class Total
    {
        /// <summary>
        /// 待发货订单
        /// </summary>
        public int TotalOrderWaitDeliver { get; set; }
        /// <summary>
        /// 售后待处理
        /// </summary>
        public int TotalOrdertWaitRefund { get; set; }
        /// <summary>
        /// 已售商品数
        /// </summary>
        public int TotalOrdertWaitPay { get; set; }
        /// <summary>
        /// 库存警戒
        /// </summary>
        public int TotalWarningStock { get; set; }
        /// <summary>
        /// 总用户人数
        /// </summary>
        public int TotalCustomer { get; set; }
    }

    public class Today
    {
        /// <summary>
        /// 销售额
        /// </summary>
        public decimal TotalAmountGoods { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public int TotalOrder { get; set; }
        /// <summary>
        /// 新增用户
        /// </summary>
        public int TotalNewCustomer { get; set; }
    }

    /// <summary>
    ///  首页Echart数据
    /// </summary>
    public class VmEchart
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 交易额
        /// </summary>
        public decimal PayOrderPrice { get; set; }
    }
}
