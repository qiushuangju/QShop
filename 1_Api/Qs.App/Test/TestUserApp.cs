﻿using System;
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
using Qs.App.UserManager;

namespace Qs.App.Test
{
    class TestUserApp :TestBase
    {
        public override ServiceCollection GetService()
        {
            var services = new ServiceCollection();

            var cachemock = new Mock<ICacheContext>();
            cachemock.Setup(x => x.Get<UserAuthSession>("tokentest")).Returns(new UserAuthSession { Account = Define.SystemUserName});
            services.AddScoped(x => cachemock.Object);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.Request.Query[Define.TokenName]).Returns("tokentest");

            services.AddScoped(x => httpContextAccessorMock.Object);

            return services;
        }
        
        /// <summary>
        /// 测试添加用户时，数据校验。
        /// 因为请求数据没有Account，Name等，该测试会提示异常
        /// </summary>
        [Test]
        public void TestValidation()
        {
            var app = _autofacServiceProvider.GetService<AppUserManager>();
            try
            {
                // app.UserAddOrUpdate(new UpdateUserReq
                // {
                //     OrganizationIds = "08f41bf6-4388-4b1e-bd3e-2ff538b44b1b",
                //    
                // },"");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 测试添加用户时，数据校验。
        /// 因为请求数据没有Account，Name等，该测试会提示异常
        /// </summary>
        [Test]
        public void TestGetUserSource()
        {
            var app = _autofacServiceProvider.GetService<AppUserManager>();
            try
            {
                // app.GetUserSource("4f5774864f1945f6836cac598555756b","",xEnum.UserType.Vip);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        

        [Test]
        public void TestAdd()
        {
            var account = "user_" + DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss");
            Console.WriteLine(account);
            var app = _autofacServiceProvider.GetService<AppUserManager>();

            var newuser = new UpdateUserReq
            {
                Account = account,
                Name = account,
                OrganizationIds = "08f41bf6-4388-4b1e-bd3e-2ff538b44b1b,543a9fcf-4770-4fd9-865f-030e562be238",
            };
            // app.AddOrUpdateWxApp(newuser, "");
            //
            // app.AddOrUpdateWxApp(new UpdateUserReq
            // {
            //     Id = newuser.Id,
            //     Password = "newpassword",
            //     Account = account,
            //     Name = "新名字",
            //     OrganizationIds = "08f41bf6-4388-4b1e-bd3e-2ff538b44b1b",
            // }, "");
        }
        
        [Test]
        public void TestLoad()
        {
            var app = _autofacServiceProvider.GetService<AppUserManager>();
            var result = app.Load(new QueryUserListReq()
            {
                Page = 1,
                Limit = 10,
                orgId = "08f41bf6-4388-4b1e-bd3e-2ff538b44b1b"
            });
            
            Console.WriteLine(JsonHelper.Instance.Serialize(result));
        }

    }
}
