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
    /// tb_UserAddress操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly AppUserAddress _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public UserAddressController(AppUserAddress app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuUserAddress req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ResUserAddress>> ListByWhere([FromQuery]ReqQuUserAddress req)
        {
            Response<List<ResUserAddress>> res = new Response<List<ResUserAddress>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ResUserAddress> GetDetail(string id)
        {
            var result = new Response<ResUserAddress>();
            result.Result = _app.GetDetail(id);
            return result;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuUserAddress req)
        {
            var result = new Response();
            _app.AddOrUpdate(req);
            return result;
        }
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response SetDefault([FromBody] ReqAuUserAddressSetDefault req)
        {
            var result = new Response();
            _app.SetDefault(req.Id);
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
