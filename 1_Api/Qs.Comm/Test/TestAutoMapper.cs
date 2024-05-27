using System;
using System.Collections.Generic;
using NUnit.Framework;
using Qs.Comm;

namespace Qs.Comm.Test
{
    class TestAutoMapper
    {
        [Test]
        public void TestConvert()
        {
           string xxx=   xConv.GetLocalIp();


            var my = new MyClass
            {
                Name = "yubao"
            };

            var dest = my.MapTo<DestClass>();
            Console.WriteLine(JsonHelper.Instance.Serialize(dest));
        }
        [Test]
        public void TestConvertList()
        {
            var users = new List<MyClass> {
                new MyClass {Name = "Qs1"}
                , new MyClass{Name = "Qs2"}

            };

            var dest = users.MapToList<MyClass, DestClass>();
            Console.WriteLine(JsonHelper.Instance.Serialize(dest));

            var dest2 = users.MapToList<DestClass>();
            Console.WriteLine(JsonHelper.Instance.Serialize(dest2));

        }
    }


    class MyClass
    {
        public string Name { get; set; }
        public string NickName { get; set; }
    }

    class DestClass
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
