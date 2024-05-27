using Newtonsoft.Json;

namespace Common.Request.thirdPlatform
{
    public class PlatformAuthParam
    {
        /// <summary>
        /// 请求的第三方平台，仅主账号可授权。淘宝：taobao，菜鸟：cainiao，京东：jdalpha，拼多多：pinduoduoWx，抖店:douyin
        /// </summary>
        public string net { get; set; }
        /// <summary>
        /// 已经授权完需要重新获取授权信息
        /// </summary>
        public string partnerId { get; set; }
        /// <summary>
        /// 授权后信息的信息回调地址
        /// </summary>
        public string callbackUrl { get; set; }
        /// <summary>
        /// web(默认），wap(只有淘宝和菜鸟可以使用）
        /// </summary>
        public string view { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}