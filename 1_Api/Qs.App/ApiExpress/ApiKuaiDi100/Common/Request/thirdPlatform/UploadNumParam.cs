using Newtonsoft.Json;

namespace Common.Request.thirdPlatform
{
    public class UploadNumParam
    {
        /// <summary>
        /// 店铺类型，TAOBAO：淘宝，JINGDONG：京东，TOUTIAO：抖店，PINDUODUO：拼多多
        /// </summary>
        public string shopType { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>
        public string shopId { get; set; }
        /// <summary>
        /// 订单号，需要填写正确，否则会被电商平台的风控系统拦截
        /// </summary>
        public string orderNum { get; set; }
        /// <summary>
        /// 快递公司编码，需要填写正确，否则会被电商平台的风控系统拦截，编码请查看参数字典
        /// </summary>
        public string kuaidiCom { get; set; }
        /// <summary>
        /// 快递单号，需要填写正确，否则会被电商平台的风控系统拦截
        /// </summary>
        public string kuaidiNum { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}