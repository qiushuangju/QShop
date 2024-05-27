using Newtonsoft.Json;

namespace Common.Request.Border
{
    public class BOrderGetCodeParam {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string taskId {get; set;}
        /// <summary>
        /// 订单ID
        /// </summary>
        public string orderId {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }

}