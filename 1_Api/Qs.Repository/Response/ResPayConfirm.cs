using Qs.Repository.Wx;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 订单确认
    /// </summary>
    public class ResPayConfirm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResPayConfirm()
        {
            WxPayParams=new ResPayParams();
        }
                              
        /// <summary>
        /// 需支付金额
        /// </summary>
        public decimal AmountTotal { get; set; }

        /// <summary>
        /// 是否已创建订单
        /// </summary>
        public bool IsCreatedOrder { get; set; }
        /// <summary>
        /// 微信支付小程序端参数
        /// </summary>
        public ResPayParams WxPayParams { get; set; }
    }
}
