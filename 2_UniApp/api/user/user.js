import request from '@/utils/request'

// api地址
const api = {
  getDetail: 'user/get',
  assets: 'user/assets',
  bindMobile: 'user/bindMobile',
  personal: 'user/personal'
}

// 当前登录的用户信息
export const getDetail = (param, option) => {
  const options = {
    isPrompt: true, //（默认 true 说明：本接口抛出的错误是否提示）
    load: true, //（默认 true 说明：本接口是否提示加载动画）
    ...option
  }
  return request.get(api.getDetail, param, options)
}

// 账户资产
export const assets = (param, option) => {
  return request.get(api.assets, param, option)
}

// 绑定手机号
export const bindMobile = (data, option) => {
  return request.post(api.bindMobile, data, option)
}

// 修改个人信息（头像昵称）
export const personal = (data, option) => {
  return request.post(api.personal, data, option)
}
