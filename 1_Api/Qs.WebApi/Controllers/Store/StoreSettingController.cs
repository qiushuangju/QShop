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
    /// tb_StoreSetting操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreSettingController : ControllerBase
    {
        private readonly AppStoreSetting _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public StoreSettingController(AppStoreSetting app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuStoreSetting req)
        {
            return _app.Load(req);
        }

        /// <summary>
        /// 公共数据
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<Dictionary<string, dynamic>> ListCommSettingData([FromQuery] ReqQuStoreSetting req)
        {
            Response<Dictionary<string, dynamic>> res = new Response<Dictionary<string, dynamic>>();
            res.Result = _app.ListCommSettingData();
            return res;
        }

        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ModelStoreSetting>> ListByWhere([FromQuery]ReqQuStoreSetting req)
        {
            Response<List<ModelStoreSetting>> res = new Response<List<ModelStoreSetting>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public Response<dynamic> GetDetail(string key)
        {
            var result = new Response<dynamic>();
            result.Result = _app.GetDetail(key);
            return result;
        }

        /// <summary>
        /// 注册设置
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody] ReqAuStoreSetting req)
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
