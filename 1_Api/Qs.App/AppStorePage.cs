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
using Qs.Repository.Vm;

namespace Qs.App
{
    /// <summary>
    /// 应用层
    /// </summary>
    public class AppStorePage : AppBaseString<ModelStorePage, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppStorePage(IUnitWork<QsDBContext> unitWork, IRepository<ModelStorePage, QsDBContext> repository,
            AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension,
            auth)
        {
            _revelanceApp = app;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuStorePage req)
        {
            //var loginContext = _auth.GetCurrentUser();
            var result = new TableData();
            result.Result = ListByWhere(req, true);
            result.Count = ListLinq(req).Count();
            return result;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResStorePage> ListByWhere(ReqQuStorePage req, bool isPage = false)
        {
            IQueryable<ModelStorePage> linq = ListLinq(req);
            List<ModelStorePage> list =
                isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();

            List<ResStorePage> listVm = new List<ResStorePage>();
            foreach (var item in list)
            {
                listVm.Add(ResStorePage.ToView(item));
            }
            return listVm.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelStorePage> ListLinq(ReqQuStorePage req)
        {
            var linq = UnitWork.Find<ModelStorePage>(p => true);
            var storeId = _auth.GetStoreId();
            if (!string.IsNullOrEmpty(storeId))
            {
                linq = linq.Where(p => p.StoreId == storeId);
            }
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.PageName.Contains(req.Key));
            }

            return linq;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuStorePage req)
        {
            var storeId = _auth.GetCurrentContext().User.StoreId;
            var pageDb = Repository.FirstOrDefault(p => p.Id == req.Id);
            var model = xConv.CopyMapper<ModelStorePage, ReqAuStorePage>(req);
            var items = xConv.JsonToObj<dynamic>(req.Items.ToString());
            var page = xConv.JsonToObj<VmPage>(req.Page.ToString());
            var pageData = new
            {
                page = page,
                items = items
            };
            model.PageData = xConv.ToJson(pageData);
            model.PageName = page.paramsData.name;
            model.StoreId = storeId;
            model.PageType = pageDb.PageType;
            model.CreateTime = pageDb.CreateTime;
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                model.PageType = (int)xEnum.PageType.Custom;
                Repository.Add(model);
            }
            else
            {
                Repository.Update(model);
            }
        }

        /// <summary>
        /// 设为首页
        /// </summary>
        public void SetHome(ReqSetHomePage req)
        {
            var storeId = _auth.GetCurrentContext().User.StoreId;
            var page = Repository.FirstOrDefault(p => p.Id == req.Id);
            UnitWork.Update<ModelStorePage>(p => p.StoreId == storeId && p.PageType == (int)xEnum.PageType.HomePage, u => new ModelStorePage()
            {
                PageType = (int)xEnum.PageType.Custom
            });
            page.PageType = (int)xEnum.PageType.HomePage;
            UnitWork.Update(page);
            UnitWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageType"></param>
        /// <returns></returns>
        public ResStorePage GetTypeDetail(int pageType)
        {
            var storeId = _auth.GetStoreId();  
            Expression<Func<ModelStorePage, bool>> where = p => p.PageType == pageType;
            if (!string.IsNullOrEmpty(storeId))
            {
                where = where.And(p => p.StoreId == storeId);
            }
            var model = UnitWork.FirstOrDefault<ModelStorePage>(where);
            ResStorePage res = ResStorePage.ToView(model);
            return res;
        }
    }
}