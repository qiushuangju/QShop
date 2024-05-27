using System;
using Autofac.Extensions.DependencyInjection;
using Qs.Comm;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Qs.Comm.Helpers;

namespace Qs.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($@"Start Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders(); //去掉默认的日志
                    // logging.AddLog4Net();
                })
                .UseServiceProviderFactory(
                    new AutofacServiceProviderFactory()) //将默认ServiceProviderFactory指定为AutofacServiceProviderFactory
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // var configuration = ConfigHelper.GetConfigRoot();
                    // var httpHost = configuration["AppSetting:HttpHost"];
                    //
                    // webBuilder.UseKestrel(options =>
                    // {
                    //     options.Limits.MaxRequestBodySize = int.MaxValue;
                    // }).UseUrls(httpHost).UseStartup<Startup>();
                    
                    // Console.WriteLine($"启动成功，接口访问地址:{httpHost}/swagger/index.html");

                    var configuration = ConfigHelper.GetConfigRoot();
                    var httpHost = configuration["AppSetting:HttpHost"];
                    

                    webBuilder.ConfigureKestrel((context, options) =>
                    {                                   
                        options.Limits.MaxRequestBodySize = 1024 * 1012 * 800;
                    }).UseUrls(httpHost).UseStartup<Startup>();
                    Console.WriteLine($"启动成功，接口访问地址:{httpHost}/swagger/index.html");
                });

      
    }
}