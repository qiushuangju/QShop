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
using Qs.Repository.Base;
using Qs.Repository.Core;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 创建订单
    /// </summary>
    public partial class ReqOrderSettlement : BaseReqAu
    {
        /// <summary>
        /// SkuID
        /// </summary>
        public string GoodsSkuId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int GoodsNum { get; set; } = 1;

        /// <summary>
        /// 购物车ID列表
        /// </summary>
        public string StrCartIds { get; set; }

        /// <summary>
        ///地址Id
        /// </summary>
        public string AddressId { get; set; }
        /// <summary>
        /// 用户优惠券Id
        /// </summary>
        public string UserCouponId { get; set; }
        /// <summary>
        /// 是否使用积分抵扣(10:使用 -10:不使用)
        /// </summary>
        public int IsUsePoints { get; set; }
        /// <summary>
        /// 支付方式(10:余额支付 20:微信支付)
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 参数验证
        /// </summary>
        public void Check()
        {

            if (string.IsNullOrEmpty(StrCartIds) && string.IsNullOrEmpty(GoodsSkuId))
            {
                throw new Exception($"购物车ID列表或SkuID不能为空");
            }
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                // new ValueTip(StoreId,"请在首页选择地区")
            } );
        }
    }
}