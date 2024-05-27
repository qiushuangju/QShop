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
    public class AppArticle : AppBaseString<ModelArticle, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppArticle(IUnitWork<QsDBContext> unitWork, IRepository<ModelArticle, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuArticle req)
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
        public List<ResArticle> ListByWhere(ReqQuArticle req, bool isPage = false)
        {
            IQueryable<ResArticle> linq = ListLinq(req);
            List<ResArticle> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.OrderByDescending(p=>p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ResArticle> ListLinq(ReqQuArticle req)
        {
            var linq = from article in UnitWork.Find<ModelArticle>(p => true)
                join file in UnitWork.Find<ModelFileUpload>(p => true) on article.ImageId equals file.Id
                join category in  UnitWork.Find<ModelArticleCategory>(p => true) on article.CategoryId equals category.Id
                       select new ResArticle
                       {
                           Id = article.Id,
                           Title = article.Title,
                           CategoryId=article.CategoryId  ,
                           Content=article.Content,
                           Sort=article.Sort,
                           Status=article.Status,
                           IsDelete = article.IsDelete,
                           UrlCover = file.FilePath,
                           CategoryName= category.Name,
                           
                       };
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Title.Contains(req.Key));
            }
            if (!string.IsNullOrEmpty(req.CateId))
            {
                linq = linq.Where(p => p.CategoryId.Contains(req.CateId));
            }
            return linq;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuArticle req)
        {
            var model = xConv.CopyMapper<ModelArticle, ReqAuArticle>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                model.CreateTime = System.DateTime.Now;
                model.UpdateTime = System.DateTime.Now;
                model.IsDelete = "0";
                Repository.Add(model);
            }
            else
            {
                model.UpdateTime = System.DateTime.Now;
                UnitWork.Update<ModelArticle>(u => u.Id == model.Id, u => new ModelArticle
                {
                    Title = model.Title,
                    ImageId = model.ImageId,
                    Content = model.Content,
                    CategoryId= model.CategoryId,
                    Sort= model.Sort,
                    Status=model.Status,
                    UpdateTime= model.UpdateTime,
                    IsDelete=model.IsDelete,
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResArticle Get(string id)
        {
            var model = UnitWork.FirstOrDefault<ModelArticle>(p => p.Id == id);
            var file = UnitWork.FirstOrDefault<ModelFileUpload>(p => p.Id == model.ImageId);
            var category= UnitWork.FirstOrDefault<ModelArticleCategory>(p => p.Id == model.CategoryId);
            ResArticle res = xConv.CopyMapper<ResArticle, ModelArticle>(model);
            res.UrlCover = file?.Thumbnail;
            res.CategoryName= category?.Name;
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Delete(string[] ids)
        {
            var list = UnitWork.Find<ModelArticle>(p => ids.Contains(p.Id));
            foreach (var item in list)
            {
                item.IsDelete = "1";
                UnitWork.Update(item);
            }
        }

    }
}