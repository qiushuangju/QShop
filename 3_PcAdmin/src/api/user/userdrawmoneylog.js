import request from '@/utils/request'

export function load(params) {
  return request({
    url: '/UserDrawmoneyLog/load',
    method: 'get',
    params
  })
}
export function getDetails(params) {
  return request({
    url: '/UserDrawmoneyLog/getDetails',
    method: 'get',
    params
  })
}

export function listByWhere(data) {
  return request({
    url: '/UserDrawmoneyLog/listByWhere',
    method: 'get',
    data
  })
}
export function dealWithDrawMoney(data) {
  return request({
    url: '/UserDrawmoneyLog/dealWithDrawMoney',
    method: 'post',
    data
  })
}
export function successDrawMoney(data) {
  return request({
    url: '/UserDrawmoneyLog/successDrawMoney',
    method: 'post',
    data
  })
}
export function failureDrawMoney(data) {
  return request({
    url: '/UserDrawmoneyLog/failureDrawMoney',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/UserDrawmoneyLog/delete',
    method: 'post',
    data
  })
}

