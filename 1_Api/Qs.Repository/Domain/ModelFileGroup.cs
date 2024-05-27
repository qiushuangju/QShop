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
	/// 
	/// </summary>
    [Table("FileGroup")]
    public partial class ModelFileGroup : StringEntity
    {
        public ModelFileGroup()
        {
          this.GroupName= string.Empty;
          this.Count= 0;
          this.SortNo= 0;
          this.Uid= string.Empty;
          this.CreateTime= DateTime.Now;
          this.IsDelete= 0;
        }

        
        /// <summary>
        /// 分组名称
        /// </summary>
        [Description("分组名称")]
        public string GroupName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Description("数量")]
        public int Count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int SortNo { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public string Uid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public System.DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        [Description("是否已删除")]
        public int IsDelete { get; set; }
    }
}