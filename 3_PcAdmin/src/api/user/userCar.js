import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/userCar/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/userCar/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/userCar/addOrUpdate',
    method: 'post',
    data
  })
}
