using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    public class ReqSetStatus
    {     
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 商品状态(10上架 -10下架)
        /// </summary>
        public int Status { get; set; }
    }
}
