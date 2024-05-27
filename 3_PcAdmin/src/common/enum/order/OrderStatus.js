import Enum from '../enum'

/**
 * 枚举类：订单状态
 * OrderStatusEnum
 */
export default new Enum([
  { key: 'ALL', name: '全部', value: 0 },
  { key: 'AFTERSALE', name: '售后', value: -50 },
  { key: 'NOTCONFIRM', name: '未确认', value: -10 },
  { key: 'NOTAUDITED', name: '拍照订单-待处理', value: 10 },
  { key: 'ADDGOODS', name: '拍照订单-已处理', value: 12 }, 
  { key: 'NOTPAID', name: '待付款', value: 20 },
  { key: 'PAID', name: '待发货', value: 30 },
  { key: 'DISTRIBUTION', name: '配送中', value: 30 },
  { key: 'DELIVERED', name: '已送达', value: 80 },
])
