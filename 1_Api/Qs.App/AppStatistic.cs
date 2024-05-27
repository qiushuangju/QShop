using System;
using System.Collections.Generic;
using System.Linq;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.UserManager.Response;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Vm;

namespace Qs.App
{
    public class AppStatistic : AppBaseString<ModelGoods, QsDBContext>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppStatistic(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoods, QsDBContext> repository,
            DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
        }


        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public ResTeamStatistics StatisticTeam(ReqTeamStatistics req)
        {
            var userId = _auth.GetCurrentContext().User.Id;
            List<ModelUser> listUser = UnitWork.Find<ModelUser>(p => p.SourceUserId.Contains(userId))
                .OrderByDescending(p => p.UserType).ThenBy(p => p.CreateTime).ToList();
            var listUserDealers = listUser.Where(p => p.UserType == (int) xEnum.UserType.Agent).ToList();
            //var listUserDealers = listUser;
            ResTeamStatistics res = new ResTeamStatistics
            {
                //TotalTeam = listUser.Count(),
                TotalDealers = listUserDealers.Count(),
                TotalCustomer = listUser.Count(p => p.UserType == (int) xEnum.UserType.Customer),


            };
            var startDate = xConv.ToDate(req.StrStartDate);
            var endDate = xConv.ToDate(req.StrEndDate).AddDays(1);
            List<ModelOrderSku> listOrder = UnitWork.Find<ModelOrderSku>(p => p.PayTime > startDate &&
                                                                              p.PayTime < endDate
                                                                              && p.IsRefund ==
                                                                              (int) xEnum.ComStatus.Disable &&
                                                                              p.SourceUserId.Contains(userId)).ToList();
            List<VmTeamStatistics> list = new List<VmTeamStatistics>();
            foreach (var user in listUserDealers)
            {
                var totaltOrder = listOrder.Where(p => p.SourceUserId.Contains(user.Id)).Count();
                var totalAmount = listOrder.Where(p => p.SourceUserId.Contains(user.Id)).Sum(p => p.SalePrice);
                var totalUser = listUser.Count(p => p.ParentId == user.Id);
                VmTeamStatistics vm = new VmTeamStatistics()
                {
                    Name = user.NickName,
                    StrUserType = xEnum.GetEnumDescription(typeof(xEnum.UserType), user.UserType),
                    UserAvater = user.UrlAvater,
                    TotalCustomer = totalUser,
                    TotalOrder = totaltOrder,
                    TotalGoodsAmount = totalAmount,
                    CreateTime = user.CreateTime
                };
                list.Add(vm);
            }

            res.TotalGoodsAmount = listOrder.Sum(p => p.SalePrice);
            res.ListData = list;
            return res;
        }

