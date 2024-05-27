import request from '@/utils/request'

// api地址
const api = {
  load: 'OrderRefundSku/load',
  apply: 'OrderRefundSku/apply',
  getRefundOrderDetail: 'OrderRefundSku/getRefundOrderDetail',
  refundDelivery: 'OrderRefundSku/refundDelivery'
}

// 售后单列表
export const load = (param, option) => {
  return request.get(api.load, param, option)
}

// 申请售后
export const apply = ( data) => {
  return request.post(api.apply, data)
}

// 售后单详情
export const getRefundOrderDetail = (orderRefundSkuId, param) => {
  return request.get(api.getRefundOrderDetail, { orderRefundSkuId, ...param })
}

// 用户发货
export const refundDelivery = ( data) => {
  return request.post(api.refundDelivery, data)
}
