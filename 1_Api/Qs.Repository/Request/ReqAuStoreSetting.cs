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
	/// 商家设置记录表
	/// </summary>
    [Table("tb_StoreSetting")]
    public partial class ReqAuStoreSetting 
    {

        /// <summary>
        ///商城设置Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 商城设置值
        /// </summary>
        public dynamic Data { get; set; }
    }
}