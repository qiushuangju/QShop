import Enum from '../../enum'

/**
 * 枚举类：短信发送的场景
 * SettingSmsSceneEnum
 */
export default new Enum([
  { key: 'CAPTCHA', name: '短信验证码', value: 'captcha' },
  { key: 'ORDER_PAY', name: '新付款订单', value: 'order_pay' }
])
