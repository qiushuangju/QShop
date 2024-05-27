import Enum from '../enum'

/**
 * 枚举类：状态枚举
 * GeneralStatus
 */
export default new Enum([
  { key: 'USABLE', name: '可用', value: 10 },
  { key: 'DISABLE', name: '禁用', value: -10 },
])