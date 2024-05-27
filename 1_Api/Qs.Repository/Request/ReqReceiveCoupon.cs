using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 领取优惠券
    /// </summary>
    public class ReqReceiveCoupon
    {
        /// <summary>
        /// 优惠券ID
        /// </summary>
        public string CouponId { get; set; }
    }
}
