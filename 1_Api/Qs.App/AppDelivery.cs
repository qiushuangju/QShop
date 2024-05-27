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
    public class AppDelivery : AppBaseString<ModelDelivery, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppDelivery(IUnitWork<QsDBContext> unitWork, IRepository<ModelDelivery, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuDelivery req)
        {           
            //var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            //var properties = _dbExtension.GetProperties("Delivery");
            //result.ColumnHeaders = properties;
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResDelivery> ListByWhere(ReqQuDelivery req, bool isPage = false)
        {
            IQueryable<ModelDelivery> linq = ListLinq(req);
            List<ModelDelivery> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            List<ResDelivery> listVm = new List<ResDelivery>();
            foreach (var item in list)
            {
                listVm.Add(ResDelivery.ToView(item));
            }
            return listVm;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelDelivery> ListLinq(ReqQuDelivery req)
        {
            var currentContent = _auth.GetCurrentContext();
            var linq = UnitWork.Find<ModelDelivery>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Name.Contains(req.Key));
            }

            if (req.OnlyStore)
            {
                var user = currentContent.User;
                linq = linq.Where(p => p.StoreId== user.StoreId);
            }
            return linq;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuDelivery req)
        {
            var model = xConv.CopyMapper<ModelDelivery, ReqAuDelivery>(req);
            model.StoreId = _auth.GetCurrentContext().User.StoreId;
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                model.Id = xConv.NewGuid();
                UnitWork.Add(model);
            }
            else
            {
                UnitWork.Update(model);
            }

            foreach (ReqDeliveryRule item in req.ListRule)
            {

                ModelDeliveryRule rule = xConv.CopyMapper<ModelDeliveryRule, ReqDeliveryRule>(item);
                rule.DeliveryId = model.Id;
                rule.Region = xConv.ListToString(item.Region);
                rule.RegionText = xConv.ToJson(item.RegionText);
                rule.StoreId = model.StoreId;
                UnitWork.AddOrUpdate(rule);
            }
            UnitWork.Save();
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        
        public ResDelivery GetDetail(string id)
        {
            var modelDb= Repository.FirstOrDefault(u => u.Id == id);
            ResDelivery res=ResDelivery.ToView(modelDb);
            res.ListRule = new List<ReqDeliveryRule>();
            var listRule = UnitWork.Find<ModelDeliveryRule>(p => p.DeliveryId == id).ToList();
            foreach (var item in listRule)
            {

                ReqDeliveryRule resRule = xConv.CopyMapper<ReqDeliveryRule, ModelDeliveryRule>(item);
                resRule.Region = xConv.ToListString(item.Region);
                resRule.RegionText = xConv.JsonToObj<List<ProvinceDelivery>>(item.RegionText);
                res.ListRule.Add(resRule);
            }
            return res;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        public new void Delete( string[] ids)
        {
            UnitWork.Delete<ModelDelivery>(p=>ids.Contains(p.Id));
            UnitWork.Delete<ModelDeliveryRule>(p => ids.Contains(p.DeliveryId));
            UnitWork.Save();
        }

    }
}