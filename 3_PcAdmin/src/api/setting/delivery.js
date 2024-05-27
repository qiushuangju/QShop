import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/delivery/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/delivery/listByWhere',
        method: 'get',
        params
    })
}

export function getDetail(params) {
    return request({
        url: '/delivery/getDetail',
        method: 'get',
        params
    })
}
export function addOrUpdate(data) {
    return request({
        url: '/delivery/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/delivery/delete',
        method: 'post',
        data
    })
}