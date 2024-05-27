using Newtonsoft.Json;

namespace Common.Request.Border
{
    public class BOrderCancelParam {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string taskId {get; set;}
        /// <summary>
        /// 订单ID
        /// </summary>
        public string orderId {get; set;}
        /// <summary>
        /// 取消原因，例：暂时不寄件了
        /// </summary>
        public string cancelMsg {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }

}