        /// <summary>
        /// 统计数据
        /// </summary>
        /// <returns></returns>
        public ResPcHomePage LoadPcHomePageData()
        {
            var storeId=_auth.GetStoreId();
            ResPcHomePage res = new ResPcHomePage();
            res.Total = new Total()
            {
                TotalOrderWaitDeliver = UnitWork.Count<ModelOrder>(p =>p.StoreId== storeId&& p.OrderStatus == (int)xEnum.OrderStatus.WaitDeliver && p.IsRefund == (int)      xEnum.ComStatus.Disable),
                TotalOrdertWaitRefund = UnitWork.Count<ModelOrderRefundSku>(p => p.StoreId == storeId && p.Status == (int)xEnum.RefundStatus.RefundedMoney),
                TotalOrdertWaitPay = UnitWork.Sum<ModelOrderSku>(p => p.StoreId == storeId && p.OrderStatus == (int)xEnum.OrderStatus.NotPaid && p.IsRefund == (int)xEnum.ComStatus.Disable, p => p.Quantity),
                TotalWarningStock = UnitWork.Count<ModelGoods>(p => p.StoreId == storeId && p.StockTotal<10),
                TotalCustomer= UnitWork.Count<ModelUser>(p => p.StoreId == storeId && p.UserType == (int)xEnum.UserType.Customer),
            };
            DateTime dtToday=xConv.ToDate(DateTime.Now) ;
            res.Today = new Today()
            {
                TotalAmountGoods= UnitWork.Sum<ModelOrderSku>(p => p.StoreId == storeId && p.OrderStatus >= (int)xEnum.OrderStatus.WaitDeliver&&p.IsRefund==(int)xEnum.ComStatus.Disable
                            && p.CreateTime > dtToday,p=>p.SalePrice),
                TotalOrder = UnitWork.Count<ModelOrderSku>(p => p.StoreId == storeId && p.OrderStatus >= (int)xEnum.OrderStatus.WaitDeliver && p.IsRefund == (int)xEnum.ComStatus.Disable
                    && p.CreateTime > dtToday),
                TotalNewCustomer = UnitWork.Count<ModelUser>(p => p.StoreId == storeId && p.UserType == (int)xEnum.UserType.Customer && p.CreateTime > dtToday),
              
            };


            DateTime startTime =xConv.ToDate(DateTime.Now.AddDays(-6)) ;
            DateTime endTime = xConv.ToDate(DateTime.Now.AddDays(1)) ;
            List<VmStatisticOrderByStatus> list = UnitWork.Find<ModelOrderSku>(p => p.StoreId == storeId &&
                    p.OrderStatus >= (int) xEnum.OrderStatus.WaitDeliver && p.IsRefund == (int) xEnum.ComStatus.Disable
                    && p.PayTime > startTime && p.PayTime < endTime)
                .GroupBy(p =>(p.PayTime.Value.Date)).Select(p=>new VmStatisticOrderByStatus
                {
                    PayDate=xConv.ToDate(p.Key),
                    OrderCount = p.Count(),
                    PayPriceTotal = p.Sum(p=>p.SalePrice)
                }).ToList();

           //统计近一周内每天的订单数和交易额
            List<VmEchart> listEchart = new List<VmEchart>();
            for (int i = 0; i < 7; i++)
            {
                DateTime time = DateTime.Now.AddDays(-i).Date;
                List<VmStatisticOrderByStatus> listDay = list.FindAll(p => p.PayDate == time);
                VmEchart vmData = new VmEchart()
                {
                    Data = time,
                    OrderCount = listDay.Sum(p => p.OrderCount),
                    PayOrderPrice = listDay.Sum(p => p.PayPriceTotal)
                };
                listEchart.Add(vmData);
            }

            res.ListEchart = listEchart;

            return res;
        }


        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<VmStatisticOperate> ListStatisticOperate(ReqStatisticOperate req)
        {

            var startDate = xConv.ToDate(req.StartDate);
            var endDate = xConv.ToDate(req.EndDate).AddDays(1);
            var listLinq = (from user in UnitWork.Find<ModelUser>(p => (p.UserType >= (int)xEnum.UserType.Agent )
                || (p.UserType == (int)xEnum.UserType.Customer && p.ParentId!=""))
                            select new VmStatisticOperate
                            {
                                UserId = user.Id,
                                RealName=user.RealName,
                                Name = user.NickName,
                                ParentId = user.ParentId,
                                SourceUserId = user.SourceUserId,
                                Phone = user.Phone,
                                UserType = user.UserType,
                                CreateTime = user.CreateTime,
                            });
            if (!string.IsNullOrEmpty(req.ParentUserId))
            {
                listLinq = listLinq.Where(p => p.SourceUserId.Contains(req.ParentUserId));
            }

            var list = listLinq.OrderByDescending(p => p.UserType).ToList();
            foreach (var item in list)
            {
                if (item.Phone == req.Key || item.Name == req.Key|| item.UserId== req.ParentUserId)
                {
                    item.ParentId = "";
                }
            }
            //List<VmStatisticOpUserOrderData> listOrderTotalPay = ListUserOrderTotal(req, "Pay");
            // List<VmStatisticOpUserOrderData> listUserOrderTotalDone = ListUserOrderTotal(req, "Done");
            var listUserId = list.Select(p => p.UserId).ToList();
            List<ModelInviteLinkRecord> listInviteRecord = UnitWork.Find<ModelInviteLinkRecord>(p => listUserId.Contains(p.InviterId) 
                && p.OpenTime > startDate && p.OpenTime < endDate).ToList();

            ModelUser rootUser = null;
            if (!string.IsNullOrEmpty(req.Key))
            {
                rootUser = UnitWork.FirstOrDefault<ModelUser>(p => p.Phone == req.Key || p.NickName == req.Key);
            }
            if (!string.IsNullOrEmpty(req.Key) && rootUser != null)
            {
                list = list.Where(p => p.SourceUserId.Contains(rootUser.Id)).ToList();
            }

            foreach (var item in list)
            {
                // var paySonList = listOrderTotalPay.Where(p => p.ParentId == item.UserId).ToList();
                // var doneSonList = listUserOrderTotalDone.Where(p => p.ParentId == item.UserId).ToList();
                // var teamDoneSonList = listUserOrderTotalDone.Where(p => p.SourceUserId.Contains(item.UserId)).ToList();

                //var teamPayList = listOrderTotalPay.Where(p => p.SourceUserId.Contains(item.UserId)).ToList();

                var sonCount = list.Count(p => p.ParentId == item.UserId);
                var teamCount = list.Count(p => p.SourceUserId.Contains(item.UserId) && p.UserId != item.UserId);

                item.SonCount = sonCount;
                item.TeamCount = teamCount;
                item.RecommendDealers = listInviteRecord.Count(p =>
                    p.InviterId == item.UserId && p.OpenType == (int)xEnum.UserType.Agent);
                item.RecommendCustomer = listInviteRecord.Count(p =>
                    p.InviterId == item.UserId && p.OpenType == (int)xEnum.UserType.Customer);
                //item.TeamGoodsPrice = teamPayList.Sum(p => p.GoodsPrice);
                var sourceCount = xConv.ToListString(item.SourceUserId).Count;
                item.Level = sourceCount <= 1 ? 1 : sourceCount;


                // item.SonGoodsPrice = paySonList.Sum(p => p.GoodsPrice);
                // item.SonTotalFreightPrice = paySonList.Sum(p => p.TotalFreightPrice);
                // item.SonOrderPrice = paySonList.Sum(p => p.OrderPrice);
                // item.SonDoneGoodsPrice = doneSonList.Sum(p => p.GoodsPrice);
                // item.SonDoneTotalFreightPrice = doneSonList.Sum(p => p.TotalFreightPrice);
                // item.SonDoneOrderPrice = doneSonList.Sum(p => p.OrderPrice);
                // item.TeamTotalFreightPrice = teamPaySonList.Sum(p => p.TotalFreightPrice);
                // item.TeamOrderPrice = teamPaySonList.Sum(p => p.OrderPrice);
                // item.TeamDoneGoodsPrice = teamDoneSonList.Sum(p => p.GoodsPrice);
                // item.TeamDoneTotalFreightPrice = teamDoneSonList.Sum(p => p.TotalFreightPrice);
                // item.TeamDoneOrderPrice = teamDoneSonList.Sum(p => p.OrderPrice);
            }
            return list;
        }



