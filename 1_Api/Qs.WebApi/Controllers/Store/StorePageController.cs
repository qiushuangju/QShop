using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qs.App;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Vm;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// tb_StorePage操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StorePageController : ControllerBase
    {
        private readonly AppStorePage _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public StorePageController(AppStorePage app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuStorePage req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ResStorePage>> ListByWhere([FromQuery]ReqQuStorePage req)
        {
            Response<List<ResStorePage>> res = new Response<List<ResStorePage>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ResStorePage> Get(string id)
        {
            var result = new Response<ResStorePage>();
            ModelStorePage model = _app.Get(id);
            ResStorePage res = xConv.CopyMapper<ResStorePage, ModelStorePage>(model);
            res.PageDataObj = xConv.JsonToObj<dynamic>(model.PageData);
            result.Result = res;
            return result;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [HttpGet]
        public Response<ResStorePage> GetTypeDetail(int pageType)
        {
            var result = new Response<ResStorePage>();
            ResStorePage res = _app.GetTypeDetail(pageType);
            res.PageDataObj = xConv.JsonToObj<dynamic>(res.PageData);
            result.Result = res;
            return result;
        }

        /// <summary>
        /// 首页组件数据
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<dynamic> GetTempPageData(string id)
        {
            var result = new Response<dynamic>();
            result.Result = VmPage.GetDefaultPage();
            return result;
            // return "";
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuStorePage req)
        {
            var result = new Response();
            _app.AddOrUpdate(req);
            return result;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response SetHome([FromBody] ReqSetHomePage req)
        {
            var result = new Response();
            _app.SetHome(req);
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
