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
    [Table("tb_GoodsSku")]
    public partial class ModelGoodsSku : StringEntity
    { 
        public ModelGoodsSku()
        {
          this.GoodsId= string.Empty;
          this.SkuName= string.Empty;
          this.SpecValueIds= string.Empty;
          this.ImageId= string.Empty;
          this.GoodsSkuNo= string.Empty;
          this.LinePrice= 0;
          this.SalePrice= 0;
          this.PurchasePrice= 0;
          this.Commission1= 0;
          this.Commission2= 0;
          this.Commission3= 0;
          this.StockStart= 0;
          this.StockNum= 0;
          this.GoodsWeight= 0;
          this.BuyMinNum= 0;
          this.BuyMaxNum= 0;
          this.SortNo= 0;
          this.DeliveryId= string.Empty;
          this.StoreId= string.Empty;
          this.CreateTime= DateTime.Now;
        }


        
        /// <summary>
        /// 商品Id
        /// </summary>
        [Description("商品Id")]
        [Browsable(false)]
        public string GoodsId { get; set; }
        /// <summary>
        /// Sku名称
        /// </summary>
        [Description("Sku名称")]
        public string SkuName { get; set; }
        /// <summary>
        /// 规格值ID集(json格式)
        /// </summary>
        [Description("规格值ID集(json格式)")]
        [Browsable(false)]
        public string SpecValueIds { get; set; }
        /// <summary>
        /// 规格图片Id
        /// </summary>
        [Description("规格图片Id")]
        [Browsable(false)]
        public string ImageId { get; set; }
        /// <summary>
        /// Sku编码
        /// </summary>
        [Description("Sku编码")]
        public string GoodsSkuNo { get; set; }
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
        /// 进价
        /// </summary>
        [Description("进价")]
        public decimal? PurchasePrice { get; set; }
        /// <summary>
        /// 一级提成
        /// </summary>
        [Description("一级提成")]
        public decimal? Commission1 { get; set; }
        /// <summary>
        /// 二级提成
        /// </summary>
        [Description("二级提成")]
        public decimal? Commission2 { get; set; }
        /// <summary>
        /// 三级提成
        /// </summary>
        [Description("三级提成")]
        public decimal? Commission3 { get; set; }
        /// <summary>
        /// 初始库存
        /// </summary>
        [Description("初始库存")]
        public int? StockStart { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        [Description("库存数量")]
        public int StockNum { get; set; }
        /// <summary>
        /// 商品重量(Kg)
        /// </summary>
        [Description("商品重量(Kg)")]
        public decimal GoodsWeight { get; set; }
        /// <summary>
        /// 最小购买量
        /// </summary>
        [Description("最小购买量")]
        public int? BuyMinNum { get; set; }
        /// <summary>
        /// 最大购买量
        /// </summary>
        [Description("最大购买量")]
        public int? BuyMaxNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int? SortNo { get; set; }
        /// <summary>
        /// 运费模板Id
        /// </summary>
        [Description("运费模板Id")]
        [Browsable(false)]
        public string DeliveryId { get; set; }
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