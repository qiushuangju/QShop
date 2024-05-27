using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.WebApi.Controllers.Goods
{
    /// <summary>
    /// tb_GoodsSku操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsSkuController : ControllerBase
    {
        private readonly AppGoodsSku _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public GoodsSkuController(AppGoodsSku app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuGoodsSku req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ResGoodSku>> ListByWhere([FromQuery]ReqQuGoodsSku req)
        {
            Response<List<ResGoodSku>> res = new Response<List<ResGoodSku>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelGoodsSku> Get(string id)
        {
            var result = new Response<ModelGoodsSku>();
            result.Result = _app.Get(id);
            return result;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuGoodsSku req)
        {
            var result = new Response();
            _app.AddOrUpdate(req);
            return result;
        }


        /// <summary>
        /// Sku修改价格
        /// </summary>
        /// <param name="req"></param>
        [HttpPost]
        public Response ChangePrice([FromBody] ReqListChangeSkuPrice req)
        {
            var result = new Response();
            _app.ChangePrice(req);
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
