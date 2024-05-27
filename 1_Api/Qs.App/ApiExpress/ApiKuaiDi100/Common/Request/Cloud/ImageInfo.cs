using Newtonsoft.Json;

namespace Common.Request.cloud
{
    public class ImageInfo {
        /// <summary>
        /// BASE_64：base64 图片格式；URL：图片地址；QR_CODE：二维码；CODE_128：code128格式的条形码
        /// </summary>
        public string type {get; set;}
        /// <summary>
        /// 图片内容
        /// </summary>
        public string content {get; set;}
        /// <summary>
        /// 图片宽度
        /// </summary>
        public string width {get; set;}
        /// <summary>
        /// 图片高度
        /// </summary>
        public string height {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}