import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/UserIncome/load',
    method: 'get',
    params
  })
}
export function getDetails(params) {
  return request({
    url: '/UserIncome/getDetails',
    method: 'get',
    params
  })
}

export function listByWhere(data) {
  return request({
    url: '/UserIncome/listByWhere',
    method: 'get',
    data
  })
}
export function addOrUpdate(data) {
  return request({
    url: '/UserIncome/addOrUpdate',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/UserIncome/delete',
    method: 'post',
    data
  })
}

