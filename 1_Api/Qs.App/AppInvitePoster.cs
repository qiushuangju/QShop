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
    public class AppInvitePoster : AppBaseString<ModelInvitePoster, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppInvitePoster(IUnitWork<QsDBContext> unitWork, IRepository<ModelInvitePoster, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResPoster Get(string id)
        {
            var linq = from poster in UnitWork.Find<ModelInvitePoster>(p => true)
                       join file in UnitWork.Find<ModelFileUpload>(p => true) on poster.ImageId equals file.Id
                       where poster.Id == id
                       select new ResPoster
                       {
                           Id = poster.Id,
                           PosterName = poster.PosterName,
                           ImageId = poster.ImageId,
                           UserId = poster.UserId,
                           CountRegUser = poster.CountRegUser,
                           CreateTime = poster.CreateTime,
                           UrlThumbnail = file.Thumbnail,
                           UrlImg = file.FilePath
                       };
            return linq.FirstOrDefault();
        }

            /// <summary>
            /// 加载列表
            /// </summary>
        public TableData Load(ReqQuInvitePoster req)
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
        public List<ResPoster> ListByWhere(ReqQuInvitePoster req, bool isPage = false)
        {
            IQueryable<ResPoster> linq = ListLinq(req);
            List<ResPoster> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ResPoster> ListLinq(ReqQuInvitePoster req)
        {
            var linq = from poster in UnitWork.Find<ModelInvitePoster>(p => true)
                join file in UnitWork.Find<ModelFileUpload>(p => true) on poster.ImageId equals file.Id
                select new ResPoster
                {
                    Id = poster.Id,
                    PosterName = poster.PosterName,
                    ImageId = poster.ImageId,
                    UserId = poster.UserId,
                    CountRegUser = poster.CountRegUser,
                    CreateTime = poster.CreateTime,
                    UrlThumbnail = file.Thumbnail,
                    UrlImg = file.FilePath
                };
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.PosterName.Contains(req.Key));
            }

            return linq;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuInvitePoster req)
        {
            var user = _auth.GetCurrentContext().User;
            var model = xConv.CopyMapper<ModelInvitePoster, ReqAuInvitePoster>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                model.UserId = user.Id;
                model.CreateTime = System.DateTime.Now;
                Repository.Add(model);
            }
            else
            {
                Repository.Update(model);
            }
        }
        
    }
}