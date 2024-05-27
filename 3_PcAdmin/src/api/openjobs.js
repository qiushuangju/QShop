import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/openJobs/load',
    method: 'get',
    params
  })
}

export function loadForRole(roleId) {
  return request({
    url: '/openJobs/loadForRole',
    method: 'get',
    params: { appId: '', firstId: roleId }
  })
}

export function add(data) {
  return request({
    url: '/openJobs/add',
    method: 'post',
    data
  })
}

export function update(data) {
  return request({
    url: '/openJobs/update',
    method: 'post',
    data
  })
}

export function del(data) {
  return request({
    url: '/openJobs/delete',
    method: 'post',
    data
  })
}
export function changeStatus(data) {
  return request({
    url: '/openJobs/changeStatus',
    method: 'post',
    data
  })
}

// 获取任务地址
export function QueryLocalHandlers(data) {
  return request({
    url: '/OpenJobs/QueryLocalHandlers',
    method: 'post',
    data
  })
}
