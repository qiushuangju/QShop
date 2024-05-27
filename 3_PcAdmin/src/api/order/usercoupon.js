import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/userCoupon/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/userCoupon/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/userCoupon/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/userCoupon/delete',
    method: 'post',
    data
  })
}

