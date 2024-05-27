using System.IO;
using System.Net.Http;
using System.Reflection;
using Qs.Comm;
using Qs.Comm.Cache;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;
using Qs.App.Request;
using Qs.App.SSO;
using Qs.Comm;
using Qs.Comm.Cache;
using Qs.Comm.Provider;

namespace Qs.App.Test
{
    [TestFixture]
    public class TestBuilder :TestBase
    {
        //测试流程需要模拟登录用户
        public override ServiceCollection GetService()
        {
            var services = new ServiceCollection();

            var cachemock = new Mock<ICacheContext>();
            cachemock.Setup(x => x.Get<UserAuthSession>("tokentest")).Returns(new UserAuthSession { Account = "test3" });
            services.AddScoped(x => cachemock.Object);

            //模拟服务端httpContext
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.Request.Query[Define.TokenName]).Returns("tokentest");
            services.AddScoped(x => httpContextAccessorMock.Object);
            
            //模拟httpclientfactory
            var mockHttpFac = new Mock<IHttpClientFactory>();
            services.AddScoped(x => mockHttpFac.Object);

            //模拟路径
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                .Replace("\\Qs.App\\bin\\Debug\\netcoreapp3.1","");
            var mockPathProvider = new Mock<IPathProvider>();
            mockPathProvider.Setup(x => x.MapPath("",false)).Returns(path);
            services.AddScoped(x => mockHttpFac.Object);
            
            var host = new Mock<IHostEnvironment>();
            host.Setup(x => x.ContentRootPath).Returns(Path.Combine(path, "Qs.WebApi"));
            services.AddScoped(x => host.Object);
            return services;
        }
    }
}
