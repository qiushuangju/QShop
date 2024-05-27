using Newtonsoft.Json;

namespace Common.Request.Border
{
    public class BOrderCallBackParam {
        /// <summary>
        /// 快递公司的编码，一律用小写字母，见《快递公司编码》,选填。
        /// </summary>
        public string kuaidicom {get; set;}
        /// <summary>
        /// 快递单号，单号的最大长度是32个字符。
        /// </summary>
        public string kuaidinum {get; set;}
        /// <summary>
        /// 状态码
        /// </summary>
        public string status {get; set;}
        
        /// <summary>
        /// 状态描述
        /// </summary>
        public string message {get; set;}
        
        /// <summary>
        /// 订单内容
        /// </summary>
        public BOrderCallBackParamData data {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }

}