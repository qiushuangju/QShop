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
using Qs.Comm;
using Qs.Repository.Core;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 
    /// </summary>          
    public partial class ReqAuGoods
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>

        public string StoreId { get; set; }

        /// <summary>
        /// 商品规格(10单规格 20多规格) 
        /// </summary>
        public int SpecType { get; set; }

       

        /// <summary>
        /// 是否开启会员折扣(1开启 0关闭)
        /// </summary>
        public int IsEnableGrade { get; set; }

        /// <summary>
        /// 会员折扣设置(0默认等级折扣 1单独设置折扣)
        /// </summary>
        public int IsAloneGrade { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 主图Id
        /// </summary>
        public List<string> ImagesIds { get; set; }

        /// <summary>
        /// 商品详情图片Id
        /// </summary>
        public List<string> ListImgDetail { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string GoodsNo { get; set; }

        /// <summary>
        /// 配送模板ID
        /// </summary>
        public string DeliveryId { get; set; }

        /// <summary>
        ///服务承诺Ids
        /// </summary>
        public List<string> ListServiceId { get; set; }

        /// <summary>
        /// 商品状态(10上架 20下架)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 排序(数字越小越靠前)
        /// </summary>
        public int Sort { get; set; }

        

        /// <summary>
        /// 重量(kg)
        /// </summary>
        public decimal GoodsWeight { get; set; }

        /// <summary>
        /// 库存计算方式(10下单减库存 20付款减库存) 
        /// </summary>
        public int DeductStockType { get; set; }

        /// <summary>
        /// 采购价(最高)
        /// </summary>
        public decimal PurchasePrice { get; set; }


        /// <summary>
        /// 商品详情 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 商品卖点
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 初始销量
        /// </summary>
        public string SalesInitial { get; set; }

        /// <summary>
        /// 是否开启积分赠送(1开启 0关闭)
        /// </summary>
        public int IsPointsGift { get; set; }

        /// <summary>
        /// 是否允许使用积分抵扣(1允许 0不允许)
        /// </summary>
        public int IsPointsDiscount { get; set; }

        /// <summary>
        /// 单独设置折扣的配置
        /// </summary>
        public List<string> AloneGradeEquity { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 商品类型（10:销售商品 20:服务商品）
        /// </summary>
        public int GoodsType { get; set; }


        /// <summary>
        /// 规格数据
        /// </summary>
        public SpecData? SpecData { get; set; }

        /// <summary>
        /// 每层搬运费
        /// </summary>
        public decimal FloorPrice { get; set; }
        /// <summary>
        /// 入户费
        /// </summary>
        public decimal EntryPrice { get; set; }
        /// <summary>
        /// 开售时间
        /// </summary>
        public DateTime? SellStartTime { get; set; }

        public void Check()
        {
         
        }

    }
     /// <summary>
     /// 规格信息
     /// </summary>
    public class SpecData
    {
        /// <summary>
        /// 规格组
        /// </summary>
        public List<SpecModel> SpecList { get; set; }
        /// <summary>
        /// Sku数据
        /// </summary>
        public List<SkuModel> SkuList { get; set; }
    }
     /// <summary>
     /// Sku信息
     /// </summary>
    public class SkuModel
    {
        /// <summary>
        /// 图片Id
        /// </summary>
        public string ImageId { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal? SalePrice { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int StockNum { get; set; }
        /// <summary>
        /// 重量(kg)
        /// </summary>
        public decimal? GoodsWeight { get; set; }
        /// <summary>
        /// 划线价
        /// </summary>
        public decimal? LinePrice { get; set; }
        /// <summary>
        /// 一级提成
        /// </summary>
        public decimal? Commission1 { get; set; }
        /// <summary>
        /// 二级提成
        /// </summary>
        public decimal? Commission2 { get; set; }
        /// <summary>
        /// 三级提成
        /// </summary>
        public decimal? Commission3 { get; set; }
        /// <summary>
        /// 采购价
        /// </summary>
        public decimal? PurchasePrice { get; set; }
        /// <summary>
        /// 售后期限
        /// </summary>
        public int AfterSaleDeadline { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int StockTotal { get; set; }


        /// <summary>
        /// Sku编码
        /// </summary>
        public string GoodsSkuNo { get; set; }

        /// <summary>
        /// 第一个规格组规格值
        /// </summary>
        public string SpecValue0 { get; set; }

        /// <summary>
        /// 第二个规格组规格值
        /// </summary>
        public string SpecValue1 { get; set; }

        /// <summary>
        /// 第三个规格组规格值
        /// </summary>
        public string SpecValue2 { get; set; }
    }

    /// <summary>
    /// 规格组
    /// </summary>
    public class SpecModel
    {
        /// <summary>
        /// 规格组Id
        /// </summary>
        public string SpecId { get; set; }
        /// <summary>
        /// 规格组排序
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// 规格组名称
        /// </summary>
        public string SpecName { get; set; }

        /// <summary>
        /// 规格值
        /// </summary>
        public List<SpecValueModel> ValueList { get; set; }
    }

    /// <summary>
    /// 规格值
    /// </summary>
    public class SpecValueModel
    {
        /// <summary>
        /// 规格值Id
        /// </summary>
        public string SpecValueId { get; set; }
        /// <summary>
        /// 规格值排序
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// 规格组排序
        /// </summary>
        public int GroupKey { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string SpecValue { get; set; }
    }
}