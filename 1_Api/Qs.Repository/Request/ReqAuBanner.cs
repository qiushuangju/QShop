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
    public partial class ReqAuBanner
    {

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// 状态(-10:禁用 10:可用)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 店铺Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
    }
}