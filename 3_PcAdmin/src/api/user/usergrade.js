import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/userGrade/load',
    method: 'get',
    params
  })
}
export function listByWhere(data) {
  return request({
    url: '/userGrade/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/userGrade/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/userGrade/delete',
    method: 'post',
    data
  })
}

