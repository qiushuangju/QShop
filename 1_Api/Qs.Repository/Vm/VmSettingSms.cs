using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 短信通知设置
    /// </summary>
    public class VmSettingSms
    {
        /// <summary>
        /// 默认短信渠道
        /// </summary>
        public string Default { get; set; } = "aliyun";

        /// <summary>
        /// 短信渠道/平台
        /// </summary>
        public EngineInfo Engine { get; set; } 

        /// <summary>
        /// 通知模板
        /// </summary>
        public SceneInfo Scene { get; set; } 
            
        //     = SceneInfo()
        // {
        //     {
        //         "captcha", new TemplateSms()
        //         {
        //             Name = "短信验证码(通知用户)",
        //             IsEnable = true,
        //             TemplateCode = "",
        //             Content = "",
        //             Variables = new TemplateVariables()
        //             {
        //                 Aliyun = new List<string>() {"${code}"},
        //                 Qcloud = new List<string>() {"${code}"},
        //                 Qiniu = new List<string>() {"${1}"}
        //             }
        //
        //         }
        //     },
        //     {
        //         "order_pay", new TemplateSmsPayOrder()
        //         {
        //             Name = "新付款订单(通知商家)",
        //             IsEnable = true,
        //             TemplateCode = "",
        //             AcceptPhone = "",
        //             Content = "您有一条新订单，订单号为：%s，请注意查看",
        //             Variables = new TemplateVariables()
        //             {
        //                 Aliyun = new List<string>() {"${order_no}"},
        //                 Qcloud = new List<string>() {"${order_no}"},
        //                 Qiniu = new List<string>() {"${1}"}
        //             }
        //         }
        //     }
        // };
    }

    public class EngineInfo
    {
        /// <summary>
        /// 阿里
        /// </summary>
        public AliSdkInfo Aliyun { get; set; }

        /// <summary>
        /// 腾讯
        /// </summary>
        public TengXunSdkInfo Qcloud { get; set; }
        /// <summary>
        /// 七牛
        /// </summary>
        public QiNiu Qiniu { get; set; }
    }

    /// <summary>
    /// 短信信息
    /// </summary>
    public class SceneInfo
    {
        /// <summary>
        /// 短信验证码
        /// </summary>
        public TemplateSms captcha { get; set; }

        /// <summary>
        /// 订单通知
        /// </summary>
        public TemplateSmsPayOrder order_pay { get; set; }

    }
    /// <summary>
    /// 阿里云Sdk信息
    /// </summary>
    public class AliSdkInfo
    {
        /// <summary>
        /// 短信平台
        /// </summary>
        public string Name { get; set; } 

        /// <summary>
        /// 渠道网址
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// AccessKeyId
        /// </summary>
        public string AccessKeyId { get; set; }
        /// <summary>
        /// AccessKeySecret
        /// </summary>
        public string AccessKeySecret { get; set; }

        /// <summary>
        ///短信签名 Sign 
        /// </summary>
        public string Sign { get; set; }

    }

    /// <summary>
    /// 腾讯云Sdk信息
    /// </summary>
    public class TengXunSdkInfo
    {
        /// <summary>
        /// 短信平台
        /// </summary>
        public string Name { get; set; } 

        /// <summary>
        /// 渠道网址
        /// </summary>
        public string Website { get; set; } 
        /// <summary>
        /// SdkAppID
        /// </summary>
        public string SdkAppID { get; set; }
        /// <summary>
        /// AccessKeyId
        /// </summary>
        public string AccessKeyId { get; set; }
        /// <summary>
        /// AccessKeySecret
        /// </summary>
        public string AccessKeySecret { get; set; }
        /// <summary>
        ///短信签名 Sign 
        /// </summary>
        public string Sign { get; set; } 
    }
    /// <summary>
    /// 七牛Sdk信息
    /// </summary>
    public class QiNiu
    {
        /// <summary>
        /// 短信平台
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 渠道网址
        /// </summary>
        public string Website { get; set; } 
        /// <summary>
        /// AccessKey
        /// </summary>
        public string AccessKey { get; set; }
        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        ///短信签名 Sign 
        /// </summary>
        public string Sign { get; set; }
    }
    /// <summary>
    /// 模板信息
    /// </summary>
    public class TemplateSms
    { 
        /// <summary>
        /// 模版名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 模版名称
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 模版名称
        /// </summary>
        public string TemplateCode { get; set; }
       
        /// <summary>
        /// 模版名称
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 模版变量
        /// </summary>
        public TemplateVariables Variables { get; set; }
    }

    /// <summary>
    /// 模板信息
    /// </summary>
    public class TemplateSmsPayOrder : TemplateSms
    {
        
        /// <summary>
        /// 接收电话
        /// </summary>
        public string AcceptPhone { get; set; }
    }

    /// <summary>
    /// 模版变量
    /// </summary>
    public class TemplateVariables
    {
        /// <summary>
        /// 阿里云变量
        /// </summary>
        public List<string> Aliyun { get; set; }
        /// <summary>
        /// 腾讯云变量
        /// </summary>
        public List<string> Qcloud { get; set; }
        /// <summary>
        /// 七牛变量
        /// </summary>
        public List<string> Qiniu { get; set; }
        
    }

}

