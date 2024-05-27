import request from '@/utils/request'
const api = {
    image: '/fileUpload/uploadImg',
    video: '/fileUpload/uploadVideo',
    cart: '/fileUpload/UploadCart'
}

export function load(params) {
    return request({
        url: '/fileUpload/load',
        method: 'get',
        params
    })
}
export function listByWhere(params) {
    return request({
        url: '/fileUpload/listByWhere',
        method: 'get',
        params
    })
}

export function moveToGroup(data) {
    return request({
        url: '/fileUpload/moveToGroup',
        method: 'post',
        data
    })
}
// export function uploadImg(data) {
//     return request({
//         url: '/uploadGroup/uploadImg',
//         method: 'post',
//         data
//     })
// }
/**
 * 上传图片
 * @param {*} data
 */
export function image(data) {
    return request({
        url: api.image,
        method: 'post',
        data
    })
}
export function uploadCart(data) {
    return request({
        url: api.cart,
        method: 'post',
        data
    })
}


/**
 * 上传视频
 * @param {*} data
 */
export function video(data) {
    return request({
        url: api.video,
        method: 'post',
        data
    })
}





export function updateGroup(data) {
    return request({
        url: '/uploadGroup/updateGroup',
        method: 'post',
        data
    })
}
export function del(data) {
    return request({
        url: '/fileUpload/delete',
        method: 'post',
        data
    })
}