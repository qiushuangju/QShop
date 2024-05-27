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
    [Table("tb_DeliveryRule")]
    public partial class ModelDeliveryRule : StringEntity
    { 
        public ModelDeliveryRule()
        {
          this.DeliveryId= string.Empty;
          this.Region= string.Empty;
          this.RegionText= string.Empty;
          this.First= 0;
          this.FirstFee= 0;
          this.Additional= 0;
          this.AdditionalFee= 0;
          this.StoreId= string.Empty;
          this.CreateTime= DateTime.Now;
        }


        
        /// <summary>
        /// 配送模板ID
        /// </summary>
        [Description("运费模板ID")]
        [Browsable(false)]
        public string DeliveryId { get; set; }
        /// <summary>
        /// 可配送区域(城市id集)
        /// </summary>
        [Description("可配送区域(城市id集)")]
        public string Region { get; set; }
        /// <summary>
        /// 可配送区域(文字展示)
        /// </summary>
        [Description("可配送区域(文字展示)")]
        public string RegionText { get; set; }
        /// <summary>
        /// 首件(个)/首重(Kg)
        /// </summary>
        [Description("首件(个)/首重(Kg)")]
        public decimal First { get; set; }
        /// <summary>
        /// 首个/首重运费(元)
        /// </summary>
        [Description("首个/首重运费(元)")]
        public decimal FirstFee { get; set; }
        /// <summary>
        /// 续件/续重
        /// </summary>
        [Description("续件/续重")]
        public decimal Additional { get; set; }
        /// <summary>
        /// 续费(元)
        /// </summary>
        [Description("续费(元)")]
        public decimal AdditionalFee { get; set; }
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