using Newtonsoft.Json;

namespace Common.Request.internationalshipment
{
    public class CustomsClearance
    {
        /// <summary>
        ///  用途(具体参考每个快递公司)
        ///  GIFT
        ///  NOT_SOLD
        ///  PERSONAL_EFFECTS
        ///  REPAIR_AND_RETURN
        ///  SAMPLE
        ///  SOLD
        /// </summary>
        public string purpose { get; set; }
        /// <summary>
        ///  是否是文件（默认 true 文件）
        /// </summary>
        public bool isDocument { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}