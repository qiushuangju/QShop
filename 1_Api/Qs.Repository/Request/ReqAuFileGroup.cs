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
	/// 
	/// </summary>
    [Table("FileGroup")]
    public partial class ReqAuFileGroup 
    {

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int? Count { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Uid { get; set; }
       
    }
}