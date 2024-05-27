/**
 * 获取当前运行的客户端(APP H5 小程序)
 * https://uniapp.dcloud.io/platform
 */
const getPlatform = () => {
  // #ifdef APP-PLUS
  const platform = 'APP'
  // #endif
  // #ifdef APP-PLUS-NVUE
  const platform = 'APP'
  // #endif
  // #ifdef H5
  const platform = weixinOfficial() ? 'H5-WEIXIN' : 'H5'
  // #endif
  // #ifdef MP-WEIXIN
  const platform = 'MP-WEIXIN'
  // #endif
  // #ifdef MP-ALIPAY
  const platform = 'MP-ALIPAY'
  // #endif
  // #ifdef MP-BAIDU
  const platform = 'MP-BAIDU'
  // #endif
  // #ifdef MP-TOUTIAO
  const platform = 'MP-TOUTIAO'
  // #endif
  // #ifdef MP-LARK	
  const platform = 'MP-LARK'
  // #endif
  // #ifdef MP-QQ
  const platform = 'MP-QQ'
  // #endif
  // #ifdef MP-KUAISHOU	
  const platform = 'MP-KUAISHOU'
  // #endif
  // #ifdef MP-360
  const platform = 'MP-360'
  // #endif
  // #ifdef QUICKAPP-WEBVIEW
  const platform = 'QUICKAPP-WEBVIEW'
  // #endif
  return platform
}

// 是否为微信公众号端
const weixinOfficial = () => {
  // #ifdef H5
  const ua = window.navigator.userAgent.toLowerCase()
  return String(ua.match(/MicroMessenger/i)) === 'micromessenger'
  // #endif
  return false
}

const platfrom = getPlatform()

export const isH5 = platfrom === 'H5'
export const isApp = platfrom === 'APP'
export const isMpWeixin = platfrom === 'MP-WEIXIN'

// 是否为微信公众号端
// 相当于H5端运行在微信内置浏览器, 但是需要使用微信的jssdk所以要单独区分
export const isWeixinOfficial = platfrom === 'H5-WEIXIN'

export default platfrom
