using System;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Qs.Comm;
using Qs.Comm.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Qs.App.Request;
using Qs.App.SSO;
using Qs.Comm;
using Qs.Comm.Cache;

namespace Qs.App.Test
{
    class TestFileApp :TestBase
    {
        public override ServiceCollection GetService()
        {
            var services = new ServiceCollection();

            var cachemock = new Mock<ICacheContext>();
            cachemock.Setup(x => x.Get<UserAuthSession>("tokentest")).Returns(new UserAuthSession { Account = Define.SystemUserName });
            services.AddScoped(x => cachemock.Object);


           var logMock = new Mock<ILogger<AppFile>>();
           services.AddScoped(x => logMock.Object);
           
            return services;
        }
        
        
        [Test]
        public void TestLoad()
        {
            var app = _autofacServiceProvider.GetService<AppFile>();
            var result = app.Load(new QueryFileListReq()
            {
                Page = 1,
                Limit = 10
            });
            
            Console.WriteLine(JsonHelper.Instance.Serialize(result.Result));
        }
        
    }
}
