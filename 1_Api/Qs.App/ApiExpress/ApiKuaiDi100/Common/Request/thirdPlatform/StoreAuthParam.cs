using Newtonsoft.Json;

namespace Common.Request.thirdPlatform
{
    public class StoreAuthParam
    {
        /// <summary>
        /// 店铺类型，TAOBAO：淘宝，JINGDONG：京东，TOUTIAO：抖店，PINDUODUO：拼多多
        /// </summary>
        public string shopType { get; set; }
        /// <summary>
        /// 调参数sign的加密参数，非空时回调才会有sign参数
        /// </summary>
        public string salt { get; set; }
        /// <summary>
        /// 授权后信息的信息回调地址
        /// </summary>
        public string callBackUrl { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}