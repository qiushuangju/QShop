// ***********************************************************************
// Assembly         : FundationAdmin
// Author           : Qs
// Created          : 03-09-2016
//
// Last Modified By : Qs
// Last Modified On : 03-09-2016
// ***********************************************************************
// <copyright file="TableData.cs" company="Microsoft">
//     版权所有(C) Microsoft 2015
// </copyright>
// <summary>layui datatable数据返回</summary>
// ***********************************************************************

using System.Collections.Generic;
using Qs.Comm;

namespace Qs.Repository.Base
{
    /// <summary>
    /// table的返回数据
    /// </summary>
    public class TableDataOrder   : TableData
    {
        /// <summary>
        /// 订单数
        /// </summary>
        public int CountOrder { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal SumOrderMoney { get; set; }
    }
}