using System;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 更新秒杀活动请求
    /// </summary>
    public class ReqUpdateSeckillActivity
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public string Id { get; set; }
        
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
    }
}