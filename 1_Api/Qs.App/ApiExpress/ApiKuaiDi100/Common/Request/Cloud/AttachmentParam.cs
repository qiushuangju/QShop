using Newtonsoft.Json;

namespace Common.Request.cloud
{
    public class AttachmentParam {
        /// <summary>
        /// 贵司内部自定义的订单编号,需要保证唯一性，非必填
        /// </summary>
        public string orderId {get; set;}
        /// <summary>
        /// 打印设备，通过打印机输出的设备码进行获取
        /// </summary>
        public string siid {get; set;}  
        /// <summary>
        /// 打印纸的高度
        /// </summary>
        public string height {get; set;}
        /// <summary>
        /// 打印纸的宽度
        /// </summary>
        public string width {get; set;}
        /// <summary>
        /// 打印状态回调地址
        /// </summary>
        public string callBackUrl {get; set;}
        /// <summary>
        /// 签名用随机字符串
        /// </summary>
        public string salt {get; set;}
        /// <summary>
        /// 需要打印的份数，默认是一份
        /// </summary>
        public string copyNum {get; set;}
        /// <summary>
        ///  打印范围，起始页，仅对多页文件支持，默认是打印整个文档
        /// </summary>
        public string startPage {get; set;}
        /// <summary>
        /// 打印范围，结束页，仅对多页文件支持，默认是打印整个文档
        /// </summary>
        public string endPage {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}