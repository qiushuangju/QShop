import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/sysarea/load',
        method: 'get',
        params
    })
}
export function get(params) {
    return request({
        url: '/sysarea/get',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/sysarea/listByWhere',
        method: 'get',
        params
    })
}
export function addOrUpdate(data) {
    return request({
        url: '/sysarea/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/sysarea/delete',
        method: 'post',
        data
    })
}