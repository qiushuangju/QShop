using Microsoft.AspNetCore.Http;
using Qs.Comm.Extensions.AutofacManager;

namespace Qs.Comm.Utilities
{
    public static class HttpContextUtil
    {
        private static IHttpContextAccessor _accessor=AutofacContainerModule.GetService<IHttpContextAccessor>();

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;
        
        /// <summary>
        /// 获取租户ID
        /// </summary>
        /// <returns></returns>
        public static string GetTenantId(this IHttpContextAccessor accessor)
        {
            string tenantId = "QsDBContext";
            
            if (accessor != null && accessor.HttpContext != null)
            {
                //读取多租户ID
                var httpTenantId = accessor.HttpContext.Request.Query[Define.TenantId];
                if (string.IsNullOrEmpty(httpTenantId))
                {
                    httpTenantId = accessor.HttpContext.Request.Headers[Define.TenantId];
                }
                
                //如果没有租户id，或租户用的是默认的QsDBContext,则不做任何调整
                if (!string.IsNullOrEmpty(httpTenantId))
                {
                    tenantId = httpTenantId;
                }
            }

            return tenantId;
        }
        
    }
}
