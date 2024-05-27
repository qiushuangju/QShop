﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.WebApi.Controllers.Sys
{
    /// <summary>
    /// 模块及按钮管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "模块按钮_Modules")]
    public class ModulesController : ControllerBase
    {
        private AppModule _app;
        private readonly IAuth _authUtil;
        /// <summary>
        /// 模块管理
        /// </summary>
        /// <param name="authUtil"></param>
        /// <param name="app"></param>
        public ModulesController(IAuth authUtil, AppModule app)
        {
            _app = app;
            _authUtil = authUtil;
        }


        /// <summary>
        /// 加载角色模块
        /// </summary>
        /// <param name="firstId">The role identifier.</param>
        /// <returns>System.String.</returns>
        [HttpGet]
        public Response<IEnumerable<Module>> LoadForRole(string firstId)
        {
            var result = new Response<IEnumerable<Module>>();
            try
            {
                result.Result = _app.LoadForRole(firstId);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 根据某角色ID获取可访问某模块的按钮箱
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<IEnumerable<ModuleElement>> LoadBtnsForRole(string moduleId, string firstId)
        {
            var result = new Response<IEnumerable<ModuleElement>>();
            try
            {
                result.Result = _app.LoadBtnsForRole(moduleId, firstId);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;

        }

        /// <summary>
        /// 获取发起页面的按钮权限
        /// </summary>
        /// <returns>System.String.</returns>
        [HttpGet]
        public Response<List<ModuleElement>> LoadBtns(string moduleId)
        {
            var result = new Response<List<ModuleElement>>();
            try
            {
                var user = _authUtil.GetCurrentContext();
                if (string.IsNullOrEmpty(moduleId))
                {
                    result.Result = user.ModuleElements;
                }
                else
                {
                    var module = user.Modules.First(u => u.Id == moduleId);
                    if (module == null)
                    {
                        throw new Exception("模块不存在");
                    }
                    result.Result = module.Elements;
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }


        #region 添加编辑模块

        //添加或修改
        [HttpPost]
        public Response<Module> Add(Module obj)
        {
            var result = new Response<Module>();
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

        //添加或修改
        [HttpPost]
        public Response Update(Module obj)
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


        [HttpPost]
        public Response Delete([FromBody] string[] ids)
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

        #endregion 添加编辑模块

        /// <summary>
        /// 新增按钮
        /// <para>当前登录用户的所有角色会自动分配按钮</para>
        /// </summary>
        [HttpPost]
        public Response<ModuleElement> AddBtn(ModuleElement obj)
        {
            var result = new Response<ModuleElement>();
            try
            {
                _app.AddBtn(obj);
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
        /// 修改按钮属性
        /// </summary>
        [HttpPost]
        public Response UpdateBtn(ModuleElement obj)
        {
            var result = new Response();
            try
            {
                _app.UpdateBtn(obj);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 删除按钮
        /// </summary>
        [HttpPost]
        public Response DeleteBtn([FromBody] string[] ids)
        {
            var result = new Response();
            try
            {
                _app.DelBtn(ids);
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