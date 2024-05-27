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
    public class AppArticleCategory : AppBaseString<ModelArticleCategory, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppArticleCategory(IUnitWork<QsDBContext> unitWork, IRepository<ModelArticleCategory, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuArticleCategory req)
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
        public List<ResArticleCategory> ListByWhere(ReqQuArticleCategory req, bool isPage = false)
        {
            IQueryable<ResArticleCategory> linq = ListLinq(req);
            List<ResArticleCategory> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ResArticleCategory> ListLinq(ReqQuArticleCategory req)
        {
            var linq =from cate in UnitWork.Find<ModelArticleCategory>(p => true)
                join file in UnitWork.Find<ModelFileUpload>(p => true) on cate.ImageId equals file.Id
                      select new ResArticleCategory
                      {
                          Id=cate.Id,
                          Name = cate.Name,
                          Status= cate.Status,
                          Sort = cate.Sort,
                          ImageId= cate.ImageId,
                          UrlIcon = file.FilePath,
                      }
                ;
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Name.Contains(req.Key));
            }
            return linq;
        }


        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuArticleCategory req)
        {
            var model = xConv.CopyMapper<ModelArticleCategory, ReqAuArticleCategory>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                model.CreateTime = System.DateTime.Now;
                model.UpdateTime = System.DateTime.Now;
                Repository.Add(model);
            }
            else
            {
                model.UpdateTime = System.DateTime.Now;
                UnitWork.Update<ModelArticleCategory>(u => u.Id == model.Id, u => new ModelArticleCategory
                {
                    Name = model.Name,
                    ImageId = model.ImageId,
                    Sort = model.Sort,
                    Status = model.Status,
                    UpdateTime = model.UpdateTime,
                });
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResArticleCategory Get(string id)
        {
            var model = UnitWork.FirstOrDefault<ModelArticleCategory>(p => p.Id == id);
            var file= UnitWork.FirstOrDefault<ModelFileUpload>(p => p.Id == model.ImageId);
            ResArticleCategory res = xConv.CopyMapper<ResArticleCategory, ModelArticleCategory>(model);
            res.UrlIcon = file?.Thumbnail;
            return res;
        }
    }
}