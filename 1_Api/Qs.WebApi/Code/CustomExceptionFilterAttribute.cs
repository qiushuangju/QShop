using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Qs.Comm;
using Qs.Comm.Extensions;

namespace Qs.WebApi.Code
{
    /// <summary>
    /// 全局异常处理 
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            context.Result = BuildExceptionResult(context.Exception);
            base.OnException(context);
        }

        /// <summary>
        ///包装处理异常格式
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private JsonResult BuildExceptionResult(Exception ex)
        {
            Response<string> res=new Response<string>();
            if (ex is CustomException)
            {//自定义异常
                var houndException = (CustomException) ex;
                res.Code= houndException.ErrorCode;
                res.Message = houndException.Message;
            }
            else if (ex is ApplicationException) //应用程序业务级异常
            {
                res.Code = 501;
                res.Message = ex.Message;
            }
            else
            {
                res.Code = 500;
                res.Message = $"{ex.Message}";
            }
            return new JsonResult(res);
        }
    }
}