using Newtonsoft.Json;

namespace Common.Request.thirdPlatform
{
    public class BranchInfoParam
    {
        /// <summary>
        /// 调用菜鸟或淘宝第三方授权接口后获取到的partnerId值
        /// </summary>
        public string partnerId { get; set; }
        /// <summary>
        /// 调用菜鸟或淘宝第三方授权接口后获取到的partnerKey值
        /// </summary>
        public string partnerKey { get; set; }
        /// <summary>
        /// 菜鸟:cainiao，淘宝:taobao，京东无界:jdalpha，拼多多:pinduoduoWx，抖店:douyin
        /// </summary>
        public string net { get; set; }
        /// <summary>
        /// 快递公司编号
        /// </summary>
        public string com { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}