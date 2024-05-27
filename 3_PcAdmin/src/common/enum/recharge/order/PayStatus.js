import Enum from '../../enum'

/**
 * 枚举类：用户充值订单--支付状态
 * PayStatusEnum
 */
export default new Enum([
  { key: 'PENDING', name: '待支付', value: 10 },
  { key: 'SUCCESS', name: '支付成功', value: 20 }
])
