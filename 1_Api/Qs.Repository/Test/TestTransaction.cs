using System;
using System.Linq;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.Repository.Test
{
    /// <summary>
    /// 测试事务
    /// </summary>
    class TestTransaction : TestBase
    {
        /// <summary>
        /// 测试事务正常提交
        /// </summary>
        [Test]
        public void NormalSubmit()
        {
            var unitWork = _autofacServiceProvider.GetService<IUnitWork<QsDBContext>>();
            unitWork.ExecuteWithTransaction(() =>
            {
                var account = "user_" + DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss");

                // AddAndUpdate(account, unitWork);
            });

        }

        /// <summary>
        /// 测试事务回滚
        /// </summary>
        [Test]
        public void SubmitWithRollback()
        {
            var unitWork = _autofacServiceProvider.GetService<IUnitWork<QsDBContext>>();
            var account = "user_" + DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss");
            try
            {
                unitWork.ExecuteWithTransaction(() =>
                {
                    // AddAndUpdate(account, unitWork);

                    throw new Exception("模拟异常");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            //如果没有插入成功，表示事务发生了回滚
            Assert.IsFalse(unitWork.Any<ModelUser>( u=>u.Id == account));

        }

   


        [Test]
        public void MultiUpdate()
        {
            var unitWork = _autofacServiceProvider.GetService<IUnitWork<QsDBContext>>();

            // var users = unitWork.Find<User>(u => u.Account.Contains("test"));
            // foreach (var user in users)
            // {
            //     user.Name  = "user_" + DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss");
            //     unitWork.Update(user);
            // }
            
            unitWork.Save();
            
        }

        [Test]
        public void MultiUpdate2()
        {
            var unitWork = _autofacServiceProvider.GetService<IUnitWork<QsDBContext>>();
            var users = unitWork.Find<ModelUser>(null).ToList();
            unitWork.ExecuteWithTransaction(()=>
            {
                foreach (var req in users)
                {
                    unitWork.Update<ModelUser>(u =>u.Id == req.Id, user => new ModelUser
                    {
                        NickName = "user_" + DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss")
                    });
                    

                }

                unitWork.Save();
            });

        }
    }
}