import Enum from '../enum'

/**
 * 枚举类：订单支付方式
 * PayTypeEnum
 */
export default new Enum([
  { key: 'ALL', name: '全部', value: 0 },
  { key: 'BALANCE', name: '余额支付', value: 10 },
  { key: 'WECHAT', name: '微信支付', value: 20 },
  { key: 'QUICKPASS', name: '云闪付', value: 30 },
  { key: 'FRIEND', name: '找人代付', value: 50 },
])