        /// <summary>
        /// 用户订单总计
        /// </summary>
        /// <param name="req"></param>
        /// <param name="strType"></param>
        /// <returns></returns>
        public List<VmStatisticOpUserOrderData> ListUserOrderTotal(ReqStatisticOperate req, string strType)
        {
            string sqlWhere = "";

            if (strType == "Pay")
            {
                if (!string.IsNullOrEmpty(req.StartDate))
                {
                    sqlWhere += $" And o.PayTime>='{xConv.ToDate(req.StartDate)}' ";
                }
                if (!string.IsNullOrEmpty(req.EndDate))
                {
                    sqlWhere += $" And o.PayTime<='{xConv.ToDate(req.EndDate).AddDays(1)}'";
                }
            }

            if (strType == "Done")
            {
                if (!string.IsNullOrEmpty(req.StartDate))
                {
                    sqlWhere += $" And o.DoneTime>='{xConv.ToDate(req.StartDate)}' ";
                }
                if (!string.IsNullOrEmpty(req.EndDate))
                {
                    sqlWhere += $" And o.DoneTime<='{xConv.ToDate(req.EndDate).AddDays(1)}'";
                }
            }
            //string sql = $@"SELECT MAX(u.Phone) Phone,u.Id UserId,MAX(u.ParentId) ParentId,MAX(u.SourceUserId) SourceUserId,
            //                SUM(o.TotalGoodsPrice) GoodsPrice FROM dbo.tb_User u, dbo.tb_OrderSku o
            //            WHERE o.UserId=u.Id 
            //            AND u.UserType>={(int)xEnum.UserType.Customer} AND u.UserType<={(int)xEnum.UserType.Agent}
            //           {sqlWhere}   GROUP BY u.Id ";
            //return UnitWork.Query<VmStatisticOpUserOrderData>(sql).ToList();
            return new List<VmStatisticOpUserOrderData>();
        }

    }
}
