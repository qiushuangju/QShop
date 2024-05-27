import request from '@/utils/request'

// api地址
const api = {
  list: 'order.comment/list',
  submit: 'order.comment/submit'
}

// 待评价订单商品列表
export const list = (orderId, param) => {
  return request.get(api.list, { orderId, ...param })
}

// 创建商品评价
export const submit = (orderId, data) => {
  return request.post(api.submit, { orderId, form: data })
}
