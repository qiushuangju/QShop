using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 充值设置
    /// </summary>
    public class VmSettingRecharge
    {
        /// <summary>
        /// 是否开启用户充值
        /// </summary>
        public bool IsEntrance { get; set; } = true;
        /// <summary>
        /// 是否可以自定义金额
        /// </summary>
        public bool IsCustom { get; set; } = true;
        /// <summary>
        /// 最低充值金额
        /// </summary>
        public decimal LowestMoney { get; set; }
        /// <summary>
        /// 是否自动匹配套餐
        /// </summary>
        public bool IsMatchPlan { get; set; } = true;
        /// <summary>
        ///积分说明
        /// </summary>
        public string Describe { get; set; }=$"1. 账户充值仅限微信在线方式支付，充值金额实时到账；\n2. 账户充值套餐赠送的金额即时到账；\n3. 账户余额有效期：自充值日起至用完即止\n4. 若有其它疑问，可拨打客服电话400-000-1234";
    }

}
