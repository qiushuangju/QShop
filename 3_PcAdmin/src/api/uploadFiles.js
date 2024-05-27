import request from '@/utils/request'


export function load(params) {
    return request({
        url: '/files/load',
        method: 'get',
        params
    })
}
export function getList(params) {
    return request({
        url: '/files/load',
        method: 'get',
        params
    })
}

export function updateGroup(data) {
    return request({
        url: '/uploadGroup/updateGroup',
        method: 'post',
        data
    })
  }

export function del(data) {
    return request({
        url: '/files/delete',
        method: 'post',
        data
    })
}