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
    /// 售后订单退货地址 App
    /// </summary>
    public class AppOrderRefundAddress : AppBaseString<ModelOrderRefundAddress, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppOrderRefundAddress(IUnitWork<QsDBContext> unitWork, IRepository<ModelOrderRefundAddress, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuOrderRefundAddress req)
        {           
            //var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            //var properties = _dbExtension.GetProperties("OrderRefundAddress");
            //result.ColumnHeaders = properties;
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ModelOrderRefundAddress> ListByWhere(ReqQuOrderRefundAddress req, bool isPage = false)
        {
            IQueryable<ModelOrderRefundAddress> linq = ListLinq(req);
            List<ModelOrderRefundAddress> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelOrderRefundAddress> ListLinq(ReqQuOrderRefundAddress req)
        {
            var linq = UnitWork.Find<ModelOrderRefundAddress>(p => true);
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
        public void AddOrUpdate(ReqAuOrderRefundAddress req)
        {
            var model = xConv.CopyMapper<ModelOrderRefundAddress, ReqAuOrderRefundAddress>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            var user = _auth.GetCurrentContext().User;
            model.StoreId = user.StoreId;
            if (isNew)
            {
                Repository.Add(model);
            }
            else
            {
                Repository.Update(model);
            }
        }
        
        /// <summary>
        /// GetDetail
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ModelOrderRefundAddress GetDetail(string id)
        {
            var storeId = _auth.GetStoreId();           
            Expression<Func<ModelOrderRefundAddress, bool>> exp = LambdaExtensions.True<ModelOrderRefundAddress>();
            if (!string.IsNullOrEmpty(storeId))
            {
                exp = exp.And(p => p.StoreId == storeId);
            }
            if (!string.IsNullOrEmpty(id))
            {
                exp = exp.And(p => p.Id == id);
            }
            var model = UnitWork.FirstOrDefault<ModelOrderRefundAddress>(exp);            
            return model;
        }
        
    }
}