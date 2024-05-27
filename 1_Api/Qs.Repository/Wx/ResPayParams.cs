namespace Qs.Repository.Wx
{
   
    /// <summary>
    ///付款返回
    /// </summary>
    public class ResPayParams
    {
        /// <summary>
        /// 订单状态 10=未支付 15=卡券待确认,20=已支付 ,30=已发货
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 应用Id
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string PartnerId { get; set; }
        /// <summary>
        /// 预支付Id
        /// </summary>
        public string PrepayId { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string SignType { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string PaySign { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }
        /// <summary>
        /// wxOpenID
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 订单详情扩展字符串,统一下单接口返回的prepay_id参数值，提交格式如：prepay_id=***
        /// </summary>
        public string Package { get; set; }
        /// <summary>
        /// 本系统唯一标识
        /// </summary>
       public string OutTradeNo { get; set; }
    }
}
