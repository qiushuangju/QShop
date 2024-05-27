import request from '@/utils/request'

// api地址
const api = {
  listByWhere: 'orderSku/listByWhere',
  getDetail: 'orderSku/getDetail',
}

export function listByWhere(param, option) {
  return request.get(api.listByWhere, param, option)
}

// 订单商品详情
export function getDetail(id) {
  return request.get(api.getDetail, {id})
}
