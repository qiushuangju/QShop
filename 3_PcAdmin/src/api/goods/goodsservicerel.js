import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/goodsServiceRel/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/goodsServiceRel/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/goodsServiceRel/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/goodsServiceRel/delete',
    method: 'post',
    data
  })
}

