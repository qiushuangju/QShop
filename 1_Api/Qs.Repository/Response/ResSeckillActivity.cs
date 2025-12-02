using System;
using System.Collections.Generic;
using System.ComponentModel;
using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Request;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 秒杀活动响应对象
    /// </summary>
    public class ResSeckillActivity : ModelSeckillActivity
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        [Description("状态名称")]
        public string StatusName { get; set; }

        /// <summary>
        /// 秒杀商品列表
        /// </summary>
        public List<ResSeckillGoods> SeckillGoods { get; set; }

        /// <summary>
        /// 转为ViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResSeckillActivity ToView(ModelSeckillActivity model)
        {
            var res = xConv.CopyMapper<ResSeckillActivity, ModelSeckillActivity>(model);
            res.StatusName = xEnum.GetEnumDescription(typeof(SeckillActivityStatus), model.Status);
            return res;
        }
    }

    /// <summary>
    /// 秒杀商品响应对象
    /// </summary>
    public class ResSeckillGoods : ModelSeckillGoods
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string GoodsName { get; set; }

        /// <summary>
        /// SKU名称
        /// </summary>
        [Description("SKU名称")]
        public string SkuName { get; set; }

        /// <summary>
        /// 商品原价
        /// </summary>
        [Description("商品原价")]
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        [Description("商品图片")]
        public string GoodsImage { get; set; }

        /// <summary>
        /// 活动状态名称
        /// </summary>
        public string ActivityStatusName { get; set; }

        /// <summary>
        /// 活动状态
        /// </summary>
        public int ActivityStatus { get; set; }

        /// <summary>
        /// 已抢百分比
        /// </summary>
        [Description("已抢百分比")]
        public decimal SoldPercent { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime? ActivityStartTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime? ActivityEndTime { get; set; }

        /// <summary>
        /// 倒计时秒数
        /// </summary>
        public long CountdownSeconds { get; set; }

        /// <summary>
        /// 转为ViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="goodsName"></param>
        /// <param name="skuName"></param>
        /// <param name="originalPrice"></param>
        /// <param name="goodsImage"></param>
        /// <param name="activityStatus"></param>
        /// <param name="activityStartTime"></param>
        /// <param name="activityEndTime"></param>
        /// <returns></returns>
        public static ResSeckillGoods ToView(ModelSeckillGoods model, string goodsName, string skuName, decimal originalPrice, string goodsImage, int activityStatus, DateTime? activityStartTime, DateTime? activityEndTime)
        {
            var res = xConv.CopyMapper<ResSeckillGoods, ModelSeckillGoods>(model);
            res.GoodsName = goodsName;
            res.SkuName = skuName;
            res.OriginalPrice = originalPrice;
            res.GoodsImage = goodsImage;
            res.ActivityStatus = activityStatus;
            res.ActivityStatusName = xEnum.GetEnumDescription(typeof(SeckillActivityStatus), activityStatus);
            res.ActivityStartTime = activityStartTime;
            res.ActivityEndTime = activityEndTime;

            // 计算已抢百分比
            if (model.SeckillStock > 0)
            {
                res.SoldPercent = Math.Round((decimal)model.SoldQuantity / model.SeckillStock * 100, 2);
            }
            else
            {
                res.SoldPercent = 100;
            }

            // 计算倒计时秒数
            res.CountdownSeconds = CalculateCountdownSeconds(activityStatus, activityStartTime, activityEndTime);

            return res;
        }

        /// <summary>
        /// 计算倒计时秒数
        /// </summary>
        /// <param name="activityStatus"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private static long CalculateCountdownSeconds(int activityStatus, DateTime? startTime, DateTime? endTime)
        {
            var now = DateTime.Now;

            switch (activityStatus)
            {
                case (int)SeckillActivityStatus.Pending: // 待开始
                    if (startTime.HasValue)
                    {
                        var timeSpan = startTime.Value - now;
                        return timeSpan.TotalSeconds > 0 ? (long)timeSpan.TotalSeconds : 0;
                    }
                    break;
                case (int)SeckillActivityStatus.Ongoing: // 进行中
                    if (endTime.HasValue)
                    {
                        var timeSpan = endTime.Value - now;
                        return timeSpan.TotalSeconds > 0 ? (long)timeSpan.TotalSeconds : 0;
                    }
                    break;
            }

            return 0;
        }
    }

    /// <summary>
    /// 秒杀抢购结果响应对象
    /// </summary>
    public class ResSeckillPurchaseResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [Description("是否成功")]
        public bool Success { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Description("订单编号")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [Description("消息")]
        public string Message { get; set; }

        /// <summary>
        /// 秒杀活动状态
        /// </summary>
        public int ActivityStatus { get; set; }

        /// <summary>
        /// 商品库存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 已售数量
        /// </summary>
        public int SoldQuantity { get; set; }
    }
}