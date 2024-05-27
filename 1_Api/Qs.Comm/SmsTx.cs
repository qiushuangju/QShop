using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;

namespace Qs.Comm
{
    /// <summary>
    /// 腾讯短信
    /// </summary>
    public class SmsTx
    {
       

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="templateId"></param>
        /// <param name="param"></param>
        public static SendSmsResponse SendSms(string phone, string templateId, string[] param)
        {
            //sms helper: https://cloud.tencent.com/document/product/382/3773 

            // 非必要步骤 实例化一个客户端配置对象，可以指定超时时间等配置
            ClientProfile clientProfile = new ClientProfile();
            // 签名方式
            clientProfile.SignMethod = ClientProfile.SIGN_TC3SHA256;
            SmsClient client = new SmsClient(Define.TengXunCred, "ap-guangzhou", clientProfile);
            SendSmsRequest req = new SendSmsRequest();
            //AppId
            req.SmsSdkAppid = "1400584665";
            req.Sign = "翠银通";
            req.SenderId = ""; // 国际/港澳台短信 senderid: 国内短信填空，默认未开通，如需开通请联系[sms helper] 
            req.PhoneNumberSet = new String[] {$"+86{phone}"}; //最多不要超过200个手机号
            req.TemplateID = templateId; // 模板 ID
            req.TemplateParamSet = param; //模板参数
            SendSmsResponse resp = client.SendSmsSync(req);
            return resp;
        }
    }
}
