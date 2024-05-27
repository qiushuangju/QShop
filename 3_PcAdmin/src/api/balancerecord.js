import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/balanceRecord/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/balanceRecord/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/balanceRecord/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/balanceRecord/delete',
    method: 'post',
    data
  })
}

