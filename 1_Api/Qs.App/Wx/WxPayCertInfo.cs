using System.Collections.Generic;
using System.Linq;
using Qs.Comm;

namespace Qs.App.Wx
{

    /// <summary>
    /// 支付证书
    /// </summary>
    public class WxPayCertInfo
    {
        /// <summary>
        /// 证书路径,注意应该填写绝对路径 "cert/WeiBangRefund_Cert.p12";（仅退款、撤销订单时需要）
        /// </summary>
        public string SslCertPath { get; set; }
        /// <summary>
        /// 证书密码 , ListWxAppConfig.List.FirstOrDefault().PartnerId
        /// </summary>
        public string SslCertPwd { get; set; }
    }
}
