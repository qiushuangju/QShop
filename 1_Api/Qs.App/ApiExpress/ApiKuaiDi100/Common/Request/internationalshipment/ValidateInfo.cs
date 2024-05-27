using Newtonsoft.Json;

namespace Common.Request.internationalshipment
{
    public class ValidateInfo
    {
        /// <summary>
        ///  月结或支付账号，详见字典表
        /// </summary>
        public string partnerId;
        /// <summary>
        ///  月结账号用户名，详见字典表
        /// </summary>
        public string partnerName;
        /// <summary>
        ///  月结账号密钥，详见字典表
        /// </summary>
        public string partnerKey;
        /// <summary>
        ///  月结账号用户密码，详见字典表
        /// </summary>
        public string partnerSecret;
        /// <summary>
        ///  账号参数，详见字典表
        /// </summary>
        public string code;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}