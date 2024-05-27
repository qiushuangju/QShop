using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Domain;
using Qs.Repository.Vm;

namespace Qs.Repository.Response
{
    public class ResOrderSettlement

    {
        /// <summary>
        /// 地址
        /// </summary>
        public ModelUserAddress Address { get; set; }

        /// <summary>
        /// 商品Sku列表
        /// </summary>
        public List<OrderSkuCreate> ListGoods { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public ResUser Personal { get; set; }

        /// <summary>
        ///订单
        /// </summary>
        public OrderInfoSettlement Order { get; set; }

        /// <summary>
        /// 优惠券
        /// </summary>
        public List<ResUserCoupon> ListCoupon { get; set; }

        /// <summary>
        /// 积分设置
        /// </summary>
        public VmSettingPoints Setting { get; set; }

    }

    /// <summary>
    /// 订单金额
    /// </summary>
    public class OrderInfoSettlement
    {
        /// <summary>
        /// 商品总数
        /// </summary>
        public int OrderTotalNum { get; set; }

        /// <summary>
        /// 商品总额
        /// </summary>
        public decimal? OrderTotalPrice { get; set; }
        /// <summary>
        /// 运费金额
        /// </summary>
        public decimal? FreightMoney { get; set; }
        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal? CouponMoney { get; set; }
        /// <summary>
        /// 积分可抵扣金额
        /// </summary>
        public decimal? UsablePointsMoney { get; set; }
        /// <summary>
        /// 积分抵扣金额
        /// </summary>
        public decimal? PointsMoney { get; set; }
        /// <summary>
        /// 实付款
        /// </summary>
        public decimal? OrderPayPrice { get; set; }

        /// <summary>
        /// 是否在配送范围
        /// </summary>
        public bool IsIntraRegion { get; set; } = true;

        /// <summary>
        /// 不配送商品
        /// </summary>
        public string UndeliveredGoods { get; set; }

    }

}
