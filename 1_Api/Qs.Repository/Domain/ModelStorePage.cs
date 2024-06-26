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
	/// 店铺自定义页面
	/// </summary>
    [Table("tb_StorePage")]
    public partial class ModelStorePage : StringEntity
    {
        public ModelStorePage()
        {
          this.PageName= string.Empty;
          this.PageData= string.Empty;
          this.StoreId= string.Empty;
          this.CreateTime= DateTime.Now;
          this.UpdateTime= DateTime.Now;
        }


        /// <summary>
        /// 页面类型(10:首页 20:活动页)
        /// </summary>
        [Description("页面类型(10:首页 20:活动页)")]
        public int? PageType { get; set; }
        /// <summary>
        /// 页面名称
        /// </summary>
        [Description("页面名称")]
        public string PageName { get; set; }
        /// <summary>
        /// 页面数据
        /// </summary>
        [Description("页面数据")]
        public string PageData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        [Browsable(false)]
        public string StoreId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public System.DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        public System.DateTime? UpdateTime { get; set; }
    }
}