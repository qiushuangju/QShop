import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/goodscate/load',
        method: 'get',
        params
    })
}
export function get(params) {
    return request({
        url: '/goodscate/get',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/goodscate/listByWhere',
        method: 'get',
        params
    })
}

export function addOrUpdate(data) {
    return request({
        url: '/goodscate/addOrUpdate',
        method: 'post',
        data
    })
}
export function changeStatus(data) {
    return request({
        url: '/goodscate/changeStatus',
        method: 'post',
        data
    })
}

export function del(data) {
    return request({
        url: '/goodscate/delete',
        method: 'post',
        data
    })
}