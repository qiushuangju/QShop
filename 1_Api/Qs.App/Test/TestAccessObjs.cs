using System;
using Qs.Comm;
using Qs.Comm.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Qs.App.Request;
using Qs.App.SSO;
using Qs.App.UserManager;
using Qs.Comm;
using Qs.Comm.Cache;
using Qs.Repository.Domain;

namespace Qs.App.Test
{
    /// <summary>
    /// 测试分配权限
    /// </summary>
    class TestAccessObjs :TestBase
    {
        public override ServiceCollection GetService()
        {
            var services = new ServiceCollection();

            var cachemock = new Mock<ICacheContext>();
            cachemock.Setup(x => x.Get<UserAuthSession>("tokentest"))
                .Returns(new UserAuthSession { Account = Define.SystemUserName });
            services.AddScoped(x => cachemock.Object);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.Request.Query[Define.TokenName])
                .Returns("tokentest");

            services.AddScoped(x => httpContextAccessorMock.Object);

            return services;
        }
    }
}
