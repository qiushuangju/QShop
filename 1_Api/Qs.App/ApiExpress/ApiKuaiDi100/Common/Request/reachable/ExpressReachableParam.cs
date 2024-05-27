using Newtonsoft.Json;

namespace Common.Request.reachable
{
    public class ExpressReachableParam {

    
        /// <summary>
        /// 快递公司的编码，一律用小写字母，见《快递公司编码》
        /// </summary>
        public  string kuaidicom {get; set;}
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string recManName {get; set;}
        /// <summary>
        /// 收件人的手机号，手机号和电话号二者其一必填
        /// </summary>
        public string recManMobile {get; set;}
        /// <summary>
        /// 收件人所在完整地址，如广东深圳市深圳市南山区科技南十二路2号金蝶软件园
        /// </summary>
        public string recManPrintAddr {get; set;}
        /// <summary>
        /// 寄件人姓名
        /// </summary>
        public string sendManName {get; set;}
        /// <summary>
        /// 寄件人的手机号，手机号和电话号二者其一必填
        /// </summary>
        public string sendManMobile {get; set;}
        /// <summary>
        /// 寄件人所在的完整地址，如广东深圳市深圳市南山区科技南十二路2号金蝶软件园B10
        /// </summary>
        public string sendManPrintAddr {get; set;}
       
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}