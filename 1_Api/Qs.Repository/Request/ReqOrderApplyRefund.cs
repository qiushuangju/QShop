using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 订单申请售后
    /// </summary>
    public partial class ReqOrderApplyRefund
    {
        /// <summary>
        /// 订单SkuId
        /// </summary>
        public string OrderSkuId { get; set; }
        /// <summary>
        /// 售后类型
        /// </summary>
        public int RefundType { get; set; }
        ///// <summary>
        /////退款原因Id
        ///// </summary>
        //public string ReasonId { get; set; }

        /// <summary>
        ///退款凭证
        /// </summary>
        public List<string> listImageId { get; set; }

        /// <summary>
        ///退款说明
        /// </summary>
        public string RefundDescription { get; set; }
    }
}
