using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 根据日期统计用户数
    /// </summary>
    public class VmStatisticUserByDate
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 用户数
        /// </summary>
        public int UserCount{ get; set; }
    }
}
