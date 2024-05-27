import store from "@/store";
import moment from 'moment'
export function timeFix() {
    const time = new Date()
    const hour = time.getHours()
    return hour < 9 ? '早上好' : hour <= 11 ? '上午好' : hour <= 13 ? '中午好' : hour < 20 ? '下午好' : '晚上好'
}

export function welcome() {
    const arr = ['休息一会儿吧', '准备吃什么呢?', '要不要打一把 DOTA', '我猜你可能累了']
    const index = Math.floor(Math.random() * arr.length)
    return arr[index]
}

/**
 * 触发 window.resize
 */
export function triggerWindowResizeEvent() {
    const event = document.createEvent('HTMLEvents')
    event.initEvent('resize', true, true)
    event.eventType = 'message'
    window.dispatchEvent(event)
}

export function handleScrollHeader(callback) {
    let timer = 0
    let beforeScrollTop = window.pageYOffset
    callback = callback || function() {}
    window.addEventListener(
        'scroll',
        event => {
            clearTimeout(timer)
            timer = setTimeout(() => {
                let direction = 'up'
                const afterScrollTop = window.pageYOffset
                const delta = afterScrollTop - beforeScrollTop
                if (delta === 0) {
                    return false
                }
                direction = delta > 0 ? 'down' : 'up'
                callback(direction)
                beforeScrollTop = afterScrollTop
            }, 50)
        },
        false
    )
}

export function isIE() {
    const bw = window.navigator.userAgent
    const compare = (s) => bw.indexOf(s) >= 0
    const ie11 = (() => 'ActiveXObject' in window)()
    return compare('MSIE') || ie11
}

/**
 * Remove loading animate
 * @param id parent element id or class
 * @param timeout
 */
export function removeLoadingAnimate(id = '', timeout = 1500) {
    if (id === '') {
        return
    }
    setTimeout(() => {
        document.body.removeChild(document.getElementById(id))
    }, timeout)
}

/**
 * 将List转为Tree
 * @param {*} list
 */
export const listToTree = (list, parent, tree) => {
    tree = typeof tree !== 'undefined' ? tree : []
    parent = typeof parent !== 'undefined' ? parent : { id: null, }
    var children = list.filter(val => {
        return val.parentId === parent.id
    })

    if (children.length > 0) {
        if (parent.id === null) {
            tree = children
        } else {
            parent['children'] = children
        }

        children.forEach((val) => {
            listToTree(list, val)
        })
    }
    return tree
}

// 节流
// 思路： 第一次先设定一个变量true，
// 第二次执行这个函数时，会判断变量是否true，
// 是则返回。当第一次的定时器执行完函数最后会设定变量为flase。
// 那么下次判断变量时则为flase，函数会依次运行。
export function throttle(fn, delay = 100) {
    // 首先设定一个变量，在没有执行我们的定时器时为null
    var timer = null
    return function() {
        // 当我们发现这个定时器存在时，则表示定时器已经在运行中，需要返回
        if (timer) return
        timer = setTimeout(() => {
            fn.apply(this, arguments)
            timer = null
        }, delay)
    }
}

// 防抖
// 首次运行时把定时器赋值给一个变量， 第二次执行时，
// 如果间隔没超过定时器设定的时间则会清除掉定时器，
// 重新设定定时器， 依次反复， 当我们停止下来时，
// 没有执行清除定时器， 超过一定时间后触发回调函数。
export function debounce(fun, delay) {
    return function(args) {
        // 获取函数的作用域和变量
        const that = this
        const _args = args
            // 每次事件被触发，都会清除当前的timeer，然后重写设置超时调用
        clearTimeout(fun.id)
        fun.id = setTimeout(function() {
            fun.call(that, _args)
        }, delay)
    }
}

/**
 * 判断是否为空对象
 * @param {*} object 源对象
 */
export function isEmptyObject(object) {
    return Object.keys(object).length === 0
}

/**
 * 判断是否为对象
 * @param {*} object
 */
export function isObject(object) {
    return Object.prototype.toString.call(object) === '[object Object]'
}

/**
 * 判断是否为对象
 * @param {*} array
 */
export function isArray(array) {
    return Object.prototype.toString.call(array) === '[object Array]'
}

/**
 * 判断是否为空
 * @param {*} object 源对象
 */
export function isEmpty(value) {
    if (isArray(value)) {
        return value.length === 0
    }
    if (isObject(value)) {
        return isEmptyObject(value)
    }
    return !value
}

/**
 * 判断是否在数组中
 * @param {*} search
 * @param {*} array
 */
