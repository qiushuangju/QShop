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
    public class AppGoodsSpec : AppBaseString<ModelGoodsSpec, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppGoodsSpec(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoodsSpec, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuGoodsSpec req)
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
        public List<ModelGoodsSpec> ListByWhere(ReqQuGoodsSpec req, bool isPage = false)
        {
            IQueryable<ModelGoodsSpec> linq = ListLinq(req);
            List<ModelGoodsSpec> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelGoodsSpec> ListLinq(ReqQuGoodsSpec req)
        {
            var linq = UnitWork.Find<ModelGoodsSpec>(p => true);
           
            return linq;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuGoodsSpec req)
        {
            var model = xConv.CopyMapper<ModelGoodsSpec, ReqAuGoodsSpec>(req);
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