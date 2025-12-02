using System;
using Qs.Repository;
using Qs.Repository.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Qs.Repository.Interface
{
    /// <summary>
    /// 秒杀活动仓储接口
    /// </summary>
    public interface ISeckillRepository : IRepository<ModelSeckillActivity, QsDBContext>
    {
        /// <summary>
        /// 获取秒杀商品列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        List<ModelSeckillGoods> GetSeckillGoodsByActivityId(string activityId);

        /// <summary>
        /// 获取秒杀商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ModelSeckillGoods GetSeckillGoodsById(string id);

        /// <summary>
        /// 批量添加秒杀商品
        /// </summary>
        /// <param name="seckillGoods"></param>
        void BatchAddSeckillGoods(List<ModelSeckillGoods> seckillGoods);

        /// <summary>
        /// 更新秒杀商品库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        bool UpdateSeckillStock(string id, int quantity);

        /// <summary>
        /// 获取用户已购买数量
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="goodsId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetUserPurchaseCount(string activityId, string goodsId, string userId);

        /// <summary>
        /// 添加秒杀订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool AddSeckillOrder(ModelSeckillOrder order);

        /// <summary>
        /// 获取秒杀订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        ModelSeckillOrder GetSeckillOrderByNo(string orderNo);

        /// <summary>
        /// 更新秒杀订单状态
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool UpdateSeckillOrderStatus(string orderNo, int status);

        /// <summary>
        /// 检查用户是否在规定时间内已抢购
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="activityId"></param>
        /// <param name="timeSpanSeconds"></param>
        /// <returns></returns>
        bool HasUserPurchasedRecently(string userId, string activityId, int timeSpanSeconds);
    }

    /// <summary>
    /// 秒杀活动仓储接口实现
    /// </summary>
    public class SeckillRepository : BaseRepository<ModelSeckillActivity, QsDBContext>, ISeckillRepository
    {
        public SeckillRepository(QsDBContext dbContext) : base(dbContext) { }

        /// <summary>
        /// 获取秒杀商品列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<ModelSeckillGoods> GetSeckillGoodsByActivityId(string activityId)
        {
            var list = _context.Set<ModelSeckillGoods>()
                .Where(sg => sg.ActivityId == activityId)
                .ToList();
            return list;
        }

        /// <summary>
        /// 获取秒杀商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ModelSeckillGoods GetSeckillGoodsById(string id)
        {
            var goods = _context.Set<ModelSeckillGoods>()
                .Where(sg => sg.Id == id)
                .FirstOrDefault();
            return goods;
        }

        /// <summary>
        /// 批量添加秒杀商品
        /// </summary>
        /// <param name="seckillGoods"></param>
        public void BatchAddSeckillGoods(List<ModelSeckillGoods> seckillGoods)
        {
            _context.Set<ModelSeckillGoods>().AddRange(seckillGoods);
            _context.SaveChanges();
        }

        /// <summary>
        /// 更新秒杀商品库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool UpdateSeckillStock(string id, int quantity)
        {
            var seckillGoods = _context.Set<ModelSeckillGoods>().Find(id);
            if (seckillGoods != null)
            {
                seckillGoods.SeckillStock = quantity;
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        /// <summary>
        /// 更新秒杀订单状态
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateSeckillOrderStatus(string orderNo, int status)
        {
            var seckillOrder = _context.Set<ModelSeckillOrder>().FirstOrDefault(o => o.OrderNo == orderNo);
            if (seckillOrder != null)
            {
                seckillOrder.Status = status;
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        /// <summary>
        /// 获取用户已购买数量
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="goodsId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserPurchaseCount(string activityId, string goodsId, string userId)
        {
            var count = _context.Set<ModelSeckillOrder>()
                .Where(o => o.ActivityId == activityId && o.GoodsId == goodsId && o.UserId == userId)
                .Count();
            return count;
        }

        /// <summary>
        /// 添加秒杀订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool AddSeckillOrder(ModelSeckillOrder order)
        {
            _context.Set<ModelSeckillOrder>().Add(order);
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// 获取秒杀订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ModelSeckillOrder GetSeckillOrderByNo(string orderNo)
        {
            var order = _context.Set<ModelSeckillOrder>()
                .Where(o => o.OrderNo == orderNo)
                .FirstOrDefault();
            return order;
        }

        /// <summary>
        /// 检查用户是否在规定时间内已抢购
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="activityId"></param>
        /// <param name="timeSpanSeconds"></param>
        /// <returns></returns>
        public bool HasUserPurchasedRecently(string userId, string activityId, int timeSpanSeconds)
        {
            var startTime = DateTime.Now.AddSeconds(-timeSpanSeconds);
            var count = _context.Set<ModelSeckillOrder>()
                .Where(o => o.UserId == userId && o.ActivityId == activityId && o.CreateTime >= startTime)
                .Count();
            return count > 0;
        }
    }
}