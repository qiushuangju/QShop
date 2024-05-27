using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 
    /// </summary>
    public  class VmStatisticUserVipByDate
    {
        /// <summary>
        /// 入会日期
        /// </summary>
        public DateTime OpenTime { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 用户总数
        /// </summary>
        public int UserTotal { get; set; }
    }
}
