using Newtonsoft.Json;

namespace Common.Request.cloud
{
    public class PrintOldParam {
        /// <summary>
        /// 任务id
        /// </summary>
        public string taskId {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}