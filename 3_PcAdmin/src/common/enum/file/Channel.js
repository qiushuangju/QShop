import Enum from '../enum'

/**
 * 枚举类：文件上传来源
 * ChannelEnum
 */
export default new Enum([
    { key: 'STORE', name: '商户后台', value: 10 },
    { key: 'CLIENT', name: '用户端', value: 20 },
    { key: 'ADMIN', name: '管理后端', value: 50 }
])