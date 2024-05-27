﻿//------------------------------------------------------------------------------
// <autogenerated>
//  codeSmith生成代码,别修改.再生成会被覆盖
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Qs.Repository.Core;

namespace Qs.Repository.Domain
{
    /// <summary>
	/// 
	/// </summary>
    [Table("tb_Coupon")]
    public partial class ModelCoupon : StringEntity
    { 
        public ModelCoupon()
        {
          this.Name= string.Empty;
          this.CouponType= ((10));
          this.SendType= ((10));
          this.MinPrice= 0;
          this.ReducePrice= 0;
          this.ExpireType= ((10));
          this.ExpireDay= 0;
          this.StartTime= DateTime.Now;
          this.EndTime= DateTime.Now;
          this.ApplyRange= ((10));
          this.ApplyRangeConfig= string.Empty;
          this.ShowImgId= string.Empty;
          this.IsShowHome= ((-10));
          this.TotalNum= ((0));
          this.ReceiveNum= ((0));
          this.Describe= string.Empty;
          this.LimitQuantity= ((1));
          this.Status= ((10));
          this.Sort= 0;
          this.StoreId= string.Empty;
          this.CreateTime= DateTime.Now;
        }


        
        /// <summary>
        /// 优惠券名称
        /// </summary>
        [Description("优惠券名称")]
        public string Name { get; set; }
        /// <summary>
        /// 优惠券类型(10:满减券 20:折扣券)
        /// </summary>
        [Description("优惠券类型(10:满减券 20:折扣券)")]
        public int? CouponType { get; set; }
        /// <summary>
        /// 发放方式:10:用户领取 20:系统发放
        /// </summary>
        [Description("发放方式:10:用户领取 20:系统发放")]
        public int SendType { get; set; }
        /// <summary>
        /// 最低消费金额
        /// </summary>
        [Description("最低消费金额")]
        public decimal MinPrice { get; set; }
        /// <summary>
        /// 减免金额 
        /// </summary>
        [Description("减免金额 ")]
        public decimal ReducePrice { get; set; }
        /// <summary>
        /// 到期类型(10领取后生效 20固定时间) 
        /// </summary>
        [Description("到期类型(10领取后生效 20固定时间) ")]
        public int ExpireType { get; set; }
        /// <summary>
        /// 领取后生效-有效天数 
        /// </summary>
        [Description("领取后生效-有效天数 ")]
        public int? ExpireDay { get; set; }
        /// <summary>
        /// 固定时间-开始时间
        /// </summary>
        [Description("固定时间-开始时间")]
        public System.DateTime StartTime { get; set; }
        /// <summary>
        /// 固定时间-结束时间
        /// </summary>
        [Description("固定时间-结束时间")]
        public System.DateTime EndTime { get; set; }
        /// <summary>
        /// 适用范围(10:全部商品 20:指定商品 30:排除商品)
        /// </summary>
        [Description("适用范围(10:全部商品 20:指定商品 30:排除商品)")]
        public int ApplyRange { get; set; }
        /// <summary>
        /// 适用范围(商品Id,隔开)
        /// </summary>
        [Description("适用范围(商品Id,隔开)")]
        public string ApplyRangeConfig { get; set; }
        /// <summary>
        /// 弹出图片Id
        /// </summary>
        [Description("弹出图片Id")]
        [Browsable(false)]
        public string ShowImgId { get; set; }
        /// <summary>
        /// 是否首页弹出(-10:不 10:是)
        /// </summary>
        [Description("是否首页弹出(-10:不 10:是)")]
        public int? IsShowHome { get; set; }
        /// <summary>
        /// 发放总数量(-1为不限制) 
        /// </summary>
        [Description("发放总数量(-1为不限制) ")]
        public int TotalNum { get; set; }
        /// <summary>
        /// 已领取数量 
        /// </summary>
        [Description("已领取数量 ")]
        public int ReceiveNum { get; set; }
        /// <summary>
        /// 优惠券描述
        /// </summary>
        [Description("优惠券描述")]
        public string Describe { get; set; }
        /// <summary>
        /// 限领次数
        /// </summary>
        [Description("限领次数")]
        public int LimitQuantity { get; set; }
        /// <summary>
        /// 状态(10:可用 -10:不可用)
        /// </summary>
        [Description("状态(10:可用 -10:不可用)")]
        public int Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int Sort { get; set; }
        /// <summary>
        /// 店铺Id
        /// </summary>
        [Description("店铺Id")]
        [Browsable(false)]
        public string StoreId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public System.DateTime CreateTime { get; set; }
    }
}