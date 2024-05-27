import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/OrderRefundSku/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/OrderRefundSku/listByWhere',
        method: 'get',
        params
    })
}
export function getDetail(params) {
    return request({
        url: '/OrderRefundSku/getDetail',
        method: 'get',
        params
    })
}
export function audit(data) {
    return request({
        url: '/OrderRefundSku/audit',
        method: 'post',
        data
    })
}
export function refundMoney(data) {
    return request({
        url: '/OrderRefundSku/refundMoney',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/OrderRefundSku/delete',
        method: 'post',
        data
    })
}