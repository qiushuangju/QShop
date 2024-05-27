using System;
using System.Threading;
using Qs.Comm;
using Qs.Comm.Cache;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Qs.App.Request;
using Qs.App.SSO;
using Qs.Comm;
using Qs.Comm.Cache;

namespace Qs.App.Test
{
    [TestFixture]
    public class TestJob :TestBase
    {
        public override ServiceCollection GetService()
        {
            var services = new ServiceCollection();

            var cachemock = new Mock<ICacheContext>();
            cachemock.Setup(x => x.Get<UserAuthSession>("tokentest")).Returns(new UserAuthSession { Account = "admin" });
            services.AddScoped(x => cachemock.Object);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.Request.Query[Define.TokenName]).Returns("tokentest");

            services.AddScoped(x => httpContextAccessorMock.Object);

            return services;
        }


        [Test]
        public void GetSysJobs()
        {
            var app = _autofacServiceProvider.GetService<AppOpenJob>();
            var result = app.QueryLocalHandlers();
            Console.WriteLine(JsonHelper.Instance.Serialize(result));
        }

        [Test]
        public void ChangeStatus()
        {
            var app = _autofacServiceProvider.GetService<AppOpenJob>();
            app.ChangeJobStatus(new ChangeJobStatusReq
            {
                Id = "f40fe48d-71a4-4f47-b324-6178d97abfb9",
                Status = 1
            });
            Thread.Sleep(60000);
        }
    }
}
