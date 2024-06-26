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
	/// 店铺快递
	/// </summary>
    [Table("tb_StoreExpress")]
    public partial class ModelStoreExpress : StringEntity
    { 
        public ModelStoreExpress()
        {
          this.ExpressName= string.Empty;
          this.KuaiDi100Code= string.Empty;
          this.KuaiDiNiaoCode= string.Empty;
          this.SortNo= 0;
          this.StoreId= string.Empty;
          this.CreateTime= DateTime.Now;
        }


        
        /// <summary>
        /// 快递公司名
        /// </summary>
        [Description("快递公司名")]
        public string ExpressName { get; set; }
        /// <summary>
        /// 快递100Code
        /// </summary>
        [Description("快递100Code")]
        public string KuaiDi100Code { get; set; }
        /// <summary>
        /// 快递鸟Code
        /// </summary>
        [Description("快递鸟Code")]
        public string KuaiDiNiaoCode { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int? SortNo { get; set; }
        /// <summary>
        /// 店铺Id
        /// </summary>
        [Description("店铺Id")]
        [Browsable(false)]
        public string StoreId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public System.DateTime? CreateTime { get; set; }
    }
}