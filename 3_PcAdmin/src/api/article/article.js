import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/article/load',
    method: 'get',
    params
  })
}
export function get(params) {
  return request({
    url: '/article/get',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/article/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/article/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/article/delete',
    method: 'post',
    data
  })
}

