using System;
using System.Collections.Generic;
using System.Linq;
using Qs.Comm;
using Qs.Comm.Extensions;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Base;
using Qs.Repository;
using Qs.App.Interface;

namespace Qs.App
{
    /// <summary>
    /// 秒杀商品展示服务
    /// </summary>
    public class AppSeckillGoods : AppBase<ModelSeckillGoods, QsDBContext>
    {
        private readonly IRepository<ModelSeckillActivity, QsDBContext> _activityRepository;
        private readonly IRepository<ModelGoods, QsDBContext> _goodsRepository;
        private readonly IRepository<ModelGoodsSku, QsDBContext> _skuRepository;
        private readonly IRepository<ModelFileUpload, QsDBContext> _fileRepository;

        public AppSeckillGoods(IUnitWork<QsDBContext> unitWork, IRepository<ModelSeckillGoods, QsDBContext> repository, IRepository<ModelSeckillActivity, QsDBContext> activityRepository, IRepository<ModelGoods, QsDBContext> goodsRepository, IRepository<ModelGoodsSku, QsDBContext> skuRepository, IRepository<ModelFileUpload, QsDBContext> fileRepository, IAuth auth) : base(unitWork, repository, auth) { _activityRepository = activityRepository; _goodsRepository = goodsRepository; _skuRepository = skuRepository; _fileRepository = fileRepository; }

        /// <summary>
        /// 分页查询秒杀商品
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public TableData Load(ReqQuSeckillGoods req)
        {
            var result = new TableData();

            var query = from sg in Repository.Queryable()
                        join act in _activityRepository.Queryable() on sg.ActivityId equals act.Id
                        join goods in _goodsRepository.Queryable() on sg.GoodsId equals goods.Id
                        join sku in _skuRepository.Queryable() on sg.SkuId equals sku.Id
                        where sg.IsValid == 1
                        where string.IsNullOrEmpty(req.ActivityId) || sg.ActivityId == req.ActivityId
                        where string.IsNullOrEmpty(req.GoodsName) || goods.GoodsName.Contains(req.GoodsName)
                        where !req.ActivityStatus.HasValue || act.Status == req.ActivityStatus.Value
                        where !req.IsValid.HasValue || sg.IsValid == req.IsValid.Value
                        select new
                        {
                            SeckillGoods = sg,
                            Activity = act,
                            Goods = goods,
                            Sku = sku
                        };

            var list = query.ToList();

            var resList = new List<ResSeckillGoods>();
            foreach (var item in list)
            {
                var goodsImage = _fileRepository.Find(item.Goods.MainImageId)?.FilePath;

                var resSg = ResSeckillGoods.ToView(
                    item.SeckillGoods,
                    item.Goods.GoodsName,
                    item.Sku.SkuName,
                    item.Sku.Price,
                    goodsImage ?? "",
                    item.Activity.Status,
                    item.Activity.StartTime,
                    item.Activity.EndTime);

                resList.Add(resSg);
            }

            result.data = resList;
            result.count = resList.Count;
            return result;
        }

        /// <summary>
        /// 获取首页秒杀商品列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<ResSeckillGoods> GetHomePageSeckillGoods(int pageSize = 20)
        {
            var now = DateTime.Now;

            var query = from sg in Repository.Queryable()
                        join act in _activityRepository.Queryable() on sg.ActivityId equals act.Id
                        join goods in _goodsRepository.Queryable() on sg.GoodsId equals goods.Id
                        join sku in _skuRepository.Queryable() on sg.SkuId equals sku.Id
                        where sg.IsValid == 1
                        where (act.Status == (int)SeckillActivityStatus.Ongoing || 
                               (act.Status == (int)SeckillActivityStatus.Pending && act.StartTime > now))
                        where sg.SoldQuantity < sg.SeckillStock
                        select new
                        {
                            SeckillGoods = sg,
                            Activity = act,
                            Goods = goods,
                            Sku = sku
                        };

            var list = query.Take(pageSize).ToList();

            var resList = new List<ResSeckillGoods>();
            foreach (var item in list)
            {
                var goodsImage = _fileRepository.Find(item.Goods.MainImageId)?.FilePath;

                var resSg = ResSeckillGoods.ToView(
                    item.SeckillGoods,
                    item.Goods.GoodsName,
                    item.Sku.SkuName,
                    item.Sku.Price,
                    goodsImage ?? "",
                    item.Activity.Status,
                    item.Activity.StartTime,
                    item.Activity.EndTime);

                resList.Add(resSg);
            }

            return resList;
        }

