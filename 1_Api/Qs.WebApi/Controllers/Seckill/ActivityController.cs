using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.WebApi.Controllers.Seckill
{
    /// <summary>
    /// 秒杀活动操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly AppSeckillActivity _app;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public ActivityController(AppSeckillActivity app)
        {
            _app = app;
        }
        
        /// <summary>
        /// 秒杀活动列表查询(分页)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public TableData Load([FromQuery]ReqQuSeckillActivity req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 获取秒杀活动详情
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<object> GetDetail(string id)
        {
            var result = new Response<object>();
            result.Result = _app.GetDetail(id);
            return result;
        }
        
        /// <summary>
        /// 获取秒杀活动商品列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<object>> GetSeckillGoods(string activityId)
        {
            var result = new Response<List<object>>();
            result.Result = _app.GetSeckillGoods(activityId).Cast<object>().ToList();
            return result;
        }
        
        /// <summary>
        /// 创建秒杀活动
        /// </summary>
        [HttpPost]
        public Response Add([FromBody]ReqAddSeckillActivity req)
        {
            var result = new Response();
            _app.Add(req);
            return result;
        }
        
        /// <summary>
        /// 更新秒杀活动
        /// </summary>
        [HttpPost]
        public Response Update([FromBody]ReqUpdateSeckillActivity req)
        {
            var result = new Response();
            _app.Update(req);
            return result;
        }
        
        /// <summary>
        /// 取消秒杀活动
        /// </summary>
        [HttpPost]
        public Response Cancel(string id)
        {
            var result = new Response();
            _app.Cancel(id);
            return result;
        }
        
        /// <summary>
        /// 批量删除秒杀活动
        /// </summary>
        [HttpPost]
        public Response Delete([FromBody]string[] ids)
        {
            var result = new Response();
            _app.Delete(ids);
            return result;
        }
    }
}