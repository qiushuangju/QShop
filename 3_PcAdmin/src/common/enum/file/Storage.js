import Enum from '../enum'

/**
 * 枚举类：文件存储方式
 * StorageEnum
 */
export default new Enum([
    { key: 'LOCAL', name: '本地', value: 'local' },
    { key: 'QINIU', name: '七牛云', value: 'qiniu' },
    { key: 'ALIYUN', name: '阿里云', value: 'aliyun' },
    { key: 'QCLOUD', name: '腾讯云', value: 'qcloud' }
])
