using System;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuOrder : BaseReqPage
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 地址Id(可选)
        /// </summary>
        public string AddressId { get; set; }
        /// <summary>
        /// 订单大状态(0:全部 10:待付款 20:待审核 30:待收货 40:已收货 -50:售后)
        /// </summary>
        public int? BigOrderStatus { get; set; }
        /// <summary>
        /// 订单状态(0:全部 -50:售后 10:拍照订单-待审核  20:待付款 30:待发货 40:配送中 80:已完成)
        /// </summary>
        public int OrderStatus { get; set; }      
        /// <summary>
        /// 订单最小状态(0:全部)
        /// </summary>
        public int? MinStatus { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 支付方式(10余额支付 20微信支付 30云闪付)
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 开始时间（付款时间）
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间（付款时间）
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 是否只查自己记录 (默认否)
        /// </summary>
        public bool OnlyMy { get; set; } = false;
    }
}