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
    public class AppSysArea : AppBaseString<ModelSysArea, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppSysArea(IUnitWork<QsDBContext> unitWork, IRepository<ModelSysArea, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuSysArea req)
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
        public List<ModelSysArea> ListByWhere(ReqQuSysArea req, bool isPage = false)
        {
            IQueryable<ModelSysArea> linq = ListLinq(req);
            List<ModelSysArea> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelSysArea> ListLinq(ReqQuSysArea req)
        {
            var linq = UnitWork.Find<ModelSysArea>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.FullName.Contains(req.Key));
            }
            if (xConv.ToInt(req.MaxLevel) !=0)
            {
                linq = linq.Where(p => p.Level <= xConv.ToInt(req.MaxLevel));
            }
            if (!string.IsNullOrEmpty(req.ParentId))
            {
                linq = linq.Where(p => p.ParentId==req.ParentId);
            }
            return linq.OrderBy(p=>p.Id);
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuSysArea req)
        {
            var model = xConv.CopyMapper<ModelSysArea, ReqAuSysArea>(req);
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