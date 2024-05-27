using Qs.Repository.Base;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuGoods : BaseReqPage
    {
        /// <summary>
        /// 商品类型(0:全部 10:本地直采 20:限时抢购 )
        /// </summary>
        public int? GoodsType { get; set; }

        /// <summary>
        /// 商品id列表(多个id用,分隔)
        /// </summary>
        public string GoodsIds { get; set; }
        /// <summary>
        /// 商品状态(10:已上架)
        /// </summary>
        public int? BigStatus { get; set; }
        /// <summary>
        /// 卖场Id
        /// </summary>
        public string RoundId { get; set; }
        /// <summary>
        /// 一级或二分类
        /// </summary>
        public string CateIdAllLevel  { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品库存
        /// </summary>
        public int? StockTotal { get; set; }     
        /// <summary>
        /// 排序方式 (10:综合 20:价格降序 30:价格升序 60:销量降序 70:新品(时间降序))
        /// </summary>
        public int? SortType { get; set; }
         /// <summary>
         /// 是否推荐
         /// </summary>
        public bool? IsRecommend { get; set; }
    }
}