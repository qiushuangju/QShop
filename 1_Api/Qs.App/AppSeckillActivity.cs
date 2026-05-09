using System;
using System.Collections.Generic;
using System.Linq;
using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Base;
using Qs.Repository;
using Qs.Comm.Extensions;
using Qs.App.Base;
using Qs.App.Interface;

namespace Qs.App
{
    /// <summary>
    /// 秒杀活动管理服务
    /// </summary>
    public class AppSeckillActivity : AppBaseString<ModelSeckillActivity, QsDBContext>
    {
        private readonly ISeckillRepository _seckillRepository;
        private readonly IRepository<ModelGoods, QsDBContext> _goodsRepository;
        private readonly IRepository<ModelGoodsSku, QsDBContext> _skuRepository;
        private readonly IRepository<ModelFileUpload, QsDBContext> _fileRepository;

        public AppSeckillActivity(IUnitWork<QsDBContext> unitWork, IRepository<ModelSeckillActivity, QsDBContext> repository, ISeckillRepository seckillRepository, IRepository<ModelGoods, QsDBContext> goodsRepository, IRepository<ModelGoodsSku, QsDBContext> skuRepository, IRepository<ModelFileUpload, QsDBContext> fileRepository, IAuth auth) : base(unitWork, repository, auth) { _seckillRepository = seckillRepository; _goodsRepository = goodsRepository; _skuRepository = skuRepository; _fileRepository = fileRepository; }

        /// <summary>
        /// 分页查询秒杀活动
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public TableData Load(ReqQuSeckillActivity req)
        {
            var result = new TableData();
            var list = Repository.Queryable()
                .WhereIF(!string.IsNullOrEmpty(req.ActivityName), it => it.ActivityName.Contains(req.ActivityName))
                .WhereIF(req.Status.HasValue, it => it.Status == req.Status.Value)
                .WhereIF(req.StartTime.HasValue, it => it.StartTime >= req.StartTime.Value)
                .WhereIF(req.EndTime.HasValue, it => it.EndTime <= req.EndTime.Value)
                .OrderByDescending(it => it.CreateTime)
                .Select(it => new ResSeckillActivity
                {
                    Id = it.Id,
                    ActivityName = it.ActivityName,
                    Description = it.Description,
                    StartTime = it.StartTime,
                    EndTime = it.EndTime,
                    Status = it.Status,
                    LimitPerUser = it.LimitPerUser,
                    CreateTime = it.CreateTime
                }).ToList();

            foreach (var item in list)
            {
                item.StatusName = xEnum.GetEnumDescription(typeof(SeckillActivityStatus), item.Status);
                // 获取秒杀商品列表
                var seckillGoods = _seckillRepository.GetSeckillGoodsByActivityId(item.Id);
                if (seckillGoods != null && seckillGoods.Count > 0)
                {
                    item.SeckillGoods = new List<ResSeckillGoods>();
                    foreach (var sg in seckillGoods)
                    {
                        var goods = _goodsRepository.Find(sg.GoodsId);
                        var sku = _skuRepository.Find(sg.SkuId);
                        var goodsImage = _fileRepository.Find(goods?.MainImageId)?.FilePath;

                        var resSg = ResSeckillGoods.ToView(
                            sg,
                            goods?.GoodsName ?? "",
                            sku?.SkuName ?? "",
                            sku?.Price ?? 0,
                            goodsImage ?? "",
                            item.Status,
                            item.StartTime,
                            item.EndTime);

                        item.SeckillGoods.Add(resSg);
                    }
                }
            }

            result.data = list;
            result.count = list.Count;
            return result;
        }

        /// <summary>
        /// 创建秒杀活动
        /// </summary>
        /// <param name="req"></param>
        public void Add(ReqAuSeckillActivity req)
        {
            req.Check();
            foreach (var sgReq in req.SeckillGoods)
            {
                sgReq.Check();
                // 检查商品和SKU是否存在
                var goods = _goodsRepository.Find(sgReq.GoodsId);
                if (goods == null)
                    throw new CustomException(404, "商品不存在");
                var sku = _skuRepository.Find(sgReq.SkuId);
                if (sku == null)
                    throw new CustomException(404, "SKU不存在");
                // 检查SKU是否属于该商品
                if (sku.GoodsId != sgReq.GoodsId)
                    throw new CustomException(400, "SKU不属于该商品");
                // 检查秒杀价格是否低于原价
                if (sgReq.SeckillPrice >= sku.Price)
                    throw new CustomException(400, "秒杀价格必须低于原价");
            }

            var activity = new ModelSeckillActivity
            {
                Id = Guid.NewGuid().ToString(),
                ActivityName = req.ActivityName,
                Description = req.Description,
                StartTime = req.StartTime,
                EndTime = req.EndTime,
                Status = (int)SeckillActivityStatus.Pending,
                LimitPerUser = req.LimitPerUser,
                CreateTime = DateTime.Now,
                Creator = Auth.UserId
            };

            Repository.Add(activity);

            // 批量添加秒杀商品
            var seckillGoodsList = new List<ModelSeckillGoods>();
            foreach (var sgReq in req.SeckillGoods)
            {
                var seckillGoods = new ModelSeckillGoods
                {
                    Id = Guid.NewGuid().ToString(),
                    ActivityId = activity.Id,
                    GoodsId = sgReq.GoodsId,
                    SkuId = sgReq.SkuId,
                    SeckillPrice = sgReq.SeckillPrice,
                    SeckillStock = sgReq.SeckillStock,
                    SoldQuantity = 0,
                    IsValid = 1
                };
                seckillGoodsList.Add(seckillGoods);
            }

            _seckillRepository.BatchAddSeckillGoods(seckillGoodsList);
        }

