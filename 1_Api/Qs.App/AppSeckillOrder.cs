using Microsoft.EntityFrameworkCore;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Core;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qs.App
{
    /// <summary>
    /// 秒杀订单服务
    /// </summary>
    public class AppSeckillOrder : AppBaseLong<ModelSeckillOrder, QsDBContext>
    {
        private readonly AppSeckillGoods _appSeckillGoods;
        private readonly AppOrder _appOrder;
        private readonly AppOrderSku _appOrderSku;

        public AppSeckillOrder(IUnitWork<QsDBContext> unitWork, IRepository<ModelSeckillOrder, QsDBContext> repository, IAuth auth, AppSeckillGoods appSeckillGoods, AppOrder appOrder, AppOrderSku appOrderSku)
            : base(unitWork, repository, auth)
        {
            _appSeckillGoods = appSeckillGoods;
            _appOrder = appOrder;
            _appOrderSku = appOrderSku;
        }

        /// <summary>
        /// 秒杀抢购
        /// </summary>
        public async Task<ModelSeckillOrder> SeckillPurchase(decimal seckillGoodsId, int quantity)
        {
            var userId = _auth.GetUserId();
            if (userId == 0)
            {
                throw new Exception("用户未登录");
            }

            // 获取秒杀商品详情
            var seckillGoods = await _appSeckillGoods.GetSeckillGoodsDetail(seckillGoodsId);

            // 验证购买数量是否超过限购
            if (quantity > seckillGoods.LimitPerUser)
            {
                throw new Exception(string.Format("每人限购 {0} 件", seckillGoods.LimitPerUser));
            }

            // 验证用户是否已经购买过该秒杀商品
            var existingOrder = await Repository.FirstOrDefaultAsync(o => o.SeckillGoodsId == seckillGoodsId && o.UserId == userId);
            if (existingOrder != null)
            {
                throw new Exception("您已经购买过该秒杀商品");
            }

            // 扣减秒杀商品库存
            var reduceStockResult = await _appSeckillGoods.ReduceStock(seckillGoodsId, quantity);
            if (!reduceStockResult)
            {
                throw new Exception("商品库存不足");
            }

            // 创建秒杀订单
            var seckillOrder = new ModelSeckillOrder
            {
                ActivityId = seckillGoods.ActivityId,
                SeckillGoodsId = seckillGoodsId,
                UserId = userId,
                GoodsId = seckillGoods.GoodsId,
                SkuId = seckillGoods.SkuId,
                Quantity = quantity,
                OriginalPrice = seckillGoods.OriginalPrice,
                SeckillPrice = seckillGoods.SeckillPrice,
                TotalPrice = seckillGoods.SeckillPrice * quantity,
                Status = 1, // 待支付
                OrderNo = GenerateSeckillOrderNo(),
                CreateTime = DateTime.Now
            };

            await Repository.AddAsync(seckillOrder);
            await UnitWork.SaveAsync();

            return seckillOrder;
        }

        /// <summary>
        /// 生成秒杀订单号
        /// </summary>
        private string GenerateSeckillOrderNo()
        {
            // 订单号格式：SK + 时间戳 + 随机数
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var random = new Random().Next(1000, 9999);
            return string.Format("SK{0}{1}", timestamp, random);
        }

        /// <summary>
        /// 支付秒杀订单
        /// </summary>
        public async Task PaySeckillOrder(decimal seckillOrderId)
        {
            var userId = _auth.GetUserId();
            if (userId == 0)
            {
                throw new Exception("用户未登录");
            }

            var seckillOrder = await Repository.FirstOrDefaultAsync(o => o.Id == seckillOrderId && o.UserId == userId);
            if (seckillOrder == null)
            {
                throw new Exception("秒杀订单不存在");
            }

            if (seckillOrder.Status != 1)
            {
                throw new Exception("订单状态异常，无法支付");
            }

            // 此处可以添加支付逻辑，调用第三方支付接口

            // 更新秒杀订单状态为已支付
            seckillOrder.Status = 2; // 已支付
            seckillOrder.PayTime = DateTime.Now;

            await Repository.UpdateAsync(seckillOrder);
            await UnitWork.SaveAsync();

            // 异步创建正式订单
            Task.Run(async () =>
            {
                await CreateFormalOrder(seckillOrder);
            });
        }

        /// <summary>
        /// 创建正式订单
        /// </summary>
        private async Task CreateFormalOrder(ModelSeckillOrder seckillOrder)
        {
            // 此处可以调用现有的订单创建逻辑
            // 由于不同的系统订单创建逻辑可能不同，这里只提供一个示例

            // 创建订单主表
            var order = new ModelOrder
            {
                OrderNo = seckillOrder.OrderNo,
                UserId = seckillOrder.UserId,
                TotalPrice = seckillOrder.TotalPrice,
                Status = 2, // 已支付
                CreateTime = DateTime.Now,
                PayTime = seckillOrder.PayTime
            };

            // 此处省略订单地址、配送方式等信息的设置

            // 调用订单服务创建订单
            // await _appOrder.CreateOrder(order);

            // 创建订单商品表
            var orderSku = new ModelOrderSku
            {
                OrderNo = seckillOrder.OrderNo,
                GoodsId = seckillOrder.GoodsId,
                SkuId = seckillOrder.SkuId,
                Quantity = seckillOrder.Quantity,
                Price = seckillOrder.SeckillPrice,
                TotalPrice = seckillOrder.TotalPrice,
                CreateTime = DateTime.Now
            };

            // 调用订单商品服务创建订单商品
            // await _appOrderSku.CreateOrderSku(orderSku);
        }

        /// <summary>
        /// 取消秒杀订单
        /// </summary>
        public async Task CancelSeckillOrder(decimal seckillOrderId)
        {
            var userId = _auth.GetUserId();
            if (userId == 0)
            {
                throw new Exception("用户未登录");
            }

            var seckillOrder = await Repository.FirstOrDefaultAsync(o => o.Id == seckillOrderId && o.UserId == userId);
            if (seckillOrder == null)
            {
                throw new Exception("秒杀订单不存在");
            }

            if (seckillOrder.Status != 1)
            {
                throw new Exception("订单状态异常，无法取消");
            }

            // 更新秒杀订单状态为已取消
            seckillOrder.Status = 3; // 已取消
            seckillOrder.CancelTime = DateTime.Now;

            await Repository.UpdateAsync(seckillOrder);

            // 恢复秒杀商品库存
            await _appSeckillGoods.RestoreStock(seckillOrder.SeckillGoodsId, seckillOrder.Quantity);

            await UnitWork.SaveAsync();
        }

        /// <summary>
        /// 获取用户秒杀订单列表
        /// </summary>
        public async Task<List<ModelSeckillOrder>> GetUserSeckillOrderList(int? status = null)
        {
            var userId = _auth.GetUserId();
            if (userId == 0)
            {
                throw new Exception("用户未登录");
            }

            var query = Repository.Find(o => o.UserId == userId);

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            var orderList = await query.OrderByDescending(o => o.CreateTime).ToListAsync();
            return orderList;
        }

        /// <summary>
        /// 获取秒杀订单详情
        /// </summary>
        public async Task<ModelSeckillOrder> GetSeckillOrderDetail(decimal id)
        {
            var userId = _auth.GetUserId();
            if (userId == 0)
            {
                throw new Exception("用户未登录");
            }

            var seckillOrder = await Repository.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
            if (seckillOrder == null)
            {
                throw new Exception("秒杀订单不存在");
            }

            return seckillOrder;
        }
    }
}
