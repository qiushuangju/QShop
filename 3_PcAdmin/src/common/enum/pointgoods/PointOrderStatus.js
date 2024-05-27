import Enum from '../enum'

/**
 * 枚举类：积分商品订单状态
 * PointOrderStatus
 * 10:未发货 20:待收货 50:已完成
 */
export default new Enum([
  { key: 'UNFILLED', name: '未发货', value: 10 },
  { key: 'WAITRECEIVING', name: '待收货', value: 20 },
  { key: 'DONE', name: '已完成', value: 50 },
])