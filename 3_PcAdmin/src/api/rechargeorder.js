import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/rechargeOrder/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/rechargeOrder/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/rechargeOrder/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/rechargeOrder/delete',
    method: 'post',
    data
  })
}

