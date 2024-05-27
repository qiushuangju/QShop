import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/goodsService/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/goodsService/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/goodsService/addOrUpdate',
    method: 'post',
    data
  })
}
export function changeStatus(data) {
  return request({
    url: '/goodsService/changeStatus',
    method: 'post',
    data
  })
}
export function changeDefault(data) {
  return request({
    url: '/goodsService/ChangeDefault',
    method: 'post',
    data
  })
}

export function del(data) {
  return request({
    url: '/goodsService/delete',
    method: 'post',
    data
  })
}

