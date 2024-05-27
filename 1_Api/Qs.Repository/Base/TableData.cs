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
    public class TableData
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 操作消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 总记录条数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public dynamic Result { get; set; }

        public TableData()
        {
            Code = 200;
            Message = "加载成功";
        }
    }
}