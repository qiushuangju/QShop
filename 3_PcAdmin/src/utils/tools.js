/**
 * 创建唯一的字符串
 * @return {string} ojgdvbvaua40
 */
function createUniqueString() {
    const timestamp = +new Date() + ''
    const randomNum = parseInt((1 + Math.random()) * 65536) + ''
    return (+(randomNum + timestamp)).toString(32)
}

/**
 * 数字存储大小格式化
 * @param {number} num 存储大小 单位：Byte
 * @param {number} digits 保留几位小数
 * @return {string} 2MB
 */
function toStorage(num, digits) {
    digits = digits || 2
    if (num < 1024) {
        return num + 'B'
    }
    num = (num * 1000 / 1024)
    const si = [
        { value: 1E18, symbol: 'E' },
        { value: 1E15, symbol: 'P' },
        { value: 1E12, symbol: 'T' },
        { value: 1E9, symbol: 'G' },
        { value: 1E6, symbol: 'M' },
        { value: 1E3, symbol: 'K' }
    ]
    for (let i = 0; i < si.length; i++) {
        if (num >= si[i].value) {
            return (num / si[i].value).toFixed(digits).replace(/\.0+$|(\.[0-9]*[1-9])0+$/, '$1') +
                si[i].symbol + 'B'
        }
    }
}

Date.prototype.Format = function(fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1, //月份   
        "d+": this.getDate(), //日   
        "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, //小时           
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分   
        "s+": this.getSeconds(), //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds() //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

Array.prototype.max = function() {

    return Math.max.apply({}, this)

}

Array.prototype.min = function() {

    return Math.min.apply({}, this)

}
export default {
    createUniqueString,
    toStorage,
    // 日期格式化 fmt 格式化字符如 'yyyy-MM-dd hh:mm:ss' v 可以是日期或字符串
    datetimeFormat: function(v, fmt) {
        if ((typeof v).toLocaleLowerCase() == 'string') { v = new Date(v.replace(/-/g, '/')) }
        var o = {
            'M+': v.getMonth() + 1, // 月份
            'd+': v.getDate(), // 日
            'h+': v.getHours(), // 小时
            'm+': v.getMinutes(), // 分
            's+': v.getSeconds(), // 秒
            'q+': Math.floor((v.getMonth() + 3) / 3), // 季度
            'S': v.getMilliseconds() // 毫秒
        }
        if (/(y+)/.test(fmt)) { fmt = fmt.replace(RegExp.$1, (v.getFullYear() + '').substr(4 - RegExp.$1.length)) }
        for (var k in o) {
            if (new RegExp('(' + k + ')').test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (('00' + o[k]).substr(('' + o[k]).length)))
            }
        }
        return fmt
    },
    // 获取日期的星期 v=日期字符串或日期对象 返回值 0-6对应 周日-周六
    getWeek: function(v) {
        if ((typeof v).toLocaleLowerCase() == 'string') { v = new Date(v.replace(/-/g, '/')) }
        var end = v.getDay()
        return end
    },
    // 字符串转日期 v必须为 -格式 如 2020-05
    toDateTime: function(v) {
        if ((typeof v).toLocaleLowerCase() == 'string') { v = new Date(v.replace(/-/g, '/')) }
        return v
    },
    // 用于将数组进行name匹配 这里用作export2excel.js插件的智能字段识别导出
    formatJson: function(filterVal, jsonData) {
        return jsonData.map(v => filterVal.map(j => v[j]))
    },
    isNullOrEmpty: function(str) {
        return str == null || str == '' ? true : false;
    },
    toArr: function(str) {
        var arr = str != null && str != '' ? str.split(",") : [];
        return arr;
    },
    dtAddDays: function(date, days) {
        var today = new Date(date)
        today.setDate(today.getDate() + days);
        return today.Format('yyyy-MM-dd');
    },
    dtAddMonths: function(date, months) {
        var today = new Date(date)
        today.setMonth(today.getMonth() + months);
        return today.Format('yyyy-MM-dd');
    },
    dtAddYears: function(date, years) {
        var today = new Date(date)
        today.setFullYear(today.getFullYear() + years);
        return today.Format('yyyy-MM-dd');
    },
    getStrDate: function(strDatetime) { //获取时间日期
        if (strDatetime == null || strDatetime == "") {
            return "";
        }
        var strDate = new Date(strDatetime).Format("yyyy-MM-dd")
        return strDate
    },
    getTextByArr: function(arr, value) {
        var text = "";
        arr.forEach(element => {
            if (element.value == value) {
                text = element.text;
            }
        });
        return text;
    },


}