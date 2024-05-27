import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/goods/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/goods/listByWhere',
        method: 'get',
        params
    })
}
export function getGoodsLabelList(params) {
    return request({
        url: '/Goods/GetGoodsLabelList',
        method: 'get',
        params
    })
}
export function get(params) {
    return request({
        url: '/Goods/Get',
        method: 'get',
        params
    })
}
export function addOrUpdate(data) {
    return request({
        url: '/goods/addOrUpdate',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/goods/delete',
        method: 'post',
        data
    })
}

export function setStatus(data) {
    return request({
        url: '/goods/setStatus',
        method: 'post',
        data
    })
}
export function setRecommend(data) {
    return request({
        url: '/goods/setRecommend',
        method: 'post',
        data
    })
}