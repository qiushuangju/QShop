using System;
using System.IO;
using System.Linq;
using Autofac.Extensions.DependencyInjection;
using Qs.Comm;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Qs.Comm;
using Qs.Comm.Extensions.AutofacManager;
using Qs.Comm.Utilities;
using Qs.Repository;

namespace Qs.App.Test
{
    public class TestBase
    {
        protected AutofacServiceProvider _autofacServiceProvider;

        [SetUp]
        public void Init()
        {
            var serviceCollection = GetService();
            serviceCollection.AddMemoryCache();
            serviceCollection.AddOptions();
            //读取Qs.WebApi的配置文件用于单元测试
            var path = AppContext.BaseDirectory;
            int pos = path.LastIndexOf("Qs.");
            var basepath = Path.Combine(path.Substring(0,pos) ,"Qs.WebApi");
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(basepath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
           
            serviceCollection.Configure<AppSetting>(config.GetSection("AppSetting"));
            // //添加log4net
            // serviceCollection.AddLogging(builder =>
            // {
            //     builder.ClearProviders(); //去掉默认的日志
            //     builder.AddConfiguration(config.GetSection("Logging"));  //读取配置文件中的Logging配置
            //     builder.AddLog4Net();
            // });
            //注入Qs.WebApi配置文件
            serviceCollection.AddScoped(x => config);

            //模拟HTTP请求
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.Request.Query[Define.TokenName]).Returns("tokentest");
            httpContextAccessorMock.Setup(x => x.HttpContext.Request.Query[Define.TenantId]).Returns("QsDBContext");
            serviceCollection.AddScoped(x => httpContextAccessorMock.Object);
            
            serviceCollection.AddDbContext<QsDBContext>();
            
            var container = AutofacExt.InitForTest(serviceCollection);
            _autofacServiceProvider = new AutofacServiceProvider(container);
            AutofacContainerModule.ConfigServiceProvider(_autofacServiceProvider);
            
            var dbtypes = config.GetSection("AppSetting:DbTypes").GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);
            
            Console.WriteLine($"单元测试数据库信息:{dbtypes[httpContextAccessorMock.Object.GetTenantId()]}/{config.GetSection("ConnectionStrings")["QsDBContext"]}");

        }

        /// <summary>
        /// 测试框架默认只注入了缓存Cache，配置Option；
        /// 如果在测试的过程中需要模拟登录用户，cookie等信息，需要重写该方法，可以参考TestFlow的写法
        /// </summary>
        public virtual ServiceCollection GetService()
        {
            return  new ServiceCollection();
        }
    }
}
