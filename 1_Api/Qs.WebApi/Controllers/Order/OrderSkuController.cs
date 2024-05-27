using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.WebApi.Controllers.Order
{
    /// <summary>
    /// tb_OrderSku操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderSkuController : ControllerBase
    {
        private readonly AppOrderSku _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public OrderSkuController(AppOrderSku app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuOrderGoods req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ResOrderSku>> ListByWhere([FromQuery]ReqQuOrderGoods req)
        {
            Response<List<ResOrderSku>> res = new Response<List<ResOrderSku>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelOrderSku> GetDetail(string id)
        {
            var result = new Response<ModelOrderSku>();
            result.Result = _app.GetDetail(id);
            return result;
        }


        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuOrderSku req)
        {
            var result = new Response();
            _app.AddOrUpdate(req);
            return result;
        }
        
        /// <summary>
        /// 取消订单
        /// </summary>
        [HttpPost]
        public Response CancelOrder([FromBody] ReqCancelOrderSku req)
        {
            var result = new Response();
            _app.CanelOrder(req);
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
