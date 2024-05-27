using System;
using System.Collections.Generic;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuOrderGoods : BaseReqPage
    {
        

        public List<string> ListOrderId { get; set; } = new List<string>() { };
        public List<string> ListOrderGoodsId { get; set; } = new List<string>() { };
        /// <summary>
        /// 子订单号
        /// </summary>
        public string SubOrderNo { get; set; }
        /// <summary>
        /// 订单大状态(0:全部 10:待付款 20:待审核 30:待收货 40:已收货 -50:售后)
        /// </summary>
        public int? BigOrderStatus { get; set; }
        /// <summary>
        /// 订单状态(0:全部 -50:售后 10:拍照订单-待审核  20:待付款 30:待发货 40:配送中 80:已完成)
        /// </summary>
        public int OrderStatus { get; set; }
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
        /// 最小状态 -30:已取消 10:待付款 30:待发货 40:待收货 50:已送达 60:已收货 80:已完成
        /// </summary>
        public int? MinOrderStatus { get; set; }
        /// <summary>
        /// 是否查询后裔
        /// </summary>
        public bool IsProgeny { get; set; } = false;

    }
}