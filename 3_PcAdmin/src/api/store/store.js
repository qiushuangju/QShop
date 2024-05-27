import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/store/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/store/listByWhere',
        method: 'get',
        params
    })
}
// 修改商城(平台管理员)
export function updateStoreInfo(data) {
    return request({
        url: '/store/UpdateStoreInfo',
        method: 'post',
        data
    })
}
// 修改商城(店铺自己)
export function updateStore(data) {
    return request({
        url: '/store/UpdateStore',
        method: 'post',
        data
    })
}
export function addStore(data) {
    return request({
        url: '/store/addStore',
        method: 'post',
        data
    })
}

export function get(params) {
    return request({
        url: '/store/get',
        method: 'get',
        params
    })
}


export function del(data) {
    return request({
        url: '/store/delete',
        method: 'post',
        data
    })
}