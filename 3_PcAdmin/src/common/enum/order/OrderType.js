import Enum from '../enum'

/**
 * 枚举类：订单类型
 * OrderTypeEnum
 * 10:常规订单 20:拍照下单
 */
export default new Enum([
  { key: 'NORMAL', name: '常规订单', value: 10 },
  { key: 'PHOTOORDER', name: '拍照下单', value: 20 },
])
