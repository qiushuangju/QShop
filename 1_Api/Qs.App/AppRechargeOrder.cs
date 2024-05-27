using System;
using System.Collections.Generic;
using System.Linq;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.App.Wx;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Vm;

namespace Qs.App
{
    /// <summary>
    /// 应用层
    /// </summary>
    public class AppRechargeOrder : AppBaseString<ModelRechargeOrder, QsDBContext>
    {
        private AppStoreSetting _appStoreSetting;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppRechargeOrder(IUnitWork<QsDBContext> unitWork, IRepository<ModelRechargeOrder, QsDBContext> repository,
            AppStoreSetting appStoreSetting, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _appStoreSetting = appStoreSetting;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuRechargeOrder req)
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
        public List<ResRechargeOrder> ListByWhere(ReqQuRechargeOrder req, bool isPage = false)
        {
            IQueryable<ModelRechargeOrder> linq = ListLinq(req);
            List<ModelRechargeOrder> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            List<ResRechargeOrder> resList = new List<ResRechargeOrder>();
            //商城信息
            List<string> storeIds = list.Select(p => p.StoreId).ToList();
            //用户信息
            List<string> userIds = list.Select(p => p.UserId).ToList();
            var listUser = UnitWork.Find<ModelUser>(p => userIds.Contains(p.Id));
            //套餐信息
            List<string> planIds= list.Select(p => p.PlanId).ToList();
            var listPlan = UnitWork.Find<ModelRechargePlan>(p => planIds.Contains(p.Id));
            foreach (var item in list)
            {
                var user = listUser.FirstOrDefault(p => p.Id == item.UserId);
                
                var plan = listPlan.FirstOrDefault(p => p.Id == item.PlanId);
                resList.Add(ResRechargeOrder.ToView(item, user,  plan));
            }
            return resList.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelRechargeOrder> ListLinq(ReqQuRechargeOrder req)
        {
            var linq = UnitWork.Find<ModelRechargeOrder>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.PlanId.Contains(req.Key)||p.OrderNo.Contains(req.Key));
            }
            if (req.StartTime != null)
            {
                linq = linq.Where(p => p.CreateTime >= req.StartTime);
            }
            if (req.EndTime != null)
            {
                linq = linq.Where(p => p.CreateTime <= req.EndTime);
            }
            if (xConv.ToInt(req.PayStatus)>0)
            {
                linq = linq.Where(p => p.PayStatus == req.PayStatus);
            }
            if (xConv.ToInt(req.RechargeType) > 0)
            {
                linq = linq.Where(p => p.RechargeType == req.RechargeType);
            }
            return linq;
        }

        /// <summary>
        /// 充值前调用
        /// </summary>
        public ResPayConfirm RechargeBefore(ReqRechargeBefore req)
        {
            var user = _auth.GetCurrentContext().User;
            var storeId = _auth.GetStoreId();
            var platform = _auth.GetPlatform();
            var listPlan = UnitWork.Find<ModelRechargePlan>(p => p.StoreId == storeId).ToList();
            ModelRechargePlan plan = new ModelRechargePlan();
            int rechargeType = 0;
            decimal payPrice = 0M;

            VmSettingRecharge recharge = _appStoreSetting.GetDetail(DefineSetting.Recharge);
            
            if (!string.IsNullOrEmpty(req.PlanId))
            {
                rechargeType = (int) xEnum.RechargeType.Package;
                plan = listPlan.FirstOrDefault(p => p.Id == req.PlanId) ?? new ModelRechargePlan();
                payPrice = plan.Money;
            }
            else if (req.CustomMoney != null)
            {
                rechargeType = (int)xEnum.RechargeType.Custom;
                if (recharge.IsMatchPlan)
                {
                    plan = listPlan.Where(p => req.CustomMoney >= p.Money).OrderByDescending(p => p.Money)
                        .FirstOrDefault() ?? new ModelRechargePlan();
                }
                payPrice = xConv.ToDecimal(req.CustomMoney);
            }

            ModelRechargeOrder order = new ModelRechargeOrder()
            {
                Id = xConv.NewGuid(),
                OrderNo = $"CZ{xConv.GenerateNo()}",
                PayPrice = payPrice,
                GiftMoney = plan.GiftMoney,
                ActualMoney = payPrice + plan.GiftMoney,
                CreateTime = DateTime.Now,
                PayStatus = (int) xEnum.PayStatus.Unpaid,
                PlanId = plan.Id,
                RechargeType = rechargeType,
                StoreId = storeId,
                UserId = user.Id
            };

            if (!string.IsNullOrEmpty(plan.Id))
            {
                ModelRechargeOrderPlan orderPlan = new ModelRechargeOrderPlan()
                {
                    OrderId = order.Id,
                    PlanId = plan.Id,
                    PlanName = plan.PlanName,
                    Money = plan.Money,
                    GiftMoney = plan.GiftMoney,
                    StoreId = plan.StoreId,
                    CreateTime = DateTime.Now
                };
                UnitWork.Add(orderPlan);
            }

            UnitWork.Add(order);
            UnitWork.Save();
            VmSettingBasicWxApp setting = _appStoreSetting.GetDetail(DefineSetting.BasicWxApp);
            var pay = new PayWx(setting);

            string payAttach = "Recharge";
            decimal amount = GetPayAmount(user, order.PayPrice);
            var payParams =
                pay.GetPayParams(user.WxOpenId, amount, "充值", order.Id, payAttach, platform);
            var res = new ResPayConfirm();
            res.WxPayParams = payParams;
            res.AmountTotal = amount;
            return res;
        }

        /// <summary>
        /// 测试用户支付0.01
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public decimal GetPayAmount(ModelUser user, decimal? amount)
        {
            var setting = UnitWork.FirstOrDefault<ModelSysSetting>(p => p.Key == "WxPayPhone") ?? new ModelSysSetting();
            List<string> listPhone = xConv.ToListString(setting.Values);
            decimal payAmout = xConv.ToDecimal(amount);
            if (listPhone.Contains(user.Phone))
            {
                payAmout = 0.01M;
            }
            return payAmout;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuRechargeOrder req)
        {
            var model = xConv.CopyMapper<ModelRechargeOrder, ReqAuRechargeOrder>(req);
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
        
    }
}