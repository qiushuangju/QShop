import request from '@/utils/request'

export function getList(params) {
    return request({
        url: '/user/load',
        method: 'get',
        params
    })
}

/**
 * 是否忽略登录用户权限，直接获取全部数据
 * 用于可以跨部门选择用户的场景
 */
export function loadAll(params) {
    return request({
        url: '/user/loadall',
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

// export function add(data) {
//     return request({
//         url: '/user/AddOrUpdateEmp',
//         method: 'post',
//         data
//     })
// }

// export function update(data) {
//     return request({
//         url: '/user/AddOrUpdateEmp',
//         method: 'post',
//         data
//     })
// }

export function addOrUpdateEmp(data) {
    return request({
        url: '/user/AddOrUpdateEmp',
        method: 'post',
        data
    })
}

export function changePassword(data) {
    return request({
        url: '/user/changepassword',
        method: 'post',
        data
    })
}

export function changeProfile(data) {
    return request({
        url: '/user/changeprofile',
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

export function del(data) {
    return request({
        url: '/user/delete',
        method: 'post',
        data
    })
}

export function loadByRole(params) {
    return request({
        url: '/user/loadByRole',
        method: 'get',
        params
    })
}
export function LoadByOrg(params) {
    return request({
        url: '/user/LoadByOrg',
        method: 'get',
        params
    })
}