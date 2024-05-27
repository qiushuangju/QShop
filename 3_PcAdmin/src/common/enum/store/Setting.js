import Enum from '../enum'

/**
 * 枚举类：商城设置项
 * SettingEnum
 */
export default new Enum([
    { key: 'DELIVERY', name: '配送设置', value: 'delivery' },
    { key: 'TRADE', name: '交易设置', value: 'trade' },
    { key: 'STORAGE', name: '上传设置', value: 'storage' },
    { key: 'FULL_FREE', name: '满额包邮设置', value: 'full_free' },
    { key: 'RECHARGE', name: '充值设置', value: 'recharge' },
    { key: 'Points', name: '积分设置', value: 'points' },
    { key: 'PageCategoryTemplate', name: '分类页模板', value: 'pageCategoryTemplate' }
])