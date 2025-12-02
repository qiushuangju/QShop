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
    /// 秒杀活动控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class SeckillActivityController : ControllerBase
    {
        private readonly AppSeckillActivity _appSeckillActivity;

        public SeckillActivityController(AppSeckillActivity appSeckillActivity)
        {
            _appSeckillActivity = appSeckillActivity;
        }

        /// <summary>
        /// 创建秒杀活动
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResult<ModelSeckillActivity>>>> CreateSeckillActivity([FromBody] CreateSeckillActivityReq req)
        {
            try
            {
                var activity = await _appSeckillActivity.CreateSeckillActivity(req);
                return ApiResult.Success(activity);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 更新秒杀活动
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResult<bool>>> UpdateSeckillActivity([FromBody] UpdateSeckillActivityReq req)
        {
            try
            {
                await _appSeckillActivity.UpdateSeckillActivity(req);
                return ApiResult.Success(true);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 取消秒杀活动
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResult<bool>>> CancelSeckillActivity([FromBody] CancelSeckillActivityReq req)
        {
            try
            {
                await _appSeckillActivity.CancelSeckillActivity(req.ActivityId);
                return ApiResult.Success(true);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取秒杀活动列表
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<ModelSeckillActivity>>>>> GetSeckillActivityList([FromQuery] GetSeckillActivityListReq req)
        {
            try
            {
                var activityList = await _appSeckillActivity.GetSeckillActivityList(req.Status, req.PageIndex, req.PageSize);
                return ApiResult.Success(activityList);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取秒杀活动详情
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResult<ModelSeckillActivity>>>> GetSeckillActivityDetail([FromQuery] decimal activityId)
        {
            try
            {
                var activity = await _appSeckillActivity.GetSeckillActivityDetail(activityId);
                return ApiResult.Success(activity);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }
    }

    /// <summary>
    /// 创建秒杀活动请求
    /// </summary>
    public class CreateSeckillActivityReq
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 活动描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 活动图片
        /// </summary>
        public string ActivityImage { get; set; }

        /// <summary>
        /// 预热时间（分钟）
        /// </summary>
        public int WarmupMinutes { get; set; }
    }

    /// <summary>
    /// 更新秒杀活动请求
    /// </summary>
    public class UpdateSeckillActivityReq : CreateSeckillActivityReq
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public decimal Id { get; set; }
    }

    /// <summary>
    /// 取消秒杀活动请求
    /// </summary>
    public class CancelSeckillActivityReq
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public decimal ActivityId { get; set; }
    }

    /// <summary>
    /// 获取秒杀活动列表请求
    /// </summary>
    public class GetSeckillActivityListReq
    {
        /// <summary>
        /// 活动状态
        /// </summary>
        public int? Status { get; set; }

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
