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
    /// 用户积分变动明细 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserPointsLogController : ControllerBase
    {
        private readonly AppUserPointsLog _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public UserPointsLogController(AppUserPointsLog app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuUserPointsLog req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ModelUserPointsLog>> ListByWhere([FromQuery]ReqQuUserPointsLog req)
        {
            Response<List<ModelUserPointsLog>> res = new Response<List<ModelUserPointsLog>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelUserPointsLog> Get(string id)
        {
            var result = new Response<ModelUserPointsLog>();
            result.Result = _app.Get(id);
            return result;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuUserPointsLog req)
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
