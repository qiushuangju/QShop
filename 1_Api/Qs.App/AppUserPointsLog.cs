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
    /// 用户积分变动明细 App
    /// </summary>
    public class AppUserPointsLog : AppBaseString<ModelUserPointsLog, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppUserPointsLog(IUnitWork<QsDBContext> unitWork, IRepository<ModelUserPointsLog, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuUserPointsLog req)
        {           
            //var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            //var properties = _dbExtension.GetProperties("UserPointsLog");
            //result.ColumnHeaders = properties;
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ModelUserPointsLog> ListByWhere(ReqQuUserPointsLog req, bool isPage = false)
        {
            IQueryable<ModelUserPointsLog> linq = ListLinq(req);
            List<ModelUserPointsLog> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelUserPointsLog> ListLinq(ReqQuUserPointsLog req)
        {
            var linq = UnitWork.Find<ModelUserPointsLog>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.OrderId.Contains(req.Key));
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
        public void AddOrUpdate(ReqAuUserPointsLog req)
        {
            var model = xConv.CopyMapper<ModelUserPointsLog, ReqAuUserPointsLog>(req);
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
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ModelUserPointsLog GetDetail(string id)
        {
            var storeId = _auth.GetStoreId();           
            Expression<Func<ModelUserPointsLog, bool>> exp = LambdaExtensions.True<ModelUserPointsLog>();
            if (!string.IsNullOrEmpty(storeId))
            {
                exp = exp.And(p => p.StoreId == storeId);
            }
            if (!string.IsNullOrEmpty(id))
            {
                exp = exp.And(p => p.Id == id);
            }
            var model = UnitWork.FirstOrDefault<ModelUserPointsLog>(exp);            
            return model;
        }
        
    }
}