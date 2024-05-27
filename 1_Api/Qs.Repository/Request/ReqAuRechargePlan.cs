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
	/// 会员充值套餐表
	/// </summary>
    [Table("tb_RechargePlan")]
    public partial class ReqAuRechargePlan 
    {

        /// <summary>
        /// 套餐Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string PlanName { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 赠送金额
        /// </summary>
        public decimal GiftMoney { get; set; }
        /// <summary>
        /// 排序(数字越小越靠前)
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 店铺Id
        /// </summary>
        public string StoreId { get; set; }
    }
}