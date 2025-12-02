using System;
using System.Collections.Generic;
using System.Linq;
using Qs.Comm;
using Qs.Comm.Extensions;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Base;
using System.Threading.Tasks;
using Qs.App.Base;
using Qs.App.Interface;

namespace Qs.App
{
    /// <summary>
    /// 秒杀抢购流程服务
    /// </summary>
    public class AppSeckillPurchase : AppBase<ModelSeckillOrder, QsDBContext>
    {
        private readonly ISeckillRepository _seckillRepository;
private readonly IRepository<ModelSeckillActivity, QsDBContext> _activityRepository;
private readonly IRepository<ModelSeckillGoods, QsDBContext> _goodsRepository;
private readonly IRepository<ModelUser, QsDBContext> _userRepository;
        private static readonly object _lockObj = new object();

        public AppSeckillPurchase(IUnitWork<QsDBContext> unitWork, IRepository<ModelSeckillOrder, QsDBContext> repository, ISeckillRepository seckillRepository, IRepository<ModelSeckillActivity, QsDBContext> activityRepository, IRepository<ModelSeckillGoods, QsDBContext> goodsRepository, IRepository<ModelUser, QsDBContext> userRepository, IAuth auth) : base(unitWork, repository, auth) { _seckillRepository = seckillRepository; _activityRepository = activityRepository; _goodsRepository = goodsRepository; _userRepository = userRepository; }

        /// <summary>
        /// 秒杀抢购
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResSeckillPurchaseResult Purchase(ReqSeckillPurchase req)
        {
            req.Check();
            var userId = Auth.UserId;

            // 检查用户是否登录
            if (string.IsNullOrEmpty(userId))
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "请先登录"
                };

            // 检查用户是否存在
            var user = _userRepository.Find(userId);
            if (user == null)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "用户不存在"
                };

