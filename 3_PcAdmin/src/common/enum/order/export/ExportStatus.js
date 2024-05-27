import Enum from '../../enum'

/**
 * 枚举类：订单导出状态
 * ExportStatusEnum
 */
export default new Enum([
  { key: 'NORMAL', name: '进行中', value: 10 },
  { key: 'COMPLETED', name: '已完成', value: 20 },
  { key: 'FAIL', name: '失败', value: 30 }
])
