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
    public class AppUserAddress : AppBaseString<ModelUserAddress, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppUserAddress(IUnitWork<QsDBContext> unitWork, IRepository<ModelUserAddress, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuUserAddress req)
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
        public List<ResUserAddress> ListByWhere(ReqQuUserAddress req, bool isPage = false)
        {
            List<ResUserAddress> listVm = new List<ResUserAddress>();
            IQueryable<ModelUserAddress> linq = ListLinq(req);
            List<ModelUserAddress> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            List<string> listAddressId = list.Select(p => p.Id).ToList();
            var listAddress =
                (from address in UnitWork.Find<ModelOrderAddress>(p => listAddressId.Contains(p.AddressId))
                    join order in UnitWork.Find<ModelOrder>(p => p.OrderStatus  >=(int)xEnum.OrderStatus.NotPaid)
                        on address.OrderId equals order.Id
                    select new
                    {
                        order.Id,
                        address.AddressId,

                    }).ToList();

            // on address.OrderId equals order.id      
            foreach (var item in list)
            {
                int countOrder = listAddress.Count(p => p.AddressId == item.Id);
                listVm.Add(ResUserAddress.ToView(item));
            }
            
            return listVm.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelUserAddress> ListLinq(ReqQuUserAddress req)
        {
            var linq =  UnitWork.Find<ModelUserAddress>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Name.Contains(req.Key));
            }

            if (xConv.ToInt(req.IsDefault) >=0)
            {
                linq = linq.Where(p => p.IsDefault ==xConv.ToBool(req.IsDefault));
            }
            if (req.OnlyMy)
            {
                var user = _auth.GetCurrentContext().User;
                linq = linq.Where(p => p.UserId == user.Id);
            }
        
            return linq;
        }

        

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuUserAddress req)
        {
            var user= _auth.GetCurrentContext().User;
            req.Check();
            var modelDb = Repository.FirstOrDefault(p => p.Id == req.Id);
            var model = xConv.CopyMapper<ModelUserAddress, ReqAuUserAddress>(req);
            var storeId = _auth.GetStoreId();
            var listRegion = req.ListRegion;
            model.ProvinceId = listRegion[0].Value;
            model.Province = listRegion[0].Label;
            model.CityId = listRegion[1].Value;
            model.City = listRegion[1].Label;
            model.RegionId = listRegion[2].Value;
            model.Region = listRegion[2].Label;
            model.FullAddress = $"{model.Province}{model.City}{model.Region }{req.Detail}";
            model.StoreId = storeId;
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                int countDefault = Repository.Count(p => p.UserId == user.Id && p.IsDefault == true);
                model.UserId = user.Id;
                model.IsDefault = countDefault > 0 ? false : true;
                Repository.Add(model);
            }
            else
            {
                model.UserId = modelDb.UserId;
                Repository.Update(model);
            }
        }
            /// <summary>
            /// 设置默认
            /// </summary>
            /// <param name="id"></param>
        public void SetDefault(string id)
        {
            var user = _auth.GetCurrentContext().User;
            Repository.Update(p=>p.UserId== user.Id,p=>new ModelUserAddress()
            {
                IsDefault = false
            });

            Repository.Update(p => p.Id == id, p => new ModelUserAddress()
            {
                IsDefault = true
            });
        }
        /// <summary>
        /// 获取地址详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResUserAddress GetDetail(string id)
        {
            var result = new Response<ModelCoupon>();
           var model= Repository.FirstOrDefault(p => p.Id == id);
           ResUserAddress res=ResUserAddress.ToView(model);
            return res;
        }
        

    }
}