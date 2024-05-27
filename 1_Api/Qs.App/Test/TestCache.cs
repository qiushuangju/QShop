using System;
using System.Collections.Generic;
using System.Linq;
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
using Qs.Repository.Domain;
using System.Reflection;
using System.ComponentModel;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Qs.App.Test
{
    public class TestCache :TestBase
    {
        public override ServiceCollection GetService()
        {
            var services = new ServiceCollection();

            var delservices = services.Where(u => u.ServiceType == typeof(ICacheContext)).ToList();
            for (int i = 0; i < delservices.Count(); i++)
            {
                services.Remove(delservices[i]);
            }
            services.AddScoped(typeof(ICacheContext), typeof(RedisCacheContext));

            return services;
        }


        /// <summary>
        /// 测试字符串
        /// </summary>
        [Test]
        public void SetString()
        {
            var app = _autofacServiceProvider.GetService<ICacheContext>();
            app.Set("Qs", "ok", DateTime.Now.AddDays(1));

            var result = app.Get<string>("Qs");
            Console.WriteLine($"redis结果:{result}");
        }
        
        /// <summary>
        /// 测试对象
        /// </summary>
        [Test]
        public void SetObj()
        {

           string xxx = GetDescription(typeof(DefineSetting), "Points");

           Dictionary<string, string> xxxxDescription = GetAttributes(typeof(DefineSetting));
            
             var app = _autofacServiceProvider.GetService<ICacheContext>();
            app.Set("user:info", new ModelUser
            {
                NickName = "测试",
                Account ="Test",
                // BizCode = "0.1.1"
            }, DateTime.Now.AddDays(1));

            var result = app.Get<ModelUser>("user:info");
            Console.WriteLine($"redis结果:{JsonHelper.Instance.Serialize(result)}");
        }

        public  Dictionary<string, string> GetAttributes(Type type)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            // Type type = typeof(T);//model.GetType()
            var t = Activator.CreateInstance(type);
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (properties.Length <= 0)
            {
                return dic;
            }
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsValueType || property.PropertyType.Name.StartsWith("String"))
                {
                    object[] objs = property.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);

                    if (objs.Length > 0 && ((DescriptionAttribute)objs[0]).Description != null)
                    {
                        string description = ((DescriptionAttribute)objs[0]).Description;
                        dic.Add(description, property.Name);
                    }
                }
            }
            return dic;
        }

        public string GetDescription(Type type,string propertyName)
        {
            string CnName = "";
          var sssx=  type.GetProperty("Points");
            foreach (var property in type.GetProperties())
            {
                

                var displayNameAttrs = property.GetCustomAttributes<DisplayNameAttribute>();
                foreach (var attr in displayNameAttrs)
                {
                    Console.WriteLine($"显示名称：{attr.DisplayName}");
                }
                if (displayNameAttrs != null && displayNameAttrs.Count() > 0)
                {
                    Console.WriteLine($"属性名称：{property.Name}");
                    if (property.Name.ToLower()==propertyName.ToLower())
                    {
                        CnName = property.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>().Description;
                    }
                   
                }
            }

            return CnName;
        }
        
        /// <summary>
        /// 测试获取不存在的key
        /// </summary>
        [Test]
        public void GetNoExistKey()
        {
            var app = _autofacServiceProvider.GetService<ICacheContext>();
           
            var result = app.Get<ModelUser>("noexistkey");
            Console.WriteLine($"redis结果:{JsonHelper.Instance.Serialize(result)}");
        }


    }
}
