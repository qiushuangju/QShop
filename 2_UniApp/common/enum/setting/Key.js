import Enum from '../enum'

/**
 * 枚举类：设置项索引
 * SettingKeyEnum
 */
export default new Enum([{
    key: 'register',
    name: '账户注册设置',
    value: 'register'
  },
  {
    key: 'pageCategoryTemplate',
    name: '分类页模板',
    value: 'pageCategoryTemplate'
  },
  {
    key: 'points',
    name: '积分设置',
    value: 'points'
  },
  {
    key: 'recharge',
    name: '充值设置',
    value: 'recharge'
  },
  {
    key: 'basicH5',
    name: 'H5站点设置',
    value: 'recharge'
  },{
    key: 'basicWxApp',
    name: '小程序设置',
    value: 'recharge'
  },{
    key: 'sms',
    name: '短信通知',
    value: 'recharge'
  }
])
