using Qs.Repository.Base;
using System;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuRechargeOrder : BaseReqPage
    {
        

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public int? PayStatus { get; set; }

        /// <summary>
        /// 充值方式
        /// </summary>
        public int? RechargeType { get; set; }
    }
}