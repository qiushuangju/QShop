﻿using System;
 using System.Threading.Tasks;
 using Qs.Comm;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Request;
 using Qs.Repository.Base;

 namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 字典所属分类管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "字典所属分类_CategoryTypes")]
    public class CategoryTypesController : ControllerBase
    {
        private readonly AppCategoryType _app;
        public CategoryTypesController(AppCategoryType app)
        {
            _app = app;
        }
        //添加
        [HttpPost]
        public Response Add(AddOrUpdateCategoryTypeReq obj)
        {
            var result = new Response();
            try
            {
                _app.Add(obj);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        //修改
       [HttpPost]
        public Response Update(AddOrUpdateCategoryTypeReq obj)
        {
            var result = new Response();
            try
            {
                _app.Update(obj);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        [HttpGet]
        public async Task<TableData> Load([FromQuery]QueryCategoryTypeListReq request)
        {
            return await _app.Load(request);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
       [HttpPost]
        public Response Delete([FromBody]string[] ids)
        {
            var result = new Response();
            try
            {
                _app.Delete(ids);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }
    }
}
