import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/rechargePlan/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/rechargePlan/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/rechargePlan/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/rechargePlan/delete',
    method: 'post',
    data
  })
}

