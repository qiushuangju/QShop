using System;
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
    /// 用户余额变动明细表 App
    /// </summary>
    public class AppUserDrawMoneyLog : AppBaseString<ModelUserDrawMoneyLog, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppUserDrawMoneyLog(IUnitWork<QsDBContext> unitWork, IRepository<ModelUserDrawMoneyLog, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuUserDrawMoneyLog req)
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
        public List<ResUserDrawMoneyLog> ListByWhere(ReqQuUserDrawMoneyLog req, bool isPage = false)
        {
            IQueryable<ResUserDrawMoneyLog> linq = ListLinq(req);
            List<ResUserDrawMoneyLog> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            foreach (var item in list)
            {
                item.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.DrawMoneyStatus), item.Status);
            }
            return list;
        }

        /// <summary>
        /// 根据id查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResUserDrawMoneyLog GetDetails(string id)
        {
            IQueryable<ResUserDrawMoneyLog> linq = ListLinq(new ReqQuUserDrawMoneyLog() { Id = id });
            ResUserDrawMoneyLog res = linq.FirstOrDefault();
            if (res != null)
            {
                res.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.DrawMoneyStatus), res.Status);
            }
            return linq.FirstOrDefault();
        }


        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ResUserDrawMoneyLog> ListLinq(ReqQuUserDrawMoneyLog req)
        {
            var linq =from log in  UnitWork.Find<ModelUserDrawMoneyLog>(p => true)
                      join user in UnitWork.Find<ModelUser>(p=>true) on log.UserId equals user.Id
                      select  new ResUserDrawMoneyLog
                      {
                          Id=log.Id,
                          UserId=log.UserId,
                          RealName=log.RealName,
                          BankCard=log.BankCard,
                          BankName=log.BankName,
                          OpeningBankName=log.OpeningBankName,
                          Describe=log.Describe,
                          Status=log.Status,
                          FailureRemark=log.FailureRemark,
                          IncomeMoney=log.IncomeMoney,
                          Money= log.Money,
                          StoreId=log.StoreId,
                          CreateTime=log.CreateTime,
                          Phone=user.Phone          ,
                          
        };
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Phone.Contains(req.Key) || p.RealName.Contains(req.Key));
            }
            if (!string.IsNullOrEmpty(req.Id))
            {
                linq = linq.Where(p => p.Id.Contains(req.Id));
            }  
            if (!string.IsNullOrEmpty(req.UserId))
            {
                linq = linq.Where(p => p.UserId== req.UserId);
            }
            // if (!string.IsNullOrEmpty(req.StoreId))
            // {
            //     linq = linq.Where(p => p.StoreId == req.StoreId);
            // }
            if (!string.IsNullOrEmpty(req.Phone))
            {
                linq = linq.Where(p => p.Phone.Contains(req.Phone));
            }
            if (xConv.ToInt(req.Status) != 0)
            {
                linq = linq.Where(p => p.Status == req.Status);
            }
            if (req.StartDate != null)
            {
                linq = linq.Where(p => p.CreateTime >= req.StartDate);
            }
            if (req.EndDate != null)
            {
                linq = linq.Where(p => p.CreateTime <= req.EndDate);
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
        public void AddOrUpdate(ReqAuUserDrawMoneyLog req)
        {
            var model = xConv.CopyMapper<ModelUserDrawMoneyLog, ReqAuUserDrawMoneyLog>(req);
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
        /// 正在处理
        /// </summary>
        /// <param name="ids"></param>
        public void DealWithDrawMoney(string[] ids)
        {
            //申请记录
            List<ModelUserDrawMoneyLog> listLog = UnitWork.Find<ModelUserDrawMoneyLog>(p => ids.Contains(p.Id)).ToList();
            listLog.ForEach(element =>
            {
                element.Status = (int)xEnum.DrawMoneyStatus.DealWith;
            });
            UnitWork.Update(listLog);
        }

        /// <summary>
        /// 提现成功
        /// </summary>
        /// <param name="ids"></param>
        public void SuccessDrawMoney(string[] ids)
        {
            //申请记录
            List<ModelUserDrawMoneyLog> listLog = UnitWork.Find<ModelUserDrawMoneyLog>(p => ids.Contains(p.Id)).ToList();
            //用户
            List<ModelUser> listUser = UnitWork.Find<ModelUser>(p => listLog.Select(p => p.UserId).Contains(p.Id)).ToList();
            UnitWork.ExecuteWithTransaction(() =>
            {
                foreach (var user in listUser)
                {
                    List<ModelUserDrawMoneyLog> logs = listLog.FindAll(p => p.UserId == user.Id);
                    //提现总额
                    decimal drawMoneyTotal = logs.Sum(p => p.Money);
                 
                    //修改申请状态
                    logs.ForEach(element =>
                    {
                        element.Status = (int)xEnum.DrawMoneyStatus.Success;
                    });
                    UnitWork.Update(logs);
                }
                UnitWork.Save();
            });
        }


        /// <summary>
        /// 提现失败
        /// </summary>
        /// <param name="req"></param>
        public void FailureDrawMoney(ReqDrawMoneyFailure req)
        {
            req.Check();
            //提现申请
            List<ModelUserDrawMoneyLog> listLog = UnitWork.Find<ModelUserDrawMoneyLog>(p => req.Ids.Contains(p.Id)).ToList();
            //用户
            List<string> userIds = listLog.Select(p => p.UserId).ToList();
            List<ModelUser> listUser = UnitWork.Find<ModelUser>(p => userIds.Contains(p.Id)).ToList();
            UnitWork.ExecuteWithTransaction(() =>
            {
                foreach (var user in listUser)
                {
                    List<ModelUserDrawMoneyLog> logs = listLog.FindAll(p => p.UserId == user.Id);
                    //提现申请总额
                    decimal drawMoneyTotal = logs.Sum(p => p.Money);
                    //修改用户佣金金额
                    user.IncomeMoney += drawMoneyTotal;
                    //修改申请记录状态、失败原因
                    logs.ForEach(element =>
                    {
                        element.Status = (int)xEnum.DrawMoneyStatus.Failure;
                        element.FailureRemark = req.FailureRemark;
                    });
                    UnitWork.Update(logs);
                }
                UnitWork.Save();
            });
        }
    }
}