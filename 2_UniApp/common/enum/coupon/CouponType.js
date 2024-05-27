import Enum from '../enum'

/**
 * 枚举类：优惠券类型
 * CouponTypeEnum
 */
export default new Enum([
  { key: 'FullDisCount', name: '满减券', value: 10 },
  { key: 'DisCount', name: '折扣券', value: 20 }
])
