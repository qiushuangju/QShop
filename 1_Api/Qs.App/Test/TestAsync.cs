using System;
using System.Threading;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Comm.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Qs.App.Request;
using Qs.App.SSO;
using Qs.Comm;
using Qs.Comm.Cache;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.App.Test
{
    public class TestDynamic : TestBase
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

        [Test]
        public void Async()
        {
            Console.WriteLine($"开始异步测试");
            
            AddOrUpdate();

            Console.WriteLine("异步测试结束");

            //延长主线程，防止程序退出
            Thread.Sleep(3000);
        }

        private async Task AddOrUpdate()
        {
            var repository = _autofacServiceProvider.GetService<IRepository<ModelUser, QsDBContext>>();

            var account = "user_" + DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss");

            var user = new ModelUser
            {
                Account = account,
                NickName = account
            };
            await repository.AddAsync(user);

            user.Account = "new_" + user.Account;

            await repository.UpdateAsync(user);
            Console.WriteLine($"更新完成");
        }
    }
}
