using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 
    /// </summary>
    public  class ReqStatistic
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 是否只查自己记录 (默认是)
        /// </summary>
        public bool OnlyMy { get; set; } = true;

        public string StoreId { get; set; }

    }
}
