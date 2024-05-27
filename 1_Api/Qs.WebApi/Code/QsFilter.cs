using System;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Comm.Model;

namespace Qs.WebApi.Code
{
    /// <summary>
    ///   自定义过滤器
    /// </summary>
    public class QsFilter : IActionFilter
    {
        private readonly IAuth _auth;
        private string href = "";
        private string reqData = "";
        private string resData = "";
        private string token = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authUtil"></param>
        /// <param name="logApp"></param>
        public QsFilter(IAuth authUtil)
        {
            _auth = authUtil;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var description =
                (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;

            var controllerName = description.ControllerName.ToLower();
            var actionName = description.ActionName.ToLower();

            token = context.HttpContext.Request.Headers[Define.TokenName];
            href = $"{context.HttpContext.Request.Method.ToLower()}:{controllerName}/{actionName}";
            //if (!href.Contains("/upload"))  //排除上传
            //{
            //    reqData = xConv.ToJson(context.ActionArguments);
            //}



            //匿名标识
            var authorize = description.MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute));
            if (authorize != null)
            {
                return;
            }
            var currnetContext = _auth.GetCurrentContext();
            if (currnetContext == null || currnetContext.User == null)
            {
                context.Result = new JsonResult(new Response
                {
                    Code = DefineErrCode.InvalidToken,
                    Message = "认证失败,Token无效或已超时!"
                });
                return;
            }
            if (currnetContext.User.Status == (int)xEnum.ComStatus.Disable && !actionName.Contains("bindwxphone"))
            {
                context.Result = new JsonResult(new Response
                {
                    Code = DefineErrCode.InvalidUserStatus,
                    Message = "内部程序,您需等待管理员审核!"
                });
                return;
            }
        }

        /// <summary>
        /// 执行完
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

            var logLevel = xEnum.LogLevel.Info;
            if (context.Exception == null)
            {
                //执行成功 取得由 API 返回的资料
                var result1 = context.Result as ObjectResult;
                if (result1 != null)
                {
                    #region  取传入值
                    //var description =
                    //    (ControllerActionDescriptor)context.ActionDescriptor;
                    //var controllername = description.ControllerName.ToLower();
                    //var actionname = description.ActionName.ToLower();

                    //var request = context?.HttpContext?.Request;
                    //// var result = context.Result as ObjectResult;
                    //var reqData = "";
                    //if (!request.Path.Value.ToLower().Contains("/files/upload"))
                    //{
                    //    if (request.Method.ToLower().Equals("get"))
                    //    {
                    //        reqData = request.QueryString.Value;
                    //    }

                    //    if (request.Method.ToLower().Equals("post")) //todo OnActionExecuted Post取不到值
                    //    {
                    //        request.EnableBuffering();

                    //        request.Body.Position = 0;
                    //        StreamReader stream = new StreamReader(request.Body);
                    //        reqData = stream.ReadToEndAsync().Result;
                    //    }
                    //}
                    #endregion

                    var result = context.Result as ObjectResult;
                    resData = xConv.ToJson(result.Value); 
                }
            }
            else
            {
                logLevel = xEnum.LogLevel.Error;
                Exception ex = context.Exception;
                resData = $" Err:{ex.Message} {ex.StackTrace}";
            }

            xLog.Add(new ModelLog()
            {
                Title = "用户访问",
                Href = href,
                CreateName = _auth.GetUserName(),
                TypeName = "访问日志",
                ApiInContent = $"token:{token},{reqData}",
                ApiOutContent = resData
            }, logLevel);

            return;

        }
    }
}
