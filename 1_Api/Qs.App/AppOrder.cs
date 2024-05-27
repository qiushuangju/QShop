using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Castle.Components.DictionaryAdapter;
using NPOI.OpenXmlFormats;
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
using Address = Qs.Repository.Response.Address;
using Exception = System.Exception;

namespace Qs.App
{
    /// <summary>
    /// 应用层
    /// </summary>
    public class AppOrder : AppBaseString<ModelOrder, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        private AppOrderSku _appOrderSku;
        private AppGoodsSku _appGoodsSku;
        private AppUserManager _appUser;
        private AppStoreSetting _appStoreSetting;
        private AppCommDistance _appDistance;
        private AppExpress _appExpress;
        private AppOrderRefundSku _appRefund;


        /// <summary>
        /// 构造函数
        /// </summary>
        public AppOrder(IUnitWork<QsDBContext> unitWork, IRepository<ModelOrder, QsDBContext> repository,
            AppRevelanceManager app,  AppOrderSku appOrderSku, AppGoodsSku appGoodsSku, AppUserManager appUser, AppCommDistance appDistance, AppStoreSetting appStoreSetting,
            AppExpress appExpress, AppOrderRefundSku appRefund, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension,
            auth)
        {
            _revelanceApp = app;
            _appDistance = appDistance;
            _appOrderSku = appOrderSku;
            _appGoodsSku = appGoodsSku;
            _appUser = appUser;
            _appStoreSetting = appStoreSetting;
            _appExpress = appExpress;
            _appRefund = appRefund;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableDataOrder Load(ReqQuOrder req)
        {
            var user = _auth.GetCurrentContext().User;
           
            var result = new TableDataOrder();
            result.Result = ListByWhere(req, true);
            result.Count = ListLinq(req).Count();

            if (!string.IsNullOrEmpty(req.AddressId))
            {
                decimal payAmount= ListLinq(req).Sum(p => p.PayPrice);
                decimal refundAmount = ListLinq(req).Sum(p => p.PayPrice);
                decimal refundFreighttAmount = ListLinq(req).Sum(p => p.PayPrice);
                result.CountOrder = ListLinq(req).Count();
                result.SumOrderMoney = (payAmount + payAmount) - refundAmount;
            }
           
            return result;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResOrder> ListByWhere(ReqQuOrder req, bool isPage = false)
        {
            IQueryable<ModelOrder> linq = ListLinq(req);
            List<ModelOrder> list =
                isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();

            return ConvVm(list).OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelOrder> ListLinq(ReqQuOrder req)
        {
            var userContent = _auth.GetCurrentContext();
            //大状态查询
            Expression<Func<ModelOrder, bool>> where = GetWhereByBigStatus(xConv.ToInt(req.BigOrderStatus));
            var linq = UnitWork.Find<ModelOrder>(where);


            linq = linq.Where(p => p.OrderStatus >= (int)xEnum.OrderStatus.ApplyCancel);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.OrderNo.Contains(req.Key));
            }

            if (req.PayType > 0)
            {
                linq = linq.Where(p => p.PayType.Equals(req.PayType));
            }

            if (xConv.ToInt(req.OrderStatus) != 0)
            {
                linq = linq.Where(p => p.OrderStatus.Equals(req.OrderStatus));
            }

            if (xConv.ToInt(req.MinStatus) != 0)
            {
                linq = linq.Where(p => p.OrderStatus >= req.MinStatus);
            }

            if (req.OnlyMy)
            {
                linq = linq.Where(p => p.UserId == userContent.User.Id);
            }
            if (!string.IsNullOrEmpty(req.UserId))
            {
                linq = linq.Where(p => (p.UserId) == req.UserId);
            }

            var storeId = _auth.GetStoreId();
            if (!string.IsNullOrEmpty(storeId))
            {
               linq = linq.Where(p => p.StoreId == storeId);
            }
            if (!string.IsNullOrEmpty(req.AddressId))
            {
                var listOrderId = UnitWork.Find<ModelOrderAddress>(p => p.AddressId == req.AddressId)
                    .Select(p => p.OrderId).ToList();
                linq = linq.Where(p => listOrderId.Contains(p.Id));
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


            return linq.OrderByDescending(p => p.CreateTime);
        }


        /// <summary>
        /// 查询导出需要导出的订单
        /// </summary>
        /// <param name="req"></param>
        /// <param name="isPage"></param>
        /// <returns></returns>
        public List<ReqExportOrder> ExportOrderListByWhere(ReqQuOrder req, bool isPage = false)
        {
            IQueryable<ModelOrder> linqOrder = ListLinq(req);
            var linq = from order in linqOrder
                       join user in UnitWork.Find<ModelUser>(p => true) on order.UserId equals user.Id
                join address in UnitWork.Find<ModelOrderAddress>(p => true) on order.Id equals address.OrderId
                join sku in UnitWork.Find<ModelOrderSku>(p => true) on order.Id equals sku.OrderId
                select new ReqExportOrder
                {
                  
                    OrderTypeName="销售订单",
                    OrderNo =order.OrderNo,
                    UserType=user.UserType,
                    Name = address.Name,
                    Phone=address.Phone,
                    FullAddress=  address.FullAddress,
                    GoodsName=sku.GoodsName,
                    SkuName= sku.SkuName,
                    Unit= sku.Unit,
                    TotalNum=sku.Quantity,
                    PayTime = order.PayTime,
                    ArrivedTime= order.ArrivedTime,
                    OrderStatus= order.OrderStatus,
                    IsRefund=order.IsRefund,
                    FreightPrice =order.FreightPrice,
                    BuyerRemark=order.BuyerRemark
                };
            var list = linq.ToList();
            foreach (var item in list)
            {
                item.StrUserType = xEnum.GetEnumDescription(typeof(xEnum.UserType), item.UserType);
                item.StrExportStatus = GetExportStatus(item);
                item.StrOrderStatus = xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), item.OrderStatus);
               
            }


            var linqRefund = from order in linqOrder
           
                join user in UnitWork.Find<ModelUser>(p => true) on order.UserId equals user.Id
                join address in UnitWork.Find<ModelOrderAddress>(p => true) on order.Id equals address.OrderId
                join skuRefund in UnitWork.Find<ModelOrderRefundSku>(p => true) on order.Id equals skuRefund.OrderId
                join sku in UnitWork.Find<ModelOrderSku>(p => true) on skuRefund.OrderSkuId equals sku.Id
                select new ReqExportOrder
                       {
                          
                           OrderTypeName = "售后订单",
                          
                           OrderNo = order.OrderNo,
                           UserType = user.UserType,
                           Name = address.Name,
                           Phone = address.Phone,
                           FullAddress = address.FullAddress,
                           GoodsName = sku.GoodsName,
                           SkuName = sku.SkuName,
                           Unit = sku.Unit,
                           // TotalNum = -skuRefund.ActualNum,
                           // PayPrice = -skuRefund.RefundMoney,
                           // TotalPrice = -skuRefund.RefundMoney* skuRefund.ActualNum,
                           PayTime = order.PayTime,
                        
                           IsRefund = order.IsRefund,  
                           // BuyerRemark = $"申请退货数量:{-skuRefund.TotalNum}"
                };
            var listRefund = linqRefund.ToList();
            foreach (var item in listRefund)
            {
                item.StrUserType = xEnum.GetEnumDescription(typeof(xEnum.UserType), item.UserType);
                item.StrExportStatus = GetExportStatus(item);
                item.StrOrderStatus = xEnum.GetEnumDescription(typeof(xEnum.RefundStatus), item.OrderStatus);
                item.ShippingFee = item.FreightPrice + item.FloorPrice + item.EntryPrice;
            }

            list.AddRange(listRefund);
            var index = 0;
            var nextOrderNo = "";
            var nextOrderType = "";
            foreach (var item in list)
            {
                index++;
                if (index>=list.Count())
                {
                    break;
                }
                nextOrderNo = list[index].OrderNo;
                nextOrderType = list[index].OrderTypeName;
                if (item.OrderNo== nextOrderNo&& item.OrderTypeName== nextOrderType)
                {
                    item.FreightPrice = null;
                    item.FloorPrice = null;
                    item.EntryPrice = null;
                    item.ShippingFee = null;
                }
            }
            return list;
        }


        

