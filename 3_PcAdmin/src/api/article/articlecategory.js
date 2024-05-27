import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/articleCategory/load',
    method: 'get',
    params
  })
}
export function get(params) {
  return request({
    url: '/articleCategory/get',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/articleCategory/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/articleCategory/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/articleCategory/delete',
    method: 'post',
    data
  })
}

