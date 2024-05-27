import request from '@/utils/request'

// api地址
const api = {
  getOrderSettlement: 'order/GetOrderSettlement',
  createOrder: 'order/CreateOrder',
}

// mode: 结算模式 (buyNow立即购买 cart购物车)

// 结算台订单信息
export const getOrderSettlement = (mode, param) => {
  return request.get(api.getOrderSettlement, { mode, ...param })
}

// 结算台订单提交
export const createOrder = (mode, data) => {
  return request.post(api.createOrder, { mode, ...data })
}
