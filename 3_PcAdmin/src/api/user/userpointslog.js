import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/userPointsLog/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/userPointsLog/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/userPointsLog/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/userPointsLog/delete',
    method: 'post',
    data
  })
}

