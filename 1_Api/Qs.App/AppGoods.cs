using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using Qs.Repository.Domain;

namespace Qs.App
{
    /// <summary>
    /// 应用层
    /// </summary>
    public class AppGoods : AppBaseString<ModelGoods, QsDBContext>
    {
        private AppGoodsCate _appCate;
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppGoods(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoods, QsDBContext> repository, AppGoodsCate appCate  ,
             DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _appCate = appCate;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuGoods req)
        {           
            var currentContext = _auth.GetCurrentContext();
            var result = new TableData();
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)111
        /// </summary>
        public List<ResGoods> ListByWhere(ReqQuGoods req, bool isPage = false)
        {
            List<ResGoods> listGoodsVm = new List<ResGoods>();
            IQueryable<ModelGoods> linq = ListLinq(req);
            List<ModelGoods> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();

            //商品主图
            List<string> listImgIdMains = list.Select(p=>p.ImageIdMains).ToList();
            List<string> listFileId = new List<string>();
            foreach (var imgIds in listImgIdMains)
            {
                listFileId.AddRange(xConv.ToListString(imgIds));
            }
            List<ModelFileUpload> listImgMain = UnitWork.Find<ModelFileUpload>(p => listFileId.Contains(p.Id)).ToList();

            foreach (var item in list)
            {
                // List<ModelGoodsSku> skus = listSku.FindAll(p => p.GoodsId == item.Id);
                ResGoods vm = ResGoods.ToView(item, listImgMain, null,null);
                listGoodsVm.Add(vm);
            }
            return listGoodsVm.ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelGoods> ListLinq(ReqQuGoods req)
        {
            var currentContext = _auth.GetCurrentContext();
            string strOrderBy = ConvOrderBy(xConv.ToInt(req.SortType));
            var linq = UnitWork.Find<ModelGoods>(p => true, strOrderBy).Where(p => p.IsDelete == (int)xEnum.YesOrNo.No);
          
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.GoodsName.Contains(req.Key));
            }
            if (!string.IsNullOrEmpty(req.GoodsName))
            {
                linq = linq.Where(p => p.GoodsName.Contains(req.GoodsName));
            }
            if (!string.IsNullOrEmpty(req.GoodsIds))
            {
                string[] searchIds = req.GoodsIds.Split(',');
                linq = linq.Where(p => searchIds.Contains(p.Id));
            }
       
            if (!string.IsNullOrEmpty(req.CateIdAllLevel))
            {
                linq = linq.Where(p => p.CateId==req.CateIdAllLevel|| p.Cate2Id == req.CateIdAllLevel||p.Cate3Id==req.CateIdAllLevel);
            }
            if (xConv.ToInt(req.BigStatus)!=0)
            {
                if (req.BigStatus==10)
                {
                    linq = linq.Where(p => p.Status>=(int)xEnum.GoodsStatus.OnSale);
                }
            }
            if (req.Status != 0)
            {
                linq = linq.Where(p => p.Status.Equals(req.Status));
            }

            if (req.StockTotal != null)
            {
                linq = linq.Where(p => p.StockTotal.Equals(req.StockTotal));
            }
           
            var storeId = _auth.GetStoreId();
            if (!string.IsNullOrEmpty(storeId))
            {
                linq = linq.Where(p => p.StoreId == storeId);
            }

            if (req.IsRecommend!=null)
            {
                linq = linq.Where(p => p.IsRecommend == req.IsRecommend);
            }

            linq = req.SortType == null ? linq.OrderBy(p => p.SortNo).ThenByDescending(p => p.CreateTime) : linq;
            return linq;
        }

