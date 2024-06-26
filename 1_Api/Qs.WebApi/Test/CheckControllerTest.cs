﻿// <copyright file="UserSessionControllerTest.cs" company="Qs.Mvc">
//     Copyright (c) http://www.Qs.net.cn. All rights reserved.
// </copyright>
// <summary>
// 测试加载用户菜单
// </summary>
// ***********************************************************************

using System;
using System.Diagnostics;
using Qs.Comm;
using Qs.Comm.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Qs.App;
using Qs.App.SSO;
using Qs.App.Test;
using Qs.Comm;
using Qs.Comm.Cache;
using Qs.WebApi.Controllers;

namespace Qs.WebApi.Test
{
    public class CheckControllerTest : TestBase
    {

        //如果要测试controller，则要重写
        public override ServiceCollection GetService()
        {
            var services = new ServiceCollection();

            var cachemock = new Mock<ICacheContext>();
            cachemock.Setup(x => x.Get<UserAuthSession>("tokentest")).Returns(new UserAuthSession{Account = "admin"});
            services.AddScoped(x => cachemock.Object);

            services.AddMvc().AddControllersAsServices();
            services.AddScoped<CheckController>();

            //读取配置文件
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            // Duplicate here any configuration sources you use.
            configurationBuilder.AddJsonFile("AppSettings.json");
            IConfiguration configuration = configurationBuilder.Build();
            services.Configure<AppSetting>(configuration.GetSection("AppSetting"));

            services.AddLogging();

            return services;
        }


        [Test]
        public void GetModulesTree()
        {
            Stopwatch watch = Stopwatch.StartNew();
            var controller = _autofacServiceProvider.GetService<CheckController>();

            var result =  controller.GetModulesTree();
            Console.WriteLine(JsonHelper.Instance.Serialize(result));
            watch.Stop();
            Console.WriteLine($"总耗时:{watch.ElapsedMilliseconds}");
        }
    }
}
