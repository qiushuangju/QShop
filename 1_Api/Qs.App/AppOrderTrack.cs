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
    ///  App
    /// </summary>
    public class AppOrderTrack : AppBaseString<ModelOrderTrack, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppOrderTrack(IUnitWork<QsDBContext> unitWork, IRepository<ModelOrderTrack, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuOrderTrack req)
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
        public List<ModelOrderTrack> ListByWhere(ReqQuOrderTrack req, bool isPage = false)
        {
            IQueryable<ModelOrderTrack> linq = ListLinq(req);
            List<ModelOrderTrack> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelOrderTrack> ListLinq(ReqQuOrderTrack req)
        {
            var linq = UnitWork.Find<ModelOrderTrack>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Attach.Contains(req.Key));
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
        public void AddOrUpdate(ReqAuOrderTrack req)
        {
            var model = xConv.CopyMapper<ModelOrderTrack, ReqAuOrderTrack>(req);
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