using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Comm.Extensions;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.App
{
    /// <summary>
    ///  App
    /// </summary>
    public class AppCoupon : AppBaseString<ModelCoupon, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppCoupon(IUnitWork<QsDBContext> unitWork, IRepository<ModelCoupon, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuCoupon req)
        {           
            var result = new TableData();
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResCoupon> ListByWhere(ReqQuCoupon req, bool isPage = false)
        {
            IQueryable<ModelCoupon> linq = ListLinq(req);
            List<ModelCoupon> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();

            List<ResCoupon> listVm = new List<ResCoupon>();
            foreach (var item in list)
            {
                listVm.Add(ResCoupon.ToView(item,null));
            }

            return listVm;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelCoupon> ListLinq(ReqQuCoupon req)
        {
            var linq = UnitWork.Find<ModelCoupon>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Name.Contains(req.Key));
            }
             var storeId = _auth.GetStoreId();
            if (!string.IsNullOrEmpty(storeId))
            {
                linq = linq.Where(p => p.StoreId == storeId);
            }
            return linq;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuCoupon req)
        {
            var model = xConv.CopyMapper<ModelCoupon, ReqAuCoupon>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            var user = _auth.GetCurrentContext().User;
            model.StoreId = _auth.GetStoreId();
            model.CouponType = (int) xEnum.CouponType.FullFree;
            if (isNew)
            {
                model.CreateTime=DateTime.Now;
                Repository.Add(model);
            }
            else
            {
                Repository.Update(model);
            }
        }

        /// <summary>
        /// 领取优惠券
        /// </summary>
        public void ReceiveCoupon(ReqReceiveCoupon req)
        {
            var user = _auth.GetCurrentContext().User;
            var coupon = Repository.FirstOrDefault(p => p.Id == req.CouponId);
            var countCoupon = UnitWork.Count<ModelUserCoupon>(p => p.UserId == user.Id && p.CouponId == coupon.Id);
            if (countCoupon >= coupon.LimitQuantity)
            {
                throw new Exception($"此优惠券限领{coupon.LimitQuantity}张,您已领完");
            }

            ModelUserCoupon userCoupon = new ModelUserCoupon();
            userCoupon.Name = coupon.Name;
            userCoupon.CouponId = coupon.Id;
            userCoupon.CouponType = coupon.CouponType;
            userCoupon.SendType = coupon.SendType;
            userCoupon.ReducePrice = coupon.ReducePrice;
            userCoupon.MinPrice = coupon.MinPrice;
            if (coupon.ExpireType == (int)xEnum.ExpireType.Days)
            {
                userCoupon.StartTime = xConv.ToDate(DateTime.Now);
                userCoupon.EndTime = userCoupon.StartTime.AddDays(xConv.ToInt(coupon.ExpireDay));
            }
            else
            {
                userCoupon.StartTime = coupon.StartTime;
                userCoupon.EndTime = coupon.EndTime;
            }
            userCoupon.ApplyRange = coupon.ApplyRange;
            userCoupon.ApplyRangeConfig = coupon.ApplyRangeConfig;
            userCoupon.Status = (int)xEnum.CouponStatus.Usable;
            userCoupon.UserId = user.Id;
            userCoupon.StoreId = coupon.StoreId;
            coupon.ReceiveNum += 1;
            Repository.Update(coupon);
            UnitWork.AddOrUpdate(userCoupon);
            UnitWork.Save();
        }

        /// <summary>
        /// GetDetail
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ModelCoupon GetDetail(string id)
        {
            var storeId = _auth.GetStoreId();           
            Expression<Func<ModelCoupon, bool>> exp = LambdaExtensions.True<ModelCoupon>();
            if (!string.IsNullOrEmpty(storeId))
            {
                exp = exp.And(p => p.StoreId == storeId);
            }
            if (!string.IsNullOrEmpty(id))
            {
                exp = exp.And(p => p.Id == id);
            }
            var model = UnitWork.FirstOrDefault<ModelCoupon>(exp);            
            return model;
        }
        
    }
}