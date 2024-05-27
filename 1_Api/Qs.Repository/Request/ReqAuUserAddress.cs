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
    /// 用户收货地址表
    /// </summary>
    [Table("tb_UserAddress")]
    public partial class ReqAuUserAddress
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        ///经度
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        ///纬度
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// 省市区
        /// </summary>
        public List<ReqRegion> ListRegion { get; set; }

        public void Check()
        {

            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(Name, "联系人"),
                // new ValueTip(StrProvince, "省市区"),
                new ValueTip(Detail, "详细地址"),

            });
            xValidation.CheckListNull(ListRegion, "省市区");
            xValidation.CheckPhone(Phone);
        }



    }

    /// <summary>
    /// 省市区
    /// </summary>
    public class ReqRegion
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Label { get; set; }
    }
}