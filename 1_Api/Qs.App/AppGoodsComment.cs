using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Comm.Extensions;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.App
{
    /// <summary>
    /// 商品评价 App
    /// </summary>
    public class AppGoodsComment : AppBaseString<ModelGoodsComment, QsDBContext>
    {
        private AppGoods _appGoods;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppGoodsComment(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoodsComment, QsDBContext> repository,
            AppGoods appGoods, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension,
            auth)
        {
            _appGoods = appGoods;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuGoodsComment req)
        {
            //var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            //var properties = _dbExtension.GetProperties("GoodsComment");
            //result.ColumnHeaders = properties;
            result.Result = ListByWhere(req, true);
            result.Count = ListLinq(req).Count();
            return result;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResGoodsComment> ListByWhere(ReqQuGoodsComment req, bool isPage = false)
        {
            IQueryable<ModelGoodsComment> linq = ListLinq(req);
            List<ModelGoodsComment> list =
                isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            List<string> listImgIds = list.Select(p => p.ImgIds).ToList();
            List<string> listUserId = list.Select(p => p.UserId).ToList();
            List<string> listSkuId = list.Select(p => p.SkuId).ToList();
            List<string> listGoodsId = list.Select(p => p.GoodsId).ToList();

            List<string> listFileId = new List<string>();
            foreach (var imgIds in listImgIds)
            {
                 listFileId.AddRange(xConv.ToListString(imgIds));
            }
       
            List<ModelUser> listUser= UnitWork.Find<ModelUser>(p => listUserId.Contains(p.Id)).ToList();
            listFileId = listFileId.Distinct().ToList();
            List<ModelFileUpload> listFile = UnitWork.Find<ModelFileUpload>(p => listFileId.Contains(p.Id)).ToList();
            List<ResGoodsComment> listRes = new List<ResGoodsComment>();
            List<ResGoods> listGoods = _appGoods.ListByWhere(new ReqQuGoods(){ GoodsIds =xConv.ListToString(listGoodsId) });
            List<ModelGoodsSku> listSku = UnitWork.Find<ModelGoodsSku>(p => listSkuId.Contains(p.Id)).ToList();
            foreach (var item in list)
            {
                listRes.Add(ResGoodsComment.ToView(item, listUser, listFile, listGoods, listSku));
            }
            return listRes;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelGoodsComment> ListLinq(ReqQuGoodsComment req)
        {
            var exp = GetWhereByStatus(xConv.ToInt(req.CommentType), req.IsShowHide);
            var linq = UnitWork.Find<ModelGoodsComment>(exp);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Id.Contains(req.Key));
            }

            if (!string.IsNullOrEmpty(req.GoodsId))
            {
                linq = linq.Where(p => p.GoodsId.Contains(req.GoodsId));
            }

            var storeId = _auth.GetStoreId();
            if (!string.IsNullOrEmpty(storeId))
            {
                linq = linq.Where(p => p.StoreId == storeId);
            }

            return linq;
        }

        /// <summary>
        /// 获取各类数量
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ResTotalGroup TotalGroup(string goodsId)
        {
            ResTotalGroup res = new ResTotalGroup()
            {
                TotalAll = UnitWork.Count(GetWhereByStatus(0).And(p=>p.GoodsId==goodsId)),
                TotalPraise = UnitWork.Count(GetWhereByStatus((int)xEnum.CommentType.Praise).And(p => p.GoodsId == goodsId)),
                TotalMiddle = UnitWork.Count(GetWhereByStatus((int)xEnum.CommentType.Middle).And(p => p.GoodsId == goodsId)),
                TotalBad = UnitWork.Count(GetWhereByStatus((int)xEnum.CommentType.Bad).And(p => p.GoodsId == goodsId)),
            };
            return res;
        }

        /// <summary>
        /// 修改商品上下架状态
        /// </summary>
        /// <param name="req"></param>
        public void SetStatus(ReqSetStatus req)
        {
            var model = Repository.FirstOrDefault(p => p.Id == req.Id);
            model.Status = req.Status;
            Repository.Update(model);
        }

        /// <summary>
        /// 根据评价类型 生成Exp
        /// </summary>
        /// <param name="commentType"></param>
        /// <param name="isShowHide"></param>
        /// <returns></returns>
        public Expression<Func<ModelGoodsComment, bool>> GetWhereByStatus( int commentType, bool isShowHide=false)
        {
            Expression<Func<ModelGoodsComment, bool>> where = p => true;

            if (!isShowHide)
            {
                where = where.And(p => p.Status == (int)xEnum.ComStatus.Normal);
            }
            switch (commentType)
            {
                case (int)xEnum.CommentType.Praise: //好评
                    where = where.And(p => p.Score >= 4);
                    break;
                case (int)xEnum.CommentType.Middle: //中评
                    where = where.And(p =>  p.Score <= 3 && p.Score >= 2);
                    break;
                case (int)xEnum.CommentType.Bad: //差评
                    where = where.And(p => p.Score <= 1);
                    break;
            }

            return where;
        }


        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuGoodsComment req)
        {
            var model = xConv.CopyMapper<ModelGoodsComment, ReqAuGoodsComment>(req);
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
        /// 新增或修改
        /// </summary>
        public void AddComment(ReqAuGoodsComment req)
        {
            var storeId = _auth.GetStoreId();
            var order = UnitWork.FirstOrDefault<ModelOrder>(p => p.Id == req.OrderId);
            var listComment = req.ListComment;
            if (order.OrderStatus < (int) xEnum.OrderStatus.Done||order.IsComment==(int)xEnum.ComStatus.Normal)
            {
                throw new Exception("订单已评价或暂时不能评价!");
            }
            foreach (var comment in listComment)
            {
                ModelGoodsComment model = new ModelGoodsComment()
                {
                    Score = comment.RateValue,
                    Content = comment.Content,
                    ImgIds = xConv.ListToString(comment.Uploaded),
                    Status = (int) xEnum.ComStatus.Normal,
                    OrderId = req.OrderId,
                    GoodsId = comment.GoodsId,
                    SkuId = comment.SkuId,
                    UserId = _auth.GetCurrentContext().User.Id,
                    StoreId = storeId,
                };
                UnitWork.Add(model);
            }

            order.OrderStatus = (int) xEnum.OrderStatus.Done;
            order.IsComment = (int)xEnum.ComStatus.Normal;
            UnitWork.Update(order);
            UnitWork.Save();
        }


    }
}