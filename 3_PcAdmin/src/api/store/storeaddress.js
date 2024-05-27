import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/storeAddress/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/storeAddress/listByWhere',
        method: 'get',
        params
    })
}
export function get(params) {
    return request({
        url: '/storeAddress/get',
        method: 'get',
        params
    })
}
export function addOrUpdate(data) {
    return request({
        url: '/storeAddress/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/storeAddress/delete',
        method: 'post',
        data
    })
}