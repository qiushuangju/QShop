import request from '@/utils/request'
export function ddlMerType(params) {
    return request({
        url: '/Comm/DdlMerType',
        method: 'get',
        params
    })
}
export function ListEnum(params) {
    return request({
        url: '/Comm/ListEnum',
        method: 'get',
        params
    })
}
export function ddlGoodsLabel(params) {
    return request({
        url: '/Comm/DdlGoodsLabel',
        method: 'get',
        params
    })
}

export function listExpress(params) {
    return request({
        url: '/Comm/listExpress',
        method: 'get',
        params
    })
}
export function getPcHomeData(params) {
    return request({
        url: '/Comm/GetPcHomeData',
        method: 'get',
        params
    })
}