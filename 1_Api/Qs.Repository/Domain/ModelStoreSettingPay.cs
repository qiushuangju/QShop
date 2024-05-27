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
	/// 店铺支付设置
	/// </summary>
    [Table("tb_StoreSettingPay")]
    public partial class ModelStoreSettingPay : StringEntity
    { 
        public ModelStoreSettingPay()
        {
          this.Code= string.Empty;
          this.Name= string.Empty;
          this.Status= 0;
          this.IsDefault= false;
          this.SortNo= 0;
          this.Values= string.Empty;
          this.StoreId= string.Empty;
          this.CreateTime= DateTime.Now;
        }


        
        /// <summary>
        /// 编号
        /// </summary>
        [Description("编号")]
        public string Code { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [Description("名字")]
        public string Name { get; set; }
        /// <summary>
        /// 状态(-10:禁用 10:正常)
        /// </summary>
        [Description("状态(-10:禁用 10:正常)")]
        public int? Status { get; set; }
        /// <summary>
        /// 是否默认支付方式
        /// </summary>
        [Description("是否默认支付方式")]
        public bool? IsDefault { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int? SortNo { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [Description("值")]
        public string Values { get; set; }
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