using Newtonsoft.Json;

namespace Common.Request.Border
{
    public class BOrderParam {

    
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
        /// <summary>
        /// 物品名称,例：文件
        /// </summary>
        public string cargo {get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string weight {get; set;}
        /// <summary>
        /// 备注
        /// </summary>
        public string remark {get; set;}
        /// <summary>
        /// 签名用随机字符串
        /// </summary>
        public string salt {get; set;}
        /// <summary>
        /// callBackUrl订单信息回调
        /// </summary>
        public string callBackUrl {get; set;}
        /// <summary>
        /// 快递业务服务类型，例：标准快递，默认为标准快递
        /// </summary>
        public string serviceType {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}