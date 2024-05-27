using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Vm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TencentCloud.Scf.V20180416.Models;

namespace Qs.Repository.Response
{
    public class ResStoreSettingPay:ModelStoreSettingPay
    {
        public WxPayObj SettingObj { get; set; }

        /// <summary>
        /// View
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResStoreSettingPay ToView(ModelStoreSettingPay model)
        {
            ResStoreSettingPay res = xConv.CopyMapper<ResStoreSettingPay, ModelStoreSettingPay>(model);
            res.SettingObj = xConv.JsonToObj<WxPayObj>(res.Values);
            return res;
        }
    }

  
    public class WxPayObj
    {
        /// <summary>
        /// 是否服务商支付
        /// </summary>
        public bool servicePay { get; set; } = false;
        /// <summary>
        /// 微信支付商户号
        /// </summary>
        public string mchid { get; set; }
        /// <summary>
        /// 微信支付秘钥
        /// </summary>
        public string signkey { get; set; }
        /// <summary>
        /// 子商户号
        /// </summary>
        public string subMchId { get; set; }
        /// <summary>
        /// apiclient_cert.pem证书
        /// </summary>
        public string certPath { get; set; }
        /// <summary>
        /// apiclient_key.pe 证书
        /// </summary>
        public string keyPath { get; set; }
        public void Check()
        {
            if (servicePay==true)
            {
                xValidation.CheckStrNull(subMchId, "子商户号");
            }
            xValidation.CheckStrNull(mchid, "微信支付商户号");
            xValidation.CheckStrNull(signkey, "微信支付秘钥");
            xValidation.CheckStrNull(certPath, "apiclient_cert.pem证书");
            xValidation.CheckStrNull(keyPath, "apiclient_key.pem证书");
        }
    }
}
