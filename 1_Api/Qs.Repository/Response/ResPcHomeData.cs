using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    public class ResPcHomeData
    {
        /// <summary>
        /// 用户数
        /// </summary>
        public int CountUser { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal CountAmount { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public int CountOrder { get; set; }
    }
}
