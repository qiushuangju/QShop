using System;
using System.Collections.Generic;
using System.Linq;
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
    /// tb_Cart操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppCart _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public CartController(AppCart app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuCart req)
        {
            return _app.Load(req);
        }

        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<ResListTotalCart> ListTotalByWhere([FromQuery] ReqQuCart req)
        {
            Response<ResListTotalCart> res = new Response<ResListTotalCart>();
            res.Result=_app.ListTotalByWhere(req);
            return res;
        }

        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ResCart>> ListByWhere([FromQuery]ReqQuCart req)
        {
            Response<List<ResCart>> res = new Response<List<ResCart>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelCart> Get(string id)
        {
            var result = new Response<ModelCart>();
            result.Result = _app.Get(id);
            return result;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuCart req)
        {
            var result = new Response();
            _app.AddOrUpdate(req);
            return result;
        }
        /// <summary>
        /// 添加购物车
        /// </summary>
        [HttpPost]
        public Response<ResAddOrUpdateCart> AddCart([FromBody] ReqAddCart req)
        {
            Response<ResAddOrUpdateCart> res = new Response<ResAddOrUpdateCart>();
            res.Result = _app.AddCart(req);
            return res;
        }

        /// <summary>
        /// 修改购物车数量
        /// </summary>
        [HttpPost]
        public Response<ResAddOrUpdateCart> UpdateCartNum([FromBody] ReqAddCart req)
        {
            Response<ResAddOrUpdateCart> res = new Response<ResAddOrUpdateCart>();
            res.Result = _app.AddOrUpdateCart(req);
            return res;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [HttpPost]
        public Response Delete([FromBody]string[] cartIds)
        {
            var result = new Response();
            _app.Delete(cartIds);
            return result;
        }

     
    }
}
