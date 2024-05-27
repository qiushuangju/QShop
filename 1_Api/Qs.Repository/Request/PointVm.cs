using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 积分操作
    /// </summary>
    public class PointVm
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public xEnum.PointType OpType { get;set; }
      
        /// <summary>
        /// 变动金额
        /// </summary>
        public decimal Point { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; } = "";
        /// <summary>
        /// 变动用户ID
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 操作备注
        /// </summary>
        public string Remark { get; set; }
    }
}
