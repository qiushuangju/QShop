using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    /// FileUpload操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly AppFileUpload _app;

        /// <summary>
        /// /构造函数
        /// </summary>
        public FileUploadController(AppFileUpload app)
        {
            _app = app;
        }

        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery] ReqQuFileUpload req)
        {
            return _app.Load(req);
        }

        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ModelFileUpload>> ListByWhere([FromQuery] ReqQuFileUpload req)
        {
            Response<List<ModelFileUpload>> res = new Response<List<ModelFileUpload>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelFileUpload> Get(string id)
        {
            var result = new Response<ModelFileUpload>();
            result.Result = _app.Get(id);
            return result;
        }

        /// <summary>
        ///  批量上传文件接口
        /// <para>客户端文本框需设置name='files'</para>
        /// </summary>
        /// <param name="files"></param>
        /// <returns>服务器存储的文件信息</returns>
        [HttpPost]
        //[AllowAnonymous]
        // [EnableCors("any")]
        [DisableRequestSizeLimit]
        public Response<IList<ModelFileUpload>> Upload(ReqAuFileUpload req)
        {
            var result = new Response<IList<ModelFileUpload>>();
            try
            {
                result.Result = _app.Add(req, xEnum.FileType.Annex);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }    
            return result;
        }

        /// <summary>
        /// 批量上传支付证书接口
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        //[DisableRequestSizeLimit]
        public Response<IList<ModelFileUpload>> UploadCart(IFormFileCollection files)
        {
            var result = new Response<IList<ModelFileUpload>>();
            try
            {
                result.Result = _app.Add(files,xEnum.FileType.PayCert);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 上传文件接口
        /// </summary>
        /// <para>客户端文本框需设置name='files'</para>
        /// <param name="model"></param>
        /// <returns>服务器存储的文件信息</returns>
        [HttpPost]
        // [AllowAnonymous]
        // [EnableCors("any")]
        [DisableRequestSizeLimit]
        public Response<IList<ModelFileUpload>> UploadImg([FromForm] ReqAuFileUpload req)
        {
            var result = new Response<IList<ModelFileUpload>>();
            try
            {
                result.Result = _app.Add(req,xEnum.FileType.Image);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 上传视频接口
        /// </summary>
        /// <para>客户端文本框需设置name='files'</para>
        /// <param name="req"></param>
        /// <returns>服务器存储的文件信息</returns>
        [HttpPost]
        // [AllowAnonymous]
        // [EnableCors("any")]
        [DisableRequestSizeLimit]
        public Response<IList<ModelFileUpload>> UploadVideo([FromForm] ReqAuFileUpload req)
        {
            var result = new Response<IList<ModelFileUpload>>();
            try
            {
                result.Result = _app.Add(req, xEnum.FileType.Video);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 移动到分组
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public Response MoveToGroup([FromBody] ReqMoveGroup req)
        {
            var result = new Response();
            _app.MoveToGroup(req);
            return result;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        [HttpPost]
        public Response Delete([FromBody] string[] ids)
        {
            var result = new Response();
            _app.Delete(ids);
            return result;
        }


    }
}
