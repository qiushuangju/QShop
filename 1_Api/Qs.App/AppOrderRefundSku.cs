using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NPOI.SS.Formula.Functions;

using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.UserManager;
using Qs.App.Wx;
using Qs.Comm;
using Qs.Comm.Extensions;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Vm;

using TencentCloud.Iot.V20180123.Models;

using static Qs.Comm.xEnum;

namespace Qs.App
{
    /// <summary>
    ///  App
    /// </summary>
    public class AppOrderRefundSku : AppBaseString<ModelOrderRefundSku, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        private AppOrderTrack _appOrderTrack;
        private AppUserManager _appUser;
        private AppStoreSetting _appStoreSetting;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppOrderRefundSku(IUnitWork<QsDBContext> unitWork, IRepository<ModelOrderRefundSku, QsDBContext> repository,
            AppRevelanceManager app, AppUserManager appUser, AppStoreSetting appStoreSetting, AppOrderTrack appOrderTrack,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
            _appUser= appUser;
            _appStoreSetting= appStoreSetting;
            _appOrderTrack = appOrderTrack;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuOrderRefundSku req)
        {           
            //var loginContext = _auth.GetCurrentUser();
            var result = new TableData();
            int rowCount = 0;
            result.Result = ListByWhere(req,ref rowCount,true);
            result.Count = ListLinq(req).Count();
            return result;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="rowCount"></param>
        /// <param name="isPage"></param>
        /// <returns></returns>
        public List<ResOrderRefundSku> ListByWhere(ReqQuOrderRefundSku req, ref int rowCount, bool isPage = false)
        {
            var userCurrent = _auth.GetCurrentContext();

            Expression<Func<ModelOrderRefundSku, bool>> whereExp = p=>true;
            if (xConv.ToInt(req.BigStatus) ==10) //待处理
            {
                //whereExp = whereExp.And(p => p.Status ==(int)xEnum.RefundStatus.Approve);

                whereExp = whereExp.And(p => p.Status >= (int)xEnum.RefundStatus.WaitAudit
                && p.Status <= (int)xEnum.RefundStatus.Shipped);
            }
            var linq = from refundSku in UnitWork.Find<ModelOrderRefundSku>(whereExp)
                       join orderSku in UnitWork.Find<ModelOrderSku>(p => true) on refundSku.OrderSkuId equals orderSku.Id
                       join user in UnitWork.Find<ModelUser>(p => true) on refundSku.UserId equals user.Id
                       join order in UnitWork.Find<ModelOrder>(p => true) on refundSku.OrderId equals order.Id
                       select new
                       {
                           RefundSku = refundSku,
                           Quantity = orderSku.Quantity,
                           UserName = user.NickName,
                           UserPhone = user.Phone,
                       };
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.UserPhone.Contains(req.Key) || p.RefundSku.OrderNo.Contains(req.Key) || p.RefundSku.RefundNo.Contains(req.Key));
            }
            if (!string.IsNullOrEmpty(req.OrderSkuId))
            {
                linq = linq.Where(p => p.RefundSku.OrderSkuId == req.OrderSkuId);
            }
            if (xConv.ToBool(req.OnlyMy))
            {
                linq = linq.Where(p => p.RefundSku.UserId == userCurrent.User.Id);
            }
            dynamic list;
            rowCount = linq.Count();
            if (isPage)
            {
                list = linq.OrderByDescending(p => p.RefundSku.CreateTime)
                    .Skip((req.Page - 1) * req.Limit)
                    .Take(req.Limit).ToList();
            }
            else
            {
                list = linq.ToList();
            }

            List<ResOrderRefundSku> listRes = new List<ResOrderRefundSku>();

