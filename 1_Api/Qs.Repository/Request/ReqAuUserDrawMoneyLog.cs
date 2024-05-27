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
	/// 用户余额变动明细表
	/// </summary>
    public partial class ReqAuUserDrawMoneyLog 
    {
        /// <summary>
        /// 银行卡id
        /// </summary>
        public string BankCardId { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Money { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        public void Check()
        {
            xValidation.CheckDecimal(Money, "提现金额");
            xValidation.CheckStrNull(BankCardId, "银行卡Id");
        }
    }

    /// <summary>
    /// 提现失败
    /// </summary>
    public partial class ReqDrawMoneyFailure
    {
        /// <summary>
        /// 
        /// </summary>
       public  List<string> Ids { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailureRemark { get; set; }

        public void Check()
        {
            xValidation.CheckListNull(Ids, "提现申请id");
            xValidation.CheckStrNull(FailureRemark, "失败原因");
        }
    }
}