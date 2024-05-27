using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// tb_GoodsCategory操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsCateController : ControllerBase
    {
        private readonly AppGoodsCate _app;

        /// <summary>
        /// /构造函数
        /// </summary>
        public GoodsCateController(AppGoodsCate app)
        {
            _app = app;
        }

        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery] ReqQuGoodsCategory req)
        {
            return _app.Load(req);
        }

        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ResGoodsCate>> ListByWhere([FromQuery] ReqQuGoodsCategory req)
        {
            Response<List<ResGoodsCate>> res = new Response<List<ResGoodsCate>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ResGoodsCate>> ListShowCate([FromQuery] ReqQuGoodsCategory req)
        {
            Response<List<ResGoodsCate>> res = new Response<List<ResGoodsCate>>();
            res.Result = _app.ListShowCate(req);
            return res;
        }
        

        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ResGoodsCate> Get(string id)
        {
            var result = new Response<ResGoodsCate>();
            result.Result = _app.Get(id);
            return result;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody] ReqAuGoodsCate req)
        {

            var result = new Response();
            _app.AddOrUpdate(req);
            return result;
        }

        /// <summary>
        /// 修改是否展示
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response ChangeStatus([FromBody] ReqAuChangeCateStatus req)
        {
            var result = new Response();
            _app.ChangeStatus(req);
            return result;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [HttpPost]
        public Response Delete([FromBody] string[] ids)
        {
            var result = new Response();
            _app.Delete(ids);
            return result;
        }

     
        
    }
}
