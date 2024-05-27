import request from '@/utils/request'

export function getList(params) {
  return request({
    url: '/roles/load',
    method: 'get',
    params
  })
}

/**
* 是否忽略登录用户权限，直接获取全部数据
* 用于可以跨部门选择角色的场景
*/
export function loadAll(params) {
  return request({
    url: '/roles/loadall',
    method: 'get',
    params
  })
}

export function loadForUser(userId) {
  return request({
    url: '/roles/loadforuser',
    method: 'get',
    params: { userId: userId }
  })
}

export function add(data) {
  return request({
    url: '/roles/add',
    method: 'post',
    data
  })
}

export function update(data) {
  return request({
    url: '/roles/update',
    method: 'post',
    data
  })
}

export function del(data) {
  return request({
    url: '/roles/delete',
    method: 'post',
    data
  })
}
// 添加用户
export function AssignRoleUsers(data) {
  return request({
    url: '/AccessObjs/AssignRoleUsers',
    method: 'post',
    data
  })
}

