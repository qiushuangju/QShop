using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 积分设置
    /// </summary>
    public class VmSettingPoints
    {
        /// <summary>
        /// 积分名称
        /// </summary>
        public string PointsName { get; set; } = "积分";
        /// <summary>
        /// 是否积分赠送
        /// </summary>
        public int IsShoppingGift { get; set; } = -10;
        /// <summary>
        /// 下单是否积分抵扣
        /// </summary>
        public int IsShoppingDiscount { get; set; } = -10;

        /// <summary>
        ///积分说明
        /// </summary>
        public string Describe { get; set; }=$"a) 积分不可兑现、不可转让,仅可在本平台使用;\nb) 您在本平台参加特定活动也可使用积分,详细使用规则以具体活动时的规则为准;\nc) 积分的数值精确到个位(小数点后全部舍弃,不进行四舍五入)\nd) 买家在完成该笔交易(订单状态为“已签收”)后才能得到此笔交易的相应积分,如购买商品参加店铺其他优惠,则优惠的金额部分不享受积分获取;";
       
        /// <summary>
        ///下单赠送比例
        /// </summary>
        public decimal GiftRatio { get; set; } = 100;

        /// <summary>
        /// 积分抵扣
        /// </summary>
        public DiscountInfo Discount{ get; set; }     =new DiscountInfo();
    }

    /// <summary>
    /// 积分抵扣
    /// </summary>
    public class DiscountInfo
    {
        /// <summary>
        /// 积分抵扣比例(1个积分抵扣金额)
        /// </summary>
        public decimal DiscountRatio { get; set; } = 0.01M;

        /// <summary>
        /// 订单做多抵扣金额
        /// </summary>
        public decimal MaxDiscountPrice { get; set; } = 10.0M;

        /// <summary>
        /// 最多抵扣比例
        /// </summary>
        public decimal MaxMoneyRatio { get; set; } = 10.0M;
    }
}
