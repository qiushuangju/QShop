import Enum from '../enum'

/**
 * 枚举类：订单状态
 * OrderStatusEnum
 */
export default new Enum([
  { key: 'Cancel', name: '已取消', value: -30 },
  { key: 'NotPaid', name: '待支付', value: 10 },
  { key: 'WaitDeliver', name: '待发货', value: 20 },
  { key: 'WaitReceiving', name: '待收货', value: 30 },
  { key: 'Done', name: '已完成', value: 80 }
])
