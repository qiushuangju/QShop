import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/invitePoster/load',
    method: 'get',
    params
  })
}
export function get(params) {
  return request({
    url: '/invitePoster/get',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/invitePoster/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/invitePoster/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/invitePoster/delete',
    method: 'post',
    data
  })
}

