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
    /// 秒杀活动管理
    /// </summary>
    [Route("[controller]/[action]")]
    public class SeckillActivityController : BaseController
    {
        private readonly AppSeckillActivity _appSeckillActivity;

        public SeckillActivityController(AppSeckillActivity appSeckillActivity)
        {
            _appSeckillActivity = appSeckillActivity;
        }

        /// <summary>
        /// 分页查询秒杀活动
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuSeckillActivity req)
        {
            return _appSeckillActivity.Load(req);
        }

        /// <summary>
        /// 获取秒杀活动详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ResSeckillActivity> GetDetail(string id)
        {
            var result = new ResponseResult<ResSeckillActivity>();
            try
            {
                var detail = _appSeckillActivity.GetDetail(id);
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
        /// 创建秒杀活动
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> Add([FromBody]ReqAuSeckillActivity req)
        {
            var result = new ResponseResult<bool>();
            try
            {
                var activityId = _appSeckillActivity.Add(req);
                result.Result = !string.IsNullOrEmpty(activityId);
                result.Success = true;
                result.Message = "创建成功";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 更新秒杀活动
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> Update([FromBody]ReqAuSeckillActivity req)
        {
            var result = new ResponseResult<bool>();
            try
            {
                var updateResult = _appSeckillActivity.Update(req);
                result.Result = updateResult;
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
        /// 取消秒杀活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> Cancel(string id)
        {
            var result = new ResponseResult<bool>();
            try
            {
                var cancelResult = _appSeckillActivity.Cancel(id);
                result.Result = cancelResult;
                result.Success = true;
                result.Message = "取消成功";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 结束秒杀活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> End(string id)
        {
            var result = new ResponseResult<bool>();
            try
            {
                var endResult = _appSeckillActivity.End(id);
                result.Result = endResult;
                result.Success = true;
                result.Message = "结束成功";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除秒杀活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ResponseResult<bool> Delete(string id)
        {
            var result = new ResponseResult<bool>();
            try
            {
                var deleteResult = _appSeckillActivity.Delete(id);
                result.Result = deleteResult;
                result.Success = true;
                result.Message = "删除成功";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取秒杀活动状态列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<Option>> GetActivityStatusList()
        {
            var result = new ResponseResult<List<Option>>();
            try
            {
                var list = new List<Option>
                {
                    new Option { Value = (int)SeckillActivityStatus.Pending, Label = "待开始" },
                    new Option { Value = (int)SeckillActivityStatus.Ongoing, Label = "进行中" },
                    new Option { Value = (int)SeckillActivityStatus.Ended, Label = "已结束" },
                    new Option { Value = (int)SeckillActivityStatus.Canceled, Label = "已取消" }
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

        /// <summary>
        /// 获取活动的秒杀商品
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ResSeckillGoods>> GetActivityGoods(string activityId)
        {
            var result = new ResponseResult<List<ResSeckillGoods>>();
            try
            {
                var list = _appSeckillActivity.GetActivityGoods(activityId);
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