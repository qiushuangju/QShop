using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.Comm;
using Qs.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 秒杀商品控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeckillGoodsController : ControllerBase
    {
        private readonly AppSeckillGoods _appSeckillGoods;

        public SeckillGoodsController(AppSeckillGoods appSeckillGoods)
        {
            _appSeckillGoods = appSeckillGoods;
        }

        /// <summary>
        /// 添加秒杀商品
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResult<ModelSeckillGoods>>> AddSeckillGoods([FromBody] AddSeckillGoodsReq req)
        {
            try
            {
                var seckillGoods = await _appSeckillGoods.AddSeckillGoods(req);
                return ApiResult.Success(seckillGoods);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 批量添加秒杀商品
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResult<List<ModelSeckillGoods>>>>> BatchAddSeckillGoods([FromBody] BatchAddSeckillGoodsReq req)
        {
            try
            {
                var seckillGoodsList = await _appSeckillGoods.BatchAddSeckillGoods(req);
                return ApiResult.Success(seckillGoodsList);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取秒杀商品列表
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<ModelSeckillGoods>>>>> GetSeckillGoodsList([FromQuery] GetSeckillGoodsListReq req)
        {
            try
            {
                var seckillGoodsList = await _appSeckillGoods.GetSeckillGoodsList(req.ActivityId, req.PageIndex, req.PageSize);
                return ApiResult.Success(seckillGoodsList);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取秒杀商品详情
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResult<ModelSeckillGoods>>>> GetSeckillGoodsDetail([FromQuery] decimal seckillGoodsId)
        {
            try
            {
                var seckillGoods = await _appSeckillGoods.GetSeckillGoodsDetail(seckillGoodsId);
                return ApiResult.Success(seckillGoods);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }
    }

    /// <summary>
    /// 添加秒杀商品请求
    /// </summary>
    public class AddSeckillGoodsReq
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public decimal ActivityId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public decimal GoodsId { get; set; }

        /// <summary>
        /// SKU ID
        /// </summary>
        public decimal SkuId { get; set; }

        /// <summary>
        /// 秒杀价格
        /// </summary>
        public decimal SeckillPrice { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 每人限购数量
        /// </summary>
        public int LimitPerUser { get; set; }
    }

    /// <summary>
    /// 批量添加秒杀商品请求
    /// </summary>
    public class BatchAddSeckillGoodsReq
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public decimal ActivityId { get; set; }

        /// <summary>
        /// 秒杀商品列表
        /// </summary>
        public List<AddSeckillGoodsReq> SeckillGoodsList { get; set; }
    }

    /// <summary>
    /// 获取秒杀商品列表请求
    /// </summary>
    public class GetSeckillGoodsListReq
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public decimal ActivityId { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
