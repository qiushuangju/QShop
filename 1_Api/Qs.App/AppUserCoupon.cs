using System.Collections.Generic;
using System.Linq;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.App
{
    /// <summary>
    /// 应用层
    /// </summary>
    public class AppUserCoupon : AppBaseString<ModelUserCoupon, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppUserCoupon(IUnitWork<QsDBContext> unitWork, IRepository<ModelUserCoupon, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuUserCoupon req)
        {           
            //var loginContext = _auth.GetCurrentUser();
            var result = new TableData();
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResUserCoupon> ListByWhere(ReqQuUserCoupon req, bool isPage = false)
        {
            List<ResUserCoupon> listVm = new List<ResUserCoupon>();
            IQueryable<ResUserCoupon> linq = ListLinq(req);
            List<ResUserCoupon> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            foreach (var item in list)
            {
                listVm.Add(ResUserCoupon.ToView(item)); 
            }

            return listVm;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ResUserCoupon> ListLinq(ReqQuUserCoupon req)
        {
            //var linq = UnitWork.Find<ModelUserCoupon>(p => true);
            var currentIUser = _auth.GetCurrentContext().User;
            var linq = from userCoupon in UnitWork.Find<ModelUserCoupon>(p => true)
                       join user in UnitWork.Find<ModelUser>(p => true) on userCoupon.UserId equals user.Id
                       select new ResUserCoupon
                       {
                            CouponId = userCoupon.CouponId,
                            Name = userCoupon.Name,
                            Status = userCoupon.Status,
                            CouponType = userCoupon.CouponType,
                            SendType = userCoupon.SendType,
                            ReducePrice = userCoupon.ReducePrice,
                            MinPrice = userCoupon.MinPrice,
                            StartTime = userCoupon.StartTime,
                            EndTime = userCoupon.EndTime,
                            ApplyRange = userCoupon.ApplyRange,
                            ApplyRangeConfig = userCoupon.ApplyRangeConfig,
                            UserId = userCoupon.UserId,
                            StoreId = userCoupon.StoreId,
                            CreateTime = userCoupon.CreateTime,
                            UpdateTime = userCoupon.UpdateTime,
                            Account = user.Account,
                            UrlAvater = user.UrlAvater,
                            UserName = user.NickName
                       };

            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Name.Contains(req.Key));
            }
            if (req.OnlyMy)
            {
                linq = linq.Where(p => p.UserId == currentIUser.Id);
            }
            if (xConv.ToInt(req.Status)!=0)
            {
                linq = linq.Where(p => p.Status == xConv.ToInt(req.Status));
            }
            if (!string.IsNullOrEmpty(req.UserName))
            {
                linq = linq.Where(p => p.UserName.Contains(req.UserName));
            }
            return linq.OrderByDescending(p => p.CreateTime);
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuUserCoupon req)
        {
            var model = xConv.CopyMapper<ModelUserCoupon, ReqAuUserCoupon>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                Repository.Add(model);
            }
            else
            {
                Repository.Update(model);
            }
        }
        
    }
}