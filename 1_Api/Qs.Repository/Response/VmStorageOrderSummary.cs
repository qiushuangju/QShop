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
    public  class VmStorageOrderSummary
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 商品分类id
        /// </summary>
        public string GoodsSkuId { get; set; }

        /// <summary>
        /// 采购总数
        /// </summary>
        public int TotalNum { get; set; }
    }
}
