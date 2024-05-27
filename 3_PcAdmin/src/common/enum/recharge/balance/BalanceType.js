import Enum from '../../enum'

/**
 * 枚举类：余额变动场景(10:用户充值 20:用户消费 30:管理员操作 40:订单退款) 
 * BalanceTypeEnum
 */
export default new Enum([
  { key: 'All', name: '全部', value: 0 },
  { key: 'RECHARGE', name: '用户充值', value: 10 },
  { key: 'CONSUME', name: '用户消费', value: 20 },
  { key: 'ADMINOP', name: '管理员操作', value: 30 },
  { key: 'REFUND', name: '订单退款', value: 40 },
])