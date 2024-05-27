import request from '@/utils/request'

// api地址
const api = {
  listByWhere : 'coupon/listByWhere',
  receiveCoupon:'coupon/ReceiveCoupon'
}

// 优惠券列表
export const listByWhere  = (param, option) => {
  const options = {
    isPrompt: true, //（默认 true 说明：本接口抛出的错误是否提示）
    load: true, //（默认 true 说明：本接口是否提示加载动画）
    ...option
  }
  return request.get(api.listByWhere, param, options)
}
// 优惠券列表
export const receiveCoupon  = (param, option) => {
  return request.post(api.receiveCoupon, param)
}

