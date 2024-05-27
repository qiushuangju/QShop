// import { axios } from '@/utils/request'
import axios from 'axios'

/**
 * api接口列表
 */
const api = {
    list: '/files.group/list',
    add: '/files.group/add',
    edit: '/files.group/edit',
    delete: '/files.group/delete'
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
 * 新增记录
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
 * 编辑记录
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