import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/goodsComment/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/goodsComment/listByWhere',
        method: 'get',
        params
    })
}
export function setStatus(data) {
    return request({
        url: '/goodsComment/setStatus',
        method: 'post',
        data
    })
}

export function addOrUpdate(data) {
    return request({
        url: '/goodsComment/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/goodsComment/delete',
        method: 'post',
        data
    })
}