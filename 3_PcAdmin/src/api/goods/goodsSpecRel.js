import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/goodsSpecRel/load',
        method: 'get',
        params
    })
}
export function listByWhere(data) {
    return request({
        url: '/goodsSpecRel/listByWhere',
        method: 'get',
        data
    })
}
export function addOrUpdate(data) {
    return request({
        url: '/goodsSpecRel/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/goodsSpecRel/delete',
        method: 'post',
        data
    })
}