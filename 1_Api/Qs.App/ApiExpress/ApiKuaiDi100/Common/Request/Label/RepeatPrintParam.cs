using Newtonsoft.Json;

namespace Common.Request.Label
{
    public class RepeatPrintParam
    {
        /// <summary>
        ///  任务ID,对应下单时返回的taskId
        /// </summary>
        /// <value></value>
        public string taskId;
        /// <summary>
        ///  快递100打印机,不填默认为下单时填入的siid
        /// </summary>
        /// <value></value>
        public string siid;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}