using Microsoft.EntityFrameworkCore;
using Qs.App.Base;
using Qs.App.Interface;
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
    /// 秒杀商品服务
    /// </summary>
    public class AppSeckillGoods : AppBaseLong<ModelSeckillGoods, QsDBContext>
    {
        private readonly AppGoods _appGoods;
        private readonly AppGoodsSku _appGoodsSku;

        public AppSeckillGoods(IUnitWork<QsDBContext> unitWork, IRepository<ModelSeckillGoods, QsDBContext> repository, IAuth auth, AppGoods appGoods, AppGoodsSku appGoodsSku)
            : base(unitWork, repository, auth)
        {
            _appGoods = appGoods;
            _appGoodsSku = appGoodsSku;
        }

        /// <summary>
        /// 添加秒杀商品
        /// </summary>
        public async Task AddSeckillGoods(ModelSeckillGoods seckillGoods)
        {
            // 验证商品是否存在
            var goods = await UnitWork.FirstOrDefaultAsync<ModelGoods>(o => o.Id == seckillGoods.GoodsId);
            if (goods == null)
            {
                throw new Exception("商品不存在");
            }

            // 如果是多规格商品，验证SKU是否存在
            if (seckillGoods.SkuId.HasValue)
            {
                var sku = await UnitWork.FirstOrDefaultAsync<ModelGoodsSku>(o => o.Id == seckillGoods.SkuId.Value);
                if (sku == null || sku.GoodsId != seckillGoods.GoodsId)
                {
                    throw new Exception("商品SKU不存在");
                }

                seckillGoods.SkuDesc = sku.SkuName;
            }

            // 设置商品基本信息
            seckillGoods.GoodsName = goods.Name;
            seckillGoods.GoodsImage = goods.ImgUrl;
            seckillGoods.OriginalPrice = seckillGoods.SkuId.HasValue ? 
                (await UnitWork.FirstOrDefaultAsync<ModelGoodsSku>(o => o.Id == seckillGoods.SkuId.Value)).Price : goods.ShopPrice;

            // 初始化已抢数量为0
            seckillGoods.GrabbedQuantity = 0;

            // 设置创建信息
            seckillGoods.CreateTime = DateTime.Now;
            seckillGoods.CreateUserId = _auth.GetUserId();

            await Repository.AddAsync(seckillGoods);
            await UnitWork.SaveAsync();
        }

        /// <summary>
        /// 批量添加秒杀商品
        /// </summary>
        public async Task BatchAddSeckillGoods(List<ModelSeckillGoods> seckillGoodsList)
        {
            foreach (var seckillGoods in seckillGoodsList)
            {
                // 验证商品是否存在
            var goods = await UnitWork.FirstOrDefaultAsync<ModelGoods>(o => o.Id == seckillGoods.GoodsId);
            if (goods == null)
            {
                throw new Exception(string.Format("商品ID {0} 不存在", seckillGoods.GoodsId));
            }

            // 如果是多规格商品，验证SKU是否存在
            if (seckillGoods.SkuId.HasValue)
            {
                var sku = await UnitWork.FirstOrDefaultAsync<ModelGoodsSku>(o => o.Id == seckillGoods.SkuId.Value);
                if (sku == null || sku.GoodsId != seckillGoods.GoodsId)
                {
                    throw new Exception(string.Format("商品SKU ID {0} 不存在", seckillGoods.SkuId.Value));
                }

                seckillGoods.SkuDesc = sku.SkuName;
            }

            // 设置商品基本信息
            seckillGoods.GoodsName = goods.Name;
            seckillGoods.GoodsImage = goods.ImgUrl;
            seckillGoods.OriginalPrice = seckillGoods.SkuId.HasValue ? 
                (await UnitWork.FirstOrDefaultAsync<ModelGoodsSku>(o => o.Id == seckillGoods.SkuId.Value)).Price : goods.ShopPrice;

                // 初始化已抢数量为0
                seckillGoods.GrabbedQuantity = 0;

                // 设置创建信息
                seckillGoods.CreateTime = DateTime.Now;
                seckillGoods.CreateUserId = _auth.GetUserId();

                await Repository.AddAsync(seckillGoods);
            }

            await UnitWork.SaveAsync();
        }

        /// <summary>
        /// 获取秒杀商品列表
        /// </summary>
        public async Task<List<ModelSeckillGoods>> GetSeckillGoodsList(decimal activityId)
        {
            var seckillGoodsList = await Repository.Find(o => o.ActivityId == activityId).ToListAsync();
            return seckillGoodsList;
        }

        /// <summary>
        /// 获取秒杀商品详情
        /// </summary>
        public async Task<ModelSeckillGoods> GetSeckillGoodsDetail(decimal id)
        {
            var seckillGoods = await Repository.FirstOrDefaultAsync(o => o.Id == id);
            if (seckillGoods == null)
            {
                throw new Exception("秒杀商品不存在");
            }

            return seckillGoods;
        }

        /// <summary>
        /// 扣减秒杀商品库存
        /// </summary>
        public async Task<bool> ReduceStock(decimal seckillGoodsId, int quantity)
        {
            // 使用乐观锁更新库存
            var seckillGoods = await Repository.FirstOrDefaultAsync(o => o.Id == seckillGoodsId);
            if (seckillGoods == null)
            {
                throw new Exception("秒杀商品不存在");
            }

            if (seckillGoods.StockQuantity < quantity)
            {
                return false; // 库存不足
            }

            seckillGoods.StockQuantity -= quantity;
            seckillGoods.GrabbedQuantity += quantity;

            await Repository.UpdateAsync(seckillGoods);
            await UnitWork.SaveAsync();

            return true;
        }

        /// <summary>
        /// 恢复秒杀商品库存
        /// </summary>
        public async Task<bool> RestoreStock(decimal seckillGoodsId, int quantity)
        {
            var seckillGoods = await Repository.FirstOrDefaultAsync(o => o.Id == seckillGoodsId);
            if (seckillGoods == null)
            {
                throw new Exception("秒杀商品不存在");
            }

            seckillGoods.StockQuantity += quantity;
            seckillGoods.GrabbedQuantity -= quantity;

            await Repository.UpdateAsync(seckillGoods);
            await UnitWork.SaveAsync();

            return true;
        }
    }
}