using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
   public class ResUserCoupon :ModelUserCoupon
    {
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public string StrCouponType { get; set; }
        /// <summary>
        /// 优惠券状态
        /// </summary>
        public string StrStatus { get; set; }
        /// <summary>
        /// 条件列表
        /// </summary>
        public List<string> ListCondition { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string UrlAvater { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResUserCoupon ToView(ModelUserCoupon model)
        {
            ResUserCoupon res = xConv.CopyMapper<ResUserCoupon, ModelUserCoupon>(model);
            res.StrCouponType = xEnum.GetEnumDescription(typeof(xEnum.CouponType), model.CouponType);
            res.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.CouponStatus), model.Status);
            res.ListCondition = new List<string>();
            res.ListCondition.Add($"订单满{res.MinPrice}");

            if (model.ApplyRange == (int)xEnum.CouponRange.Goods)
            {
                res.ListCondition.Add($"指定商品");
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResUserCoupon ToView(ResUserCoupon model)
        {
            // ResUserCoupon res = xConv.CopyMapper<ResUserCoupon, ModelUserCoupon>(model);
            model.StrCouponType = xEnum.GetEnumDescription(typeof(xEnum.CouponType), model.CouponType);
            model.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.CouponStatus), model.Status);
            model.ListCondition = new List<string>();
            model.ListCondition.Add($"订单满{model.MinPrice}");

            if (model.ApplyRange == (int)xEnum.CouponRange.Goods)
            {
                model.ListCondition.Add($"指定商品");
            }
            return model;
        }
    }
}
