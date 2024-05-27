/**
 * 首页图标库
 */
import fCoupon from '@/assets/icons/home/functions/coupon.svg?inline'
import fGoods from '@/assets/icons/home/functions/goods.svg?inline'
import fOrder from '@/assets/icons/home/functions/order.svg?inline'
import fRefund from '@/assets/icons/home/functions/refund.svg?inline'
import fStatistics from '@/assets/icons/home/functions/statistics.svg?inline'
import fUser from '@/assets/icons/home/functions/user.svg?inline'
import fDecorate from '@/assets/icons/home/functions/decorate.svg?inline'

import oSale from '@/assets/icons/home/overview/sale.svg?inline'
import oIncrease from '@/assets/icons/home/overview/increase.svg?inline'

import sGoods from '@/assets/icons/home/statistics/goods.svg?inline'
import sOrder from '@/assets/icons/home/statistics/order.svg?inline'
import sUser from '@/assets/icons/home/statistics/user.svg?inline'
import sConsume from '@/assets/icons/home/statistics/consume.svg?inline'

// 常用功能
const functions = {
  coupon: fCoupon,
  goods: fGoods,
  order: fOrder,
  refund: fRefund,
  statistics: fStatistics,
  user: fUser,
  decorate: fDecorate
}

// 实时概况
const overview = { sale: oSale, increase: oIncrease }

// 统计
const statistics = { goods: sGoods, order: sOrder, user: sUser, consume: sConsume }

export { functions, overview, statistics }
