using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Domain;

namespace Qs.WebApi.Controllers.Goods
{
    /// <summary>
    /// tb_Goods操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        private readonly AppGoods _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public GoodsController(AppGoods app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public TableData Load([FromQuery]ReqQuGoods req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public Response<List<ResGoods>> ListByWhere([FromQuery]ReqQuGoods req)
        {
            Response<List<ResGoods>> res = new Response<List<ResGoods>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<ResGoods> Get(string id)
        {
            var result = new Response<ResGoods>();
            result.Result = _app.Get(id);
            return result;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ModelGoodsService>> GetServiceList(string goodsId)
        {
            var result = new Response<List<ModelGoodsService>>();
            result.Result = _app.GetServiceList(goodsId);
            return result;
        }
        

        /// <summary>
        /// 修改上架状态
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public Response<ResGoods> SetStatus([FromBody] ReqSetStatus req)
        {
            var result = new Response<ResGoods>();
            _app.SetStatus(req);
            return result;
        }

        /// <summary>
        /// 修改上架状态
        /// </summary>
        [HttpPost]
        public Response<ResGoods> SetRecommend([FromBody] ReqSetRecommend req)
        {
            var result = new Response<ResGoods>();
            _app.SetRecommend(req);
            return result;
        }
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuGoods req)
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
