import Enum from '../../enum'

/**
 * 枚举类：售后单状态
 * RefundStatusEnum
 */
export default new Enum([
  { key: 'NORMAL', name: '进行中', value: 0 },
  { key: 'REJECTED', name: '已拒绝', value: 10 },
  { key: 'COMPLETED', name: '已完成', value: 20 },
  { key: 'CANCELLED', name: '已取消', value: 30 }
])
