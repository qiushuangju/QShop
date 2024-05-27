using Qs.Comm;
using Qs.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TencentCloud.Ame.V20190916.Models;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class ResOrderRefundSku: ModelOrderRefundSku
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        public string UrlSkuThumbnail { get; set; }

        // public ModelGoodsSku GoodsSku { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 申请时订单状态
        /// </summary>
        public string StrOrderStatus { get; set; }
        /// <summary>
        /// 售后类型
        /// </summary>
        public string StrRefundType { get; set; }
        /// <summary>
        /// 售后状态
        /// </summary>
        public string StrStatus { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        ///   售后凭证
        /// </summary>
        public List<string> ListRefundProof { get; set; }

        /// <summary>
        /// 售后记录
        /// </summary>
        public List<ModelOrderTrack> ListRecord { get; set; }

         /// <summary>
         /// 退货地址
         /// </summary>
        public ModelOrderRefundAddress RefundAddress { get; set; }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="model"></param>
       /// <param name="listFile"></param>
       /// <returns></returns>
        public static ResOrderRefundSku ToView(ModelOrderRefundSku model, List<ModelFileUpload> listFile=null)
        {
            var listFileId = xConv.ToListString(model.RefundProof);
            List<string> listRefundProof = listFile.Where(p=> listFileId.Contains(p.Id)).Select(p=>p.Thumbnail).ToList();
            var file = listFile.FirstOrDefault(p => (p.Id) == model.SkuImageId) ?? new ModelFileUpload();
            ResOrderRefundSku res = xConv.CopyMapper<ResOrderRefundSku, ModelOrderRefundSku>(model);
            res.StrOrderStatus = xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), xConv.ToInt(res.CurrentOrderStatus));
            res.StrRefundType = xEnum.GetEnumDescription(typeof(xEnum.RefundType), xConv.ToInt(res.RefundType));
            res.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.RefundStatus), xConv.ToInt(res.Status));
            res.UrlSkuThumbnail = file?.Thumbnail;
            res.ListRefundProof = listRefundProof;
            return res;

        }
    }
}
