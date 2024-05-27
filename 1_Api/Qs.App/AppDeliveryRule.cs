using System.Collections.Generic;
using System.Linq;
using Qs.App.Base;
using Qs.App.Interface;
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
    ///  App
    /// </summary>
    public class AppDeliveryRule : AppBaseString<ModelDeliveryRule, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppDeliveryRule(IUnitWork<QsDBContext> unitWork, IRepository<ModelDeliveryRule, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuDeliveryRule req)
        {           
            //var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            //var properties = _dbExtension.GetProperties("DeliveryRule");
            //result.ColumnHeaders = properties;
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ModelDeliveryRule> ListByWhere(ReqQuDeliveryRule req, bool isPage = false)
        {
            IQueryable<ModelDeliveryRule> linq = ListLinq(req);
            List<ModelDeliveryRule> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelDeliveryRule> ListLinq(ReqQuDeliveryRule req)
        {
            var linq = UnitWork.Find<ModelDeliveryRule>(p => true);
           
            
            return linq;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuDeliveryRule req)
        {
            var model = xConv.CopyMapper<ModelDeliveryRule, ReqAuDeliveryRule>(req);
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