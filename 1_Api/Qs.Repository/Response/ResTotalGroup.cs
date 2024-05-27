using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
   public  class ResTotalGroup
    {
        /// <summary>
        /// 全部数量
        /// </summary>
        public   int TotalAll { get; set; }
        /// <summary>
        /// 好评数量
        /// </summary>
        public int TotalPraise { get; set; }
        /// <summary>
        /// 中评数量
        /// </summary>
        public int TotalMiddle { get; set; }
        /// <summary>
        /// 差评数量
        /// </summary>
        public int TotalBad { get; set; }
    }
}