        /// <summary>
        /// 排序类型转为string
        /// </summary>
        /// <param name="sortType"></param>
        /// <returns></returns>
        public string ConvOrderBy(int sortType)
        {
            string strOrderBy = "";
            switch (sortType)
            { //(10:综合 20:价格降序 30:价格升序 60:销量降序 70:新品(时间降序))
                case 10:
                    strOrderBy = "";
                    break;
                case 20:
                    strOrderBy = "SalePrice Desc";
                    break;
                case 30:
                    strOrderBy = "SalePrice";
                    break;
                case 60:
                    strOrderBy = "SalesActual+SalesInitial Desc";
                    break;
                case 70:
                    strOrderBy = "CreateTime";
                    break;
                default:
                    strOrderBy = "";
                    break;
            }
            return strOrderBy;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResGoods Get(string id)
        {
            var model = Repository.FirstOrDefault(p=>p.Id==id);
           
            List<ModelGoodsSku> listSku = UnitWork.Find<ModelGoodsSku>(p => p.GoodsId == model.Id).ToList();
            List<ModelGoodsSpec> listSpec = UnitWork.Find<ModelGoodsSpec>(p => p.GoodsId == model.Id).ToList();
            List<ModelGoodsSpecValue> listSpecValue = UnitWork.Find<ModelGoodsSpecValue>(p => p.GoodsId == model.Id).ToList();
           
            List<string> listSkuImgIds = listSku.Select(p => p.ImageId).ToList();
            List<string> listMainImgIds = xConv.ToListString(model.ImageIdMains) ;
            List<string> listDetailImgIds = xConv.ToListString(model.ImageIdDetails);

            listSkuImgIds.AddRange(listMainImgIds);
            listSkuImgIds.AddRange(listDetailImgIds);   
            
            List<ModelFileUpload> listFile = UnitWork.Find<ModelFileUpload>(p => listSkuImgIds.Contains(p.Id)).ToList();  
            ResGoods vm = ResGoods.ToView(model, listFile, listSku,listSpec,listSpecValue);
            vm.CategoryId = string.IsNullOrEmpty(model.Cate3Id)
                ? (string.IsNullOrEmpty(model.Cate2Id) ? model.CateId : model.Cate2Id)
                : model.Cate3Id;
            
            return vm;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public List<ModelGoodsService> GetServiceList(string goodsId)
        {
            var model = Repository.FirstOrDefault(p => p.Id == goodsId)??new ModelGoods();                
            var list = UnitWork.Find<ModelGoodsService>(p => model.ServiceIds.Contains(p.Id)).ToList();

            return list;
        }


        /// <summary>
        /// 修改商品上下架状态
        /// </summary>
        /// <param name="req"></param>
        public void SetStatus(ReqSetStatus req)
        {
            var model = Repository.FirstOrDefault(p => p.Id == req.Id);
            model.Status = req.Status;
            Repository.Update(model);
        }  
        
        /// <summary>
        /// 修改商品上下架状态
        /// </summary>
        /// <param name="req"></param>
        public void SetRecommend(ReqSetRecommend req)
        {
            var model = Repository.FirstOrDefault(p => p.Id == req.Id);
            model.IsRecommend = req.IsRecommend;
            Repository.Update(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(string[] ids)
        {
            UnitWork.ExecuteWithTransaction(() => {
                foreach (var id in ids)
                {
                    UnitWork.Update<ModelGoods>(u => u.Id == id, u => new ModelGoods()
                    {
                        IsDelete=(int)xEnum.YesOrNo.Yes

                    });
                }
                UnitWork.Save();
            });
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuGoods req)
        {
            req.Check();
            var user = _auth.GetCurrentContext().User;  
            var model = xConv.CopyMapper<ModelGoods, ReqAuGoods>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                model.Id = xConv.NewGuid();
            }
            UnitWork.ExecuteWithTransaction(() =>
            {
                UnitWork.Delete<ModelGoodsSpec>(p => p.GoodsId == model.Id);
                UnitWork.Delete<ModelGoodsSpecValue>(p => p.GoodsId == model.Id);
                List<ModelGoodsSpec> listSpec = new List<ModelGoodsSpec>();
                List<ModelGoodsSpecValue> listSpecValue = new List<ModelGoodsSpecValue>();
                req.SpecData?.SpecList.ForEach(spec =>
                {
                    ModelGoodsSpec modelSpec = new ModelGoodsSpec();
                    modelSpec.SpecName = spec.SpecName;
                    modelSpec.Id = xConv.NewGuid();
                    modelSpec.GoodsId = model.Id;
                    modelSpec.SortNo = spec.Key;
                    listSpec.Add(modelSpec);
                    foreach (var value in spec.ValueList)
                    {
                        ModelGoodsSpecValue modelSpecValue = new ModelGoodsSpecValue();
                        modelSpecValue.Id = xConv.NewGuid();
                        modelSpecValue.GoodsId = model.Id;
                        modelSpecValue.GroupKey = value.GroupKey;
                        modelSpecValue.Key = value.Key;
                        modelSpecValue.SpecId = modelSpec.Id;
                        modelSpecValue.SpecValue = value.SpecValue;
                        listSpecValue.Add(modelSpecValue);
                    }
                });

                UnitWork.Delete<ModelGoodsSku>(p => p.GoodsId == model.Id);
                List<ModelGoodsSku> listSku = new List<ModelGoodsSku>();
                req.SpecData?.SkuList.ForEach(sku =>
                {
                    ModelGoodsSku modelSku = new ModelGoodsSku();
                    modelSku.GoodsId = model.Id;
                    modelSku.GoodsId = model.Id;
                    modelSku.ImageId = sku.ImageId;
                    modelSku.SalePrice = xConv.ToDecimal(sku.SalePrice);
                    modelSku.GoodsWeight = xConv.ToDecimal(sku.GoodsWeight);
                    modelSku.StockNum = sku.StockNum;
                    modelSku.LinePrice = xConv.ToDecimal(sku.LinePrice);
                    modelSku.Commission1 = xConv.ToDecimal(sku.Commission1);
                    modelSku.Commission2 = xConv.ToDecimal(sku.Commission2);
                    modelSku.Commission3 = xConv.ToDecimal(sku.Commission3);
                    modelSku.PurchasePrice = xConv.ToDecimal(sku.PurchasePrice);
                    modelSku.GoodsSkuNo = sku.GoodsSkuNo;
                    modelSku.SpecValueIds = "";
                    modelSku.SkuName = "";
                    modelSku.StoreId = user.StoreId;
                    modelSku.DeliveryId = req.DeliveryId;
                    if (!string.IsNullOrEmpty(sku.SpecValue0))
                    {
                        var specId = listSpec[0].Id;
                        ModelGoodsSpecValue specValue =
                            listSpecValue.FirstOrDefault(p => p.SpecId == specId && p.SpecValue == sku.SpecValue0);
                        modelSku.SpecValueIds += $"{specValue.Id},";
                        modelSku.SkuName += $"{specValue.SpecValue},";
                    }
                    if (!string.IsNullOrEmpty(sku.SpecValue1))
                    {
                        var specId = listSpec[1].Id;
                        ModelGoodsSpecValue specValue =
                            listSpecValue.FirstOrDefault(p => p.SpecId == specId && p.SpecValue == sku.SpecValue1);
                        modelSku.SpecValueIds += $"{specValue.Id},";
                        modelSku.SkuName += $"{specValue.SpecValue},";
                    }
                    if (!string.IsNullOrEmpty(sku.SpecValue2))
                    {
                        var specId = listSpec[2].Id;
                        ModelGoodsSpecValue specValue =
                            listSpecValue.FirstOrDefault(p => p.SpecId == specId && p.SpecValue == sku.SpecValue2);
                        modelSku.SpecValueIds += $"{specValue.Id},";
                        modelSku.SkuName += $"{specValue.SpecValue},";
                    }

                    modelSku.SpecValueIds = xConv.RemoveLastChar(modelSku.SpecValueIds);
                    modelSku.SkuName = xConv.RemoveLastChar(modelSku.SkuName);
                    listSku.Add(modelSku);
                });
                model.SalesInitial = xConv.ToInt(req.SalesInitial);
                model.ImageIdMains = xConv.AddSplit(req.ImagesIds, ",");
                model.ImageIdDetails = xConv.AddSplit(req.ListImgDetail, ",");
                model.LinePrice = listSku.Min(p => p.LinePrice);
                model.SalePrice = listSku.Min(p => p.SalePrice);
                model.PurchasePrice = listSku.Min(p => p.PurchasePrice);
                model.DeliveryId = req.DeliveryId;
                model.StoreId = user.StoreId;
                model.ServiceIds = xConv.ListToString(req.ListServiceId);
                var cate = _appCate.Get(req.CategoryId)?? new ResGoodsCate();    
                model.CateId = cate.CateId;
                model.Cate2Id = cate.Cate2Id;
                model.Cate3Id = cate.Cate3Id;
                UnitWork.AddOrUpdate(model);
                UnitWork.BatchAdd<ModelGoodsSpec>(listSpec);
                UnitWork.BatchAdd<ModelGoodsSpecValue>(listSpecValue);
                UnitWork.BatchAdd<ModelGoodsSku>(listSku);
                UnitWork.Save();
            });
        }

        /// <summary>
        /// 修改是否首页展示
        /// </summary>
        public void JobSoldOutGoods()
        {
            //DateTime dtNow = DateTime.Now;
            //var list = UnitWork.Find<ModelGoods>(p => p.StopTime <= dtNow);
            //foreach (var item in list)
            //{
            //    item.Status = (int)xEnum.GoodsStatus.NotOn;
            //    UnitWork.AddOrUpdate(item);
            //}
            //UnitWork.Save();
        }

        ///// <summary>
        ///// 修改是否首页展示
        ///// </summary>
        //public void JobSoldOutGoods()
        //{
        //    DateTime dtNow=DateTime.Now;
        //    UnitWork.Update<ModelGoods>(p => p.StopTime <= dtNow, u => new ModelGoods()
        //    {
        //        Status = (int)xEnum.GoodsStatus.NotOn
        //    });
        //    UnitWork.Save();
        //}
    }
}