using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.App;

namespace Qs.Controllers.Seckill
{
    /// <summary>
    /// 秒杀抢购流程
    /// </summary>
    [Route("[controller]/[action]")]
    public class SeckillPurchaseController : BaseController
    {
        private readonly AppSeckillPurchase _appSeckillPurchase;

        public SeckillPurchaseController(AppSeckillPurchase appSeckillPurchase)
        {
            _appSeckillPurchase = appSeckillPurchase;
        }

        /// <summary>
        /// 秒杀抢购
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ResSeckillPurchaseResult> Purchase([FromBody]ReqSeckillPurchase req)
        {
            var result = new ResponseResult<ResSeckillPurchaseResult>();
            try
            {
                var purchaseResult = _appSeckillPurchase.Purchase(req);
                result.Result = purchaseResult;
                result.Success = purchaseResult.Success;
                result.Message = purchaseResult.Message;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 异步秒杀抢购
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseResult<ResSeckillPurchaseResult>> PurchaseAsync([FromBody]ReqSeckillPurchase req)
        {
            var result = new ResponseResult<ResSeckillPurchaseResult>();
            try
            {
                var purchaseResult = await _appSeckillPurchase.PurchaseAsync(req);
                result.Result = purchaseResult;
                result.Success = purchaseResult.Success;
                result.Message = purchaseResult.Message;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 查询用户秒杀订单
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public TableData GetUserOrders(int pageSize = 20, int pageIndex = 1)
        {
            return _appSeckillPurchase.GetUserOrders(pageSize, pageIndex);
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ModelSeckillOrder> GetOrderDetail(string orderNo)
        {
            var result = new ResponseResult<ModelSeckillOrder>();
            try
            {
                var order = _appSeckillPurchase.GetOrderDetail(orderNo);
                result.Result = order;
                result.Success = true;
                result.Message = "获取成功";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> CancelOrder(string orderNo)
        {
            var result = new ResponseResult<bool>();
            try
            {
                var cancelResult = _appSeckillPurchase.CancelOrder(orderNo);
                result.Result = cancelResult;
                result.Success = true;
                result.Message = cancelResult ? "取消成功" : "取消失败";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取用户已购买数量
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<int> GetUserPurchasedCount(string activityId, string goodsId)
        {
            var result = new ResponseResult<int>();
            try
            {
                var count = _appSeckillPurchase.GetUserPurchasedCount(activityId, goodsId);
                result.Result = count;
                result.Success = true;
                result.Message = "获取成功";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 同步秒杀活动状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> SyncActivityStatus()
        {
            var result = new ResponseResult<bool>();
            try
            {
                _appSeckillPurchase.SyncActivityStatus();
                result.Success = true;
                result.Message = "同步成功";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取订单状态列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<Option>> GetOrderStatusList()
        {
            var result = new ResponseResult<List<Option>>();
            try
            {
                var list = new List<Option>
                {
                    new Option { Value = (int)SeckillOrderStatus.PendingPayment, Label = "待支付" },
                    new Option { Value = (int)SeckillOrderStatus.Paid, Label = "已支付" },
                    new Option { Value = (int)SeckillOrderStatus.Shipped, Label = "已发货" },
                    new Option { Value = (int)SeckillOrderStatus.Completed, Label = "已完成" },
                    new Option { Value = (int)SeckillOrderStatus.Canceled, Label = "已取消" },
                    new Option { Value = (int)SeckillOrderStatus.Refunded, Label = "已退款" }
                };
                result.Result = list;
                result.Success = true;
                result.Message = "获取成功";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
    }
}