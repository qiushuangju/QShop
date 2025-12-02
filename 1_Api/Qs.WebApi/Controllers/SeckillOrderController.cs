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
    /// 秒杀订单控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SeckillOrderController : ControllerBase
    {
        private readonly AppSeckillOrder _appSeckillOrder;

        public SeckillOrderController(AppSeckillOrder appSeckillOrder)
        {
            _appSeckillOrder = appSeckillOrder;
        }

        /// <summary>
        /// 秒杀抢购
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResult<ModelSeckillOrder>>>> SeckillPurchase([FromBody] SeckillPurchaseReq req)
        {
            try
            {
                var seckillOrder = await _appSeckillOrder.SeckillPurchase(req.SeckillGoodsId, req.Quantity);
                return ApiResult.Success(seckillOrder);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 支付秒杀订单
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResult<bool>>> PaySeckillOrder([FromBody] PaySeckillOrderReq req)
        {
            try
            {
                await _appSeckillOrder.PaySeckillOrder(req.SeckillOrderId);
                return ApiResult.Success(true);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 取消秒杀订单
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResult<bool>>> CancelSeckillOrder([FromBody] CancelSeckillOrderReq req)
        {
            try
            {
                await _appSeckillOrder.CancelSeckillOrder(req.SeckillOrderId);
                return ApiResult.Success(true);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取用户秒杀订单列表
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<ModelSeckillOrder>>>>> GetUserSeckillOrderList([FromQuery] GetUserSeckillOrderListReq req)
        {
            try
            {
                var orderList = await _appSeckillOrder.GetUserSeckillOrderList(req.Status);
                return ApiResult.Success(orderList);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取秒杀订单详情
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResult<ModelSeckillOrder>>>> GetSeckillOrderDetail([FromQuery] decimal seckillOrderId)
        {
            try
            {
                var seckillOrder = await _appSeckillOrder.GetSeckillOrderDetail(seckillOrderId);
                return ApiResult.Success(seckillOrder);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }
    }

    /// <summary>
    /// 秒杀抢购请求
    /// </summary>
    public class SeckillPurchaseReq
    {
        /// <summary>
        /// 秒杀商品ID
        /// </summary>
        public decimal SeckillGoodsId { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 支付秒杀订单请求
    /// </summary>
    public class PaySeckillOrderReq
    {
        /// <summary>
        /// 秒杀订单ID
        /// </summary>
        public decimal SeckillOrderId { get; set; }
    }

    /// <summary>
    /// 取消秒杀订单请求
    /// </summary>
    public class CancelSeckillOrderReq
    {
        /// <summary>
        /// 秒杀订单ID
        /// </summary>
        public decimal SeckillOrderId { get; set; }
    }

    /// <summary>
    /// 获取用户秒杀订单列表请求
    /// </summary>
    public class GetUserSeckillOrderListReq
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public int? Status { get; set; }
    }
}
