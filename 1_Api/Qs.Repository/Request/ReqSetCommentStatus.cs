using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    public class ReqSetCommentStatus
    {     
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 商品状态(10:显示 -10:隐藏)
        /// </summary>
        public int Status { get; set; }
    }
}
