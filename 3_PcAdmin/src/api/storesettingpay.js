import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/storeSettingPay/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/storeSettingPay/listByWhere',
        method: 'get',
        params
    })
}
export function getDetail(params) {
    return request({
        url: '/storeSettingPay/get',
        method: 'get',
        params
    })
}
export function setStatus(data) {
    return request({
        url: '/storeSettingPay/SetStatus',
        method: 'post',
        data
    })
}
export function setDefault(data) {
    return request({
        url: '/storeSettingPay/SetDefault',
        method: 'post',
        data
    })
}

export function addOrUpdate(data) {
    return request({
        url: '/storeSettingPay/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/storeSettingPay/delete',
        method: 'post',
        data
    })
}