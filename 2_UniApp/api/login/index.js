import request from '@/utils/request'

// api地址
const api = {
  loginByPhoneCode: 'Check/LoginByPhoneCode',
  loginWx: 'Check/LoginWx',
  bindWxPhone: 'Check/BindWxPhone',
  logout: 'Check/logout',
}

// 用户登录(手机号+验证码)
export function loginByPhoneCode(data) {
  return request.post(api.loginByPhoneCode, data)
}

// 微信小程序快捷登录(获取微信用户基本信息)
export function loginWx(data, option) {
  return request.post(api.loginWx, data, option)
}

// 微信小程序快捷登录(授权手机号)
export function bindWxPhone(data, option) {
	console.log("data",data);
	console.log("option",option)
  return request.post(api.bindWxPhone, data, option)
}

//退出登录
export function logout() {
  return request.post(api.logout)
}
