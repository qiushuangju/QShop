using System;
using System.Linq;
using Qs.Comm;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.Repository.Test
{
    /// <summary>
    /// 测试UnitWork
    /// </summary>
    class TestUnitWork : TestBase
    {
        /// <summary>
        /// 测试存储过程
        /// </summary>
        [Test]
        public void ExecProcedure()                                                                           
        {
            var unitWork = _autofacServiceProvider.GetService<IUnitWork<QsDBContext>>();
            var users = unitWork.ExecProcedure<ModelUser>("sp_alluser");
            Console.WriteLine(JsonHelper.Instance.Serialize(users));
        }
        
    }
}