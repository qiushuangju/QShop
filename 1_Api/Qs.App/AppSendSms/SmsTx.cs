using System;
using Qs.Comm;
using Qs.Repository.Vm;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;

namespace Qs.App
{
    
    /// <summary>
    /// 腾讯短信
    /// </summary>
    public class SmsTx : ISmsHelper
    {
        VmSettingSms _vm;

        public SmsTx(VmSettingSms vm)
        {
            _vm = vm;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        public ResPhoneCode SendPhoneCode(string phone,string code)
        {
            ResPhoneCode res = new ResPhoneCode();     
            res.SmsRes = SendSms(phone, "2051450", new[] { code, "5" });
            return res;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        public ResPhoneCode SendNewOrderToStore(string phone)
        {
            ResPhoneCode res = new ResPhoneCode();     
            res.SmsRes = SendSms(phone, "2051514", new[] { "1", "5" });
            return res;
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="templateId"></param>
        /// <param name="param"></param>
        private  SendSmsResponse SendSms(string phone, string templateId, string[] param)
        {
            //sms helper: https://cloud.tencent.com/document/product/382/3773 

            

            // 非必要步骤 实例化一个客户端配置对象，可以指定超时时间等配置
            ClientProfile clientProfile = new ClientProfile();
            // 签名方式
            clientProfile.SignMethod = ClientProfile.SIGN_TC3SHA256;
            SmsClient client = new SmsClient(Define.TengXunCred, "ap-guangzhou", clientProfile);
            SendSmsRequest req = new SendSmsRequest();
            //AppId
            req.SmsSdkAppid = $"{_vm.Engine.Qcloud.SdkAppID}";
            req.Sign = $"{_vm.Engine.Qcloud.Sign}";
            req.SenderId = ""; // 国际/港澳台短信 senderid: 国内短信填空，默认未开通，如需开通请联系[sms helper] 
            req.PhoneNumberSet = new String[] {$"+86{phone}"}; //最多不要超过200个手机号
            req.TemplateID = templateId; // 模板 ID
            req.TemplateParamSet = param; //模板参数
            SendSmsResponse resp = client.SendSmsSync(req);
            return resp;
        }
    }

    /// <summary>
    /// 验证码返回
    /// </summary>
    public class ResPhoneCode
    {
        /// <summary>
        /// 短信返回值
        /// </summary>
        public SendSmsResponse SmsRes { get; set; }
        // /// <summary>
        // /// 验证码
        // /// </summary>
        // public string Code { get; set; }
        /// <summary>
        ///验证码Id
        /// </summary>
        public string VerifyCodeId { get; set; }
    }
}
