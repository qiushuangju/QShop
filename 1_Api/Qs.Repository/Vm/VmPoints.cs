using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 积分操作
    /// </summary>
    public class VmPoints
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public xEnum.PointType OpType { get;set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; } = "";
        /// <summary>
        /// 操作备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 余额变动用户Id(如变动用户即当前用户可不传)
        /// </summary>
        public string UserId { get; set; } = "";
    }

  
}
