using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qs.Comm;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 表单操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppRole _app;
        private readonly AppRevelanceManager _revelanceManagerApp;
        public RolesController(AppRevelanceManager revelanceManagerApp, AppRole app)
        {
            _revelanceManagerApp = revelanceManagerApp;
            _app = app;
        }

        [HttpGet]
        public Response<Role> Get(string id)
        {
            var result = new Response<Role>();
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
        /// 添加角色，如果当前登录用户不是System，则直接把新角色分配给当前登录用户
        /// </summary>
        [HttpPost]
        public Response<RoleView> Add(RoleView obj)
        {
            var result = new Response<RoleView>();
            try
            {
                _app.Add(obj);
                result.Result = obj;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 更新角色属性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response Update(RoleView obj)
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
        /// 获取所有的角色
        /// 为了控制权限，通常只用于流程实例选择执行角色，其他地方请使用Load
        /// </summary>
        [HttpGet]
        public async Task<TableResp<Role>> LoadAll([FromQuery] QueryRoleListReq request)
        {
            return await _app.LoadAll(request);
        }

        /// <summary>
        /// 获取登录用户可以访问的角色
        /// </summary>
        [HttpGet]
        public Response<List<Role>> Load([FromQuery] QueryRoleListReq request)
        {
            var result = new Response<List<Role>>();
            try
            {
                result.Result = _app.Load(request);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 加载用户的角色
        /// </summary>
        [HttpGet]
        public Response<List<string>> LoadForUser(string userId)
        {
            var result = new Response<List<string>>();
            try
            {
                result.Result = _revelanceManagerApp.Get(Define.UserRole, true, userId);
            }
            catch (Exception e)
            {
                result.Code = 500;
                result.Message = e.InnerException?.Message ?? e.Message;
            }

            return result;
        }

        [HttpPost]
        [AllowAnonymous]
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