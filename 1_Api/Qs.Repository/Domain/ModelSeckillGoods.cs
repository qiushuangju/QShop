using Qs.Repository.Core;
using System;

namespace Qs.Repository.Domain
{
    /// <summary>
    /// 秒杀商品表
    /// </summary>
    public class ModelSeckillGoods : LongEntity
    {
        /// <summary>
        /// 秒杀活动ID
        /// </summary>
        public decimal ActivityId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public decimal GoodsId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 商品主图
        /// </summary>
        public string GoodsImage { get; set; }

        /// <summary>
        /// 商品原价
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 秒杀价格
        /// </summary>
        public decimal SeckillPrice { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 已抢数量
        /// </summary>
        public int GrabbedQuantity { get; set; }

        /// <summary>
        /// 每人限购数量
        /// </summary>
        public int LimitPerUser { get; set; }

        /// <summary>
        /// 商品SKU ID（如果是多规格商品）
        /// </summary>
        public decimal? SkuId { get; set; }

        /// <summary>
        /// SKU规格描述（如果是多规格商品）
        /// </summary>
        public string SkuDesc { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public decimal CreateUserId { get; set; }
    }
}
