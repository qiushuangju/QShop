using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Qs.Comm;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Qs.WebApi.Code
{
    /// <summary>
    /// 在swagger界面加上http header
    /// </summary>
    public class GlobalHttpHeaderOperationFilter : IOperationFilter
    {
        private IOptions<AppSetting> _appConfiguration;

        public GlobalHttpHeaderOperationFilter(IOptions<AppSetting> appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            var actionAttrs = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var isAnony = actionAttrs.Any(a => a.GetType() == typeof(AllowAnonymousAttribute));

            //不是匿名，则添加默认的X-Token   和默认的AppKey
            if (!isAnony)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = Define.TokenName,  
                    In = ParameterLocation.Header,
                    Description = "当前登录用户登录token",
                    Required = false
                });
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "platform",
                    In = ParameterLocation.Header,
                    Description = "AppKey",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Default = new OpenApiString(Define.AppWebStore)
                    }
                });
               
            }
        }
    }
}
