import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/storeExpress/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/storeExpress/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/storeExpress/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/storeExpress/delete',
    method: 'post',
    data
  })
}

