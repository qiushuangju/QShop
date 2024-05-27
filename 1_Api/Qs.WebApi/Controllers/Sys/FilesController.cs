
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qs.Comm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;

namespace Qs.WebApi.Controllers
{
    /// <summary>  文件上传</summary>
    /// <remarks> Qs, 2019-03-08. </remarks>

    [Route("api/[controller]/[action]")]
    [ApiController]                                   
    public class FilesController :ControllerBase
    {

        private AppFile _app;

        public FilesController(AppFile app)
        {
            _app = app;
        }
        
        /// <summary>
        /// 加载附件列表
        /// </summary>
        [HttpGet]
        public async Task<TableData> Load([FromQuery]QueryFileListReq request)
        {
            return await _app.Load(request);
        }
        
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
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
       
        /// <summary>
        ///  批量上传文件接口
        /// <para>客户端文本框需设置name='files'</para>
        /// </summary>
        /// <param name="files"></param>
        /// <returns>服务器存储的文件信息</returns>
        [HttpPost]
        [AllowAnonymous]
        // [EnableCors("any")]
        [DisableRequestSizeLimit]
        public Response<IList<ModelFileUpload>> Upload(IFormFileCollection files)
        {
            var result = new Response<IList<ModelFileUpload>>();
            try
            {
                result.Result = _app.Add(files);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
