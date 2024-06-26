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
	/// 商品管理
	/// </summary>
    [Table("tb_Goods")]
    public partial class ModelGoods : StringEntity
    { 
        public ModelGoods()
        {
          this.GoodsName= string.Empty;
          this.SubTitle= string.Empty;
          this.IsRecommend= false;
          this.GoodsNo= string.Empty;
          this.Status= 0;
          this.ImageIdMains= string.Empty;
          this.ImageIdDetails= string.Empty;
          this.LinePrice= 0;
          this.SalePrice= 0;
          this.PurchasePrice= 0;
          this.SpecType= 0;
          this.DeductStockType= 0;
          this.StockTotal= 0;
          this.SalesInitial= 0;
          this.SalesActual= 0;
          this.CateId= string.Empty;
          this.Cate2Id= string.Empty;
          this.Cate3Id= string.Empty;
          this.StopTime= null;
          this.CompanyCode= string.Empty;
          this.BrandId= string.Empty;
          this.SortNo= 0;
          this.DeliveryId= string.Empty;
          this.ServiceIds= string.Empty;
          this.StoreId= string.Empty;
          this.IsDelete= 0;
          this.CreateTime= DateTime.Now;
        }


        
        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string GoodsName { get; set; }
        /// <summary>
        /// (小标题/商品描述)
        /// </summary>
        [Description("(小标题/商品描述)")]
        public string SubTitle { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        [Description("是否推荐")]
        public bool? IsRecommend { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        [Description("商品编码")]
        public string GoodsNo { get; set; }
        /// <summary>
        /// 商品状态(-10:下架 10:上架)
        /// </summary>
        [Description("商品状态(-10:下架 10:上架)")]
        public int Status { get; set; }
        /// <summary>
        /// 缩略图(主图)
        /// </summary>
        [Description("缩略图(主图)")]
        [Browsable(false)]
        public string ImageIdMains { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        [Description("商品图片")]
        [Browsable(false)]
        public string ImageIdDetails { get; set; }
        /// <summary>
        /// 划线价
        /// </summary>
        [Description("划线价")]
        public decimal? LinePrice { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        [Description("售价")]
        public decimal? SalePrice { get; set; }
        /// <summary>
        /// 商品进价
        /// </summary>
        [Description("商品进价")]
        public decimal? PurchasePrice { get; set; }
        /// <summary>
        /// 商品规格(10单规格 20多规格)
        /// </summary>
        [Description("商品规格(10单规格 20多规格)")]
        public int SpecType { get; set; }
        /// <summary>
        /// 库存计算方式(10下单减库存 20付款减库存) 
        /// </summary>
        [Description("库存计算方式(10下单减库存 20付款减库存) ")]
        public int DeductStockType { get; set; }
        /// <summary>
        /// 库存总量(包含所有sku)
        /// </summary>
        [Description("库存总量(包含所有sku)")]
        public int StockTotal { get; set; }
        /// <summary>
        /// 初始销量
        /// </summary>
        [Description("初始销量")]
        public int SalesInitial { get; set; }
        /// <summary>
        /// 实际销量
        /// </summary>
        [Description("实际销量")]
        public int SalesActual { get; set; }
        /// <summary>
        /// 一级分类ID
        /// </summary>
        [Description("一级分类ID")]
        [Browsable(false)]
        public string CateId { get; set; }
        /// <summary>
        /// 二级分类ID
        /// </summary>
        [Description("二级分类ID")]
        [Browsable(false)]
        public string Cate2Id { get; set; }
        /// <summary>
        /// 三级分类ID
        /// </summary>
        [Description("三级分类ID")]
        [Browsable(false)]
        public string Cate3Id { get; set; }
        /// <summary>
        /// 下架时间
        /// </summary>
        [Description("下架时间")]
        public System.DateTime? StopTime { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Description("供应商编码")]
        public string CompanyCode { get; set; }
        /// <summary>
        /// 品牌Id
        /// </summary>
        [Description("品牌Id")]
        [Browsable(false)]
        public string BrandId { get; set; }
        /// <summary>
        /// 排序编号
        /// </summary>
        [Description("排序编号")]
        public int SortNo { get; set; }
        /// <summary>
        /// 运费模板Id
        /// </summary>
        [Description("运费模板Id")]
        [Browsable(false)]
        public string DeliveryId { get; set; }
        /// <summary>
        /// 服务承诺Ids
        /// </summary>
        [Description("服务承诺Ids")]
        [Browsable(false)]
        public string ServiceIds { get; set; }
        /// <summary>
        /// 店铺Id
        /// </summary>
        [Description("店铺Id")]
        [Browsable(false)]
        public string StoreId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int IsDelete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public System.DateTime CreateTime { get; set; }
    }
}