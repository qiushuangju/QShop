﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a CodeSmith Template.
//
//     DO NOT MODIFY contents of this file. Changes to this
//     file will be lost if the code is regenerated.
//     Author:Yubao Li
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Qs.Repository.Core;

namespace Qs.Repository.Domain
{
    /// <summary>
	/// 分类类型
	/// </summary>
      [Table("CategoryType")]
    public partial class CategoryType : StringEntity
    {
        public CategoryType()
        {
          this.Name= string.Empty;
          this.CreateTime= DateTime.Now;
        }

        /// <summary>
	    /// 名称
	    /// </summary>
         [Description("名称")]
        public string Name { get; set; }
        /// <summary>
	    /// 创建时间
	    /// </summary>
         [Description("创建时间")]
        public System.DateTime CreateTime { get; set; }

    }
}