export function inArray(search, array) {
    return array.includes(search)
}

/**
 * 初始化
 */
export function initDate() {
    var initDate = [
        moment().subtract(7, 'days').format('YYYY-MM-DD'),
        moment().subtract(-1, 'days').format('YYYY-MM-DD'),
    ];
    return initDate;
}
/**
 * 初始化
 */
export function initPickerOptions() {
    var pickerOptions = {
        shortcuts: [{
                text: '最近一周',
                onClick(picker) {
                    const end = moment().subtract(-1, 'days').format('YYYY-MM-DD');
                    const start = moment().subtract(7, 'days').format('YYYY-MM-DD');
                    picker.$emit('pick', [start, end])
                },
            },
            {
                text: '最近一个月',
                onClick(picker) {
                    const end = moment().subtract(-1, 'days').format('YYYY-MM-DD');
                    const start = moment().subtract(1, 'months').format('YYYY-MM-DD');
                    picker.$emit('pick', [start, end])
                },
            },
            {
                text: '最近三个月',
                onClick(picker) {
                    const end = moment().subtract(-1, 'days').format('YYYY-MM-DD');
                    const start = moment().subtract(3, 'months').format('YYYY-MM-DD');
                    picker.$emit('pick', [start, end])
                },
            },
        ],
    };
    return pickerOptions;
}

// 
/**
 * 数组求合集
 * @param {*} arrA
 * @param {*} arrB
 * @param {*} field 字段名称 可选(对象数组才需要 如:"id")
 */
export function intersection(arrA, arrB, field) {
    return arrA.filter(t => (field ? arrB.map(i => i[field]).includes(t[field]) : arrB.includes(t)))
}
/**
 * 获取指定天数的日期
 * @param day
 * @returns {string}
 */
export function getDateByDay(day) {
    var today = new Date()
    var targetdaySeconds = today.getTime() + 1000 * 60 * 60 * 24 * day
    today.setTime(targetdaySeconds) // 注意，这行是关键代码
    return today.getFullYear() + '-' + zeroFillLeft(today.getMonth() + 1) + '-' + zeroFillLeft(today.getDate())
}

/**
 * 左侧补0
 * @param value
 * @returns {*}
 */
export function zeroFillLeft(value) {
    return (value.toString().length === 1) ? ('0' + value) : value
}



/**
 * 批量给指定对象赋值
 * @param obj obj 指定的对象,一般为vue实例
 * @param obj assignment 赋值的元素 { a: '123' }
 */
export function assignment(obj, assignment) {
    Object.keys(assignment).forEach(key => {
        obj[key] = assignment[key]
    })
}

/**
 * 对象转URL
 * @param {object} obj
 */
export const urlEncode = (obj = {}) => {
    const result = []
    for (const key in obj) {
        const item = obj[key]
        if (!item) {
            continue
        }
        if (isArray(item)) {
            item.forEach(val => {
                result.push(key + '=' + val)
            })
        } else {
            result.push(key + '=' + item)
        }
    }
    return result.join('&')
}

/**
 * 生成url (带参数)
 * @param {string} path 链接
 * @param {object} params query参数
 */
export function buildUrL(path, params) {
    const queryStr = urlEncode(params)
    if (!isEmpty(queryStr)) {
        return path + '?' + queryStr
    }
    return path
}


/**
 * 检查按钮权限
 * @param {string} moduleName 模块标识
 * @param {string} btnName 按钮
 */
export function checkPermission(moduleName, btnName) {
    moduleName = moduleName.toLowerCase();
    btnName = btnName.toLowerCase();
    var isShow = false;

    var listModules = store.getters.modules;
    listModules.forEach(itemLevel1 => { //一级目录
        itemLevel1.children.forEach(itemLevel2 => { //二级目录
            if (itemLevel2.children == null || itemLevel2.children.length == 0) { //无三级目录
                if (itemLevel2.item.code.toLowerCase() == moduleName) {
                    itemLevel2.item.elements.forEach(btn => {
                        if (btn.btnCode.toLowerCase() == btnName) {
                            isShow = true;
                        }
                    })
                }
            } else { //有三级目录
                itemLevel2.children.forEach(itemLevel3 => {
                    if (itemLevel3.item.code.toLowerCase() == moduleName) {
                        itemLevel3.item.elements.forEach(btn => {
                            if (btn.btnCode.toLowerCase() == btnName) {
                                isShow = true;
                            }
                        })
                    }
                })
            }

        })
    })
    return isShow;
}