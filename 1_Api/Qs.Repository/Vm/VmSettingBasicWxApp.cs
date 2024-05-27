using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 小程序设置
    /// </summary>
    public class VmSettingBasicWxApp
    {
        /// <summary>
        /// 店铺Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 微信小程序ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        ///  微信小程序-AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 微信支付-店铺Id
        /// </summary>
        public string PartnerId { get; set; }
        /// <summary>
        ///微信支付-商户AppKey 
        /// </summary>
        public string PartnerAppKey { get; set; }
        /// <summary>
        ///微信支付-证书Cert 
        /// </summary>
        public string CertPem { get; set; }
        /// <summary>
        ///微信支付-证书Key 
        /// </summary>
        public string KeyPem { get; set; }


    }


}

