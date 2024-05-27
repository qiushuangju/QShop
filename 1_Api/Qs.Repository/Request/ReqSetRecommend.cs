using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    public class ReqSetRecommend
    {     
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }
    }
}
