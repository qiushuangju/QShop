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
    /// 支付设置 App
    /// </summary>
    public class AppStoreSettingPay : AppBaseString<ModelStoreSettingPay, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppStoreSettingPay(IUnitWork<QsDBContext> unitWork, IRepository<ModelStoreSettingPay, QsDBContext> repository,
            AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuStoreSettingPay req)
        {
            var result = new TableData();
            result.Result = ListByWhere(req, true);
            result.Count = ListLinq(req).Count();
            return result;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ModelStoreSettingPay> ListByWhere(ReqQuStoreSettingPay req, bool isPage = false)
        {
            IQueryable<ModelStoreSettingPay> linq = ListLinq(req);
            List<ModelStoreSettingPay> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelStoreSettingPay> ListLinq(ReqQuStoreSettingPay req)
        {
            var linq = UnitWork.Find<ModelStoreSettingPay>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Name.Contains(req.Key));
            }
            var storeId = _auth.GetStoreId();
            if (!string.IsNullOrEmpty(storeId))
            {
                linq = linq.Where(p => p.StoreId == storeId);
            }
            else if (req.OnlyStore)
            {
                var user = _auth.GetCurrentContext().User;
                linq = linq.Where(p => p.StoreId == user.StoreId);
            }
            return linq;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuStoreSettingPay req)
        {
            req.Check();
            var storeId = _auth.GetStoreId();
            var isNew = string.IsNullOrEmpty(req.Id) ? true : false;
            var model = Repository.FirstOrDefault(p => p.Id == req.Id) ?? new ModelStoreSettingPay();
            model.Values = xConv.ToJson(req.SettingObj);
            model.StoreId = storeId;
            model.Name = req.Name;
            UnitWork.AddOrUpdate(model);
            UnitWork.Save();
        }
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="req"></param>
        public void SetStatus(ReqSetSettingPayStatus req)
        {
            var model = Repository.FirstOrDefault(p => p.Id == req.Id);
            model.Status = req.Status;
            Repository.Update(model);
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="req"></param>
        public void SetDefault(ReqSetSettingPayDefault req)
        {
            var storeId = _auth.GetStoreId();
            if (req.IsDefault)
            {
                UnitWork.Update<ModelStoreSettingPay>(p => p.StoreId == storeId, u => new ModelStoreSettingPay
                {
                    IsDefault = !req.IsDefault,
                });
            }
            var model = Repository.FirstOrDefault(p => p.Id == req.Id);
            model.IsDefault = req.IsDefault;
            UnitWork.Update(model);
            UnitWork.Save();
        }

        /// <summary>
        /// GetDetail
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResStoreSettingPay GetDetail(string id)
        {
            var storeId = _auth.GetStoreId();           
            Expression<Func<ModelStoreSettingPay, bool>> exp = LambdaExtensions.True<ModelStoreSettingPay>();
            if (!string.IsNullOrEmpty(storeId))
            {
                exp = exp.And(p => p.StoreId == storeId);
            }
            if (!string.IsNullOrEmpty(id))
            {
                exp = exp.And(p => p.Id == id);
            }
            var model = UnitWork.FirstOrDefault<ModelStoreSettingPay>(exp);
            ResStoreSettingPay res= ResStoreSettingPay.ToView(model);
            return res;
        }
        
    }
}