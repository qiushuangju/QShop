using System.Collections.Generic;

namespace Qs.Comm
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class AppSetting
    {

        public AppSetting()
        {              
            Version = "";
            UploadPath = "";
        }
        /// <summary>
        /// 版本信息
        /// 如果为demo,则屏蔽Post请求
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 数据库类型 SqlServer、MySql
        /// </summary>
        public Dictionary<string, string> DbTypes { get; set; }

        /// <summary>
        /// 附件上传路径
        /// </summary>
        public string UploadPath { get; set; }
        /// <summary>
        /// 支付证书上传陆军
        /// </summary>
        public string UploadPathCart { get; set; }              
        /// <summary>
        /// Redis服务器配置
        /// </summary>
        public string RedisConf { get; set; }

       
    }
}
