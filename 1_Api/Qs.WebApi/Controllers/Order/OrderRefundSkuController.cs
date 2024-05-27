using System;
using System.Collections.Generic;
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
    /// 售后信息
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderRefundSkuController : ControllerBase
    {
        private readonly AppOrderRefundSku _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public OrderRefundSkuController(AppOrderRefundSku app) 
        {
            _app = app;
        }                                                  
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuOrderRefundSku req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ResOrderRefundSku>> ListByWhere([FromQuery]ReqQuOrderRefundSku req)
        {
            int rowCount = 0;
            Response<List<ResOrderRefundSku>> res = new Response<List<ResOrderRefundSku>>();
            res.Result = _app.ListByWhere(req,ref rowCount, false);
            return res;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelOrderRefundSku> Get(string id)
        {
            var result = new Response<ModelOrderRefundSku>();
            result.Result = _app.Get(id);
            return result;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response Apply([FromBody] ReqOrderApplyRefund req)
        {
            var result = new Response();
            _app.ApplyRefund(req);
            return result;
        }

        /// <summary>
        /// 申请取消订单-审核
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<bool> Audit(ReqRefundAudit req)
        {
           
            var result = new Response<bool>();
            _app.Audit(req);
            result.Result = true;
            return result;
        }

        /// <summary>
        /// 获取退款订单详情
        /// </summary>
        [HttpGet]
        public Response<ResOrderRefundSku> GetRefundOrderDetail(string orderRefundSkuId)
        {
            var result = new Response<ResOrderRefundSku>();
            result.Result = _app.GetRefundOrderDetail(orderRefundSkuId);
            return result;
        }
         
        /// <summary>
        /// 获取退款订单详情
        /// </summary>
        [HttpPost]
        public Response<bool> RefundDelivery([FromBody] ReqRefundDelivery req)
        {
            var result = new Response<bool>();
            _app.RefundDelivery(req);
            result.Result = true;
            return result;
        }

        /// <summary>
        ///售后-退款
        /// </summary>
        /// <param name="req"></param>
        [HttpPost]
        public Response<bool> RefundMoney(ReqRefundMoney req) {
            var result = new Response<bool>();
            _app.RefundMoney(req);
            result.Result = true;
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
