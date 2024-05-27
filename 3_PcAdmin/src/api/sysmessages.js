import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sysMessages/load',
    method: 'get',
    params
  })
}

export function read(data) {
  return request({
    url: '/sysMessages/read',
    method: 'post',
    data
  })
}
export function addOrUpdateNotice(data) {
  return request({
    url: '/sysMessages/addOrUpdateNotice',
    method: 'post',
    data
  })
}
export function del(data) {
  return request({
    url: '/sysMessages/delete',
    method: 'post',
    data
  })
}
export function get(params) {
  return request({
    url: '/sysMessages/get',
    method: 'get',
    params
  })
}


