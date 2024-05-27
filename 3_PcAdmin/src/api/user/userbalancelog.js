import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/userBalanceLog/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/userBalanceLog/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/userBalanceLog/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/userBalanceLog/delete',
    method: 'post',
    data
  })
}

