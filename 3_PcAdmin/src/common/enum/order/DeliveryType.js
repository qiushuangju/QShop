import Enum from '../enum'

/**
 * 枚举类：配送方式
 * DeliveryTypeEnum
 */
export default new Enum([
  { key: 'DELIVERY', name: '平台配送', value: 10 },
  { key: 'TAKESELF', name: '自提', value: 20 }
])
