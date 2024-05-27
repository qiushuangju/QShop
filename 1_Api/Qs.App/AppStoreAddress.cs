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
    /// 店铺地址 App
    /// </summary>
    public class AppStoreAddress : AppBaseString<ModelStoreAddress, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppStoreAddress(IUnitWork<QsDBContext> unitWork, IRepository<ModelStoreAddress, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuStoreAddress req)
        {           
            //var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            //var properties = _dbExtension.GetProperties("StoreAddress");
            //result.ColumnHeaders = properties;
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResStoreAddress> ListByWhere(ReqQuStoreAddress req, bool isPage = false)
        {
            IQueryable<ModelStoreAddress> linq = ListLinq(req);
            List<ModelStoreAddress> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            List<ResStoreAddress> listVm = new List<ResStoreAddress>();
            foreach (var item in list)
            {
                listVm.Add(ResStoreAddress.ToView(item));
            }
            return listVm;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelStoreAddress> ListLinq(ReqQuStoreAddress req)
        {
            var linq = UnitWork.Find<ModelStoreAddress>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Name.Contains(req.Key));
            }
            if ((req.Type!=null))
            {
                linq = linq.Where(p => p.Type== req.Type);
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
        public void AddOrUpdate(ReqAuStoreAddress req)
        {
            var model = xConv.CopyMapper<ModelStoreAddress, ReqAuStoreAddress>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            var user = _auth.GetCurrentContext().User;

            model.StoreId = user.StoreId;
            model.ProvinceId = req.ListRegion[0];
            model.CityId = req.ListRegion[1];
            model.RegionId = req.ListRegion[2];
            var areaRegion = UnitWork.FirstOrDefault<ModelSysArea>(p => model.RegionId==(p.Id));
            model.Province = areaRegion.Province;
            model.City = areaRegion.City;
            model.Region = areaRegion.District;
            model.FullAddress = $"{model.Province}{model.City}{model.Region},{model.Detail}";
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