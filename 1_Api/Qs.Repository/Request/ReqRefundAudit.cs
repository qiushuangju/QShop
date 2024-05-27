using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    public class ReqRefundAudit
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string RefundSkuId { get; set; }

        /// <summary>
        /// 售后状态(-10:审核拒绝 20:审核通过)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 地址Id
        /// </summary>
        public string StoreAddresId { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string SellerMark { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(RefundSkuId, "订单Id"),
            });
            if (Status != (int)xEnum.RefundStatus.UnApprove && Status != (int)xEnum.RefundStatus.Approve)
            {
                throw new Exception("审核状态只能同意,或者拒绝");
            }
            if (Status == (int)xEnum.RefundStatus.UnApprove)
            {
                xValidation.CheckStrNull(new List<ValueTip>()
                {
                    new ValueTip(SellerMark, "拒绝原因"),
                });
            }
            if (Status == (int)xEnum.RefundStatus.Approve)
            {
                xValidation.CheckStrNull(new List<ValueTip>()
                {
                    new ValueTip(StoreAddresId, "地址Id"),
                });
            }
        }
    }
}