        /// <summary>
        /// 订单明细导出状态
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string GetExportStatus(ReqExportOrder order)
        {
            string statusName = "";
            if (order.IsRefund==(int)xEnum.ComStatus.Normal)
            {
                statusName = "售后";
            }
            else if(order.OrderStatus!=(int)xEnum.OrderStatus.Done)
            {
                statusName = "未完成";
            }
            else if (order.OrderStatus ==(int)xEnum.OrderStatus.Done)
            {
                statusName = "已完成";
            }

            return statusName;
        }   

       

        /// <summary>
        /// List转化为Vm
        /// </summary>
        /// <param name="listOrder"></param>
        /// <returns></returns>
        public List<ResOrder> ConvVm(List<ModelOrder> listOrder)
        {

            //订单id
            List<string> listOrderId = listOrder.Select(p => p.Id).ToList();
            //用户信息
            List<ModelUser> listUser = UnitWork.Find<ModelUser>(p => listOrder.Select(p => p.UserId).ToList().Contains(p.Id)).ToList();
            var listSourceUserId = listUser.Select(p => p.SourceUserId).ToList();
            var strSourceUserId = xConv.ListToString(listSourceUserId);
            var listSourceUser = UnitWork.Find<ModelUser>(p => strSourceUserId.Contains(p.Id)).ToList();

            //配送地址
            List<ModelOrderAddress> listOrderAddress = UnitWork.Find<ModelOrderAddress>(p => true)
                .Where(p => listOrder.Select(p => p.Id).ToList().Contains(p.OrderId)).ToList();
            List<ModelOrderRefundSku> listRefund =
                UnitWork.Find<ModelOrderRefundSku>(p => listOrderId.Contains(p.OrderId)).ToList();  
            //订单商品信息
            List<ResOrderSku> listOrderSkuAll =
                _appOrderSku.ListByWhere(new ReqQuOrderGoods {ListOrderId = listOrderId});
            List<ResOrder> listOrderVm = new List<ResOrder>();
            VmSettingTrade trade = _appStoreSetting.GetDetail(DefineSetting.Trade);
            foreach (var order in listOrder)
            {
                ModelOrderRefundSku refund = listRefund.FirstOrDefault(p => p.OrderId == order.Id)??new ModelOrderRefundSku();
                ModelUser user = listUser.Find(p => p.Id == order.UserId);
                List<ResOrderSku> listOrderSku = listOrderSkuAll.FindAll(p => p.OrderId == order.Id);
             
                ModelOrderAddress orderAddressInfo = listOrderAddress.Find(p => p.OrderId == order.Id);
                int refundDays= trade.Order.RefundDays;
                ResOrder vm = ResOrder.ToView(order, user, listOrderSku, refundDays, orderAddressInfo);
                List<ModelUser> listOrderSourceUser = listSourceUser.Where(p => order.SourceUserId.Contains(p.Id)).ToList();
                var strSourceUser = "";
                foreach (var item in listOrderSourceUser)
                {
                    string shortUserType =
                        $"{xEnum.GetEnumDescription(typeof(xEnum.UserType), item.UserType).Substring(0, 1)}";
                    string name = !string.IsNullOrEmpty(item.RealName) ? item.RealName : item.NickName;
                    strSourceUser += $"[{shortUserType}]{name}=>";
                }

                vm.StrSourceUser = strSourceUser;
                vm.RefundStatus = xConv.ToInt(refund.Status);
                vm.StrRefundStatus = xEnum.GetEnumDescription(typeof(xEnum.RefundStatus), vm.RefundStatus);
                vm.DateTimeRefund = refund.CreateTime;

                listOrderVm.Add(vm);
            }

            return listOrderVm;
        }


