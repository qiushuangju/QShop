using System;
using System.ComponentModel;
using Qs.Repository.Base;
using Qs.Comm.Extensions;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 秒杀活动查询请求
    /// </summary>
    public class ReqQuSeckillActivity : BaseReqPage
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        [Description("活动名称")]
        public string ActivityName { get; set; }

        /// <summary>
        /// 状态 (-10:已取消 0:待开始 10:进行中 20:已结束)
        /// </summary>
        [Description("状态")]
        public int? Status { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 秒杀商品查询请求
    /// </summary>
    public class ReqQuSeckillGoods : BaseReqPage
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        [Description("活动ID")]
        public string ActivityId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string GoodsName { get; set; }

        /// <summary>
        /// 活动状态 (-10:已取消 0:待开始 10:进行中 20:已结束)
        /// </summary>
        public int? ActivityStatus { get; set; }

        /// <summary>
        /// 是否有效 (0:无效 1:有效)
        /// </summary>
        public int? IsValid { get; set; }
    }

    /// <summary>
    /// 秒杀抢购请求
    /// </summary>
    public class ReqSeckillPurchase
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        [Description("活动ID")]
        public string ActivityId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Description("商品ID")]
        public string GoodsId { get; set; }

        /// <summary>
        /// SKU ID
        /// </summary>
        [Description("SKU ID")]
        public string SkuId { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        [Description("购买数量")]
        public int Quantity { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public void Check()
        {
            if (string.IsNullOrEmpty(ActivityId))
                throw new CustomException(400, "活动ID不能为空");
            if (string.IsNullOrEmpty(GoodsId))
                throw new CustomException(400, "商品ID不能为空");
            if (string.IsNullOrEmpty(SkuId))
                throw new CustomException(400, "SKU ID不能为空");
            if (Quantity < 1)
                throw new CustomException(400, "购买数量必须大于0");
        }
    }
}