import { Message, MessageBox } from 'element-ui'
import tools from '@/utils/tools'

//手机号码
export const validatePhone = (rule, value, callback) => {
    const phoneReg = /^1[3456789]\d{9}$/
    if (value === "") {
        callback(new Error("请输入手机号"));
    } else {
        if (!/^1[3456789]\d{9}$/.test(value)) {
            callback(new Error("请输入正确的手机号"));
        } else {
            callback();
        }
    }
};
///正负金额
export const validateMoney = (rule, value, callback) => {
    const moneyReg = /^(-?[0-9]+)(\.\d{1,2})?$/
    if (value === "") {
        callback(new Error("请输入金额"));
    } else {
        if (moneyReg.test(value) && Number(value) != 0) {
            callback();
        } else {
            callback(new Error("请输入正确的金额"));
        }
    }
};
//数字
export const validateDigit = (rule, value, callback) => {
        const reg = /^[0-9]+$/;
        if (value === "") {
            callback(new Error("不为空"));
        } else {
            if (reg.test(value)) {
                callback();
            } else {
                callback(new Error("请输入正确数字"));
            }
        }
    }
    //最大长度
export const maxlenth = (rule, value, callback) => {
    if (value.length > 60) {
        callback(new Error("长度不能大于60"));
    } else {
        callback();
    }
};

/**
 * 图片上传
 * @param {file} file el-upload文件对象
 * @param {number} size 限制的文件大小(kb) 默认10M
 */
export const validImgUpload = (file, size) => {
    size = +size || 10240
    const isSizeOut = file.size / 1024 > size
    console.log(size, '上传图片大小不能超过' + tools.toStorage(size * 1024));
    if (isSizeOut) {
        Message({
            message: '上传图片大小不能超过' + tools.toStorage(size * 1024),
            type: 'error',
            duration: 4 * 1000
        })
    }
    return !isSizeOut
}

/* 合法uri*/
export function validateURL(textval) {
    const urlregex = /^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/
    return urlregex.test(textval)
}

/* 小写字母*/
export function validateLowerCase(str) {
    const reg = /^[a-z]+$/
    return reg.test(str)
}

/* 大写字母*/
export function validateUpperCase(str) {
    const reg = /^[A-Z]+$/
    return reg.test(str)
}

/* 大小写字母*/
export function validatAlphabets(str) {
    const reg = /^[A-Za-z]+$/
    return reg.test(str)
}


export const validatePwd = (rule, value, callback) => {
    const reg = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{8,16}$/;
    if (value === "") {
        callback(new Error("8-16位,且包含字母数字"));
    } else {
        if (reg.test(value)) {
            callback();
        } else {
            callback(new Error("8-16位,且包含字母数字"));
        }
    }
}