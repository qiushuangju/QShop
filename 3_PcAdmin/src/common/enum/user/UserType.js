import Enum from '../enum'

/**
 * 枚举类：用户类型类型
 * UserTypeEnum
 */
export default new Enum([
  { key: 'USER', name: '用户', value: 10 },
  { key: 'VIP', name: '会员', value: 15 },
  { key: 'BUSINESS', name: '业务', value: 20 },
  { key: 'DRIVER', name: '司机', value: 30 },
  { key: 'DISPATCH', name: '调度', value: 40 }, 
  { key: 'MANAGER', name: '管理员', value: 80 },
])
