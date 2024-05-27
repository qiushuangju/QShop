using Newtonsoft.Json;

namespace Common.Request.internationalshipment
{
    public class Payment
    {
        /// <summary>
        ///  运费支付方式(支付方式:SHIPPER-寄方付 {get; set;}CONSIGNEE-收方付 默认SHIPPER)
        ///  关税(支付方式:DDU-寄方支付 {get; set;}DDP-收方支付 {get; set;}默认DDP)
        /// </summary>
        public string paymentType { get; set; }
        /// <summary>
        ///  账号
        /// </summary>
        public string account { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}