            List<string> listFileId = new List<string>();
            foreach (var item in list)
            {
                listFileId.AddRange(xConv.ToListString(item.RefundSku.RefundProof));
                listFileId.Add(item.RefundSku.SkuImageId); 
            }
            var listFile = UnitWork.Find<ModelFileUpload>(p => listFileId.Contains(p.Id)).ToList();
            foreach (var item in list)
            {
                ResOrderRefundSku res = ResOrderRefundSku.ToView(item.RefundSku, listFile);
                res.Quantity = item.Quantity;
                res.UserName = item.UserName;
                res.UserPhone = item.UserPhone;
               
                listRes.Add(res);
            }
            return listRes;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelOrderRefundSku> ListLinq(ReqQuOrderRefundSku req)
        {
            var linq = UnitWork.Find<ModelOrderRefundSku>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.OrderNo.Contains(req.Key));
            }

            if (!string.IsNullOrEmpty(req.OrderSkuId))
            {
                linq = linq.Where(p => p.OrderSkuId==req.OrderSkuId); 
            }
           
            if (req.OnlyMy)
            {
                var user = _auth.GetCurrentContext().User;
                linq = linq.Where(p => p.UserId == user.Id);
            }
            return linq;
        }
        
        /// <summary>
        /// 获取退款订单详情
        /// </summary>
        /// <param name="orderRefundSkuId"></param>
        /// <returns></returns>
        public ResOrderRefundSku GetRefundOrderDetail(string orderRefundSkuId)
        {
            var refundSku = UnitWork.FirstOrDefault<ModelOrderRefundSku>(p => p.Id == orderRefundSkuId);
            var sku = UnitWork.FirstOrDefault<ModelOrderSku>(p => p.Id == refundSku.OrderSkuId);
            List<string> listFileId = xConv.ToListString(refundSku.RefundProof);
            listFileId.Add(refundSku.SkuImageId);
            var listFile = UnitWork.Find<ModelFileUpload>(p => listFileId.Contains(p.Id)).ToList();
           var vmSku=  ResOrderRefundSku.ToView(refundSku, listFile);
          
            vmSku.Quantity = sku.Quantity;

            ModelOrderRefundAddress refundAddress = UnitWork.FirstOrDefault<ModelOrderRefundAddress>(p => p.OrderRefundId == refundSku.Id);
            var list = UnitWork.Find<ModelOrderTrack>(p => p.Type == (int)xEnum.OrderTrackType.Refund && p.OutTradeNo == refundSku.Id)
                .OrderBy(p => p.CreateTime)
                .ToList();

            vmSku.RefundAddress = refundAddress;
            vmSku.ListRecord = list;
            return vmSku;
        }

        
        /// <summary>
        ///     订单申请退款
        /// </summary>
        /// <param name="req"></param>
        public void ApplyRefund(ReqOrderApplyRefund req)
        {
            var user = _auth.GetCurrentContext().User;
            var orderSku =
                UnitWork.FirstOrDefault<ModelOrderSku>(p => p.Id == req.OrderSkuId );
            var countApply = UnitWork.Count<ModelOrderRefundSku>(p => p.OrderSkuId == orderSku.Id && p.Status >= (int)xEnum.RefundStatus.WaitAudit);
            if (countApply > 0)
            {
                throw new Exception("此商品已申请售后,不可再次申请!");

            }
            var order = UnitWork.FirstOrDefault<ModelOrder>(p => p.Id == orderSku.OrderId);
            // order.Status = (int) xEnum.OrderStatus.RefundApply;
            var orderAddress = UnitWork.FirstOrDefault<ModelOrderAddress>(p => p.OrderId == orderSku.OrderId);
            var refundStatus = xEnum.RefundStatus.WaitAudit;

            if (order.OrderStatus == (int)xEnum.OrderStatus.WaitDeliver) //未配送订单 售后直接到待退款  
            {
                refundStatus = xEnum.RefundStatus.WaitAudit;
            }
            var refundReason = "";
            //var refundReason = UnitWork.FirstOrDefault<ModelRefundReason>(p => p.Id == req.ReasonId).Title;

            ModelOrderRefundSku model = new ModelOrderRefundSku()
            {
                RefundNo = $"S{xConv.GenerateNo()}",
                OrderId = orderSku.OrderId,
                OrderNo = order.OrderNo,
                CurrentOrderStatus = order.OrderStatus,
                OrderSkuId = req.OrderSkuId,
                GoodsId = orderSku.GoodsId,
                GoodsName = orderSku.GoodsName,
                SkuId = orderSku.GoodsSkuId,
                SkuName = orderSku.SkuName,
                SkuImageId = orderSku.SkuImageId,
                RefundQuantity = orderSku.Quantity,
                RefundType = req.RefundType,
                RefundReason = refundReason,
                Mark = req.RefundDescription,
                AmountExpectRefund = orderSku.PayPrice,
                RefundProof = xConv.ListToString(req.listImageId),
                Status = (int)refundStatus,
                UserId = order.UserId,

            };
            model.Status = (int)xEnum.RefundStatus.WaitAudit;
            UnitWork.AddOrUpdate(model);
            UnitWork.AddOrUpdate(orderSku);
            UnitWork.AddOrUpdate(order);
            UnitWork.Save();

            ModelOrderTrack track = new ModelOrderTrack()
            {
                OutTradeNo = model.Id,
                Attach = model.OrderNo,
                Type = (int)xEnum.OrderTrackType.Refund,
                Info = $"用户申请售后,状态为:{xEnum.GetEnumDescription(typeof(xEnum.RefundStatus), (int)refundStatus)}"
            };
            UnitWork.Add(track);
            UnitWork.Save();
        }

        /// <summary>
        /// 售后审核
        /// </summary>
        public void Audit(ReqRefundAudit req)
        {
            req.Check();
            var refund = Repository.FirstOrDefault(p => p.Id == req.RefundSkuId);
            if (refund.Status != (int)xEnum.RefundStatus.WaitAudit)
            {
                throw new Exception($"此状态:{xEnum.GetEnumDescription(typeof(xEnum.RefundStatus), refund.Status)}不可审核");
            }
            refund.Status = req.Status;
            refund.AuditTime=DateTime.Now;
            if (req.Status == (int)xEnum.RefundStatus.UnApprove)
            {
                refund.SellerMark = req.SellerMark;
            }
            if (req.Status == (int)xEnum.RefundStatus.Approve)
            {
                var refundAddress = UnitWork.FirstOrDefault<ModelStoreAddress>(p => p.Id == req.StoreAddresId);
                ModelOrderRefundAddress address = new ModelOrderRefundAddress
                {
                    OrderRefundId = refund.Id,
                    Name = refundAddress.Name,
                    Phone = refundAddress.Phone,
                    ProvinceId = refundAddress.ProvinceId,
                    CityId = refundAddress.CityId,
                    RegionId = refundAddress.RegionId,
                    Detail = refundAddress.Detail,
                    Province = refundAddress.Province,
                    City = refundAddress.City,
                    Region = refundAddress.Region,
                    FullAddress = refundAddress.FullAddress,
                    StoreAddressId = refundAddress.Id,
                    StoreId = refundAddress.StoreId
                };
                UnitWork.AddOrUpdate(address);
            }

            ModelOrderTrack track = new ModelOrderTrack()
            {
                OutTradeNo = refund.Id,
                Attach = refund.OrderNo,
                Type = (int)xEnum.OrderTrackType.Refund,
                Info = $"售后审核,审核状态:{xEnum.GetEnumDescription(typeof(xEnum.RefundStatus), (int)req.Status)}"
            };
            UnitWork.Add(track);
            UnitWork.AddOrUpdate(refund);
            UnitWork.Save();
        }

        /// <summary>
        /// 退货填写物流信息
        /// </summary>
        /// <param name="req"></param>
        public void RefundDelivery(ReqRefundDelivery req)
        {
            req.Check();
            var orderRefundSku = UnitWork.FirstOrDefault<ModelOrderRefundSku>(p => p.Id == req.OrderRefundSkuId);
            var orderSku = UnitWork.FirstOrDefault<ModelOrderSku>(p => p.Id == orderRefundSku.OrderSkuId);
            var express = UnitWork.FirstOrDefault<ModelStoreExpress>(p=>p.Id==req.ExpressId);
            orderRefundSku.ExpressCompany = express.ExpressName;
            orderRefundSku.ExpressNo = req.ExpressNo;
            orderRefundSku.Status = (int)xEnum.RefundStatus.Shipped;
            orderRefundSku.DeliveryTime = DateTime.Now;
            UnitWork.AddOrUpdate(orderRefundSku);
            UnitWork.Save();
            ModelOrderTrack track = new ModelOrderTrack()
            {
                OutTradeNo = orderRefundSku.Id,
                Attach = orderRefundSku.OrderNo,
                Type = (int)xEnum.OrderTrackType.Refund,
                Info = $"用户已发货,{orderRefundSku.ExpressCompany},运单号:{orderRefundSku.ExpressNo}"
            };
            UnitWork.AddOrUpdate(orderRefundSku);
            UnitWork.Add(track);
            UnitWork.Save();
        }

        /// <summary>
        /// 售后-退款
        /// </summary>
        /// <param name="req"></param>
        public void RefundMoney(ReqRefundMoney req)
        {
           
            var refund = UnitWork.FirstOrDefault<ModelOrderRefundSku>(p=>p.Id==req.OrderRefundSkuId);
            if (refund.Status != (int)xEnum.RefundStatus.Shipped)
            {
                throw new Exception($"状态:{xEnum.GetEnumDescription(typeof(xEnum.RefundStatus), refund.Status)},不可退款!");
            }
            var order = UnitWork.FirstOrDefault<ModelOrder>(p => p.Id == refund.OrderId);

            UnitWork.ExecuteWithTransaction(() =>
            {
                refund.Status = (int)xEnum.RefundStatus.RefundedMoney;
                refund.RefundTime = DateTime.Now;
                UnitWork.AddOrUpdate(refund);
                UnitWork.Save() ;
                if (order.PayType == (int)xEnum.PayType.Balance) //余额支付退款
                {
                    //余额操作
                    _appUser.OpBalance(new Repository.Request.VmBalance()
                    {
                        OpType = xEnum.BalanceType.Refund,
                        Money = xConv.ToDecimal(refund.AmountExpectRefund),
                        OrderId = order.Id,
                        Remark = $"{order.OrderNo},退款",
                        UserId = order.UserId,
                    });
                }
                else if (order.PayType == (int)xEnum.PayType.Wx)
                { //微信支付退款
                    VmSettingBasicWxApp setting = _appStoreSetting.GetDetail(DefineSetting.BasicWxApp);
                    var pay = new PayWx(setting);
                    pay.Refund(order.Id, order.Id, order.PayPrice, xConv.ToDecimal(refund.AmountExpectRefund));
                }
            });
              
        }

    }
}