import axios from 'axios'

/**
 * api接口列表
 */
const api = {
    list: '/page/list',
    defaultData: '/page/defaultData',
    detail: '/page/detail',
    add: '/page/add',
    edit: '/page/edit',
    delete: '/page/delete',
    setHome: '/page/setHome'
}

/**
 * 列表记录
 */
export function list(params) {
    return axios({
        url: api.list,
        method: 'get',
        params
    })
}

/**
 * 详情记录
 */
export function defaultData(params) {
    return axios({
        url: api.defaultData,
        method: 'get',
        params
    })
}

/**
 * 页面详情
 */
export function detail(params) {
    return axios({
        url: api.detail,
        method: 'get',
        params
    })
}

/**
 * 设置默认首页
 * @param {*} data
 */
export function setHome(data) {
    return axios({
        url: api.setHome,
        method: 'post',
        data: data
    })
}

/**
 * 新增页面
 * @param {*} data
 */
export function add(data) {
    return axios({
        url: api.add,
        method: 'post',
        data
    })
}

/**
 * 编辑页面
 * @param {*} data
 */
export function edit(data) {
    return axios({
        url: api.edit,
        method: 'post',
        data
    })
}

/**
 * 删除记录
 * @param {*} data
 */
export function deleted(data) {
    return axios({
        url: api.delete,
        method: 'post',
        data: data
    })
}