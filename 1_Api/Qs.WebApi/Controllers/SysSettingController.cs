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
    /// 系统设置 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SysSettingController : ControllerBase
    {
        private readonly AppSysSetting _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public SysSettingController(AppSysSetting app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuSysSetting req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ModelSysSetting>> ListByWhere([FromQuery]ReqQuSysSetting req)
        {
            Response<List<ModelSysSetting>> res = new Response<List<ModelSysSetting>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelSysSetting> Get(string id)
        {
            var result = new Response<ModelSysSetting>();
            result.Result = _app.Get(id);
            return result;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuSysSetting req)
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
