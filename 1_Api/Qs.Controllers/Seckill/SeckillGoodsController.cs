using System;
using System.Collections.Generic;
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
    /// 秒杀商品展示
    /// </summary>
    [Route("[controller]/[action]")]
    public class SeckillGoodsController : BaseController
    {
        private readonly AppSeckillGoods _appSeckillGoods;

        public SeckillGoodsController(AppSeckillGoods appSeckillGoods)
        {
            _appSeckillGoods = appSeckillGoods;
        }

        /// <summary>
        /// 分页查询秒杀商品
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuSeckillGoods req)
        {
            return _appSeckillGoods.Load(req);
        }

        /// <summary>
        /// 首页秒杀商品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ResSeckillGoods>> GetHomePageSeckillGoods()
        {
            var result = new ResponseResult<List<ResSeckillGoods>>();
            try
            {
                var list = _appSeckillGoods.GetHomePageSeckillGoods();
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

        /// <summary>
        /// 获取进行中的秒杀商品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ResSeckillGoods>> GetOngoingSeckillGoods()
        {
            var result = new ResponseResult<List<ResSeckillGoods>>();
            try
            {
                var list = _appSeckillGoods.GetOngoingSeckillGoods();
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

        /// <summary>
        /// 获取待开始的秒杀商品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ResSeckillGoods>> GetPendingSeckillGoods()
        {
            var result = new ResponseResult<List<ResSeckillGoods>>();
            try
            {
                var list = _appSeckillGoods.GetPendingSeckillGoods();
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

        /// <summary>
        /// 获取秒杀商品详情
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ResSeckillGoods> GetDetail(string activityId, string goodsId)
        {
            var result = new ResponseResult<ResSeckillGoods>();
            try
            {
                var detail = _appSeckillGoods.GetDetail(activityId, goodsId);
                result.Result = detail;
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
        /// 根据商品ID获取秒杀活动
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ResSeckillGoods>> GetByGoodsId(string goodsId)
        {
            var result = new ResponseResult<List<ResSeckillGoods>>();
            try
            {
                var list = _appSeckillGoods.GetByGoodsId(goodsId);
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

        /// <summary>
        /// 实时更新商品状态
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ResSeckillGoods> UpdateStatus(string activityId, string goodsId)
        {
            var result = new ResponseResult<ResSeckillGoods>();
            try
            {
                var detail = _appSeckillGoods.GetDetail(activityId, goodsId);
                if (detail != null)
                {
                    // 重新计算倒计时
                    detail.CalculateCountdown();
                    // 重新计算已抢百分比
                    if (detail.SeckillStock > 0)
                    {
                        detail.PurchasedPercentage = (int)Math.Round((double)detail.SoldQuantity / detail.SeckillStock * 100);
                    }
                }
                result.Result = detail;
                result.Success = true;
                result.Message = "更新成功";
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
                var count = _appSeckillGoods.GetUserPurchasedCount(activityId, goodsId);
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
    }
}