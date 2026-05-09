namespace Qs.Repository.Request
{
    /// <summary>
    /// 秒杀抢购请求
    /// </summary>
    public class ReqSeckillBuy
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityId { get; set; }
        
        /// <summary>
        /// 商品ID
        /// </summary>
        public string GoodsId { get; set; }
        
        /// <summary>
        /// SKU ID
        /// </summary>
        public string SkuId { get; set; }
        
        /// <summary>
        /// 购买数量
        /// </summary>
        public int BuyCount { get; set; }
        
        /// <summary>
        /// 用户收货地址ID
        /// </summary>
        public string AddressId { get; set; }
    }
}