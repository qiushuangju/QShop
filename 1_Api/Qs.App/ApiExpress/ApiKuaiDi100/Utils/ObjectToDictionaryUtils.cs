using System;
using System.Collections.Generic;
using System.Reflection;
namespace Utils
{
    public class ObjectToDictionaryUtils{
        public static Dictionary<string, string> ObjectToMap(object obj)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
    
            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型
    
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance); // 获取当前type公共属性
    
            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();
    
                if (m != null && m.IsPublic)
                {
                    // 进行判NULL处理
                    if (m.Invoke(obj, new object[] { }) != null)
                    {
                        map.Add(p.Name, m.Invoke(obj, new object[] { })
                                         .ToString()); // 向字典添加元素
                    }
                }
            }
            return map;
        }
    }
    
}