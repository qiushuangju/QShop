using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Comm.Cache;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Comm.Extensions;
namespace Qs.App
{
    /// <summary>
    /// 秒杀商品应用层
    /// </summary>
    public class AppSeckillGoods
    {
        private readonly IUnitWork<QsDBContext> _unitWork;
        private readonly IAuth _auth;
        private readonly ICacheContext _cacheContext;
        private readonly AppOrder _appOrder;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppSeckillGoods(IUnitWork<QsDBContext> unitWork, IAuth auth, ICacheContext cacheContext, AppOrder appOrder)
        {
            _unitWork = unitWork;
            _auth = auth;
            _cacheContext = cacheContext;
            _appOrder = appOrder;
        }
        
        /// <summary>
        /// 获取秒杀商品列表
        /// </summary>
        public List<ResSeckillGoods> GetSeckillGoodsList(int status = 0)
        {
            var now = DateTime.Now;
            IQueryable<ModelSeckillActivity> activityLinq = _unitWork.Find<ModelSeckillActivity>(p => p.IsDelete == (int)xEnum.YesOrNo.No);
            
            if(status != 0)
            {
                if (status == (int)xEnum.SeckillActivityStatus.WaitStart)
                {
                    activityLinq = activityLinq.Where(p => p.StartTime > now);
                }
                else if (status == (int)xEnum.SeckillActivityStatus.Ongoing)
                {
                    activityLinq = activityLinq.Where(p => p.StartTime <= now && p.EndTime > now);
                }
                else if (status == (int)xEnum.SeckillActivityStatus.Ended)
                {
                    activityLinq = activityLinq.Where(p => p.EndTime <= now);
                }
            }
            
            var activityIds = activityLinq.Select(p => p.Id).ToList();
            var seckillGoods = _unitWork.Find<ModelSeckillGoods>(p => activityIds.Contains(p.ActivityId) && p.IsDelete == (int)xEnum.YesOrNo.No)
                .OrderBy(p => p.SortNo).ToList();
            
            var goodsIds = seckillGoods.Select(p => p.GoodsId).ToList();
            var goodsList = _unitWork.Find<ModelGoods>(p => goodsIds.Contains(p.Id) && p.IsDelete == (int)xEnum.YesOrNo.No).ToList();
            
            var result = new List<ResSeckillGoods>();
            foreach(var sg in seckillGoods)
            {
                var goods = goodsList.FirstOrDefault(p => p.Id == sg.GoodsId);
                if(goods == null) continue;
                
                var activity = activityLinq.FirstOrDefault(p => p.Id == sg.ActivityId);
                if(activity == null) continue;
                
                var progress = sg.SeckillStock > 0 ? (double)sg.SoldCount / sg.SeckillStock * 100 : 0;
                var remainingStock = sg.SeckillStock - sg.SoldCount;
                
                result.Add(new ResSeckillGoods
                {
                    Id = sg.Id,
                    ActivityId = sg.ActivityId,
                    ActivityName = activity.ActivityName,
                    GoodsId = sg.GoodsId,
                    GoodsName = goods.GoodsName,
                    SubTitle = goods.SubTitle,
                    ImageIdMains = goods.ImageIdMains,
                    OriginalPrice = goods.SalePrice ?? 0m,
                    SeckillPrice = sg.SeckillPrice,
                    SeckillStock = sg.SeckillStock,
                    SoldCount = sg.SoldCount,
                    Progress = sg.SeckillStock > 0 ? (double)Math.Round((decimal)sg.SoldCount / sg.SeckillStock * 100, 2) : 0.0,
                    RemainingStock = remainingStock,
                    StartTime = activity.StartTime,
                    EndTime = activity.EndTime,
                    Status = activity.Status,
                    RemainingTime = GetRemainingTime(activity.Status, activity.StartTime, activity.EndTime)
                });
            }
            
            return result;
        }
        
        /// <summary>
        /// 获取剩余时间
        /// </summary>
        private long GetRemainingTime(int status, DateTime startTime, DateTime endTime)
        {
            var now = DateTime.Now;
            if(status == (int)xEnum.SeckillActivityStatus.WaitStart)
            {
                return (long)(startTime - now).TotalSeconds;
            }
            else if(status == (int)xEnum.SeckillActivityStatus.Ongoing)
            {
                return (long)(endTime - now).TotalSeconds;
            }
            return 0;
        }
        
