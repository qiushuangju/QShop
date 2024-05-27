using System;
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
    /// 商品服务承诺 App
    /// </summary>
    public class AppGoodsService : AppBaseString<ModelGoodsService, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppGoodsService(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoodsService, QsDBContext> repository,
            AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension,
            auth)
        {
            _revelanceApp = app;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuGoodsService req)
        {
            //var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            //var properties = _dbExtension.GetProperties("GoodsService");
            //result.ColumnHeaders = properties;
            result.Result = ListByWhere(req, true);
            result.Count = ListLinq(req).Count();
            return result;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ModelGoodsService> ListByWhere(ReqQuGoodsService req, bool isPage = false)
        {
            IQueryable<ModelGoodsService> linq = ListLinq(req);
            List<ModelGoodsService> list =
                isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelGoodsService> ListLinq(ReqQuGoodsService req)
        {
            var linq = UnitWork.Find<ModelGoodsService>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.ServiceName.Contains(req.Key));
            }

            if (req.OnlyStore)
            {
                var storeId = _auth.GetStoreId();
                linq = linq.Where(p => p.StoreId == storeId);
            }

            return linq;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuGoodsService req)
        {
            var model = xConv.CopyMapper<ModelGoodsService, ReqAuGoodsService>(req);
            var user = _auth.GetCurrentContext().User;
            model.StoreId = user.StoreId;
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

        /// <summary>
        /// 新增或修改
        /// </summary>
        public  void DeleteModel(string[] ids)
        {
            foreach (var id in ids)
            {
                var countRef = UnitWork.Count<ModelGoods>(p => p.ServiceIds.Contains(id));
                if (countRef > 0)
                {
                    throw new Exception($"该记录被{countRef}个商品引用，不允许删除");
                }

                UnitWork.Delete<ModelGoodsService>(p => p.Id == id);
            }

            UnitWork.Save();
        }
    }
}