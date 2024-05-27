import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/goodsSku/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/goodsSku/listByWhere',
        method: 'get',
        params
    })
}
export function addOrUpdate(data) {
    return request({
        url: '/goodsSku/addOrUpdate',
        method: 'post',
        data
    })
}
export function changePrice(data) {
    return request({
        url: '/goodsSku/ChangePrice',
        method: 'post',
        data
    })
}

export function del(data) {
    return request({
        url: '/goodsSku/delete',
        method: 'post',
        data
    })
}
export function updatePurchasePrice(data) {
    return request({
        url: '/goodsSku/updatePurchasePrice',
        method: 'post',
        data
    })
}