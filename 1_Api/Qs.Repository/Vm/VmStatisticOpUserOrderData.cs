using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
   /// <summary>
   /// 
   /// </summary>
   public class VmStatisticOpUserOrderData
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get;set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 归属路径
        /// </summary>
        public string SourceUserId { get; set; }
        /// <summary>
        /// 购物金额
        /// </summary>
        public decimal? GoodsPrice { get; set; }
        // /// <summary>
        // /// 运费金额
        // /// </summary>
        // public decimal? TotalFreightPrice { get; set; }
        // /// <summary>
        // /// 订单金额
        // /// </summary>
        // public decimal? OrderPrice { get; set; }

     


    }
}
