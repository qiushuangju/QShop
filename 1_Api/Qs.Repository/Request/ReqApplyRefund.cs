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
    /// 获取支付参数
    /// </summary>
    public partial class ReqApplyRefund
    {  
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 退货类型(10:仅退款 20:退货退款)
        /// </summary>
        public int RefundType { get; set; }
        /// <summary>
        /// 退货理由
        /// </summary>
        public string RefundReason { get; set; }

        /// <summary>
        /// 退货凭证
        /// </summary>
        public List<string> RefundProof { get; set; }



        /// <summary>
        /// 参数验证
        /// </summary>
        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(OrderId,"订单Id")  ,
                new ValueTip(RefundReason,"退货理由")
            });
            if (xConv.ToInt(RefundType)!=0)
            {
                throw new Exception("退货类型不能为空");
            }
        }
    }
}