using Newtonsoft.Json;

namespace Common.Request.Border
{
    public class BOrderQueryParam {
        /// <summary>
        /// 寄件地址
        /// </summary>
        /// <value></value>
        public string sendAddr {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore}).Replace("\r\n","").Replace(" ","");
        }
    }

}