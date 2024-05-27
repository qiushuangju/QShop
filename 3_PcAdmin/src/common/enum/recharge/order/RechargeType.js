import Enum from '../../enum'

/**
 * 枚举类：用户充值订单--充值方式
 * RechargeTypeEnum
 */
export default new Enum([
  { key: 'CUSTOM', name: '自定义金额', value: 10 },
  { key: 'PLAN', name: '套餐充值', value: 20 }
])
