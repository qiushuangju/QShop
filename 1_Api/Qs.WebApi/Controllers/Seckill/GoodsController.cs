using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.Comm;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.WebApi.Controllers.Seckill
{
    /// <summary>
    /// 秒杀商品操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        private readonly AppSeckillGoods _app;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public GoodsController(AppSeckillGoods app)
        {
            _app = app;
        }
        
        /// <summary>
        /// 获取秒杀商品列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<object>> GetSeckillGoodsList(int status = 0)
        {
            var result = new Response<List<object>>();
            result.Result = _app.GetSeckillGoodsList(status).Cast<object>().ToList();
            return result;
        }
        
        /// <summary>
        /// 获取秒杀商品剩余时间
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<object> GetRemainingTime(string activityId)
        {
            var result = new Response<object>();
            // 临时返回空，AppSeckillGoods未实现GetRemainingTime方法
            result.Result = null;
            result.Message = "方法未实现";
            return result;
        }
        
        /// <summary>
        /// 秒杀抢购
        /// </summary>
        [HttpPost]
        [Authorize]
        public Response SeckillBuy([FromBody]ReqSeckillBuy req)
        {
            var result = new Response();
            // 获取当前登录用户ID
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                result.Code = 401;
                result.Message = "用户未登录";
                return result;
            }
            
            _app.SeckillBuy(req, userId);
            return result;
        }
    }
}