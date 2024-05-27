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
    public class AppGoodsSpecValue : AppBaseString<ModelGoodsSpecValue, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppGoodsSpecValue(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoodsSpecValue, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuGoodsSpecValue req)
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
        public List<ModelGoodsSpecValue> ListByWhere(ReqQuGoodsSpecValue req, bool isPage = false)
        {
            IQueryable<ModelGoodsSpecValue> linq = ListLinq(req);
            List<ModelGoodsSpecValue> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelGoodsSpecValue> ListLinq(ReqQuGoodsSpecValue req)
        {
            var linq = UnitWork.Find<ModelGoodsSpecValue>(p => true);
           
            return linq;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuGoodsSpecValue req)
        {
            var model = xConv.CopyMapper<ModelGoodsSpecValue, ReqAuGoodsSpecValue>(req);
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