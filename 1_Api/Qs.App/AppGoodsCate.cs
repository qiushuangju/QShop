using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

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
    public class AppGoodsCate : AppBaseString<ModelGoodsCate, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppGoodsCate(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoodsCate, QsDBContext> repository,
            AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension,
            auth)
        {
            _revelanceApp = app;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuGoodsCategory req)
        {
            var user = _auth.GetCurrentContext().User;
            var result = new TableData();
            result.Result = ListByWhere(req, true);
            result.Count = ListLinq(req).Count();
            return result;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResGoodsCate> ListByWhere(ReqQuGoodsCategory req, bool isPage = false)
        {
            IQueryable<ResGoodsCate> linq = ListLinq(req);
            List<ResGoodsCate> list =
                isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.OrderBy(p => p.SortNo).ToList();
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResGoodsCate Get(string id)
        {
            var linq = from cate in UnitWork.Find<ModelGoodsCate>(p => true)
                join file in UnitWork.Find<ModelFileUpload>(p => true) on cate.ImageId equals file.Id into temp
                from fileItem in temp.DefaultIfEmpty()
                where cate.Id == id
                select new ResGoodsCate
                {
                    Id = cate.Id,
                    Name = cate.Name,
                    ParentId = cate.ParentId,
                    Level = cate.Level,
                    Status = cate.Status,
                    StoreId = cate.StoreId,
                    CreateTime = cate.CreateTime,
                    SortNo = cate.SortNo,
                    CateId = cate.CateId,
                    Cate2Id = cate.Cate2Id,
                    Cate3Id = cate.Cate3Id,
                    UrlImg = fileItem == null ? "" : fileItem.FilePath,
                    ImageId = fileItem == null ? "" : fileItem.Id
                };
            return linq.FirstOrDefault();
        }


        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ResGoodsCate> ListLinq(ReqQuGoodsCategory req)
        {
            var linq = from cate in UnitWork.Find<ModelGoodsCate>(null)
                join file in UnitWork.Find<ModelFileUpload>(p => true) on cate.ImageId equals file.Id
                select new ResGoodsCate
                {
                    Id = cate.Id,
                    Name = cate.Name,
                    ParentId = string.IsNullOrEmpty(cate.ParentId)?null: cate.ParentId,
                    Level = cate.Level,
                    SortNo = cate.SortNo,
                    Status = cate.Status,
                    StoreId = cate.StoreId,
                    UrlImg = file.FilePath,
                    CateId = cate.CateId,
                    Cate2Id = cate.Cate2Id,
                    Cate3Id = cate.Cate3Id,
                };
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Name.Contains(req.Key));
            }

            if (req.ListCateId != null && req.ListCateId.Count > 0)
            {
                linq = linq.Where(p => req.ListCateId.Contains(p.Id));
            }
            if (xConv.ToInt(req.Status)!=0)
            {
                linq = linq.Where(p => req.Status== req.Status);
            }
            if (req.Level != null)
            {
                linq = linq.Where(p => p.Level == xConv.ToInt(req.Level));
            }
            return linq;
        }

        /// <summary>
        /// 查询前端显示分类
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ResGoodsCate> ListShowCate(ReqQuGoodsCategory req)
        {
            var linq = ListLinq(new ReqQuGoodsCategory() { });
            return linq.ToList();
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuGoodsCate req)
        {
            var user = _auth.GetCurrentContext().User;
            var modelDb = Repository.FirstOrDefault(p => p.Id == req.Id);
            var model = xConv.CopyMapper<ModelGoodsCate, ReqAuGoodsCate>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                model.Id = xConv.NewGuid();
            }
            var parent = Repository.FirstOrDefault(p => p.Id == req.ParentId)??new ModelGoodsCate();
            model.Level = xConv.ToInt(parent.Level) + 1;
            if (model.Level==1)
            {
                model.CateId = model.Id;
            }
            if (model.Level == 2)
            {
                model.CateId = parent.Id;
                model.Cate2Id = model.Id;
            }
            if (model.Level == 3)
            {
                model.CateId = parent.CateId;
                model.Cate2Id = parent.Cate2Id;
                model.Cate3Id = model.Id;
            }
            model.StoreId = user.StoreId;
            if (isNew)
            {   
                Repository.Add(model);
            }
            else
            {
                model.Status = modelDb.Status;
                Repository.Update(model);
            }
        }

        /// <summary>
        /// 修改是否首页展示
        /// </summary>
        public void ChangeStatus(ReqAuChangeCateStatus req)
        {
          req.Check();
          UnitWork.Update<ModelGoodsCate>(u => u.Id == req.Id, u => new ModelGoodsCate()
          {
             Status = req.Status
          });
          UnitWork.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override void Delete(string[] ids)
        {
            foreach(var id in ids)
            {
                var countCateSon = UnitWork.Count<ModelGoodsCate>(p => p.ParentId == id);
                if (countCateSon > 0)
                {
                    throw new Exception("当前分类下存在子分类，不允许删除");
                }
                var countGoods = UnitWork.Count<ModelGoods>(p => p.CateId == id || p.Cate2Id == id || p.Cate3Id == id);
                if (countGoods > 0)
                {
                    throw new Exception($"当前分类被{countGoods}个商品引用，不允许删除");
                }
                UnitWork.Delete<ModelGoodsCate>(p=>p.Id==id);
            }
            UnitWork.Save();
        }

    }
}