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
	/// 售后订单退货地址
	/// </summary>
    public partial class ReqAuOrderRefundAddress 
    {

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 售后订单Id
        /// </summary>
        public string OrderRefundId { get; set; }
        /// <summary>
        /// 退货地址Id
        /// </summary>
        public string StoreAddressId { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 门牌号
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 省Id
        /// </summary>
        public string ProvinceId { get; set; }
        /// <summary>
        /// 市Id
        /// </summary>
        public string CityId { get; set; }
        /// <summary>
        /// 区Id
        /// </summary>
        public string RegionId { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string FullAddress { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 商铺Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateTime { get; set; }
    }
}