using System;
using System.Collections.Generic;
using System.ComponentModel;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Comm.Extensions;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 秒杀活动新增/修改请求
    /// </summary>
    public class ReqAuSeckillActivity : BaseReqAu
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        [Description("活动名称")]
        public string ActivityName { get; set; }

        /// <summary>
        /// 活动描述
        /// </summary>
        [Description("活动描述")]
        public string Description { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Description("开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Description("结束时间")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态 (-10:已取消 0:待开始 10:进行中 20:已结束)
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 每人限购数量
        /// </summary>
        [Description("每人限购数量")]
        public int LimitPerUser { get; set; }

        /// <summary>
        /// 活动商品列表
        /// </summary>
        public List<ReqAuSeckillGoods> SeckillGoods { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public void Check()
        {
            if (string.IsNullOrEmpty(ActivityName))
                throw new CustomException(400, "活动名称不能为空");
            if (StartTime >= EndTime)
                throw new CustomException(400, "结束时间必须晚于开始时间");
            if (LimitPerUser < 1)
                throw new CustomException(400, "每人限购数量必须大于0");
            if (SeckillGoods == null || SeckillGoods.Count == 0)
                throw new CustomException(400, "请至少添加一个秒杀商品");
        }
    }

    /// <summary>
    /// 秒杀商品新增/修改请求
    /// </summary>
    public class ReqAuSeckillGoods
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string GoodsId { get; set; }

        /// <summary>
        /// SKU ID
        /// </summary>
        public string SkuId { get; set; }

        /// <summary>
        /// 秒杀价格
        /// </summary>
        public decimal SeckillPrice { get; set; }

        /// <summary>
        /// 秒杀库存
        /// </summary>
        public int SeckillStock { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public void Check()
        {
            if (string.IsNullOrEmpty(GoodsId))
                throw new CustomException(400, "商品ID不能为空");
            if (string.IsNullOrEmpty(SkuId))
                throw new CustomException(400, "SKU ID不能为空");
            if (SeckillPrice < 0)
                throw new CustomException(400, "秒杀价格不能为负数");
            if (SeckillStock < 1)
                throw new CustomException(400, "秒杀库存必须大于0");
        }
    }
}