        /// <summary>
        /// 获取进行中的秒杀商品
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<ResSeckillGoods> GetOngoingSeckillGoods(int pageSize = 20)
        {
            var query = from sg in Repository.Queryable()
                        join act in _activityRepository.Queryable() on sg.ActivityId equals act.Id
                        join goods in _goodsRepository.Queryable() on sg.GoodsId equals goods.Id
                        join sku in _skuRepository.Queryable() on sg.SkuId equals sku.Id
                        where sg.IsValid == 1
                        where act.Status == (int)SeckillActivityStatus.Ongoing
                        where sg.SoldQuantity < sg.SeckillStock
                        select new
                        {
                            SeckillGoods = sg,
                            Activity = act,
                            Goods = goods,
                            Sku = sku
                        };

            var list = query.Take(pageSize).ToList();

            var resList = new List<ResSeckillGoods>();
            foreach (var item in list)
            {
                var goodsImage = _fileRepository.Find(item.Goods.MainImageId)?.FilePath;

                var resSg = ResSeckillGoods.ToView(
                    item.SeckillGoods,
                    item.Goods.GoodsName,
                    item.Sku.SkuName,
                    item.Sku.Price,
                    goodsImage ?? "",
                    item.Activity.Status,
                    item.Activity.StartTime,
                    item.Activity.EndTime);

                resList.Add(resSg);
            }

            return resList;
        }

        /// <summary>
        /// 获取待开始的秒杀商品
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<ResSeckillGoods> GetPendingSeckillGoods(int pageSize = 20)
        {
            var query = from sg in Repository.Queryable()
                        join act in _activityRepository.Queryable() on sg.ActivityId equals act.Id
                        join goods in _goodsRepository.Queryable() on sg.GoodsId equals goods.Id
                        join sku in _skuRepository.Queryable() on sg.SkuId equals sku.Id
                        where sg.IsValid == 1
                        where act.Status == (int)SeckillActivityStatus.Pending
                        where sg.SoldQuantity < sg.SeckillStock
                        select new
                        {
                            SeckillGoods = sg,
                            Activity = act,
                            Goods = goods,
                            Sku = sku
                        };

            var list = query.Take(pageSize).ToList();

            var resList = new List<ResSeckillGoods>();
            foreach (var item in list)
            {
                var goodsImage = _fileRepository.Find(item.Goods.MainImageId)?.FilePath;

                var resSg = ResSeckillGoods.ToView(
                    item.SeckillGoods,
                    item.Goods.GoodsName,
                    item.Sku.SkuName,
                    item.Sku.Price,
                    goodsImage ?? "",
                    item.Activity.Status,
                    item.Activity.StartTime,
                    item.Activity.EndTime);

                resList.Add(resSg);
            }

            return resList;
        }

        /// <summary>
        /// 获取秒杀商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResSeckillGoods GetDetail(string id)
        {
            var sg = Repository.Find(id);
            if (sg == null)
                throw new CustomException(404, "秒杀商品不存在");

            var activity = _activityRepository.Find(sg.ActivityId);
            if (activity == null)
                throw new CustomException(404, "秒杀活动不存在");

            var goods = _goodsRepository.Find(sg.GoodsId);
            if (goods == null)
                throw new CustomException(404, "商品不存在");

            var sku = _skuRepository.Find(sg.SkuId);
            if (sku == null)
                throw new CustomException(404, "SKU不存在");

            var goodsImage = _fileRepository.Find(goods.MainImageId)?.FilePath;

            var resSg = ResSeckillGoods.ToView(
                sg,
                goods.GoodsName,
                sku.SkuName,
                sku.Price,
                goodsImage ?? "",
                activity.Status,
                activity.StartTime,
                activity.EndTime);

            return resSg;
        }

        /// <summary>
        /// 根据商品ID获取秒杀商品信息
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ResSeckillGoods GetByGoodsId(string goodsId)
        {
            var now = DateTime.Now;

            var query = from sg in Repository.Queryable()
                        join act in _activityRepository.Queryable() on sg.ActivityId equals act.Id
                        where sg.GoodsId == goodsId
                        where sg.IsValid == 1
                        where sg.SoldQuantity < sg.SeckillStock
                        where (act.Status == (int)SeckillActivityStatus.Ongoing || 
                               (act.Status == (int)SeckillActivityStatus.Pending && act.StartTime > now))
                        select new { SeckillGoods = sg, Activity = act };

            var result = query.FirstOrDefault();
            if (result == null)
                return null;

            var goods = _goodsRepository.Find(result.SeckillGoods.GoodsId);
            var sku = _skuRepository.Find(result.SeckillGoods.SkuId);
            var goodsImage = _fileRepository.Find(goods?.MainImageId)?.FilePath;

            if (goods == null || sku == null)
                return null;

            var resSg = ResSeckillGoods.ToView(
                result.SeckillGoods,
                goods.GoodsName,
                sku.SkuName,
                sku.Price,
                goodsImage ?? "",
                result.Activity.Status,
                result.Activity.StartTime,
                result.Activity.EndTime);

            return resSg;
        }
    }
}