        /// <summary>
        /// 根据大状态 生成Exp
        /// </summary>
        /// <param name="bigStatus"></param>
        /// <param name="onlyMy"></param>
        /// <returns></returns>
        public Expression<Func<ModelOrder, bool>> GetWhereByBigStatus(int bigStatus,bool onlyMy=false)
        {
            var userId = "";
            if (onlyMy)
            {
                userId = _auth.GetCurrentContext().User.Id;
            }
            Expression<Func<ModelOrder, bool>> where =p=>true;
            if (!string.IsNullOrEmpty(userId))
            {
                where = where.And(p => p.UserId == userId);
            }
            switch (bigStatus)
            {
                case (int)xEnum.OrderBigStatus.NotPaid: //待付款
                    where = where.And(p => p.OrderStatus == (int)xEnum.OrderStatus.NotPaid);
                    break;
                case (int)xEnum.OrderBigStatus.WaitDeliver: //待发货
                    where = where.And(p => p.OrderStatus == (int)xEnum.OrderStatus.WaitDeliver);
                    break;
                case (int)xEnum.OrderBigStatus.WaitReceiving: //待收货
                    where = where.And(p => p.OrderStatus == (int)xEnum.OrderStatus.WaitReceiving);
                    break;
                case (int)xEnum.OrderBigStatus.WaitComment: //待评价
                    where = where.And(p => p.OrderStatus == (int)xEnum.OrderStatus.Done&& p.IsComment == (int)xEnum.ComStatus.Disable);
                    break;
                case (int)xEnum.OrderBigStatus.Done: //已完成
                    where = where.And(p => p.OrderStatus == (int)xEnum.OrderStatus.Done);
                    break;
                case (int)xEnum.OrderBigStatus.Refund: //售后
                    where = where.And(p => p.IsRefund == (int)xEnum.ComStatus.Normal);
                    break;
            }

            return where;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuOrder req)
        {
            var model = xConv.CopyMapper<ModelOrder, ReqAuOrder>(req);
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
        /// 创建订单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResOrderSettlement GetOrderSettlement(ReqOrderSettlement req)
        {
            req.Check();
            ResOrderSettlement res = new ResOrderSettlement();
            var user = _auth.GetCurrentContext().User;
            res.Personal = xConv.CopyMapper<ResUser, ModelUser>(user);
            List<OrderSkuCreate> listSkuCreate = GetListSku(req.GoodsSkuId, req.GoodsNum, req.StrCartIds);
          
            decimal couponMoney = 0;
            if (!string.IsNullOrEmpty(req.UserCouponId))
            {
                couponMoney = xConv.ToDecimal(UnitWork.FirstOrDefault<ModelUserCoupon>(p => p.Id == req.UserCouponId)?.ReducePrice);
            }

            decimal pointsMoney = GetPointMoney(req);
            OrderInfoSettlement orderSettlement = new OrderInfoSettlement()
            {
                OrderTotalNum = listSkuCreate.Sum(p => p.Quantity),
                OrderTotalPrice = listSkuCreate.Sum(p => (p.Sku.SalePrice * p.Quantity)),
                CouponMoney = couponMoney,
                UsablePointsMoney = GetPointMoney(req,true),
                PointsMoney = pointsMoney,
                
            };
            ModelUserAddress address = !string.IsNullOrEmpty(req.AddressId) ? UnitWork.FirstOrDefault<ModelUserAddress>(p => p.Id == req.AddressId) :
                UnitWork.FirstOrDefault<ModelUserAddress>(p => p.UserId == user.Id && p.IsDefault == true);
            var vmFreightMoney = GetFreighMoney(orderSettlement, listSkuCreate,address);
            orderSettlement.FreightMoney = vmFreightMoney.FreighMoney;
            orderSettlement.IsIntraRegion = vmFreightMoney.IsIntraRegion;
            orderSettlement.UndeliveredGoods = vmFreightMoney.UndeliveredGoods;
            orderSettlement.OrderPayPrice = orderSettlement.OrderTotalPrice + orderSettlement.FreightMoney - orderSettlement.CouponMoney - orderSettlement.PointsMoney;
           
            ResUserAddress vmAddress = ResUserAddress.ToView(address);
            res.ListCoupon = ListUsedCoupon(listSkuCreate);
            res.Address = vmAddress;
            res.ListGoods = listSkuCreate;
            res.Order = orderSettlement;
            res.Setting = _appStoreSetting.GetDetail(DefineSetting.Points);
            return res;
        }


        /// <summary>
        /// 计算订单可抵扣积分
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ignoreIsUserPoint">是否忽略参数IsUserPoint</param>
        /// <returns></returns>
        public decimal GetPointMoney(ReqOrderSettlement req,bool ignoreIsUserPoint=false)
        {
            var user = _auth.GetCurrentContext().User;
            VmSettingPoints settingPoints = _appStoreSetting.GetDetail(DefineSetting.Points);
            List<OrderSkuCreate> listSkuCreate = GetListSku(req.GoodsSkuId, req.GoodsNum, req.StrCartIds);
            decimal pointsMoney = 0;
            decimal orderTotalPrice = xConv.ToDecimal(listSkuCreate.Sum(p => (p.Sku.SalePrice * p.Quantity)));
            if ( settingPoints.IsShoppingDiscount == (int)xEnum.ComStatus.Normal) //积分抵扣
            {
                if (ignoreIsUserPoint)
                {
                    req.IsUsePoints = (int) xEnum.ComStatus.Normal;
                }
                if (req.IsUsePoints == (int)xEnum.ComStatus.Normal)
                {
                    pointsMoney = (user.Points * settingPoints.Discount.DiscountRatio);
                    var maxMoney = settingPoints.Discount.MaxDiscountPrice;
                    var maxRatio = settingPoints.Discount.MaxMoneyRatio;
                    var maxPointsMoney = orderTotalPrice * (maxRatio / 100);
                    if (pointsMoney > maxPointsMoney)
                    {
                        pointsMoney = maxPointsMoney;
                    }
                    if (pointsMoney > maxMoney)
                    {
                        pointsMoney = maxMoney;
                    }
                }
               
            }
            return xConv.ToMoney(pointsMoney);
        }

        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="listSkuCreate"></param>
        /// <returns></returns>
        public VmGetFreighMoney GetFreighMoney(OrderInfoSettlement orderInfo, List<OrderSkuCreate> listSkuCreate,
            ModelUserAddress address)
        {

            VmGetFreighMoney vmFreighMoney = new VmGetFreighMoney();
            if (address != null)
            {
                VmSettingFullFree fullFree = _appStoreSetting.GetDetail(DefineSetting.FullFree);
                List<string> listGoodsId = listSkuCreate.Select(p => p.Sku.GoodsId).ToList();

                var listGoods = UnitWork.Find<ModelGoods>(p => listGoodsId.Contains(p.Id)).Distinct().ToList();
                var listDeliveryId = listGoods.Select(p => p.DeliveryId).Distinct().ToList();
                var listDelivery = UnitWork.Find<ModelDelivery>(p => listDeliveryId.Contains(p.Id));
                var listDeliveryRule = UnitWork.Find<ModelDeliveryRule>(p => listDeliveryId.Contains(p.DeliveryId)
                                                                             && p.Region.Contains(address.CityId));
                decimal freightMoney = 0;
                if (fullFree.IsOpen == (int) xEnum.ComStatus.Normal &&
                    orderInfo.OrderPayPrice >= fullFree.Money) //开启满额包邮 ,并且满足满额
                {
                    vmFreighMoney.FreighMoney = 0;
                    return vmFreighMoney;
                }

                foreach (var sku in listSkuCreate)
                {
                    ModelGoods goods = listGoods.FirstOrDefault(p => p.Id == sku.Sku.GoodsId);
                    var delivery = listDelivery.FirstOrDefault(p => p.Id == goods.DeliveryId) ?? new ModelDelivery();
                    var rule = listDeliveryRule
                        .Where(p => p.DeliveryId == delivery.Id && p.Region.Contains(address.CityId))
                        .FirstOrDefault();
                    if (rule == null)
                    {
                        vmFreighMoney.IsIntraRegion = false;
                        vmFreighMoney.UndeliveredGoods = $"{xConv.ToShortStr(goods.GoodsName)}";
                        return vmFreighMoney;
                    }
                }

                if (listDelivery.Count() == 1) //订单所有商品同一个运费模板
                {
                    var delivery = listDelivery.FirstOrDefault() ?? new ModelDelivery();
                    var rule = listDeliveryRule.FirstOrDefault() ??
                               new ModelDeliveryRule();
                    //首重(件/Kg)
                    var countWeightOrGoods = delivery.Method == (int) xEnum.DeliveryMethod.PerUnit
                        ? listSkuCreate.Sum(p => p.Quantity)
                        : listSkuCreate.Sum(p => p.Sku.GoodsWeight * p.Quantity);
                    //续重(件/Kg)
                    int countAdditional = rule.Additional <= 0
                        ? 0
                        : xConv.ToInt(countWeightOrGoods - rule.First) % xConv.ToInt(rule.Additional) == 0
                            ? xConv.ToInt(countWeightOrGoods - rule.First) / xConv.ToInt(rule.Additional)
                            : xConv.ToInt(countWeightOrGoods - rule.First) / xConv.ToInt(rule.Additional) + 1;

                    if (countWeightOrGoods <= rule.First) //低于等于首重
                    {
                        freightMoney = xConv.ToDecimal(rule.FirstFee);
                    }
                    else
                    {
                        freightMoney = xConv.ToDecimal(rule.FirstFee) + rule.AdditionalFee * countAdditional;
                    }
                }

                if (listDelivery.Count() > 1) //订单所有商品不同运费模板
                {
                    var maxFirstFee = listDeliveryRule.OrderByDescending(p => p.FirstFee).FirstOrDefault();
                    int countFirstGoods = 0;
                    decimal firstFree = 0;

                    foreach (var sku in listSkuCreate)
                    {
                        ModelGoods goods = listGoods.FirstOrDefault(p => p.Id == sku.Sku.GoodsId);
                        var delivery = listDelivery.FirstOrDefault(p => p.Id == goods.DeliveryId) ??
                                       new ModelDelivery();
                        var rule = listDeliveryRule
                                       .Where(p => p.DeliveryId == delivery.Id && p.Region.Contains(address.CityId))
                                       .FirstOrDefault() ??
                                   new ModelDeliveryRule();
                        if (delivery.Id == maxFirstFee.DeliveryId && countFirstGoods == 0) //首重商品(所有商品首重运费最贵的商品)
                        {
                            countFirstGoods = 1;
                            firstFree = maxFirstFee.FirstFee;

                            //重量/件数(件/Kg)
                            var countOneWeightOrGoods = delivery.Method == (int) xEnum.DeliveryMethod.PerUnit
                                ? sku.Quantity
                                : sku.Sku.GoodsWeight * sku.Quantity;
                            //续重(件/Kg)
                            int countOneAdditional = rule.Additional <= 0
                                ? 0
                                : xConv.ToInt(countOneWeightOrGoods - rule.First) % xConv.ToInt(rule.Additional) == 0
                                    ? xConv.ToInt(countOneWeightOrGoods - rule.First) / xConv.ToInt(rule.Additional)
                                    : xConv.ToInt(countOneWeightOrGoods - rule.First) / xConv.ToInt(rule.Additional) +
                                      1;

                            if (countOneWeightOrGoods <= rule.First) //低于等于首重
                            {
                                freightMoney += firstFree;
                            }
                            else
                            {
                                freightMoney += xConv.ToDecimal(rule.FirstFee) +
                                                rule.AdditionalFee * countOneAdditional;
                            }
                        }
                        else //续重商品(除首重商品外都算续重),整个商品都算续重
                        {
                            //重量/件数(件/Kg)
                            var countOneWeightOrGoods = delivery.Method == (int) xEnum.DeliveryMethod.PerUnit
                                ? sku.Quantity
                                : sku.Sku.GoodsWeight * sku.Quantity;
                            
                            int countOneAdditional = 0;
                            if (rule.Additional!=0)//续重(件/Kg)
                            {
                                countOneAdditional =
                                    xConv.ToInt(countOneWeightOrGoods) % xConv.ToInt(rule.Additional) == 0
                                        ? xConv.ToInt(countOneWeightOrGoods) / xConv.ToInt(rule.Additional)
                                        : xConv.ToInt(countOneWeightOrGoods) / xConv.ToInt(rule.Additional) + 1;
                            }
                           

                            freightMoney += rule.AdditionalFee * countOneAdditional;
                        }
                    }
                }

                vmFreighMoney.FreighMoney = freightMoney;
            }

            return vmFreighMoney;
        }


        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public object CreateOrder(ReqOrderSettlement req)
        {
            object res = null;
            ModelOrder order = new ModelOrder();
            try
            {
                req.Check();
                xValidation.CheckStrNull(new List<ValueTip>()
                {
                    new ValueTip(req.AddressId, "地址")
                });
                var user = _auth.GetCurrentContext().User;
                var storeId = _auth.GetStoreId();
                var platform = _auth.GetPlatform();
                VmSettingPoints settingPoints = _appStoreSetting.GetDetail(DefineSetting.Points);
                ResOrderSettlement orderSettlement = GetOrderSettlement(req);
                List<OrderSkuCreate> listSkuCreate = GetListSku(req.GoodsSkuId, req.GoodsNum, req.StrCartIds)
                    .OrderBy(p => p.Sku.SalePrice).ToList();
                var userCoupon = UnitWork.FirstOrDefault<ModelUserCoupon>(p => p.Id == req.UserCouponId);
                if (userCoupon != null)
                {
                    userCoupon.Status = (int) xEnum.CouponStatus.Used;
                    UnitWork.AddOrUpdate(userCoupon);
                }


                var newOrderId = xConv.NewGuid();
                List<ModelOrderSku> listOrderSku = new List<ModelOrderSku>();
                UnitWork.ExecuteWithTransaction(() =>
                {
                    ModelUserAddress userAddress =
                        UnitWork.FirstOrDefault<ModelUserAddress>(p => p.Id == req.AddressId);
                    ModelOrderAddress orderAddress = xConv.CopyMapper<ModelOrderAddress, ModelUserAddress>(userAddress);
                    orderAddress.Id = xConv.NewGuid();
                    orderAddress.OrderId = newOrderId;
                    orderAddress.AddressId = userAddress.Id;
                    UnitWork.Add(orderAddress);

                    order.Id = newOrderId;
                    order.OrderNo =$"DD{xConv.GenerateNo()}" ;
                    order.OrderStatus = (int) xEnum.OrderStatus.NotPaid;

                    order.CouponId = userCoupon != null ? userCoupon.Id : "";
                    order.UserId = user.Id;
                    order.IsRefund = (int) xEnum.ComStatus.Disable;
                    order.ParentId = user.ParentId;
                    order.SourceUserId = user.SourceUserId;
                    order.TotalGoodsPrice = xConv.ToDecimal(orderSettlement.Order.OrderTotalPrice);
                    order.CouponMoney = xConv.ToDecimal(orderSettlement.Order.CouponMoney);
                    order.PointsMoney = orderSettlement.Order.PointsMoney;
                    order.PointsNum =
                        xConv.ToInt(orderSettlement.Order.PointsMoney / settingPoints.Discount.DiscountRatio);
                    order.FreightPrice = orderSettlement.Order.FreightMoney;
                    order.PayType = req.PayType;
                    order.PayPrice = xConv.ToDecimal(order.TotalGoodsPrice + order.FreightPrice - order.CouponMoney -
                                                     order.PointsMoney);
                    order.StoreId = storeId;
                    order.Platform = platform;
                    order.ParentId = user.ParentId;

                    var rateCouponDiscount = order.CouponMoney / order.TotalGoodsPrice;
                    var ratePointsDiscount = order.PointsMoney / order.TotalGoodsPrice;
                    var lasetGoodsIndex = listSkuCreate.Sum(p => p.Quantity) - 1;
                    int index = 0;

                    if(!string.IsNullOrEmpty(req.StrCartIds))
                    {
                        UnitWork.Update<ModelCart>(p => req.StrCartIds.Contains(p.Id), u => new ModelCart
                        {
                            OrderId = newOrderId
                        });
                    }
                    foreach (var item in listSkuCreate)
                    {
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            var couponMoney = xConv.ToMoney(item.Sku.SalePrice * rateCouponDiscount);
                            var pointsMoney = xConv.ToMoney(item.Sku.SalePrice * ratePointsDiscount);

                            if (index == lasetGoodsIndex)
                            {
                                couponMoney = order.CouponMoney - listOrderSku.Sum(p => p.CouponMoney);
                                pointsMoney = xConv.ToDecimal(order.PointsMoney - listOrderSku.Sum(p => p.PointsMoney));
                            }

                            index++;
                            ModelOrderSku orderSku = new ModelOrderSku()
                            {
                                GoodsId = item.Sku.GoodsId,
                                GoodsSkuId = item.Sku.Id,
                                GoodsName = item.Sku.GoodsName,
                                OrderId = newOrderId,
                                OrderStatus = (int) xEnum.OrderStatus.NotPaid,
                                GoodsNo = item.Sku.GoodsSkuNo,
                                SkuImageId = item.Sku.ImageId,
                                SkuName = item.Sku.SkuName,
                                Quantity = 1,
                                IsComment = false,
                                SalePrice = xConv.ToDecimal(item.Sku.SalePrice),
                                CouponMoney = couponMoney,
                                PointsMoney = pointsMoney,
                                PayPrice = xConv.ToDecimal(item.Sku.SalePrice - couponMoney - pointsMoney),
                                PointsNum = xConv.ToInt(pointsMoney / settingPoints.Discount.DiscountRatio),
                                Commission1 = item.Sku.Commission1,
                                Commission2 = item.Sku.Commission2,
                                UserId = user.Id,
                                StoreId = storeId,
                                SourceUserId = user.SourceUserId
                            };
                            listOrderSku.Add(orderSku);
                            UnitWork.Add(orderSku);
                        }
                    }

                    UnitWork.Add(order);
                    UnitWork.Save();
                });

            }
            catch (Exception e)
            {
                return new
                {
                    IsCreatedOrder = false
                };
            }
            try
            {
                res = PayOrderBefore(order.Id, req.PayType);
            }
            catch (Exception e)
            {
                res = new
                {
                    IsCreatedOrder = true
                };
            }
            return res;
        }

