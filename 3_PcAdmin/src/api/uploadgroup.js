import request from '@/utils/request'

export function getList(params) {
    return request({
        url: '/fileGroup/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/fileGroup/listByWhere',
        method: 'get',
        params
    })
}

export function addOrUpdate(data) {
    return request({
        url: '/fileGroup/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/fileGroup/delete',
        method: 'post',
        data
    })
}