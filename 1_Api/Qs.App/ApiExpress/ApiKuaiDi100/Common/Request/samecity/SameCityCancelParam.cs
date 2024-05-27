using Newtonsoft.Json;

namespace Common.Request.samecity
{
    public class SameCityCancelParam
    {

        /// <summary>
        /// 任务ID
        /// </summary>
        public string taskId { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        public string cancelMsg { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}