        /// <summary>
        /// 支付前
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="payType">支付类型</param>
        public object PayOrderBefore(string orderId,int payType)
        {
            var user = _auth.GetCurrentContext().User;
            var order = UnitWork.FirstOrDefault<ModelOrder>(p => p.Id == orderId);
            dynamic res = null;
            UnitWork.ExecuteWithTransaction(() =>
            {
                _appUser.OpPoints(new VmPoints()
                {
                    OpType = xEnum.PointType.ConsumeOrder,
                    OrderId = order.Id,
                    Points = xConv.ToInt(order.PointsNum),
                    Remark = $"订单{order.OrderNo},抵扣{order.PointsMoney}元",
                    UserId = order.UserId
                });

                if (payType==(int)xEnum.PayType.Balance)
                {//余额支付
                    _appUser.OpBalance(new VmBalance()
                    {
                        OpType = xEnum.BalanceType.ConsumeOrder,
                        OrderId = order.Id,
                        Money = (order.PayPrice),
                        Remark = $"订单{order.OrderNo},支付{order.PayPrice}",
                        UserId = order.UserId
                    });
                    OrderPay(order.Id, order.PayPrice);
                    res = new
                    {
                        IsCreatedOrder = true
                    };
                }
                else
                {//微信支付
                    VmSettingBasicWxApp setting = _appStoreSetting.GetDetail(DefineSetting.BasicWxApp);
                    var payPrice = xConv.ToDecimal(order.PayPrice);
                    var platform = _auth.GetPlatform();
                    var pay = new PayWx(setting);
                    var payParams =
                        pay.GetPayParams(user.WxOpenId, payPrice, $"订单:{order.OrderNo}", order.Id,  "Order",
                            platform);
                    res = new ResPayConfirm();
                    res.WxPayParams = payParams;
                    res.AmountTotal = payPrice;
                    res.IsCreatedOrder = true;
                }
                UnitWork.Save();
            });
         