        /// <summary>
        /// 更新秒杀活动
        /// </summary>
        /// <param name="req"></param>
        public void Update(ReqAuSeckillActivity req)
        {
            req.Check();
            var activity = Repository.Find(req.Id);
            if (activity == null)
                throw new CustomException(404, "活动不存在");

            // 如果活动已开始或已结束，不允许修改
            if (activity.Status == (int)SeckillActivityStatus.Ongoing || 
                activity.Status == (int)SeckillActivityStatus.Ended)
                throw new CustomException(400, "活动已开始或已结束，不允许修改");

            activity.ActivityName = req.ActivityName;
            activity.Description = req.Description;
            activity.StartTime = req.StartTime;
            activity.EndTime = req.EndTime;
            activity.Status = req.Status;
            activity.LimitPerUser = req.LimitPerUser;
            activity.UpdateTime = DateTime.Now;
            activity.Updater = Auth.UserId;

            Repository.Update(activity);

            // 先删除原有秒杀商品
            var existingGoods = _seckillRepository.GetSeckillGoodsByActivityId(activity.Id);
            if (existingGoods != null && existingGoods.Count > 0)
            {
                foreach (var sg in existingGoods)
                {
                    _seckillRepository.Delete(sg);
                }
            }

            // 添加新的秒杀商品
            foreach (var sgReq in req.SeckillGoods)
            {
                sgReq.Check();
                var goods = _goodsRepository.Find(sgReq.GoodsId);
                if (goods == null)
                    throw new CustomException(404, "商品不存在");
                var sku = _skuRepository.Find(sgReq.SkuId);
                if (sku == null)
                    throw new CustomException(404, "SKU不存在");
                if (sku.GoodsId != sgReq.GoodsId)
                    throw new CustomException(400, "SKU不属于该商品");
                if (sgReq.SeckillPrice >= sku.Price)
                    throw new CustomException(400, "秒杀价格必须低于原价");

                var seckillGoods = new ModelSeckillGoods
                {
                    Id = Guid.NewGuid().ToString(),
                    ActivityId = activity.Id,
                    GoodsId = sgReq.GoodsId,
                    SkuId = sgReq.SkuId,
                    SeckillPrice = sgReq.SeckillPrice,
                    SeckillStock = sgReq.SeckillStock,
                    SoldQuantity = 0,
                    IsValid = 1
                };
                _seckillRepository.Add(seckillGoods);
            }
        }

        /// <summary>
        /// 取消秒杀活动
        /// </summary>
        /// <param name="id"></param>
        public void Cancel(string id)
        {
            var activity = Repository.Find(id);
            if (activity == null)
                throw new CustomException(404, "活动不存在");

            // 如果活动已结束，不允许取消
            if (activity.Status == (int)SeckillActivityStatus.Ended)
                throw new CustomException(400, "活动已结束，不允许取消");

            activity.Status = (int)SeckillActivityStatus.Canceled;
            activity.UpdateTime = DateTime.Now;
            activity.Updater = Auth.UserId;
            Repository.Update(activity);

            // 同步取消秒杀商品
            var existingGoods = _seckillRepository.GetSeckillGoodsByActivityId(activity.Id);
            if (existingGoods != null && existingGoods.Count > 0)
            {
                foreach (var sg in existingGoods)
                {
                    sg.IsValid = 0;
                    _seckillRepository.Update(sg);
                }
            }
        }

        /// <summary>
        /// 结束秒杀活动
        /// </summary>
        /// <param name="id"></param>
        public void End(string id)
        {
            var activity = Repository.Find(id);
            if (activity == null)
                throw new CustomException(404, "活动不存在");

            // 如果活动已结束，不允许重复结束
            if (activity.Status == (int)SeckillActivityStatus.Ended)
                throw new CustomException(400, "活动已结束");

            activity.Status = (int)SeckillActivityStatus.Ended;
            activity.UpdateTime = DateTime.Now;
            activity.Updater = Auth.UserId;
            Repository.Update(activity);

            // 同步结束秒杀商品
            var existingGoods = _seckillRepository.GetSeckillGoodsByActivityId(activity.Id);
            if (existingGoods != null && existingGoods.Count > 0)
            {
                foreach (var sg in existingGoods)
                {
                    sg.IsValid = 0;
                    _seckillRepository.Update(sg);
                }
            }
        }

        /// <summary>
        /// 获取秒杀活动详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResSeckillActivity GetDetail(string id)
        {
            var activity = Repository.Find(id);
            if (activity == null)
                throw new CustomException(404, "活动不存在");

            var res = ResSeckillActivity.ToView(activity);

            // 获取秒杀商品列表
            var seckillGoods = _seckillRepository.GetSeckillGoodsByActivityId(activity.Id);
            if (seckillGoods != null && seckillGoods.Count > 0)
            {
                res.SeckillGoods = new List<ResSeckillGoods>();
                foreach (var sg in seckillGoods)
                {
                    var goods = _goodsRepository.Find(sg.GoodsId);
                    var sku = _skuRepository.Find(sg.SkuId);
                    var goodsImage = _fileRepository.Find(goods?.MainImageId)?.FilePath;

                    var resSg = ResSeckillGoods.ToView(
                        sg,
                        goods?.GoodsName ?? "",
                        sku?.SkuName ?? "",
                        sku?.Price ?? 0,
                        goodsImage ?? "",
                        activity.Status,
                        activity.StartTime,
                        activity.EndTime);

                    res.SeckillGoods.Add(resSg);
                }
            }

            return res;
        }
    }
}