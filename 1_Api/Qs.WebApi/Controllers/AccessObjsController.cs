
using System;
using Qs.Comm;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.Comm;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 分配资源/分配字段等
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "分配资源_AccessObjs")]
    public class AccessObjsController : ControllerBase
    {
        private readonly AppRevelanceManager _app;
        private readonly IAuth _authUtil;
        public AccessObjsController(IAuth authUtil, AppRevelanceManager app) 
        {
            _app = app;
            _authUtil = authUtil;
        }

        /// <summary>
        /// 添加关联
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response Assign(AssignReq request)
        {
            var result = new Response();
            try
            {
                _app.Assign(request);
            }
            catch (Exception ex)
            {
                  result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }
        /// <summary>
        /// 取消关联
        /// </summary>
        [HttpPost]
        public Response UnAssign(AssignReq request)
        {
            var result = new Response();
            try
            {
                _app.UnAssign(request);
            }
            catch (Exception ex)
            {
                  result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }
        
        /// <summary>
        /// 角色分配数据字段权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response AssignDataProperty(AssignDataReq request)
        {
            var result = new Response();
            try
            {
                _app.AssignData(request);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }
        /// <summary>
        /// 取消角色的数据字段权限
        /// <para>如果Properties为空，则把角色的某一个模块权限全部删除</para>
        /// <para>如果moduleId为空，直接把角色的所有授权删除</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string lockobj = "lock";
        [HttpPost]
        public Response UnAssignDataProperty(AssignDataReq request)
        {
            var result = new Response();
            try
            {
                lock (lockobj)
                {
                    _app.UnAssignData(request);
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }
        
        
        /// <summary>
        /// 角色分配用户，整体提交，会覆盖之前的配置
        /// </summary>
        [HttpPost]
        public Response AssignRoleUsers(AssignRoleUsers request)
        {
            var result = new Response();
            try
            {
                _app.AssignRoleUsers(request);
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