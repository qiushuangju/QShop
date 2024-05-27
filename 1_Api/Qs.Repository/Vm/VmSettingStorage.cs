using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 上传设置
    /// </summary>
    public class VmSettingStorage
    {
        /// <summary>
        /// 默认短信渠道
        /// </summary>
        public string Default { get; set; } = "local";

        /// <summary>
        /// 短信渠道/平台
        /// </summary>
        public EngineStorageInfo Engine { get; set; } 

     
    }

    public class EngineStorageInfo
    {
        /// <summary>
        /// 阿里
        /// </summary>
        public AliSdkStorageInfo Aliyun { get; set; }

        /// <summary>
        /// 腾讯
        /// </summary>
        public TengXunSdkStorageInfo Qcloud { get; set; }
        /// <summary>
        /// 七牛
        /// </summary>
        public QiNiuStorageInfo Qiniu { get; set; }
    }

  
    /// <summary>
    /// 阿里云Sdk信息
    /// </summary>
    public class AliSdkStorageInfo  : BaseStorageInfo
    {
        /// <summary>
        /// AccessKeyId
        /// </summary>
        public string access_key_id { get; set; }

        /// <summary>
        /// AccessKeySecret
        /// </summary>
        public string access_key_secret { get; set; }

    }

    /// <summary>
    /// 腾讯云Sdk信息
    /// </summary>
    public class TengXunSdkStorageInfo   : BaseStorageInfo
    {
        /// <summary>
        /// 所属地域 Region
        /// </summary>
        public string region { get; set; }
        /// <summary>
        /// SecretId
        /// </summary>
        public string secret_id { get; set; }
        /// <summary>
        /// SecretKey
        /// </summary>
        public string secret_key { get; set; }
    }
    /// <summary>
    /// 七牛Sdk信息
    /// </summary>
    public class QiNiuStorageInfo : BaseStorageInfo
    {
        /// <summary>
        /// ACCESS_KEY AK
        /// </summary>
        public string access_key { get; set; }
        /// <summary>
        /// SECRET_KEY SK
        /// </summary>
        public string secret_key { get; set; } 
    }

    public class BaseStorageInfo
    {
        /// <summary>
        /// 存储空间名称
        /// </summary>
        public string Bucket { get; set; }
        /// <summary>
        /// 空间域名 Domain
        /// </summary>
        public string Domain { get; set; }

    }

}