            // 检查秒杀活动是否存在
            var activity = _activityRepository.Find(req.ActivityId);
            if (activity == null)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "秒杀活动不存在"
                };

            // 检查活动状态
            var now = DateTime.Now;
            if (activity.Status == (int)SeckillActivityStatus.Canceled)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "秒杀活动已取消"
                };

            if (activity.Status == (int)SeckillActivityStatus.Ended)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "秒杀活动已结束"
                };

            if (activity.Status == (int)SeckillActivityStatus.Pending)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "秒杀活动尚未开始"
                };

            // 检查活动时间
            if (now < activity.StartTime)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "秒杀活动尚未开始"
                };

            if (now > activity.EndTime)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "秒杀活动已结束"
                };

            // 检查秒杀商品是否存在
            var seckillGoods = _goodsRepository.FirstOrDefault(sg => 
                sg.ActivityId == req.ActivityId && 
                sg.GoodsId == req.GoodsId && 
                sg.SkuId == req.SkuId && 
                sg.IsValid == 1);

            if (seckillGoods == null)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "该商品当前未参与秒杀活动"
                };

            // 检查库存
            if (seckillGoods.SoldQuantity + req.Quantity > seckillGoods.SeckillStock)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "商品库存不足"
                };

            // 检查用户购买限制
            var purchasedCount = _seckillRepository.GetUserPurchaseCount(
                activity.Id,
                req.GoodsId,
                userId);

            if (purchasedCount + req.Quantity > activity.LimitPerUser)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = string.Format("每人限购{0}件，您已购买{1}件", activity.LimitPerUser, purchasedCount)
                };

            // 限制用户在规定时间内只能抢购一次
            var requestLimitSeconds = 10;
            if (_seckillRepository.HasUserPurchasedRecently(userId, activity.Id, requestLimitSeconds))
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "您的抢购请求过于频繁，请稍后重试"
                };

            // 使用锁机制确保线程安全
            lock (_lockObj)
            {
                return PurchaseInternal(req, userId, activity, seckillGoods);
            }
        }

        /// <summary>
        /// 内部抢购逻辑
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <param name="activity"></param>
        /// <param name="seckillGoods"></param>
        /// <returns></returns>
        private ResSeckillPurchaseResult PurchaseInternal(ReqSeckillPurchase req, string userId, ModelSeckillActivity activity, ModelSeckillGoods seckillGoods)
        {
            var now = DateTime.Now;

            // 重新检查库存
            if (seckillGoods.SoldQuantity + req.Quantity > seckillGoods.SeckillStock)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "商品库存不足"
                };

            // 重新检查用户购买限制
            var purchasedCount = _seckillRepository.GetUserPurchaseCount(
                activity.Id,
                req.GoodsId,
                userId);

            if (purchasedCount + req.Quantity > activity.LimitPerUser)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = string.Format("每人限购{0}件，您已购买{1}件", activity.LimitPerUser, purchasedCount)
                };

            // 创建秒杀订单
            var order = new ModelSeckillOrder
            {
                Id = Guid.NewGuid().ToString(),
                OrderNo = GenerateOrderNo(),
                ActivityId = activity.Id,
                GoodsId = req.GoodsId,
                SkuId = req.SkuId,
                UserId = userId,
                Quantity = req.Quantity,
                SeckillPrice = seckillGoods.SeckillPrice,
                TotalAmount = seckillGoods.SeckillPrice * req.Quantity,
                Status = (int)SeckillOrderStatus.PendingPayment,
                CreateTime = now
            };

            // 扣减库存
            var updateResult = _seckillRepository.UpdateSeckillStock(seckillGoods.Id, req.Quantity);
            if (!updateResult)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "库存扣减失败，请重试"
                };

            // 保存订单
            var orderResult = _seckillRepository.AddSeckillOrder(order);
            if (!orderResult)
                return new ResSeckillPurchaseResult
                {
                    Success = false,
                    Message = "订单创建失败，请重试"
                };

            return new ResSeckillPurchaseResult
            {
                Success = true,
                OrderNo = order.OrderNo,
                Message = "抢购成功",
                ActivityStatus = activity.Status,
                Stock = seckillGoods.SeckillStock - seckillGoods.SoldQuantity - req.Quantity,
                SoldQuantity = seckillGoods.SoldQuantity + req.Quantity
            };
        }

        /// <summary>
        /// 异步抢购
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResSeckillPurchaseResult> PurchaseAsync(ReqSeckillPurchase req)
        {
            return await Task.Run(() => Purchase(req));
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        private string GenerateOrderNo()
        {
            var now = DateTime.Now;
            return $"SK{now:yyyyMMddHHmmssfff}{new Random().Next(1000, 9999)}";
        }

        /// <summary>
        /// 查询用户秒杀订单
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public TableData GetUserOrders(int pageSize = 20, int pageIndex = 1)
        {
            var result = new TableData();
            var userId = Auth.UserId;

            if (string.IsNullOrEmpty(userId))
                throw new CustomException(401, "请先登录");

            var list = Repository.Queryable()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreateTime)
                .Page(pageIndex, pageSize)
                .Select(o => new
                {
                    o.Id,
                    o.OrderNo,
                    o.ActivityId,
                    o.GoodsId,
                    o.SkuId,
                    o.Quantity,
                    o.SeckillPrice,
                    o.TotalAmount,
                    o.Status,
                    o.CreateTime
                }).ToList();

            result.data = list;
            result.count = Repository.Queryable().Where(o => o.UserId == userId).Count();
            return result;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ModelSeckillOrder GetOrderDetail(string orderNo)
        {
            var userId = Auth.UserId;

            if (string.IsNullOrEmpty(userId))
    throw new CustomException(401, "请先登录");

            var order = _seckillRepository.GetSeckillOrderByNo(orderNo);
            if (order == null)
                throw new CustomException(404, "订单不存在");

            if (order.UserId != userId)
                throw new CustomException(403, "无权查看该订单");

            return order;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool CancelOrder(string orderNo)
        {
            var userId = Auth.UserId;

            if (string.IsNullOrEmpty(userId))
    throw new CustomException(401, "请先登录");

            var order = _seckillRepository.GetSeckillOrderByNo(orderNo);
            if (order == null)
                throw new CustomException(404, "订单不存在");

            if (order.UserId != userId)
                throw new CustomException(403, "无权操作该订单");

            // 检查订单状态
            if (order.Status == (int)SeckillOrderStatus.Canceled)
                throw new CustomException(400, "订单已取消");

            if (order.Status == (int)SeckillOrderStatus.Paid)
                throw new CustomException(400, "订单已支付，不允许取消");

            // 更新订单状态为已取消
            var updateResult = _seckillRepository.UpdateSeckillOrderStatus(orderNo, (int)SeckillOrderStatus.Canceled);
            if (!updateResult)
                return false;

            // 恢复库存
            var seckillGoods = _goodsRepository.Find(order.GoodsId);
            if (seckillGoods != null)
            {
                _seckillRepository.UpdateSeckillStock(seckillGoods.Id, -order.Quantity);
            }

            return true;
        }

        /// <summary>
        /// 获取用户已购买数量
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public int GetUserPurchasedCount(string activityId, string goodsId)
        {
            var userId = Auth.UserId;
            if (string.IsNullOrEmpty(userId))
                return 0;

            return _seckillRepository.GetUserPurchaseCount(activityId, goodsId, userId);
        }

        /// <summary>
        /// 同步秒杀活动状态
        /// </summary>
        public void SyncActivityStatus()
        {
            var now = DateTime.Now;

            // 更新待开始的活动为进行中
            var pendingActivities = _activityRepository.Queryable()
                .Where(a => a.Status == (int)SeckillActivityStatus.Pending && a.StartTime <= now)
                .ToList();

            foreach (var activity in pendingActivities)
            {
                activity.Status = (int)SeckillActivityStatus.Ongoing;
                activity.UpdateTime = now;
                _activityRepository.Update(activity);
            }

            // 更新进行中的活动为已结束
            var ongoingActivities = _activityRepository.Queryable()
                .Where(a => a.Status == (int)SeckillActivityStatus.Ongoing && a.EndTime <= now)
                .ToList();

            foreach (var activity in ongoingActivities)
            {
                activity.Status = (int)SeckillActivityStatus.Ended;
                activity.UpdateTime = now;
                _activityRepository.Update(activity);

                // 同步结束秒杀商品
                var seckillGoods = _seckillRepository.GetSeckillGoodsByActivityId(activity.Id);
                if (seckillGoods != null && seckillGoods.Count > 0)
                {
                    foreach (var sg in seckillGoods)
                    {
                        sg.IsValid = 0;
                        _goodsRepository.Update(sg);
                    }
                }
            }
        }
    }
}