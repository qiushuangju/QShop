using Newtonsoft.Json;

namespace Common.Request.Electronic
{
    public class CancelPrint
    {

        /// <summary>
        /// 电子面单客户账户或月结账号，需贵司向当地快递公司网点申请
        /// </summary>
        public string partnerId { get; set; }
        /// <summary>
        /// 电子面单密码，需贵司向当地快递公司网点申请
        /// </summary>
        public string partnerKey { get; set; }
        /// <summary>
        /// 电子面单密钥，需贵司向当地快递公司网点申请； 是否必填该属性，请查看参数字典
        /// </summary>
        public string partnerSecret { get; set; }
        /// <summary>
        /// 电子面单客户账户名称，需贵司向当地快递公司网点申请； 是否必填该属性，请查看参数字典
        /// </summary>
        public string partnerName { get; set; }
        /// <summary>
        /// 收件网点名称,由快递公司当地网点分配， 若使用淘宝授权填入（taobao），使用菜鸟授权填入（cainiao）
        /// </summary>
        public string net { get; set; }
        /// <summary>
        /// 电子面单承载编号，需贵司向当地快递公司网点申请； 是否必填该属性，请查看参数字典
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 快递公司的编码，一律用小写字母
        /// </summary>
        public string kuaidicom { get; set; }
        /// <summary>
        /// 快递公司订单号(对应下单时返回的kdComOrderNum，如果没有可以不传，否则必传)
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string kuaidinum { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        public string reason { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}