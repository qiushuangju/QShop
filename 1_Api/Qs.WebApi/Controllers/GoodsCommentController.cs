using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;      
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 商品评价 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsCommentController : ControllerBase
    {
        private readonly AppGoodsComment _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public GoodsCommentController(AppGoodsComment app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public TableData Load([FromQuery]ReqQuGoodsComment req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ResGoodsComment>> ListByWhere([FromQuery]ReqQuGoodsComment req)
        {
            Response<List<ResGoodsComment>> res = new Response<List<ResGoodsComment>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }

        /// <summary>
        /// 获取分组数量
        /// </summary>
        [HttpGet]
        public Response<ResTotalGroup> TotalGroup(string goodsId)
        {
            var result = new Response<ResTotalGroup>();
            result.Result = _app.TotalGroup(goodsId);
            return result;
        }

        /// <summary>
        /// 修改显示状态
        /// </summary>
        [HttpPost]
        public Response<ResGoods> SetStatus([FromBody] ReqSetStatus req)
        {
            var result = new Response<ResGoods>();
            _app.SetStatus(req);
            return result;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelGoodsComment> Get(string id)
        {
            var result = new Response<ModelGoodsComment>();
            result.Result = _app.Get(id);
            return result;
        }

        /// <summary>
        /// 新增评价
        /// </summary>
        [HttpPost]
        public Response AddComment([FromBody] ReqAuGoodsComment req)
        {
            var result = new Response();
            _app.AddComment(req);
            return result;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody] ReqAuGoodsComment req)
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
            _app.Delete(ids);
            return result;
        }

     
    }
}
