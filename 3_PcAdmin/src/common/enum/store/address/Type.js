import Enum from '../../enum'

/**
 * 枚举类：地址类型
 * AddressTypeEnum
 */
export default new Enum([
  { key: 'DELIVERY', name: '发货地址', value: 10 },
  { key: 'RETURN', name: '退货地址', value: 20 }
])
