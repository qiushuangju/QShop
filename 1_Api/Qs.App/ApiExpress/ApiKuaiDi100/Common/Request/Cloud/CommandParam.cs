using Newtonsoft.Json;

namespace Common.Request.cloud
{
    public class CommandParam {
        /// <summary>
        /// 可识别打印指令，内容需我司硬件指令进行协商确定
        /// </summary>
        public string content {get; set;}
        /// <summary>
        /// 打印设备，通过打印机输出的设备码进行获取
        /// </summary>
        public string siid {get; set;}
        /// <summary>
        /// 签名用随机字符串
        /// </summary>
        public string salt {get; set;}
        /// <summary>
        /// 打印状态回调地址
        /// </summary>
        public string callBackUrl {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}