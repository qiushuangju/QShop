import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/sysLog/load',
    method: 'get',
    params
  })
}

export function loadForRole(roleId) {
  return request({
    url: '/sysLogs/loadForRole',
    method: 'get',
    params: { appId: '', firstId: roleId }
  })
}

export function add(data) {
  return request({
    url: '/sysLogs/add',
    method: 'post',
    data
  })
}

export function update(data) {
  return request({
    url: '/sysLogs/update',
    method: 'post',
    data
  })
}

export function del(data) {
  return request({
    url: '/sysLogs/delete',
    method: 'post',
    data
  })
}