            return res;
        }

        /// <summary>
        /// 当前商品可使用优惠券
        /// </summary>
        /// <param name="listSkuCreate"></param>
        /// <returns></returns>
        public List<ResUserCoupon> ListUsedCoupon(List<OrderSkuCreate> listSkuCreate)
        {
            List<ResUserCoupon> list = new List<ResUserCoupon>();
            var user = _auth.GetCurrentContext().User;
            var listUsedCoupon = new List<ModelUserCoupon>();
            var listUserCoupon = UnitWork.Find<ModelUserCoupon>(p => p.Status == (int)xEnum.CouponStatus.Usable
                                                                     && p.UserId == user.Id);
            foreach (var coupon in listUserCoupon)
            {
                if (coupon.ApplyRange == (int)xEnum.CouponRange.All)
                {
                    listUsedCoupon.Add(coupon);
                }
                if (coupon.ApplyRange == (int)xEnum.CouponRange.Goods)
                {
                    var listRangeGoodsId = xConv.ToListString(coupon.ApplyRangeConfig);
                    var listUserCouponGoodsId = listSkuCreate.Where(p => listRangeGoodsId.Contains(p.Sku.GoodsId)).Select(p => p.Sku.GoodsId).ToList();

                    var sumGoodsAmount = listSkuCreate.Where(p => listUserCouponGoodsId.Contains(p.Sku.GoodsId))
                        .Sum(p => p.Quantity * p.Sku.SalePrice);
                    if (sumGoodsAmount >= coupon.MinPrice)
                    {
                        listUsedCoupon.Add(coupon);
                    }
                }

                if (coupon.ApplyRange == (int)xEnum.CouponRange.Exclude)
                {
                    var listRangeGoodsId = xConv.ToListString(coupon.ApplyRangeConfig);
                    var listUnUserCouponGoodsId = listSkuCreate.Where(p => listRangeGoodsId.Contains(p.Sku.GoodsId)).Select(p => p.Sku.GoodsId).ToList();

                    var sumGoodsAmount = listSkuCreate.Where(p => !listUnUserCouponGoodsId.Contains(p.Sku.GoodsId))
                        .Sum(p => p.Quantity * p.Sku.SalePrice);
                    if (sumGoodsAmount >= coupon.MinPrice)
                    {
                        listUsedCoupon.Add(coupon);
                    }
                }
            }

            foreach (var item in listUsedCoupon)
            {
                list.Add(ResUserCoupon.ToView(item));
            }
            return list;
        }
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="goodsSkuId"></param>
        /// <param name="goodsNum"></param>
        /// <param name="strCartIds"></param>
        /// <returns></returns>
        public List<OrderSkuCreate> GetListSku(string goodsSkuId,int goodsNum,string strCartIds)
        {
            List<OrderSkuCreate> listVmCreate = new List<OrderSkuCreate>();
            //获取订单商品信息
            if (!string.IsNullOrEmpty(goodsSkuId)) //立即购买
            {
                ModelGoodsSku sku = UnitWork.FirstOrDefault<ModelGoodsSku>(p => p.Id == goodsSkuId);
                ModelGoods goods = UnitWork.FirstOrDefault<ModelGoods>(p => p.Id == sku.GoodsId);
                List<ModelFileUpload> listFile =
                    UnitWork.Find<ModelFileUpload>(p => sku.ImageId.Contains(p.Id)).ToList();
                listVmCreate.Add(new OrderSkuCreate()
                {
                    Sku = ResGoodSku.ToView(sku, goods, listFile),
                    Quantity = goodsNum
                });
            }
            else //购物车下单
            {
                List<string> listCartId = xConv.ToListString(strCartIds);
                List<ModelCart> listCart = UnitWork.Find<ModelCart>(p => listCartId.Contains(p.Id)).ToList();
                List<string> listSkuId = listCart.Select(p => p.GoodsSkuId).ToList();
                List<string> listGoodsId = listCart.Select(p => p.GoodsId).ToList();
                List<ModelGoodsSku> listSku = UnitWork.Find<ModelGoodsSku>(p => listSkuId.Contains(p.Id)).ToList();
                List<string> listFileId = listSku.Select(p=>p.ImageId).ToList();
                List<ModelFileUpload> listFile =
                    UnitWork.Find<ModelFileUpload>(p => listFileId.Contains(p.Id)).ToList();
               
                var listGoods = UnitWork.Find<ModelGoods>(p => listGoodsId.Contains(p.Id)).ToList();


                foreach (var cart in listCart)
                {
                    ModelGoodsSku sku = listSku.FirstOrDefault(p => p.Id == cart.GoodsSkuId);
                    ModelGoods goods = listGoods.FirstOrDefault(p => p.Id == sku.GoodsId);
                    listVmCreate.Add(new OrderSkuCreate()
                    {
                        Sku = ResGoodSku.ToView(sku, goods,listFile),
                        Quantity = cart.GoodsNum
                    });
                   
                }
            }
            return listVmCreate;
        }

        
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="req"></param>
        public object CancelOrder(ReqCancelOrder req)
        {
            var userCurrent = _auth.GetCurrentContext().User;
            ModelOrder order = Repository.FirstOrDefault(p => p.Id == req.OrderId);
            if (order.OrderStatus == (int) xEnum.OrderStatus.NotPaid ) //订单待处理-待分配 取消订单
            {
                UnitWork.ExecuteWithTransaction(() =>
                {
                    order.OrderStatus = (int) xEnum.OrderStatus.Cancel;
                    UnitWork.Update(order);
                    UnitWork.Save();
                });
            }
            else
            {
                throw new Exception(
                    $"订单状态:{xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), order.OrderStatus)},不可取消");
            }
            return true;
        }

       
        /// <summary>
        /// 申请取消订单
        /// </summary>
        /// <param name="req"></param>
        public void ApplyCancelOrder(ReqCancelOrder req)
        {
            var userCurrent = _auth.GetCurrentContext().User;
            ModelOrder order = Repository.FirstOrDefault(p => p.Id == req.OrderId);
            if (order.OrderStatus == (int)xEnum.OrderStatus.WaitDeliver) //订单待处理-待分配 取消订单
            {
                UnitWork.ExecuteWithTransaction(() =>
                {
                    order.OrderStatus = (int)xEnum.OrderStatus.ApplyCancel;
                    UnitWork.Update(order);
                    UnitWork.Save();
                });
            }
            else
            {
                throw new Exception(
                    $"订单状态:{xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), order.OrderStatus)},不可取消");
            }
                
        }

