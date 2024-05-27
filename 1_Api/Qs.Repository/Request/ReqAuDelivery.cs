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
using Qs.Repository.Domain;
using TencentCloud.Ecm.V20190719.Models;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ReqAuDelivery
    {

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 计费方式(10按件数 20按重量)
        /// </summary>
        public int Method { get; set; }

        /// <summary>
        /// 排序方式(数字越小越靠前)
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 配送规则
        /// </summary>
        public List<ReqDeliveryRule> ListRule { get; set; }


    }

    public class ReqDeliveryRule
    {
        public string Id { get; set; }
        /// <summary>
        /// 城市Id
        /// </summary>
        public List<string> Region { get; set; }
        /// <summary>
        /// 可配送区域
        /// </summary>
        public List<ProvinceDelivery> RegionText { get; set; }

        /// <summary>
        /// 首件(个)/首重(Kg)
        /// </summary>
        public decimal First { get; set; }

        /// <summary>
        ///运费(元)
        /// </summary>
        public decimal FirstFee { get; set; }

        /// <summary>
        /// 续件(个)/续重(Kg)
        /// </summary>
        public decimal Additional { get; set; }

        /// <summary>
        /// 续费(元)
        /// </summary>
        public decimal AdditionalFee { get; set; }




    }

    public class ProvinceDelivery
    {
           public string Name { get; set; }

           /// <summary>
           /// 城市
           /// </summary>
           public List<CityDelivery> Chilren { get; set; }
    }

    public class CityDelivery
    {
        public string Name { get; set; }
    }
}