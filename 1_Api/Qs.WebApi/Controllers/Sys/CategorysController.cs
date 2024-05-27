using System;
using System.Threading.Tasks;
using Qs.Comm;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using System.Collections.Generic;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 分类（字典）管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "分类字典_Categorys")]
    public class CategorysController : ControllerBase
    {
        private readonly AppCategory _app;
        
        /// <summary>
        /// 获取分类详情
        /// </summary>
        /// <param name="id">分类id</param>
        /// <returns></returns>
        [HttpGet]
        public Response<Category> Get(string id)
        {
            var result = new Response<Category>();
            try
            {
                result.Result = _app.Get(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <returns></returns>
       [HttpPost]
        public Response Add(AddOrUpdateCategoryReq obj)
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

        /// <summary>
        /// 修改分类（字典）
        /// </summary>
        /// <returns></returns>
       [HttpPost]
        public Response Update(AddOrUpdateCategoryReq obj)
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
        public async Task<TableData> Load([FromQuery]QueryCategoryListReq request)
        {
            return await _app.Load(request);
        }

        /// <summary>
        /// 加载分类下所有的值
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<List<Category>> LoadByTypeId(string typeId)
        {
            var result = new Response<List<Category>>();
            result.Result = _app.LoadByTypeId(typeId);
            return result;
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

        public CategorysController(AppCategory app) 
        {
            _app = app;
        }
    }
}
