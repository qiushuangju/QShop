import axios from 'axios'
/**
 * api接口列表
 */
const api = {
    image: '/upload/image',
    video: '/upload/video'
}

/**
 * 上传图片
 * @param {*} data
 */
export function image(data) {
    return axios({
        url: api.image,
        method: 'post',
        data
    })
}

/**
 * 上传视频
 * @param {*} data
 */
export function video(data) {
    return axios({
        url: api.video,
        method: 'post',
        data
    })
}