        /// <summary>
        /// 秒杀抢购
        /// </summary>
        public async Task<Qs.Comm.Response> SeckillBuy(ReqSeckillBuy req, string userId = null)
        {
            if(string.IsNullOrEmpty(userId))
            {
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.LoginExpiration, Message = "登录已过期" };
            }
            
            // 1. 检查接口限流
            var limitKey = $"Seckill:Limit:{userId}";
            var requestCount = _cacheContext.Get<int>(limitKey);
            requestCount++;
            _cacheContext.Set(limitKey, requestCount, DateTime.Now.AddSeconds(1));
            if(requestCount > 5) // 限制1秒内最多5次请求
            {
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.ServerError, Message = "请求过于频繁，请稍后再试" };
            }
            
            var activity = _unitWork.FirstOrDefault<ModelSeckillActivity>(p => p.Id == req.ActivityId && p.IsDelete == (int)xEnum.YesOrNo.No);
            if(activity == null)
            {
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.ServerError, Message = "秒杀活动不存在" };
            }
            
            if(requestCount > activity.MaxRequestPerSecond)
            {
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.ServerError, Message = "请求过于频繁，请稍后再试" };
            }
            
            // 2. 检查活动状态
            var now = DateTime.Now;
            if(activity.Status != (int)xEnum.SeckillActivityStatus.Ongoing)
            {
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.ServerError, Message = "活动未开始或已结束" };
            }
            
            // 3. 检查秒杀商品
            var seckillGoods = _unitWork.FirstOrDefault<ModelSeckillGoods>(p => p.GoodsId == req.GoodsId && p.ActivityId == req.ActivityId && p.IsDelete == (int)xEnum.YesOrNo.No);
            if(seckillGoods == null)
            {
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.ServerError, Message = "秒杀商品不存在" };
            }
            
            if(seckillGoods.SeckillStock - seckillGoods.SoldCount < req.BuyCount)
            {
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.ServerError, Message = "库存不足" };
            }
            
            // 4. 检查用户限购
            var userBuyCount = _unitWork.Find<ModelSeckillUserRecord>(p => p.ActivityId == req.ActivityId && p.UserId == userId)
                .Sum(p => p.BuyCount);
            if(userBuyCount + req.BuyCount > activity.LimitPerUser)
            {
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.ServerError, Message = "超过限购数量" };
            }
            
            // 5. 再次检查库存防止超卖
            seckillGoods = _unitWork.FirstOrDefault<ModelSeckillGoods>(p => p.GoodsId == req.GoodsId && p.ActivityId == req.ActivityId && p.IsDelete == (int)xEnum.YesOrNo.No);
            if(seckillGoods == null || seckillGoods.SeckillStock - seckillGoods.SoldCount < req.BuyCount)
            {
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.ServerError, Message = "库存不足" };
            }
            
            // 6. 扣减库存
            seckillGoods.SoldCount += req.BuyCount;
            _unitWork.Update(seckillGoods);
            
            // 7. 记录用户抢购记录
            var record = new ModelSeckillUserRecord
            {
                Id = Guid.NewGuid().ToString(),
                ActivityId = req.ActivityId,
                GoodsId = seckillGoods.GoodsId,
                SkuId = req.SkuId,
                UserId = userId,
                BuyCount = req.BuyCount
            };
            _unitWork.Add(record);
                
                // 8. 异步创建订单
                await CreateOrderAsync(record, seckillGoods);
                
                _unitWork.Save();
                
                return new Qs.Comm.Response { Code = (int)Qs.Comm.ResponseType.OperSuccess, Message = "抢购成功" };
        }
        
        /// <summary>
        /// 异步创建订单
        /// </summary>
        private async Task CreateOrderAsync(ModelSeckillUserRecord record, ModelSeckillGoods seckillGoods)
        {
            try
            {
                // 这里可以使用消息队列或者后台任务处理订单创建
                await Task.Run(() =>
                {
                    // 调用订单服务创建订单
                    // _appOrder.CreateSeckillOrder(record, seckillGoods);
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine("创建秒杀订单失败: " + ex.Message);
            }
        }
    }
}