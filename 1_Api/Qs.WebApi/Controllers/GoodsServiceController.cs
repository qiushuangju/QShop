using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;      
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Request;


namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 商品服务承诺 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsServiceController : ControllerBase
    {
        private readonly AppGoodsService _app;

        /// <summary>
        /// /构造函数
        /// </summary>
        public GoodsServiceController(AppGoodsService app)
        {
            _app = app;
        }

        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]                 
        [AllowAnonymous]
        public TableData Load([FromQuery] ReqQuGoodsService req)
        {
            return _app.Load(req);
        }

        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ModelGoodsService>> ListByWhere([FromQuery]ReqQuGoodsService req)
        {
            Response<List<ModelGoodsService>> res = new Response<List<ModelGoodsService>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelGoodsService> Get(string id)
        {
            var result = new Response<ModelGoodsService>();
            result.Result = _app.Get(id);
            return result;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuGoodsService req)
        {
            var result = new Response();
            _app.AddOrUpdate(req);
            return result;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [HttpPost]
        public Response Delete([FromBody]string[] ids)
        {
            var result = new Response();
            _app.DeleteModel(ids);
            return result;
        }

     
    }
}
