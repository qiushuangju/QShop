using System;
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

namespace Qs.App
{
    /// <summary>
    /// 秒杀活动应用层
    /// </summary>
    public class AppSeckillActivity : AppBaseString<ModelSeckillActivity, QsDBContext>
    {
        private DbExtension _dbExtension;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppSeckillActivity(IUnitWork<QsDBContext> unitWork, IRepository<ModelSeckillActivity, QsDBContext> repository, 
             DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _dbExtension = dbExtension;
        }
        
        /// <summary>
        /// 加载秒杀活动列表
        /// </summary>
        public TableData Load(ReqQuSeckillActivity req)
        {           
            var result = new TableData();
            var linq = ListLinq(req);
            
            result.Count = linq.Count();
            var isPage = req.Page > 0 && req.Limit > 0;
            result.Result = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return result;
        }
        
        /// <summary>
        /// 查询秒杀活动
        /// </summary>
        public IQueryable<ModelSeckillActivity> ListLinq(ReqQuSeckillActivity req)
        {
            var storeId = _auth.GetStoreId();
            var linq = UnitWork.Find<ModelSeckillActivity>(p => p.IsDelete == (int)xEnum.YesOrNo.No && p.StoreId == storeId);
            
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.ActivityName.Contains(req.Key));
            }
            
            if (req.Status != 0)
            {
                linq = linq.Where(p => p.Status.Equals(req.Status));
            }
            
            // 根据状态过滤
            if (req.Status == (int)xEnum.SeckillActivityStatus.WaitStart)
            {
                linq = linq.Where(p => p.StartTime > DateTime.Now);
            }
            else if (req.Status == (int)xEnum.SeckillActivityStatus.Ongoing)
            {
                linq = linq.Where(p => p.StartTime <= DateTime.Now && p.EndTime > DateTime.Now);
            }
            else if (req.Status == (int)xEnum.SeckillActivityStatus.Ended)
            {
                linq = linq.Where(p => p.EndTime <= DateTime.Now);
            }
            
            return linq.OrderByDescending(p => p.CreateTime);
        }
        
        /// <summary>
        /// 创建秒杀活动
        /// </summary>
        public void Add(ReqAddSeckillActivity req)
        {
            var entity = new ModelSeckillActivity
            {
                Id = Guid.NewGuid().ToString(),
                ActivityName = req.ActivityName,
                ActivityDesc = req.ActivityDesc,
                StartTime = req.StartTime,
                EndTime = req.EndTime,
                LimitPerUser = req.LimitPerUser,
                MaxRequestPerSecond = req.MaxRequestPerSecond,
                StoreId = _auth.GetStoreId(),
                Status = (int)xEnum.SeckillActivityStatus.WaitStart
            };
            
            Repository.Add(entity);
            
            // 添加秒杀商品
            foreach (var goods in req.SeckillGoods)
            {
                var seckillGoods = new ModelSeckillGoods
                {
                    Id = Guid.NewGuid().ToString(),
                    ActivityId = entity.Id,
                    GoodsId = goods.GoodsId,
                    SkuId = goods.SkuId,
                    SeckillPrice = goods.SeckillPrice,
                    SeckillStock = goods.SeckillStock,
                    SortNo = goods.SortNo
                };
                UnitWork.Add(seckillGoods);
            }
            
            UnitWork.Save();
        }
        
        /// <summary>
        /// 更新秒杀活动
        /// </summary>
        public void Update(ReqUpdateSeckillActivity req)
        {
            var entity = Repository.FirstOrDefault(p => p.Id == req.Id);
            if(entity == null)
            {
                throw new CommonException("秒杀活动不存在", (int)Qs.Comm.ResponseType.ServerError);
            }
            
            entity.ActivityName = req.ActivityName;
            entity.ActivityDesc = req.ActivityDesc;
            entity.StartTime = req.StartTime;
            entity.EndTime = req.EndTime;
            entity.LimitPerUser = req.LimitPerUser;
            entity.MaxRequestPerSecond = req.MaxRequestPerSecond;
            
            Repository.Update(entity);
            UnitWork.Save();
        }
        
        /// <summary>
        /// 取消秒杀活动
        /// </summary>
        public void Cancel(string id)
        {
            var entity = Repository.FirstOrDefault(p => p.Id == id);
            if(entity == null)
            {
                throw new CommonException("秒杀活动不存在", (int)Qs.Comm.ResponseType.ServerError);
            }
            
            if(entity.Status == (int)xEnum.SeckillActivityStatus.Ended)
            {
                throw new CommonException("活动已结束，无法取消", (int)Qs.Comm.ResponseType.ServerError);
            }
            
            entity.Status = (int)xEnum.SeckillActivityStatus.Canceled;
            Repository.Update(entity);
            UnitWork.Save();
        }
        
        /// <summary>
        /// 获取秒杀活动详情
        /// </summary>
        public ModelSeckillActivity GetDetail(string id)
        {
            var entity = Repository.FirstOrDefault(p => p.Id == id && p.IsDelete == (int)xEnum.YesOrNo.No);
            if(entity == null)
            {
                throw new CommonException("秒杀活动不存在", (int)Qs.Comm.ResponseType.ServerError);
            }
            return entity;
        }
        
        /// <summary>
        /// 获取秒杀活动商品
        /// </summary>
        public List<ModelSeckillGoods> GetSeckillGoods(string activityId)
        {
            return UnitWork.Find<ModelSeckillGoods>(p => p.ActivityId == activityId && p.IsDelete == (int)xEnum.YesOrNo.No)
                .OrderBy(p => p.SortNo).ToList();
        }
        
        /// <summary>
        /// 更新活动状态（定时任务调用）
        /// </summary>
        public void UpdateActivityStatus()
        {
            var now = DateTime.Now;
            
            // 将待开始的活动改为进行中
            var waitStartActivities = UnitWork.Find<ModelSeckillActivity>(p => 
                p.Status == (int)xEnum.SeckillActivityStatus.WaitStart && p.StartTime <= now);
            foreach(var activity in waitStartActivities)
            {
                activity.Status = (int)xEnum.SeckillActivityStatus.Ongoing;
                Repository.Update(activity);
            }
            
            // 将进行中的活动改为已结束
            var ongoingActivities = UnitWork.Find<ModelSeckillActivity>(p => 
                p.Status == (int)xEnum.SeckillActivityStatus.Ongoing && p.EndTime <= now);
            foreach(var activity in ongoingActivities)
            {
                activity.Status = (int)xEnum.SeckillActivityStatus.Ended;
                Repository.Update(activity);
            }
            
            UnitWork.Save();
        }
    }
}