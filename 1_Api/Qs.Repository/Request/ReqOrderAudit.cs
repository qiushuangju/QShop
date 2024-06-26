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
    /// 审核订单
    /// </summary>
    public partial class ReqOrderAudit
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }
                                             
     
        /// <summary>
        /// 参数验证
        /// </summary>
        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(OrderId,"订单Id")  ,
            });
          
        }
    }

}