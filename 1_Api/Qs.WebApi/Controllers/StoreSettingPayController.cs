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
    /// 支付设置 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreSettingPayController : ControllerBase
    {
        private readonly AppStoreSettingPay _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public StoreSettingPayController(AppStoreSettingPay app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuStoreSettingPay req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ModelStoreSettingPay>> ListByWhere([FromQuery]ReqQuStoreSettingPay req)
        {
            Response<List<ModelStoreSettingPay>> res = new Response<List<ModelStoreSettingPay>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ResStoreSettingPay> Get(string id)
        {
            var result = new Response<ResStoreSettingPay>();
            result.Result = _app.GetDetail(id);
            return result;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        [HttpPost]
        public Response SetStatus([FromBody] ReqSetSettingPayStatus req)
        {
            var result = new Response();
            _app.SetStatus(req);
            return result;
        }

        /// <summary>
        /// 设为默认
        /// </summary>
        [HttpPost]
        public Response SetDefault([FromBody] ReqSetSettingPayDefault req)
        {
            var result = new Response();
            _app.SetDefault(req);
            return result;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuStoreSettingPay req)
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
