import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/order/load',
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
// 订单改价
export function updatePrice(data) {
    return request({
        url: '/order/updatePrice',
        method: 'post',
        data
    })
}
// 申请取消-审核
export function auditCancel(data) {
    return request({
        url: '/order/AuditCancel',
        method: 'post',
        data
    })
}

// 发货
export function delivery(data) {
    return request({
        url: '/order/delivery',
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