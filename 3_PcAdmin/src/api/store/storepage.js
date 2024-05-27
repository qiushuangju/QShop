import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/storePage/load',
        method: 'get',
        params
    })
}
export function listByWhere(data) {
    return request({
        url: '/storePage/listByWhere',
        method: 'get',
        data
    })
}
export function addOrUpdate(data) {
    return request({
        url: '/storePage/addOrUpdate',
        method: 'post',
        data
    })
}
export function setHome(data) {
    return request({
        url: '/storePage/SetHome',
        method: 'post',
        data
    })
}
export function defaultData(data) {
    return request({
        url: '/storePage/GetTempPageData',
        method: 'get',
        data
    })
}

export function getDetail(params) {
    return request({
        url: '/storePage/Get',
        method: 'get',
        params
    })
}
export function del(data) {
    return request({
        url: '/storePage/delete',
        method: 'post',
        data
    })
}