using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 
    /// </summary>
    public  class VmStatisticOrderGoodsNumByDate
    { 
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 商品总数
        /// </summary>
        public int? GoodsTotalNum { get; set; }
    }

    /// <summary>
    /// 日订单统计
    /// </summary>
    public class VmStatisticOrderByDate
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int? OrderCount { get; set; }


        /// <summary>
        /// 交易额
        /// </summary>
        public decimal? TotalPayPrice { get; set; }
    }

    /// <summary>
    /// 月订单统计
    /// </summary>
    public class VmStatisticOrderByMonth
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int? OrderCount { get; set; }


        /// <summary>
        /// 交易额
        /// </summary>
        public decimal? TotalPayPrice { get; set; }
    }

    /// <summary>
    /// 年订单统计
    /// </summary>
    public class VmStatisticOrderByYear
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int? CreateTime { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int? OrderCount { get; set; }


        /// <summary>
        /// 交易额
        /// </summary>
        public decimal? TotalPayPrice { get; set; }
    }

    /// <summary>
    /// 统计订单数和总运费
    /// </summary>
    public class VmOrderFreightPriceByDate
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int? CreateTime { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int? OrderCount { get; set; }

        /// <summary>
        /// 送货总运费
        /// </summary>
        public decimal? OrderTotalFreightPrice { get; set; }

        /// <summary>
        /// 退货总运费
        /// </summary>
        public decimal? RefundTotalFreightPrice { get; set; }

        /// <summary>
        /// 运费汇总（送货+退货）
        /// </summary>
        public decimal? TotalFreightPrice { get; set; }
    }

    /// <summary>
    /// 根据司机统计运费
    /// </summary>
    public class VmOrderFreightPriceByDriver
    {
        /// <summary>
        /// 司机姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 司机电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 司机类型
        /// </summary>
        public int? DriverType { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int? OrderCount { get; set; }

        /// <summary>
        /// 送货总运费
        /// </summary>
        public decimal? OrderTotalFreightPrice { get; set; }

        /// <summary>
        /// 退货总运费
        /// </summary>
        public decimal? RefundTotalFreightPrice { get; set; }

        /// <summary>
        /// 运费汇总（送货+退货）
        /// </summary>
        public decimal? TotalFreightPrice { get; set; }
    }

    /// <summary>
    /// 根据业务员统计客户、订单、订单金额
    /// </summary>
    public class VmOrderPriceByBusiness
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 客户数
        /// </summary>
        public int UserCount { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 交易额
        /// </summary>
        public decimal TotalPayPrice { get; set; }
    }

    /// <summary>
    /// 根据用户统计销售提成、开卡提成、总提成
    /// </summary>
    public class VmStatisticUserIncomeByDate 
    { 
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 销售提成
        /// </summary>

        public decimal SalesIncomeMoney { get; set; }

        /// <summary>
        /// 开年卡提成
        /// </summary>

        public decimal VipIncomeMoney { get; set; }
        /// <summary>
        /// 售后扣除提成
        /// </summary>

        public decimal ToalRefundIncomeMoney { get; set; }


        /// <summary>
        /// 总提成
        /// </summary>

        public decimal ToalncomeMoney { get; set; }
    }

    /// <summary>
    /// 统计订单数和总运费
    /// </summary>
    public class VmOrderFreightTotal
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string StrDate { get; set; }

        /// <summary>
        /// 运营仓
        /// </summary>
        public string StorageId { get; set; }
        /// <summary>
        /// 运营仓名称
        /// </summary>
        public string StorageName { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 总配送费
        /// </summary>
        public decimal? TotalFreightPrice { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal? FreightPrice { get; set; }
        /// <summary>
        /// 入户费
        /// </summary>
        public decimal? EntryPrice { get; set; }
        /// <summary>
        /// 上楼费
        /// </summary>
        public decimal? FloorPrice { get; set; }
    }
}
