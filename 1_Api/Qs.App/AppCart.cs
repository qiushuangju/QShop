using System;
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
    public class AppCart : AppBaseString<ModelCart, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        private AppGoods _appGoods;
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppCart(IUnitWork<QsDBContext> unitWork, IRepository<ModelCart, QsDBContext> repository,
            AppRevelanceManager app, AppGoods appGoods, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
            _appGoods = appGoods;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuCart req)
        {
            //var loginContext = _auth.GetCurrentUser();
            var result = new TableData();
        
            result.Result = ListByWhere(req, true);
            result.Count = ListLinq(req).Count();
            return result;
        }

        /// <summary>
        /// 列表与合计
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResListTotalCart ListTotalByWhere( ReqQuCart req)
        {
            ResListTotalCart res = new ResListTotalCart();
            var list = ListByWhere(req);
            res.List = list;
            res.SumNum = list.Sum(p => p.GoodsNum);
            return res;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResCart> ListByWhere(ReqQuCart req, bool isPage = false)
        {
            IQueryable<ResCart> linq = ListLinq(req);
            List<ResCart> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();

            var listFileId = list.Select(p => p.ImageId).ToList();
            var listFile = UnitWork.Find<ModelFileUpload>(p => listFileId.Contains(p.Id)).ToList();
            List<ResCart> listVm=new List<ResCart>();
            foreach (var item in list)
            {
                listVm.Add(ResCart.ToView(listFile,item));
            }
            return listVm;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ResCart> ListLinq(ReqQuCart req)
        {
            var user = _auth.GetCurrentContext().User;
            var linq = from cart in UnitWork.Find<ModelCart>(p => true)
                       join goods in UnitWork.Find<ModelGoods>(p => true) on cart.GoodsId equals goods.Id
                       join sku in UnitWork.Find<ModelGoodsSku>(p => true) on cart.GoodsSkuId equals sku.Id
                       select new ResCart
                       {
                           Id = cart.Id,
                           GoodsId = cart.GoodsId,
                           GoodsName = goods.GoodsName,
                           SkuName = sku.SkuName,
                           GoodsSkuId = cart.GoodsSkuId,
                           GoodsNum = cart.GoodsNum,
                           SalePrice=cart.SalePrice,
                           UserId = cart.UserId,
                           StoreId = cart.StoreId,
                           CreateTime = cart.CreateTime,
                           ImageId = cart.ImageId,
                       };
            if (req.OnlyMy)
            {
                linq = linq.Where(p => p.UserId == user.Id);
            }
            return linq.OrderByDescending(p => p.CreateTime);
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuCart req)
        {
            var model = xConv.CopyMapper<ModelCart, ReqAuCart>(req);
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
        /// 新增购物车
        /// </summary>
        /// <param name="req"></param>
        public ResAddOrUpdateCart AddCart(ReqAddCart req)
        {
           return AddOrUpdateCart(req);
        }

        /// <summary>
        /// 修改购物车数量
        /// </summary>
        /// <param name="req"></param>
        public ResAddOrUpdateCart AddOrUpdateCart(ReqAddCart req)
        {
            ResAddOrUpdateCart res = new ResAddOrUpdateCart();
            var user = _auth.GetCurrentContext().User;
            var storeId = _auth.GetStoreId();
            ModelCart cartDb = UnitWork.FirstOrDefault<ModelCart>(p => (p.GoodsSkuId == req.GoodsSkuId||p.Id==req.GoodsSkuId) && p.UserId == user.Id);
            if (cartDb == null)
            {
                ModelGoodsSku sku = UnitWork.FirstOrDefault<ModelGoodsSku>(p => p.Id == req.GoodsSkuId);
                cartDb = new ModelCart();
                cartDb.GoodsId = sku.GoodsId;
                cartDb.GoodsSkuId = req.GoodsSkuId;
                cartDb.SkuName = sku.SkuName;
                cartDb.SpecValueIds = sku.SpecValueIds;
                cartDb.UserId = _auth.GetCurrentContext().User.Id;
                cartDb.SalePrice = sku.SalePrice;
                cartDb.ImageId = sku.ImageId;
                cartDb.StoreId = storeId;
                cartDb.GoodsNum = req.GoodsNum;
            }
            else
            {
                cartDb.GoodsNum = req.GoodsNum;
            }
            UnitWork.AddOrUpdate(cartDb);
            UnitWork.Save();
            res.CartTotal = UnitWork.Sum<ModelCart>(p => p.UserId == user.Id,p=>p.GoodsNum);
            return res;
        }
    }
}