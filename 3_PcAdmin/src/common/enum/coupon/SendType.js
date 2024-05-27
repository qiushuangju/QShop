import Enum from '../enum'

/**
 * 枚举类：优惠券发放方式
 * SendTypeEnum
 */
export default new Enum([
    { key: 'UserGet', name: '用户领取', value: 10 },
    { key: 'SysSend', name: '系统方法', value: 20 }
])