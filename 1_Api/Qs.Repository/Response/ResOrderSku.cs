using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.App.ApiKuaiDiNiao.Res;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
   public class ResOrderSku : ModelOrderSku
    {
        /// <summary>
        /// Sku图片
        /// </summary>
        public string SkuImageUrl { get; set; }
        /// <summary>
        /// 售后数量
        /// </summary>
        public int QuantityRefund { get; set; }
        /// <summary>
        ///退款金额
        /// </summary>
        public decimal TotalAmountRefund { get; set; }
        /// <summary>
        ///佣金金额
        /// </summary>
        public decimal AmountCommission { get; set; }
        /// <summary>
        ///售后扣除佣金
        /// </summary>
        public decimal AmountCommissionRefund { get; set; }

        /// <summary>
        ///订单状态
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 状态名
        /// </summary>
        public string StrOrderStatus { get; set; }

        /// <summary>
        /// 售后状态
        /// </summary>
        public int RefundStatus { get; set; }

        /// <summary>
        /// 售后状态
        /// </summary>
        public string StrRefundStatus { get; set; }
        /// <summary>
        /// 是否有售后单
        /// </summary>
        public bool IsHaveRefund { get; set; }
       
        /// <summary>
        /// 申请售后时间
        /// </summary>
        public DateTime? DateTimeRefund { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public static ResOrderSku ToView(ModelOrderSku sku,  ModelOrderRefundSku refund, List<ModelFileUpload> listFile)
        {
            ModelFileUpload file= listFile.FirstOrDefault(p=>p.Id==sku.SkuImageId);
            ResOrderSku res = xConv.CopyMapper<ResOrderSku, ModelOrderSku>(sku);
            res.StrOrderStatus= xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), res.OrderStatus);
            res.SourceUserId = sku.SourceUserId;
            res.SkuImageUrl = file.Thumbnail;
           
            res.RefundStatus = refund==null?0: xConv.ToInt(refund.Status);
            res.StrRefundStatus = refund == null?"": xEnum.GetEnumDescription(typeof(xEnum.RefundStatus), res.RefundStatus);
            res.DateTimeRefund = refund == null ? null : refund.CreateTime;
            return res;
        }

       
    }
}
