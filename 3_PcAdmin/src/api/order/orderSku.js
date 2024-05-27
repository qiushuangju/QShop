import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/orderSku/load',
        method: 'get',
        params
    })
}


export function getDetail(params) {
    return request({
        url: '/order/getDetail',
        method: 'get',
        params
    })
}
export function addOrUpdate(data) {
    return request({
        url: '/order/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/order/delete',
        method: 'post',
        data
    })
}