using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm.Extensions;
using Qs.Comm.Model;

namespace Qs.Comm
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class xLog
    {
        /// <summary>
        ///   在网站根目录下创建日志目录
        /// </summary>
        public static string Path = AppContext.BaseDirectory + "log";

        /// <summary>
        ///   实际的写日志操作
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="logLevel">日志等级 (默认文件前缀为Info Err级别前缀为Err)</param>
        /// <param name="Prefix">日志文件特殊前缀</param>
        public static void Add(ModelLog obj, xEnum.LogLevel logLevel = xEnum.LogLevel.Info,string Prefix="")
        {
            if (DefineAppSetting.LogLevel > (int)logLevel)// 配置日志等级大于当前日志级别则不记录
            {
                return;
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); //获取当前系统时间

            if (!Directory.Exists(Path)) //如果日志目录不存在就创建
            {
                Directory.CreateDirectory(Path);
            }
            string filePath = $@"{Path}\Info{DateTime.Now:yyyyMMdd}.txt"; //用日期对日志文件命名
            if ((int)logLevel >= (int)xEnum.LogLevel.Error)
            {
                filePath = $@"{Path}\Err{DateTime.Now:yyyyMMdd}.txt"; //用日期对日志文件命名 
            }

            if (!string.IsNullOrEmpty(Prefix))
            {
                filePath = $@"{Path}\{Prefix}{DateTime.Now:yyyyMMdd}.txt"; //用日期对日志文件命名  
            }


            var createUser = string.IsNullOrEmpty(obj.CreateName) ? " " : $" 创建人:{obj.CreateName} ";
            var title = string.IsNullOrEmpty(obj.Title) ? "" : $"标题:{obj.Title} ";
            var typeName = string.IsNullOrEmpty(obj.TypeName) ? "" : $"类型:{obj.TypeName} ";
            var href = string.IsNullOrEmpty(obj.Href) ? "" : $"路径:{obj.Href} ";

            //向日志文件写入内容
            string writeContent =
                $"{time} {title}{typeName}{createUser}{href} 出参:{obj.ApiOutContent} 入参:{obj.ApiInContent}";

            FileStream fStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

            using (StreamWriter writer = new StreamWriter(fStream))
            {
                writer.WriteLine(writeContent);
                writer.Flush();
                writer.Close();
                fStream.Close();
            }
        }


    }
}
