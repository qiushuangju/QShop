using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Qs.Comm;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Response;
using Qs.Repository.Request;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 用户消息及系统消息
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    // [ApiExplorerSettings(GroupName = "消息中心_SysMessages")]
    public class SysMessagesController : ControllerBase
    {
        private readonly AppSysMessage _app;
        public SysMessagesController(AppSysMessage app)
        {
            _app = app;
        }
        /// <summary>
        /// 加载列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public TableData Load([FromQuery] QuerySysMessageListReq request)
        {
            return _app.Load(request);
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        [HttpGet]
        public Response<List<ResMsgSubType>> ListMessageType()
        {
            Response<List<ResMsgSubType>> res = new Response<List<ResMsgSubType>>();
            res.Result =_app.ListMessageType();
            return res;
        }

        /// <summary>
        /// 获取消息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Response<ModelSysMessage> Get(string id)
        {
            var result = new Response<ModelSysMessage>();
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
        /// 阅读消息（即消息置为已读）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
       [HttpPost]
        public Response Read(ReadMsgReq obj)
        {
            var result = new Response();
            try
            {
                _app.Read(obj);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 新增或修改公告
        /// </summary>
        [HttpPost]
        public Response AddOrUpdateNotice([FromBody] ReqAuSysMessage req)
        {
            var result = new Response();
            _app.AddOrUpdateNotice(req);
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
                _app.Del(ids);

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
