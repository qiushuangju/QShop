import Enum from '../enum'

/**
 * 枚举类：订单支付方式
 * PayTypeEnum
 */
export default new Enum([
  { key: 'WeChat', name: '微信支付', value: 10 },
  { key: 'Balance', name: '余额支付', value: 20 }
])
