using Newtonsoft.Json;

namespace Common.Request.internationalshipment
{
    public class InvoiceInfo
    {
        /// <summary>
        ///  发票日期（格式：yyyy-MM-dd）
        /// </summary>
        public string date { get; set; }
        /// <summary>
        ///  发票号
        /// </summary>
        public string number { get; set; }
        /// <summary>
        ///  发票类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        ///  发票抬头,base64字符或常规字符,不同快递公司要求不一样
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///  发票签名（BASE64字符串）
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        ///  是否启用无纸化贸易
        /// </summary>
        public bool pltEnable { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}