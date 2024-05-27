import Enum from '../enum'

/**
 * 枚举类：文件存储方式
 * FileTypeEnum
 */
export default new Enum([
    { key: 'IMAGE', name: '图片', value: 10 },
    { key: 'ANNEX', name: '附件', value: 20 },
    { key: 'VIDEO', name: '视频', value: 30 }
])
