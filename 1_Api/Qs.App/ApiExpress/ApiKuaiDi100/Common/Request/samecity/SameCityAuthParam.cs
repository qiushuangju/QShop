using Newtonsoft.Json;

namespace Common.Request.samecity
{
    public class SameCityAuthParam
    {
        /// <summary>
        /// 快递公司，一律用小写字母，见参数字典
        /// </summary>
        public string com { get; set; }
        /// <summary>
        /// 授权店铺id
        /// </summary>
        public string storeId { get; set; }
        /// <summary>
        /// 授权后信息的信息回调地址
        /// </summary>
        public string callbackUrl { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}