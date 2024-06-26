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
	/// 售后订单退货地址
	/// </summary>
    [Table("tb_OrderRefundAddress")]
    public partial class ModelOrderRefundAddress : StringEntity
    { 
        public ModelOrderRefundAddress()
        {
          this.OrderRefundId= string.Empty;
          this.StoreAddressId= string.Empty;
          this.Name= string.Empty;
          this.Phone= string.Empty;
          this.Province= string.Empty;
          this.City= string.Empty;
          this.Region= string.Empty;
          this.Detail= string.Empty;
          this.ProvinceId= string.Empty;
          this.CityId= string.Empty;
          this.RegionId= string.Empty;
          this.FullAddress= string.Empty;
          this.UserId= string.Empty;
          this.StoreId= string.Empty;
          this.CreateTime= DateTime.Now;
        }


        
        /// <summary>
        /// 售后订单Id
        /// </summary>
        [Description("售后订单Id")]
        [Browsable(false)]
        public string OrderRefundId { get; set; }
        /// <summary>
        /// 退货地址Id
        /// </summary>
        [Description("退货地址Id")]
        [Browsable(false)]
        public string StoreAddressId { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Description("联系人")]
        public string Name { get; set; }
        /// <summary>
        /// 联系号码
        /// </summary>
        [Description("联系号码")]
        public string Phone { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        [Description("省")]
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        [Description("市")]
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        [Description("区")]
        public string Region { get; set; }
        /// <summary>
        /// 门牌号
        /// </summary>
        [Description("门牌号")]
        public string Detail { get; set; }
        /// <summary>
        /// 省Id
        /// </summary>
        [Description("省Id")]
        [Browsable(false)]
        public string ProvinceId { get; set; }
        /// <summary>
        /// 市Id
        /// </summary>
        [Description("市Id")]
        [Browsable(false)]
        public string CityId { get; set; }
        /// <summary>
        /// 区Id
        /// </summary>
        [Description("区Id")]
        [Browsable(false)]
        public string RegionId { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        [Description("详细地址")]
        public string FullAddress { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Browsable(false)]
        public string UserId { get; set; }
        /// <summary>
        /// 商铺Id
        /// </summary>
        [Description("商铺Id")]
        [Browsable(false)]
        public string StoreId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public System.DateTime CreateTime { get; set; }
    }
}