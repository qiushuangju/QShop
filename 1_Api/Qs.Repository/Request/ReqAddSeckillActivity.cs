using System;
using System.Collections.Generic;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 添加秒杀活动请求
    /// </summary>
    public class ReqAddSeckillActivity
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }
        
        /// <summary>
        /// 活动描述
        /// </summary>
        public string ActivityDesc { get; set; }
        
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        
        /// <summary>
        /// 用户限购数量
        /// </summary>
        public int LimitPerUser { get; set; }
        
        /// <summary>
        /// 每秒最大请求数
        /// </summary>
        public int MaxRequestPerSecond { get; set; }
        
        /// <summary>
        /// 秒杀商品列表
        /// </summary>
        public List<ReqSeckillGoodsItem> SeckillGoods { get; set; }
    }
    
    /// <summary>
    /// 秒杀商品项
    /// </summary>
    public class ReqSeckillGoodsItem
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string GoodsId { get; set; }
        
        /// <summary>
        /// SKU ID
        /// </summary>
        public string SkuId { get; set; }
        
        /// <summary>
        /// 秒杀价格
        /// </summary>
        public decimal SeckillPrice { get; set; }
        
        /// <summary>
        /// 秒杀库存
        /// </summary>
        public int SeckillStock { get; set; }
        
        /// <summary>
        /// 排序编号
        /// </summary>
        public int SortNo { get; set; }
    }
}