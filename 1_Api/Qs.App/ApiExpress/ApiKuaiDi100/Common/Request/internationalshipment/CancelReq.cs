using Newtonsoft.Json;

namespace Common.Request.internationalshipment
{
    public class CancelReq
    {
        /// <summary>
        ///  快遞單號
        /// </summary>
        public string kuaidicom { get; set; }
        /// <summary>
        ///  快遞單號
        /// </summary>
        public string kuaidinum { get; set; }
        /// <summary>
        ///  訂單id
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        ///  驗證信息
        /// </summary>
        public ValidateInfo vi { get; set; }
        /// <summary>
        ///  驗證信息
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        ///  取消原因
        /// </summary>
        public string reason { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}