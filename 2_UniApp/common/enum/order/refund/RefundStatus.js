import Enum from '../../enum'

/**
 * 枚举类：售后单状态
 * RefundStatusEnum
 */
export default new Enum([
  { key: 'UnApprove', name: '已拒绝', value: -10 },
  { key: 'WaitAudit', name: '待审核', value: 10 },
  { key: 'Approve', name: '审核通过', value: 20 },  
  { key: 'Shipped', name: '用户已发货', value: 30 },
  { key: 'Received', name: '已收货', value: 40 },
  { key: 'Received', name: '已退款', value: 80 },
])
