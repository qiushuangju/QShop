import request from '@/utils/request'

export function load(params) {
    return request({
        url: '/user/load',
        method: 'get',
        params
    })
}
export function get(params) {
    return request({
        url: '/user/get',
        method: 'get',
        params
    })
}

export function listByWhere(data) {
    return request({
        url: '/user/listByWhere',
        method: 'get',
        data
    })
}
export function addOrUpdate(data) {
    return request({
        url: '/user/addOrUpdate',
        method: 'post',
        data
    })
}
export function changeUserType(data) {
    return request({
        url: '/user/ChangeUserType',
        method: 'post',
        data
    })
}
export function changePassword(data) {
    return request({
        url: '/user/changePassword',
        method: 'post',
        data
    })
}
export function changeUserStatus(data) {
    return request({
        url: '/user/changeUserStatus',
        method: 'post',
        data
    })
}
export function checkUser(data) {
    return request({
        url: '/user/checkUser',
        method: 'post',
        data
    })
}
export function addOrUpdateManagement(data) {
    return request({
        url: '/user/addOrUpdateManagement',
        method: 'post',
        data
    })
}
export function changeChildBusinessUser(data) {
    return request({
        url: '/user/changeChildBusinessUser',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/user/delete',
        method: 'post',
        data
    })
}
export function rechargeBalance(data) {
    return request({
        url: '/user/rechargeBalance',
        method: 'post',
        data
    })
}