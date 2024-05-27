using Newtonsoft.Json;

namespace Common.Request.Corder
{
    public class COrderQueryPriceParam {
        /// <summary>
        /// 快递公司编码
        /// </summary>
        public string kuaidicom {get; set;}
        /// <summary>
        /// 出发地地址，最小颗粒到市级，例如：广东省深圳市
        /// </summary>
        public string sendManPrintAddr {get; set;}
        /// <summary>
        /// 目的地地址，最小颗粒到市级，例如：广东省深圳市
        /// </summary>
        public string recManPrintAddr {get; set;}  
        /// <summary>
        /// 重量
        /// </summary>
        public string weight {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}