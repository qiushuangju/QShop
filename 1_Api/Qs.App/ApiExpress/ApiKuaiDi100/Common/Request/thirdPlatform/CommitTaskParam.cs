using Newtonsoft.Json;

namespace Common.Request.thirdPlatform
{
    public class CommitTaskParam
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
        /// 订单状态，UNPAY：未付款；UNSHIP：待发货（默认值）；SHIPED：等待卖家确认收货；FINISH：交易成功/完成；CLOSE：交易关闭/取消
        /// </summary>
        public string orderStatus { get; set; }
        /// <summary>
        /// 订单更新的最小时间，格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string updateAtMin { get; set; }
        /// <summary>
        /// 订单更新的最大时间，格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string updateAtMax { get; set; }
        /// <summary>
        /// 调参数sign的加密参数，非空时回调才会有sign参数
        /// </summary>
        public string salt { get; set; }
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