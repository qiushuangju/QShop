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
    public class AppWpsNotify : AppBaseString<ModelWpsNotify, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppWpsNotify(IUnitWork<QsDBContext> unitWork, IRepository<ModelWpsNotify, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuWpsNotify req)
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
        public List<ModelWpsNotify> ListByWhere(ReqQuWpsNotify req, bool isPage = false)
        {
            IQueryable<ModelWpsNotify> linq = ListLinq(req);
            List<ModelWpsNotify> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelWpsNotify> ListLinq(ReqQuWpsNotify req)
        {
            var linq = UnitWork.Find<ModelWpsNotify>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Id.Contains(req.Key));
            }
           
            return linq;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuWpsNotify req)
        {
            var model = xConv.CopyMapper<ModelWpsNotify, ReqAuWpsNotify>(req);
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