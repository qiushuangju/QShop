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
	/// 用户余额变动明细表
	/// </summary>
    [Table("tb_UserBalanceLog")]
    public partial class ModelUserBalanceLog : StringEntity
    {
        public ModelUserBalanceLog()
        {
          this.UserId= string.Empty;
          this.OrderId= string.Empty;
          this.Scene= 0;
          this.Money= 0;
          this.Describe= string.Empty;
          this.Remark= string.Empty;
          this.StoreId= string.Empty;
          this.CreateTime= DateTime.Now;
        }

        
        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Browsable(false)]
        public string UserId { get; set; }
        /// <summary>
        /// 订单ID(可选)
        /// </summary>
        [Description("订单Id(可选)")]
        [Browsable(false)]
        public string OrderId { get; set; }
        /// <summary>
        /// 余额变动场景(10用户充值 20用户消费 30管理员操作 40订单退款)
        /// </summary>
        [Description("余额变动场景(10用户充值 20用户消费 30管理员操作 40订单退款)")]
        public int Scene { get; set; }
        /// <summary>
        /// 变动金额
        /// </summary>
        [Description("变动金额")]
        public decimal Money { get; set; }
        /// <summary>
        /// 操作后余额
        /// </summary>
        [Description("操作后余额")]
        public decimal? Balance { get; set; }
        /// <summary>
        /// 描述/说明
        /// </summary>
        [Description("描述/说明")]
        public string Describe { get; set; }
        /// <summary>
        /// 管理员备注
        /// </summary>
        [Description("管理员备注")]
        public string Remark { get; set; }
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