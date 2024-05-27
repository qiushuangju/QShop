using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    public class ResCoupon:ModelCoupon
    {
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public string StrCouponType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StrStatus { get; set; }
        /// <summary>
        /// 条件列表
        /// </summary>
        public List<string> ListCondition { get; set; }

        /// <summary>
        /// 首页图片
        /// </summary>
        public string UrlHomeImg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResCoupon ToView(ModelCoupon model,List<ModelFileUpload> listImg)
        {
            ResCoupon res = xConv.CopyMapper<ResCoupon, ModelCoupon>(model);
            res.ListCondition = new List<string>();
            res.ListCondition.Add($"订单满{res.MinPrice}");
            res.StrCouponType = xEnum.GetEnumDescription(typeof(xEnum.CouponType), model.CouponType);
            res.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.CouponStatus), model.Status);


            if (model.ApplyRange == (int)xEnum.CouponRange.Goods)
            {
                res.ListCondition.Add($"指定商品");
            }

            if (!string.IsNullOrEmpty(model.ShowImgId)&&listImg!=null)
            {
                res.UrlHomeImg = listImg.FirstOrDefault(p => p.Id == model.ShowImgId).FilePath;
            }
          
            return res;
        }
    }
}
