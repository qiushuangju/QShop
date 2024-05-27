using System;
using Qs.Comm;
using Qs.Comm.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Qs.App.Request;
using Qs.App.SSO;
using Qs.App.Wx;

namespace Qs.App.Test
{
    class TestTemp : TestBase
    {
        public override ServiceCollection GetService()
        {
           
            var services = new ServiceCollection();

            var cachemock = new Mock<ICacheContext>();
            cachemock.Setup(x => x.Get<UserAuthSession>("tokentest")).Returns(new UserAuthSession { Account = Define.SystemUserName });
            services.AddScoped(x => cachemock.Object);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.Request.Query[Define.TokenName]).Returns("tokentest");

            services.AddScoped(x => httpContextAccessorMock.Object);

            return services;
        }

        /// <summary>
        /// </summary>
        [Test]
        public void TestValidation()
        {
            Random r = new Random();
            var imUserId = $"{(char)('a' + r.Next(0, 26))}{xConv.GenerateNo()}{xConv.GenerateRandomCode(4)}";
            TencentImApi api = new TencentImApi();
            // api.ImAccountImport(xConv.NewGuid());

            // var app = _autofacServiceProvider.GetService<AppLockersConfig>();
            // var appCode = _autofacServiceProvider.GetService<AppPhoneCode>();
            try
            {
                // appCode.SendPhoneCode("17773121044");
                // for (int i = 1; i < 501; i++)
                // {
                //     app.AddOrUpdate(new ReqAuLockers()
                //     {
                //         AmountMonth = 100,BoxNo = i,Level = 10,LevelExplain = "很标准",Status = 10 ,
                //         CreateTime = DateTime.Now
                //     });
                // }

                // for (int i = 1; i < 501; i++)
                // {
                //     app.AddOrUpdate(new ReqAuLockers()
                //     {
                //         AmountMonth = 100,
                //         BoxNo = i,
                //         Level = 20,
                //         LevelExplain = "很高级",
                //         Status = 10,
                //         CreateTime = DateTime.Now
                //     });
                // }
                string xx = "1";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
