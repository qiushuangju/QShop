import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/storeSetting/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/storeSetting/listByWhere',
        method: 'get',
        params
    })
}
export function GetDetail(params) {
    return request({
        url: '/storeSetting/GetDetail',
        method: 'get',
        params
    })
}

export function addOrUpdate(data) {
    return request({
        url: '/storeSetting/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/storeSetting/delete',
        method: 'post',
        data
    })
}