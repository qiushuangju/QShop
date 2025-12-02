using Qs.Repository.Base;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 秒杀活动查询对象
    /// </summary>
    public class ReqQuSeckillActivity : BaseReqPage
    {
        /// <summary>
        /// 活动状态
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// 活动名称
        /// </summary>
        public string Key { get; set; }
    }
}