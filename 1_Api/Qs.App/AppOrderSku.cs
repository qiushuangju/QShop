using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
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
    /// 应用层
    /// </summary>
    public class AppOrderSku : AppBaseString<ModelOrderSku, QsDBContext>
    {
     
        private AppCategory _appDic;
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppOrderSku(IUnitWork<QsDBContext> unitWork, IRepository<ModelOrderSku, QsDBContext> repository,
            AppCategory appDic ,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            
            _appDic = appDic;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuOrderGoods req)
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
        public List<ResOrderSku> ListByWhere(ReqQuOrderGoods req, bool isPage = false)
        {
            IQueryable<ModelOrderSku> linq = ListLinq(req);
            List<ModelOrderSku> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            var listFileId = list.Select(p => p.SkuImageId).ToList();
            var listFile = UnitWork.Find<ModelFileUpload>(p => listFileId.Contains(p.Id)).ToList();                                        
            List<ResOrderSku> listRes = ConvVm(list, listFile);
            return listRes;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelOrderSku> ListLinq(ReqQuOrderGoods req)
        {

            var userContent = _auth.GetCurrentContext();
           
            var linq = UnitWork.Find<ModelOrderSku>(p=>true); 
            linq = linq.Where(p => p.OrderStatus >= (int)xEnum.OrderStatus.NotPaid);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.SubOrderNo.Contains(req.Key));
            }
            if (xConv.ToInt(req.OrderStatus) != 0)
            {
                linq = linq.Where(p => p.OrderStatus.Equals(req.OrderStatus));
            }

            if (req.IsProgeny) //查询后裔
            {
                linq = linq.Where(p => p.SourceUserId.Contains(userContent.User.SourceUserId));
            }

            var storeId = _auth.GetStoreId();
            if (!string.IsNullOrEmpty(storeId))
            {
                linq = linq.Where(p => p.StoreId.Equals(storeId));
            }

            if (xConv.ToInt(req.MinOrderStatus)!=0)
            {
                linq = linq.Where(p => p.OrderStatus >= req.MinOrderStatus); 
            }
            //创建时间
            if (req.StartTime != null)
            {
                linq = linq.Where(p => p.PayTime >= req.StartTime);
            }
            if (req.EndTime != null)
            {
                linq = linq.Where(p => p.PayTime <= req.EndTime);
            }

            if (req.ListOrderId.Count > 0)
            {
                linq = linq.Where(p => req.ListOrderId.Contains(p.OrderId));
            }
            if (req.ListOrderGoodsId.Count() > 0)
            {
                linq = linq.Where(p => req.ListOrderGoodsId.Contains(p.Id));
            }
            return linq.OrderByDescending(p=>p.CreateTime);
        }

       

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuOrderSku req)
        {
            var model = xConv.CopyMapper<ModelOrderSku, ReqAuOrderSku>(req);
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
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResOrderSku GetDetail(string id)
        {
            var orderSku = UnitWork.FirstOrDefault<ModelOrderSku>(p => p.Id == id);
            var order = UnitWork.FirstOrDefault<ModelOrder>(p => p.Id == orderSku.OrderId);
          
           var refund= UnitWork.FirstOrDefault<ModelOrderRefundSku>(p => p.OrderSkuId== orderSku.Id);
            List<ModelFileUpload> listFile = UnitWork.Find<ModelFileUpload>(p => p.Id == orderSku.SkuImageId).ToList();
            ResOrderSku res = ResOrderSku.ToView(orderSku,  refund, listFile);
            var listRefundSku = UnitWork.Find<ModelOrderRefundSku>(p => p.OrderSkuId == id && p.Status >= (int)xEnum.RefundStatus.WaitAudit).ToList();
            res.QuantityRefund = xConv.ToInt(listRefundSku.Sum(p => p.RefundQuantity));
            res.OrderStatus = xConv.ToInt(orderSku.OrderStatus);
            res.IsHaveRefund = UnitWork.Count<ModelOrderRefundSku>(p => p.OrderId == order.Id) > 0;
            return res;
        }

        
        /// <summary>
        /// List转化为Vm
        /// </summary>
        /// <param name="listOrderSku"></param>
        /// <returns></returns>
        public List<ResOrderSku> ConvVm(List<ModelOrderSku> listOrderSku ,List<ModelFileUpload> listFile)
        {
            decimal rateCommission = xConv.ToDecimal(_appDic.GetDicByCode("GoodsRateCommission").DtValue);
            //订单id
            List<string> listOrderSkuId = listOrderSku.Select(p => p.Id).ToList();
            List<ModelOrderRefundSku> listRefund =
                UnitWork.Find<ModelOrderRefundSku>(p => listOrderSkuId.Contains(p.OrderSkuId) && p.Status >= (int)xEnum.RefundStatus.WaitAudit).ToList();
           
            List<ResOrderSku> listOrderVm = new List<ResOrderSku>();
            foreach (var orderSku in listOrderSku)
            {
            
                 var itemRefundSku = listRefund.Where(p => p.OrderSkuId == orderSku.Id);
                ModelOrderRefundSku refund = listRefund.FirstOrDefault(p => p.OrderSkuId == orderSku.Id) ??
                                             new ModelOrderRefundSku();

                ResOrderSku vm = ResOrderSku.ToView(orderSku,  refund, listFile);
                vm.IsHaveRefund = listRefund.Count(p => p.OrderSkuId == orderSku.Id) > 0;
                vm.QuantityRefund = xConv.ToInt(itemRefundSku.Sum(p => p.RefundQuantity));
                vm.AmountCommission = orderSku.SalePrice * rateCommission;
                vm.TotalAmountRefund = xConv.ToDecimal(vm.QuantityRefund * vm.PayPrice * rateCommission);
               
                listOrderVm.Add(vm);
            }

            return listOrderVm;
        }

        /// <summary>
        ///取消订单
        /// </summary>
        /// <param name="req"></param>
        public void CanelOrder(ReqCancelOrderSku req)
        {
            req.Check();
            var userId = _auth.GetCurrentContext().User.Id;
            Repository.Delete(p => p.Id == req.Id&&p.UserId== userId);
        }
    }
}