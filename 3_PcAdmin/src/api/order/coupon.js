import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/coupon/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/coupon/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/coupon/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/coupon/delete',
    method: 'post',
    data
  })
}

