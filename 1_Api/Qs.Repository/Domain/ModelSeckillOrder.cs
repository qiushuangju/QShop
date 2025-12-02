using Qs.Repository.Core;
using System;

namespace Qs.Repository.Domain
{
    /// <summary>
    /// 秒杀订单表
    /// </summary>
    public class ModelSeckillOrder : LongEntity
    {
        /// <summary>
        /// 秒杀活动ID
        /// </summary>
        public decimal ActivityId { get; set; }

        /// <summary>
        /// 秒杀商品ID
        /// </summary>
        public decimal SeckillGoodsId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public decimal UserId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public decimal GoodsId { get; set; }

        /// <summary>
        /// 商品SKU ID（如果是多规格商品）
        /// </summary>
        public decimal? SkuId { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品原价
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 秒杀价格
        /// </summary>
        public decimal SeckillPrice { get; set; }

        /// <summary>
        /// 订单总价
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 订单状态：1-待支付，2-已支付，3-已取消，4-已完成
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime? CancelTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
