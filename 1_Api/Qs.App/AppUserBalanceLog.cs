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
using Qs.Repository.Vm;

namespace Qs.App
{
    /// <summary>
    /// 应用层
    /// </summary>
    public class AppUserBalanceLog : AppBaseString<ModelUserBalanceLog, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppUserBalanceLog(IUnitWork<QsDBContext> unitWork,
            IRepository<ModelUserBalanceLog, QsDBContext> repository,
            AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension,
            auth)
        {
            _revelanceApp = app;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuUserBalanceLog req)
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
        public List<ResUserBalanceLog> ListByWhere(ReqQuUserBalanceLog req, bool isPage = false)
        {
            IQueryable<ResUserBalanceLog> linq = ListLinq(req).OrderByDescending(p => p.CreateTime);
            List<ResUserBalanceLog> list =
                isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            //List<ResUserBalanceLog> listVm = new List<ResUserBalanceLog>();
            foreach (var item in list)
            {
                item.StrScene = xEnum.GetEnumDescription(typeof(xEnum.BalanceType), item.Scene);
                //listVm.Add(ResUserBalanceLog.ToView(item));
            }
            return list.ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ResUserBalanceLog> ListLinq(ReqQuUserBalanceLog req)
        {
            //var linq = UnitWork.Find<ModelUserBalanceLog>(p => true);
            var linq = from log in UnitWork.Find<ModelUserBalanceLog>(p => true)
                       join user in UnitWork.Find<ModelUser>(p => true) on log.UserId equals user.Id
                       select new ResUserBalanceLog
                       {
                           Id=log.Id,
                           UserId = log.UserId,
                           OrderId = log.OrderId,
                           Scene = log.Scene,
                           Money = log.Money,
                           Describe = log.Describe,
                           Remark = log.Remark,
                           StoreId = log.StoreId,
                           CreateTime = log.CreateTime,
                           Account= user.Account,
                           UrlAvater=user.UrlAvater,
                           Name=user.NickName,
                           Phone=user.Phone
                       };

            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Name.Contains(req.Key)|| p.Phone.Contains(req.Key));
            }
            if (req.OnlyMy)
            {
                var user = _auth.GetCurrentContext().User;
                linq = linq.Where(p => p.UserId == user.Id);
            }
            if (req.Scene > 0)
            {
                linq = linq.Where(p => p.Scene.Equals(req.Scene));
            }

            if (!string.IsNullOrEmpty(req.Account))
            {
                linq = linq.Where(p => p.Account.Contains(req.Account));
            }

            if (!string.IsNullOrEmpty(req.Name))
            {
                linq = linq.Where(p => p.Name.Contains(req.Name));
            }

            return linq.OrderByDescending(p=>p.CreateTime);
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuUserBalanceLog req)
        {
            var model = xConv.CopyMapper<ModelUserBalanceLog, ReqAuUserBalanceLog>(req);
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