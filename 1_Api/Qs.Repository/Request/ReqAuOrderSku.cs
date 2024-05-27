﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a CodeSmith Template.
//
//     DO NOT MODIFY contents of this file. Changes to this
//     file will be lost if the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Qs.Repository.Core;

namespace Qs.Repository.Request
{
    /// <summary>
	/// 
	/// </summary>
    [Table("tb_OrderSku")]
    public partial class ReqAuOrderSku 
    {

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public string GoodsId { get; set; }
        /// <summary>
        /// SkuId唯一标识
        /// </summary>
        public string GoodsSkuId { get; set; }
        /// <summary>
        /// 订单ID 
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// Sku图片Id
        /// </summary>
        public string ImageId { get; set; }
        /// <summary>
        /// 库存计算方式(10下单减库存 20付款减库存) 
        /// </summary>
        public int DeductStockType { get; set; }
        /// <summary>
        /// 规格类型(10单规格 20多规格) 
        /// </summary>
        public int SpecType { get; set; }
        /// <summary>
        /// SKU的规格属性
        /// </summary>
        public string GoodsProps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DetailPics { get; set; }
        /// <summary>
        /// 商品重量(Kg)
        /// </summary>
        public decimal GoodsWeight { get; set; }
        /// <summary>
        /// 优惠券折扣金额 
        /// </summary>
        public decimal CouponMoney { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public string TotalNum { get; set; }
        /// <summary>
        /// 零售价
        /// </summary>
        public decimal? RetailPrice { get; set; }
        /// <summary>
        /// 会员价
        /// </summary>
        public decimal VipPrice { get; set; }
        /// <summary>
        /// 商品总价(数量×单价)
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 实际付款价(折扣和优惠后)
        /// </summary>
        public decimal TotalPayPrice { get; set; }
        /// <summary>
        /// 赠送的积分数量
        /// </summary>
        public string PointsBonus { get; set; }
        /// <summary>
        /// 是否已评价(0否 1是)
        /// </summary>
        public string IsComment { get; set; }
        /// <summary>
        /// 来源Id(上下级,隔开)
        /// </summary>
        public string SourceUserId { get; set; }
        /// <summary>
        /// 是否存在会员等级折扣(预留)
        /// </summary>
        public int? IsUserGrade { get; set; }
        /// <summary>
        /// 会员折扣比例(0-10) (预留)
        /// </summary>
        public int? GradeRatio { get; set; }
        /// <summary>
        /// 会员折扣的商品单价(预留)
        /// </summary>
        public decimal? GradeGoodsPrice { get; set; }
        /// <summary>
        /// 会员折扣的总额差(预留)
        /// </summary>
        public decimal? GradeTotalMoney { get; set; }
        /// <summary>
        /// 积分金额(预留)
        /// </summary>
        public decimal? PointsMoney { get; set; }
        /// <summary>
        /// 积分抵扣数量(预留)
        /// </summary>
        public string PointsNum { get; set; }
        /// <summary>
        /// 店铺Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateTime { get; set; }
    }
}