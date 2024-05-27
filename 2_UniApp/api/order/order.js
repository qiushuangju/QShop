import request from '@/utils/request'

// api地址
const api = {
  getOrderGroupCount: 'order/getOrderGroupCount',
  load:'order/load',
  listByWhere: 'order/listByWhere',
  getDetail: 'order/GetDetail',
  express: 'order/express',
  cancelOrder: 'order/cancelOrder',
  applyCancelOrder: 'order/applyCancelOrder',
  receipt: 'order/receipt',
  payOrderBefore: 'order/payOrderBefore'
}

// 当前用户待处理的订单数量
export function getOrderGroupCount(param, option) {
  return request.get(api.getOrderGroupCount, param, option)
}

export function load(param, option) {
  return request.get(api.load, param, option)
}
// 我的订单列表
export function listByWhere(param, option) {
  return request.get(api.listByWhere, param, option)
}

// 订单详情
export function getDetail(id) {
  return request.get(api.getDetail, {id})
}

// 获取物流信息
export function express(orderId, param) {
  return request.get(api.express, { orderId, ...param })
}

// 取消订单
export function cancelOrder(orderId, data) {
  return request.post(api.cancelOrder, { orderId, ...data })
}
// 申请取消订单
export function applyCancelOrder(orderId, data) {
  return request.post(api.applyCancelOrder, { orderId, ...data })
}

// 确认收货
export function receipt(orderId, data) {
  return request.post(api.receipt, { orderId, ...data })
}

// 立即支付
export function payOrderBefore(orderId, payType, param) {
  return request.post(api.payOrderBefore, { orderId, payType, ...param })
}
