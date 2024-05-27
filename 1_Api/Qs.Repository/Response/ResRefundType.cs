using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 退货退款类型
    /// </summary>
   public class ResRefundType
    {
        /// <summary>
        /// 类型
        /// </summary>
        public List<EnumEntity>  ListType { get; set; }

        /// <summary>
        /// 流程说明
        /// </summary>
        public string RefundExplain { get; set; }
    }
}
