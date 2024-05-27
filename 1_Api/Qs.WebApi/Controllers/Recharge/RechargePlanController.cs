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
    /// tb_RechargePlan操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RechargePlanController : ControllerBase
    {
        private readonly AppRechargePlan _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public RechargePlanController(AppRechargePlan app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public TableData Load([FromQuery]ReqQuRechargePlan req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ModelRechargePlan>> ListByWhere([FromQuery]ReqQuRechargePlan req)
        {
            Response<List<ModelRechargePlan>> res = new Response<List<ModelRechargePlan>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelRechargePlan> Get(string id)
        {
            var result = new Response<ModelRechargePlan>();
            result.Result = _app.Get(id);
            return result;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuRechargePlan req)
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
