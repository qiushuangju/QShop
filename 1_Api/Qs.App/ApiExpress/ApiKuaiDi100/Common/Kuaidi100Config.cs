
namespace Common{
    /// <summary>
    /// 快递100的基础账号信息，可以在这里获取 
    /// https://poll.kuaidi100.com/manager/page/myinfo/enterprise
    /// </summary>
    public class KuaiDi100Config
    {
        /// <summary>
        /// 授权key
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// customer
        /// </summary>
        public string customer { get; set; }
        /// <summary>
        /// secret
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        /// 快递100打印机或者云盒设备码（使用电子面单打印接口或云打印相关接口必填）
        /// </summary>
        public string siid { get; set; }
        /// <summary>
        /// userid
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 短信模板id
        /// </summary>
        public string tid { get; set; }
        /// <summary>
        /// 电子面单快递公司账号信息（月结账号）
        /// </summary>
        public string partnerId { get; set; }
        /// <summary>
        /// 电子面单快递公司账号信息（账号密码）
        /// </summary>
        public string partnerKey { get; set; }
    }
}