        /// <summary>
        /// 申请取消订单-审核
        /// </summary>
        /// <param name="req"></param>
        public void AuditCancel(ReqAuditCancel req)
        {
            var userCurrent = _auth.GetCurrentContext().User;
            ModelOrder order = Repository.FirstOrDefault(p => p.Id == req.OrderId);
            if (order.OrderStatus == (int) xEnum.OrderStatus.ApplyCancel) //
            {
                order.OrderStatus = req.OrderStatus;
                UnitWork.ExecuteWithTransaction(() =>
                {
                    if (req.OrderStatus == (int) xEnum.OrderStatus.Cancel) //同意取消
                    {
                        RefundMoney(order, "");
                    }

                    UnitWork.Update(order);
                    UnitWork.Save();
                });
            }
            else
            {
                throw new Exception(
                    $"订单状态:{xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), order.OrderStatus)},不可审核");
            }
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="order"></param>
        /// <param name="returnMoneyRemark">退款备注</param>
        public void RefundMoney(ModelOrder order, string returnMoneyRemark)
        {
            if (order.PayType == (int)xEnum.PayType.Balance) //余额支付退款
            {
                //余额操作
                _appUser.OpBalance(new Repository.Request.VmBalance()
                {
                    OpType = xEnum.BalanceType.Refund,
                    Money = xConv.ToDecimal(order.OrderPrice),
                    OrderId = order.Id,
                    Remark = $"{returnMoneyRemark},{order.OrderNo}",
                    UserId = order.UserId,
                    FreightPrice = order.FreightPrice,
                });
            }
            else if (order.PayType == (int)xEnum.PayType.Wx)
            { //微信支付退款
                VmSettingBasicWxApp setting = _appStoreSetting.GetDetail(DefineSetting.BasicWxApp);
                var pay = new PayWx(setting);
                pay.Refund(order.Id, order.Id, order.PayPrice, xConv.ToDecimal(order.PayPrice));
            }
        }

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="req"></param>
        public void Delivery(ReqDelivery req)
        {
            var userCurrent = _auth.GetCurrentContext().User;
            ModelOrder order = Repository.FirstOrDefault(p => p.Id == req.OrderId);
            if (order.OrderStatus == (int)xEnum.OrderStatus.WaitDeliver) //
            {
                order.OrderStatus = (int)xEnum.OrderStatus.WaitReceiving;
                order.ExpressId = req.ExpressId;
                order.ExpressNo = req.ExpressNo;
                order.SendTime = DateTime.Now;
                UnitWork.Update(order);
                    UnitWork.Save();
            }
            else
            {
                throw new Exception(
                    $"订单状态:{xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), order.OrderStatus)},不可发货");
            }
        }
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="req"></param>
        public void Receipt(ReqReceipt req)
        {
            ModelOrder order = Repository.FirstOrDefault(p => p.Id == req.OrderId);
            if (order.OrderStatus == (int)xEnum.OrderStatus.WaitReceiving) 
            {
                order.OrderStatus = (int)xEnum.OrderStatus.Done;
                order.DoneTime=DateTime.Now;
                UnitWork.Update(order);
                UnitWork.Save();
            }
            else
            {
                throw new Exception(
                    $"订单状态:{xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), order.OrderStatus)},不可收货");
            }
        }
        
        /// <summary>
        /// 使用优惠券
        /// </summary>
        /// <param name="order"></param>
        /// <param name="userCouponId"></param>
        public void PayCoupon(ModelOrder order, string userCouponId)
        {
            var user = _auth.GetCurrentContext().User;
            if (!string.IsNullOrEmpty(userCouponId))
            {
                ModelUserCoupon coupon = UnitWork.FirstOrDefault<ModelUserCoupon>(p => p.Id == userCouponId
                    && p.UserId == user.Id);
                if (coupon == null)
                {
                    throw new Exception("系统找不到此优惠券!");
                }

                if (coupon.Status != (int) xEnum.CouponStatus.Usable)
                {
                    throw new Exception(
                        $"此优惠券{xEnum.GetEnumDescription(typeof(xEnum.CouponStatus), coupon.Status)},当前不可使用!");
                }

                coupon.Status = (int) xEnum.CouponStatus.Used;
                order.CouponId = userCouponId;
                order.CouponMoney = coupon.ReducePrice;
                UnitWork.Update(coupon);
            }
        }

       
        

        /// <summary>
        /// 获取订单详情
        /// </summary>
        public ResOrder GetDetail(string id)
        {
            ModelOrder order = Repository.FirstOrDefault(p => p.Id == id);
            if (order == null)
            {
                throw new Exception($"系统找不到此订单!");
            }

            List<ResOrder> list= ConvVm(new List<ModelOrder>() {order});
          // //下单用户
          // var userOrder = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == order.UserId);
          //   ResOrder res = xConv.CopyMapper<ResOrder, ModelOrder>(order);
          //   res.StrOrderStatus = xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), order.OrderStatus);
          //   res.OrderUser = xConv.CopyMapper<OrderUser, ModelUser>(userOrder);
          //   res.CreateUserName = userOrder?.NickName;
          //   res.CreateUserAvater = userOrder?.UrlAvater;
          //   res.IsHaveRefund = UnitWork.Count<ModelOrderRefundSku>(p => p.OrderId == order.Id)>0;
          //
          //   //配送订单
          //   ModelOrderAddress address = UnitWork.FirstOrDefault<ModelOrderAddress>(p => p.OrderId == id);
          //   if (address != null)
          //   {
          //       res.AddressInfo = new Address()
          //       {
          //           Name = $"{address.Name}",
          //           FullAddress = address.FullAddress,
          //           ContactInfo = $"{address.Phone}",
          //           Phone = address.Phone,
          //       };
          //   }
          //
          //
          //   //物流追踪
          //   res.ListTrackRecord = UnitWork.Find<ModelOrderTrack>(p => p.OutTradeNo == order.Id).Select(p => new OrderTrack
          //   {
          //       CreateTime = p.CreateTime,
          //       Info = p.Info
          //   }).OrderByDescending(p => p.CreateTime).ToList();
          //
          //
          //   //订单商品
          //   List<ModelOrderSku> listOrderSku = UnitWork.Find<ModelOrderSku>(p => p.OrderId == id).ToList();             
          //   var listRefundSku = UnitWork.Find<ModelOrderRefundSku>(p => p.OrderId == order.Id && p.Status >= (int)xEnum.RefundStatus.WaitAudit).ToList();
          //   List<ResOrderSku> listSku = new EditableList<ResOrderSku>();
          //   foreach (var orderSku in listOrderSku)
          //   {
          //       ResOrderSku sku = ResOrderSku.ToView(orderSku,null);
          //       sku.QuantityRefund = xConv.ToInt(listRefundSku.Where(p => p.SkuId == orderSku.GoodsSkuId).Sum(p => p.RefundQuantity));
          //       listSku.Add(sku);
          //   }
          //
          //   res.ListSku = listSku;
          //
          //   //订单金额
          //   res.OrderMoney = new OrderMoney()
          //   {
          //       TotalGoodsPrice = order.TotalGoodsPrice,
          //       CouponMoney = order.CouponMoney,
          //       OrderPrice = order.OrderPrice,
          //       PayPrice = order.PayPrice
          //   };
          //   res.Id = order.Id;
          //   res.BuyerRemark = order.BuyerRemark;
          //   res.OrderNo = order.OrderNo;
          //   res.StrPayType = xEnum.GetEnumDescription(typeof(xEnum.PayType), order.PayType);
          //   res.CreateTime = order.CreateTime;
          //   res.PayTime = order.PayTime;
          //   //当前登录用户（制单人）
          //   res.CurrentUserName = _auth.GetCurrentContext().User?.NickName;
          //   res.TrackInfo = _appExpress.GetLasetTrack(order.Id);
            return list.FirstOrDefault();
        }
      
        /// <summary>
        ///获取支付参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public object GetPayParams(ReqGetPayParam req)
        {
            var user = _auth.GetCurrentContext().User;
            ModelOrder order = Repository.FirstOrDefault(p => p.Id == req.OrderId);                          
            order.PayUserId = user.Id;    
            if (req.PayType == (int) xEnum.PayType.Wx)
            {
                VmSettingBasicWxApp setting = _appStoreSetting.GetDetail(DefineSetting.BasicWxApp);
                var pay = new PayWx(setting);
                string payAttach = "Order";
                var payParams =
                    pay.GetPayParams(user.WxOpenId, xConv.ToDecimal(order.OrderPrice), "订单", order.Id, payAttach,
                        req.AppKey);
                var res = new ResPayConfirm();
                res.WxPayParams = payParams;
                res.AmountTotal = xConv.ToDecimal(order.OrderPrice);
                return res;
            }
            else if (req.PayType == (int) xEnum.PayType.Balance)
            {
                var pwd = xConv.MD5Encoding(req.BalancePwd, xConv.ToStrDateTime(user.CreateTime));
                if (string.IsNullOrEmpty(user.BalancePwd))
                {
                    throw new CustomException(DefineErrCode.PayPwdIsNull, "未设置余额密码,请先设置余额密码!");
                }

                if (user.BalancePwd != pwd)
                {
                    throw new Exception("余额密码错误!");
                }

                _appUser.OpBalance(new Repository.Request.VmBalance()
                {
                    OpType = xEnum.BalanceType.ConsumeOrder,
                    Money = xConv.ToDecimal(order.OrderPrice),
                    OrderId = order.Id,
                    Remark = $"订单支付,{order.OrderNo}"
                });
                var res = true;
                return res;
            }

            UnitWork.Update(order);
            return true;
        }

        /// <summary>
        /// 支付成功
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="totalAmount"></param>
        public void OrderPay(string orderId, decimal totalAmount)
        {
            var order = UnitWork.FirstOrDefault<ModelOrder>(p => p.Id == orderId);
            order.OrderStatus = (int)xEnum.OrderStatus.WaitDeliver;
            order.PayPrice = totalAmount;
            order.PayTime = DateTime.Now;
            var listOrderSku = UnitWork.Find<ModelOrderSku>(p => p.OrderId == orderId).ToList();
            var freight = order.FreightPrice;
            foreach (var orderSku in listOrderSku)
            {
                var totalGoodsPrice = xConv.ToDecimal(orderSku.PayPrice * orderSku.Quantity);
                if (!string.IsNullOrEmpty(order.CouponId)) //有优惠券
                {
                    var rate = (xConv.ToDecimal(order.PayPrice) - xConv.ToDecimal(order.FreightPrice)) / order.TotalGoodsPrice;
                    // orderSku.Freight=
                    orderSku.PayPrice = Math.Ceiling(xConv.ToDecimal(orderSku.PayPrice * rate) * 100) / 100;
                    totalGoodsPrice = Math.Ceiling(xConv.ToDecimal(totalGoodsPrice * rate) * 100) / 100;
                }

                orderSku.OrderStatus = (int)xEnum.OrderStatus.WaitDeliver;
                orderSku.PayTime = DateTime.Now;
                UnitWork.AddOrUpdate(orderSku);
            }
            //扣减库存
            DeductStock(order);
            UnitWork.AddOrUpdate(order);        
            UnitWork.Delete<ModelCart>(p => p.OrderId == order.Id);
            UnitWork.Save();
        }

        /// <summary>
        /// 扣减库存
        /// </summary>
        /// <param name="order"></param>
        public void DeductStock(ModelOrder order)
        {
            List<ModelOrderSku> listOrderGoods = UnitWork.Find<ModelOrderSku>(p => p.OrderId == order.Id).ToList();
            List<string> listSkuId = listOrderGoods.Select(p => p.GoodsSkuId).ToList();
            List<string> listGoodsId = listOrderGoods.Select(p => p.GoodsId).ToList();
            List<ModelGoodsSku> listSku = UnitWork.Find<ModelGoodsSku>(p => listSkuId.Contains(p.Id)).ToList();
            List<ModelGoods> listGoods = UnitWork.Find<ModelGoods>(p => listGoodsId.Contains(p.Id)).ToList();
            foreach (var goods in listGoods)
            {
                List<ModelGoodsSku> listStockSku = new List<ModelGoodsSku>();
                var checkSkuStock = listSku.Where(p => p.GoodsId == goods.Id).ToList();
                foreach (var sku in checkSkuStock)
                {
                    ModelOrderSku orderSku = listOrderGoods.FirstOrDefault(p => p.GoodsSkuId == sku.Id) ??
                                             new ModelOrderSku();
                    if (sku.StockNum - orderSku.Quantity < 0)
                    {
                        throw new Exception($"{orderSku.GoodsName},{orderSku.SkuName}库存不够!");
                    }
                }

                if (order.OrderStatus == (int) xEnum.OrderStatus.WaitDeliver &&
                    goods.DeductStockType == (int) xEnum.DeductStockType.Paid)
                {
                    listStockSku = listSku.Where(p => p.GoodsId == goods.Id).ToList();
                }

                if (order.OrderStatus == (int) xEnum.OrderStatus.NotPaid &&
                    goods.DeductStockType == (int) xEnum.DeductStockType.Order)
                {
                    listStockSku = listSku.Where(p => p.GoodsId == goods.Id).ToList();
                }

                foreach (var sku in listStockSku)
                {
                    ModelOrderSku orderSku = listOrderGoods.FirstOrDefault(p => p.GoodsSkuId == sku.Id) ??
                                             new ModelOrderSku();
                    sku.StockNum = sku.StockNum - orderSku.Quantity;
                    goods.SalesActual = goods.SalesActual + orderSku.Quantity;
                    UnitWork.AddOrUpdate(sku);
                }

                goods.StockTotal = UnitWork.Sum<ModelGoodsSku>(p => p.GoodsId == goods.Id, p => p.StockNum);
                UnitWork.AddOrUpdate(goods);
            }

            UnitWork.Save();
        }
        
        /// <summary>
        /// 获取个状态算订单数量
        /// </summary>
        public ResGetOrderGroupCount GetOrderGroupCount(ReqGetOrderGroupCount req)
        {
            ResGetOrderGroupCount res = new ResGetOrderGroupCount();
            res.CountNotPaid = UnitWork.Count<ModelOrder>(GetWhereByBigStatus((int)xEnum.OrderBigStatus.NotPaid, true)); 
            res.CountWaitDeliver = UnitWork.Count<ModelOrder>(GetWhereByBigStatus((int)xEnum.OrderBigStatus.WaitDeliver, true) );
            res.CountWaitReceiving = UnitWork.Count<ModelOrder>(GetWhereByBigStatus((int)xEnum.OrderBigStatus.WaitReceiving,true));
            res.CountWaitComment = UnitWork.Count<ModelOrder>(GetWhereByBigStatus((int)xEnum.OrderBigStatus.WaitComment));
            res.CountRefund = UnitWork.Count<ModelOrderRefundSku>(p => p.UserId == _auth.GetCurrentContext().User.Id && p.Status <= (int)xEnum.RefundStatus.Shipped
            && p.Status >= (int)xEnum.RefundStatus.WaitAudit); 
            return res;
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        public void Received(ReqOrderReceived req)
        {
            var order = UnitWork.FirstOrDefault<ModelOrder>(p => p.Id == req.OrderId);
            order.OrderStatus = (int) xEnum.OrderStatus.Done;
            order.DoneTime = DateTime.Now;
            Repository.Update(order);
                
            ModelOrderTrack track = new ModelOrderTrack()
            {
                OutTradeNo = order.Id,
                Attach = order.OrderNo,
                Type = (int)xEnum.OrderTrackType.Order,
                Info = "用户申请售后"
            };
            UnitWork.Add(track);
            UnitWork.Save();
        }

        /// <summary>
        /// 订单自动收货
        /// 订单发货N天，订单状态自动更新为已收货
        /// </summary>
        public void OrderReceipt()
        {
            //N天前
            DateTime endTime = DateTime.Now.AddDays(-Define.DaysReceived);
            //N天前已发货的订单
            List<ModelOrder> listOrder = UnitWork.Find<ModelOrder>(p =>
                p.OrderStatus == (int) xEnum.OrderStatus.WaitReceiving &&
                p.ArrivedTime <= endTime).ToList();
            foreach (var order in listOrder)
            {
                order.OrderStatus = (int) xEnum.OrderStatus.Done;
                order.DoneTime = DateTime.Now;
                UnitWork.Update(order);
                UnitWork.Save();
            }
        }

        /// <summary>
        /// 订单已完成
        /// 订单送达N天，订单状态自动更新为已完成
        /// </summary>
        public void OrderDone()
        {
            //N天前
            DateTime endTime = DateTime.Now.AddDays(-Define.DaysDone);
            //N天前已送达的订单/售后已拒绝
            List<ModelOrder> listOrder = UnitWork.Find<ModelOrder>(p =>
                (p.OrderStatus == (int) xEnum.OrderStatus.Done)
                && p.DoneTime <= endTime).ToList();

            //下单用户
            List<ModelUser> listUser =
                UnitWork.Find<ModelUser>(p => listOrder.Select(p => p.UserId).Contains(p.Id)).ToList();
         
            //更新订单状态

            foreach (var order in listOrder)
            {
                UnitWork.ExecuteWithTransaction(() =>
                {
                    //修改订单状态、完成时间
                    order.OrderStatus = (int) xEnum.OrderStatus.Done;
                    order.DoneTime = DateTime.Now;
                 
                    UnitWork.Update(order);
                    UnitWork.Save();
                });   
            }
        }


        /// <summary>
        /// 查询导出需要导出的订单
        /// </summary>
        /// <param name="req"></param>
        /// <param name="isPage"></param>
        /// <returns></returns>
        public List<ReqExportOrder> ExportOrderByWhere(ReqQuFreightByDriver req, bool isPage = false)
        {
            IQueryable<ModelOrder> linqOrder = ListLinq(req);
            var linq = from order in linqOrder
                       join user in UnitWork.Find<ModelUser>(p => true) on order.UserId equals user.Id
                       join address in UnitWork.Find<ModelOrderAddress>(p => true) on order.Id equals address.OrderId
                       join sku in UnitWork.Find<ModelOrderSku>(p => true) on order.Id equals sku.OrderId
                       select new ReqExportOrder
                       {
                           OrderTypeName = "销售订单",
                           OrderNo = order.OrderNo,
                           UserType = user.UserType,
                           Name = address.Name,
                           Phone = address.Phone,
                           FullAddress = address.FullAddress,
                           GoodsName = sku.GoodsName,
                           SkuName = sku.SkuName,
                           Unit = sku.Unit,
                           TotalNum = sku.Quantity,
                           PayTime = order.PayTime,
                           ArrivedTime = order.ArrivedTime,
                           OrderStatus = order.OrderStatus,
                           IsRefund = order.IsRefund,
                           FreightPrice = order.FreightPrice,
                           BuyerRemark = order.BuyerRemark
                       };
            var list = linq.ToList();
            foreach (var item in list)
            {
                item.StrUserType = xEnum.GetEnumDescription(typeof(xEnum.UserType), item.UserType);
                item.StrExportStatus = GetExportStatus(item);
                item.StrOrderStatus = xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), item.OrderStatus);

            }


            var linqRefund = from order in linqOrder

                             join user in UnitWork.Find<ModelUser>(p => true) on order.UserId equals user.Id
                             join address in UnitWork.Find<ModelOrderAddress>(p => true) on order.Id equals address.OrderId
                             join skuRefund in UnitWork.Find<ModelOrderRefundSku>(p => true) on order.Id equals skuRefund.OrderId
                             join sku in UnitWork.Find<ModelOrderSku>(p => true) on skuRefund.OrderSkuId equals sku.Id
                             select new ReqExportOrder
                             {
                                 OrderTypeName = "售后订单",
                                 OrderNo = order.OrderNo,
                                 UserType = user.UserType,
                                 Name = address.Name,
                                 Phone = address.Phone,
                                 FullAddress = address.FullAddress,
                                 GoodsName = sku.GoodsName,
                                 SkuName = sku.SkuName,
                                 Unit = sku.Unit,
                                 PayTime = order.PayTime,

                                 OrderStatus = xConv.ToInt(skuRefund.Status),
                                 IsRefund = order.IsRefund,
                                 // BuyerRemark = $"申请退货数量:{-skuRefund.TotalNum}"
                             };
            var listRefund = linqRefund.ToList();
            foreach (var item in listRefund)
            {
                item.StrUserType = xEnum.GetEnumDescription(typeof(xEnum.UserType), item.UserType);
                item.StrExportStatus = GetExportStatus(item);
                item.StrOrderStatus = xEnum.GetEnumDescription(typeof(xEnum.RefundStatus), item.OrderStatus);
            }

            list.AddRange(listRefund);
            var index = 0;
            var nextOrderNo = "";
            var nextOrderType = "";
            foreach (var item in list)
            {
                index++;
                if (index >= list.Count)
                {
                    break;
                }
                nextOrderNo = list[index].OrderNo;
                nextOrderType = list[index].OrderTypeName;
                if (item.OrderNo == nextOrderNo && item.OrderTypeName == nextOrderType)
                {
                    item.FreightPrice = null;
                    item.FloorPrice = null;
                    item.EntryPrice = null;
                }
            }
            return list;
        }


    }
}