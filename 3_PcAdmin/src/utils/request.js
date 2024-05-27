import axios from 'axios'
import { Message, MessageBox } from 'element-ui'
import store from '../store'
import { getToken } from '@/utils/auth'
import qs from 'qs'
// 创建axios实例
const service = axios.create({
        baseURL: process.env.VUE_APP_BASE_API, // api的base_url
        timeout: 50000 // 请求超时时间
    })
    // request拦截器
service.interceptors.request.use(config => {
    config.headers['tenantId'] = store.getters.tenantid
    config.headers['Content-Type'] = 'application/json; charset=utf-8'
    config.headers['platform'] = 'WebStore'
    if (store.getters.token) {
        config.headers['X-Token'] = getToken() // 让每个请求携带自定义token 请根据实际情况自行修改
    }

    if (config.method === 'get') {
        // 如果是get请求，且params是数组类型如arr=[1,2]，则转换成arr=1&arr=2
        config.paramsSerializer = function(params) {
            return qs.stringify(params, { arrayFormat: 'repeat' })
        }
    }
    return config
}, error => {
    // Do something with request error
    console.log(error) // for debug
    Promise.reject(error)
})

// respone拦截器
service.interceptors.response.use(
    response => {
        /**
         * code为非200是抛错 可结合自己业务进行修改
         */
        const res = response.data
        if (res.code !== 200) {
            // 50008:非法的token; 50012:其他客户端登录了;  50014:Token 过期了;
            if (res.code === 50008 || res.code === 50012 || res.code === 50014) {
                MessageBox.confirm('登录已超时，可以【取消】继续留在该页面，或者【重新登录】', '超时提醒', {
                    confirmButtonText: '重新登录',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    store.dispatch('FedLogOut').then(() => {
                        location.reload() // 为了重新实例化vue-router对象 避免bug
                    })
                })
            } else {
                Message({
                    message: res.message || res.msg,
                    type: 'error',
                    duration: 3 * 1000
                })
            }
            return Promise.reject('error')
        } else {
            return response.data
        }
    },
    error => {
        Message({
            message: 'err：' + error.message,
            type: 'error',
            duration: 10 * 1000
        })
        return Promise.reject(error)
    }
)

export default service