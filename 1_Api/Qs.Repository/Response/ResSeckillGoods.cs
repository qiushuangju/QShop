using System;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 秒杀商品返回实体
    /// </summary>
    public class ResSeckillGoods
    {
        /// <summary>
        /// 秒杀商品ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityId { get; set; }
        
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }
        
        /// <summary>
        /// 商品ID
        /// </summary>
        public string GoodsId { get; set; }
        
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        
        /// <summary>
        /// 商品副标题
        /// </summary>
        public string SubTitle { get; set; }
        
        /// <summary>
        /// 商品主图
        /// </summary>
        public string ImageIdMains { get; set; }
        
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }
        
        /// <summary>
        /// 秒杀价
        /// </summary>
        public decimal SeckillPrice { get; set; }
        
        /// <summary>
        /// 秒杀库存
        /// </summary>
        public int SeckillStock { get; set; }
        
        /// <summary>
        /// 已售数量
        /// </summary>
        public int SoldCount { get; set; }
        
        /// <summary>
        /// 进度百分比
        /// </summary>
        public double Progress { get; set; }
        
        /// <summary>
        /// 剩余库存
        /// </summary>
        public int RemainingStock { get; set; }
        
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        
        /// <summary>
        /// 活动状态
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// 剩余时间（秒）
        /// </summary>
        public long RemainingTime { get; set; }
    }
}