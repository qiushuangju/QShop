using Newtonsoft.Json;

namespace Common.Request.BorderOfficial
{
    public class BOrderOfficialQueryDetailParam {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string taskId {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }

}