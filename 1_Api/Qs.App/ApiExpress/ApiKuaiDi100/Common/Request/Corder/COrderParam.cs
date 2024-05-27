using Newtonsoft.Json;

namespace Common.Request.Corder
{
    public class COrderParam {

    
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
        /// 重量
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
        /// 预约日期，例如：今天/明天/后天
        /// </summary>
        public string dayType {get; set;}
        
        /// <summary>
        /// 预约起始时间，24小时制（HH:mm），例如：09:00
        /// </summary>
        public string pickupStartTime {get; set;}
        
        /// <summary>
        /// 预约截止时间，24小时制（HH:mm），例如：10:00
        /// </summary>
        public string pickupEndTime {get; set;}
        
        /// <summary>
        /// 是否开启订阅功能 0：不开启(默认) 1：开启 说明开启订阅功能时：pollCallBackUrl必须填入 此功能只针对有快递单号的单
        /// </summary>
        public string op {get; set;}
        
        /// <summary>
        /// 如果op设置为1时，pollCallBackUrl必须填入，用于跟踪回调
        /// </summary>
        public string pollCallBackUrl {get; set;}

        /// <summary>
        /// 添加此字段表示开通行政区域解析功能 。
        /// 0：关闭（默认）
        /// 1：开通行政区域解析功能以及物流轨迹增加物流状态名称
        /// 4：开通行政解析功能以及物流轨迹增加物流高级状态名称、状态值并且返回出发、目的及当前城市信息(详见：快递信息推送接口文档)
        /// </summary>
        public string resultv2 {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}