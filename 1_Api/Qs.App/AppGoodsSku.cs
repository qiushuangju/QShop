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
    public class AppGoodsSku : AppBaseString<ModelGoodsSku, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppGoodsSku(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoodsSku, QsDBContext> repository,
            AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension,
            auth)
        {
            _revelanceApp = app;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuGoodsSku req)
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
        public List<ResGoodSku> ListByWhere(ReqQuGoodsSku req, bool isPage = false)
        {
            IQueryable<ModelGoodsSku> linq = ListLinq(req);
            List<ModelGoodsSku> list =
                isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            List<string> goodsIds = list.Select(p => p.GoodsId).ToList();
            var listGoods = UnitWork.Find<ModelGoods>(p => goodsIds.Contains(p.Id)).ToList();
            List<ResGoodSku> listSku = new List<ResGoodSku>();
            var listListImageId= list.Select(p => p.ImageId).ToList();
            var listFile = UnitWork.Find<ModelFileUpload>(p => listListImageId.Contains(p.Id)).ToList();
            foreach (var item in list)
            {
                var goods = listGoods.Find(p => p.Id == item.GoodsId);
                if (goods != null)
                {
                    ResGoodSku sku = ResGoodSku.ToView(item, goods, listFile);
                    listSku.Add(sku);
                }
            }
            return listSku.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelGoodsSku> ListLinq(ReqQuGoodsSku req)
        {
            var currentContext = _auth.GetCurrentContext();
            var linq = from sku in UnitWork.Find<ModelGoodsSku>(p => true)
                join goods in UnitWork.Find<ModelGoods>(p => true) on sku.GoodsId equals goods.Id
                select new
                {
                    sku = sku,
                    goods,
                };
            if (!string.IsNullOrEmpty(req.GoodsName))
            {
                linq = linq.Where(p => p.goods.GoodsName.Contains(req.Key));
            }
            
            if (req.ListSkuId!=null&& req.ListSkuId.Count>0)
            {
                linq = linq.Where(p => req.ListSkuId.Contains(p.sku.Id));
            }
            if (!string.IsNullOrEmpty(req.GoodsId))
            {
                linq = linq.Where(p => p.sku.GoodsId == req.GoodsId);
            }

            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.goods.GoodsName.Contains(req.Key));
            }
            if (req.OnlyStore)
            {
                var user = currentContext.User;
                linq = linq.Where(p => p.sku.StoreId == user.StoreId);
            }

            return linq.Select(p => p.sku);
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuGoodsSku req)
        {
            var model = xConv.CopyMapper<ModelGoodsSku, ReqAuGoodsSku>(req);
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
        /// Sku修改价格
        /// </summary>
        /// <param name="req"></param>
        public void ChangePrice(ReqListChangeSkuPrice req)
        {
            var listReq = req.List;
            var goodsMinPrice = listReq.Min(p => p.SalePrice);
            var goodsId = (listReq.FirstOrDefault() ?? new ReqChangeSkuPrice()).GoodsId;
            foreach (var item in listReq)
            {
                UnitWork.Update<ModelGoodsSku>(p => p.Id == item.Id, p => new ModelGoodsSku()
                {
                    SalePrice = item.SalePrice
                });
            }
            UnitWork.Update<ModelGoods>(p => p.Id == goodsId, p => new ModelGoods()
            {
                SalePrice = goodsMinPrice
            });

            UnitWork.Save();
        }
    }
}