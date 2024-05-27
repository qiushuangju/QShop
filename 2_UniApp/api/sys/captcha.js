import request from '@/utils/request'

// api地址
const api = {
  imageCaptcha: 'check/getImageCaptcha',
  sendSmsCaptcha: 'check/sendSmsCaptcha'
}

// 图形验证码
export function getImageCaptcha() {
  return request.get(api.imageCaptcha, {}, { load: false })
}

// 发送短信验证码
export function sendSmsCaptcha(data) {
  return request.post(api.sendSmsCaptcha, data, { load: false })
}
