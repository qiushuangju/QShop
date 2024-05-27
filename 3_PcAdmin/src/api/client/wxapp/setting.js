// import { axios } from '@/utils/request'

/**
 * api接口列表
 */
const api = {
    detail: '/client.wxapp.setting/detail',
    update: '/client.wxapp.setting/update'
}

/**
 * 获取设置
 */
export function detail(key) {
    // return axios({
    //   url: api.detail,
    //   method: 'get',
    //   params: { key }
    // })
}

/**
 * 更新设置
 * @param {*} data
 */
export function update(key, data) {
    // return axios({
    //   url: api.update,
    //   method: 'post',
    //   data: { key, ...data }
    // })
}