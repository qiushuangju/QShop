using System;
using System.Collections.Generic;
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
    /// tb_RechargeOrder操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RechargeOrderController : ControllerBase
    {
        private readonly AppRechargeOrder _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public RechargeOrderController(AppRechargeOrder app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuRechargeOrder req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ResRechargeOrder>> ListByWhere([FromQuery]ReqQuRechargeOrder req)
        {
            Response<List<ResRechargeOrder>> res = new Response<List<ResRechargeOrder>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelRechargeOrder> Get(string id)
        {
            var result = new Response<ModelRechargeOrder>();
            result.Result = _app.Get(id);
            return result;
        }

        /// <summary>
        /// 充值前调用
        /// </summary>
        [HttpPost]
        public Response<ResPayConfirm> RechargeBefore([FromBody] ReqRechargeBefore req)
        {
            var result = new Response<ResPayConfirm>();
            result.Result = _app.RechargeBefore(req);
            return result;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuRechargeOrder req)
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
