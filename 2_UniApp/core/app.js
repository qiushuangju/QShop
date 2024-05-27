import store from '@/store'
import * as util from '@/utils/util'
import { paginate } from '@/common/constant'

/**
 * 显示成功提示框
 */
export const showSuccess = (msg, callback) => {
  uni.showToast({
    title: msg,
    icon: 'success',
    mask: true,
    duration: 1500,
    success() {
      callback && callback()
    }
  })
}

/**
 * 显示失败提示框
 */
export const showError = (msg, callback) => {
  uni.showModal({
    title: '友情提示',
    content: msg,
    showCancel: false,
    success(res) {
      callback && callback()
    }
  })
}

/**
 * 显示纯文字提示框
 */
export const showToast = (msg, duration = 1500, mask = true) => {
  uni.showToast({
    title: msg, // 提示的内容
    icon: 'none',
    mask, // 是否显示透明蒙层，防止触摸穿透
    duration // 提示的延迟时间，单位毫秒，默认：1500  
  })
}

/**
 * tabBar页面路径列表 (用于链接跳转时判断)
 * tabBarLinks为常量, 无需修改
 */
export const getTabBarLinks = () => {
  const tabBarLinks = [
    'pages/index/index',
    'pages/category/index',
    'pages/cart/index',
    'pages/user/index'
  ]
  return tabBarLinks
}

/**
 * 生成完整的H5地址 [带参数]
 * @param {string} h5Url H5访问地址
 * @param {string} path 页面路径
 * @param {object} params 页面参数
 * @return {string}
 */
export const buildUrL = (h5Url, path, params) => {
  let complete = h5Url
  if (!util.isEmpty(path)) {
    complete += '#/' + path
    const shareParamsStr = getShareUrlParams(params)
    if (!util.isEmpty(shareParamsStr)) {
      complete += '?' + shareParamsStr
    }
  }
  return complete
}

/**
 * 生成转发的url参数(string格式)
 */
export const getShareUrlParams = (params) => {
  return util.urlEncode({
    // refereeId: store.getters.userId, // 推荐人ID
    ...params
  })
}

/**
 * 跳转到指定页面url
 * 支持tabBar页面
 * @param {string}  url   页面路径
 * @param {object}  query 页面参数
 * @param {string}  modo  跳转类型(默认navigateTo)
 */
export const navTo = (url, query = {}, modo = 'navigateTo') => {
  if (!url || url.length == 0) {
    return false
  }
  // tabBar页面, 使用switchTab
  if (util.inArray(url, getTabBarLinks())) {
    uni.switchTab({
      url: `/${url}`
    })
    return true
  }
  // 生成query参数
  const queryStr = !util.isEmpty(query) ? '?' + util.urlEncode(query) : ''
  // 普通页面, 使用navigateTo
  modo === 'navigateTo' && uni.navigateTo({
    url: `/${url}${queryStr}`
  })
  // 特殊指定, 使用redirectTo
  modo === 'redirectTo' && uni.redirectTo({
    url: `/${url}${queryStr}`
  })
  return true
}

/**
 * 获取购物车商品总数量
 * @param {*} value 
 */
export const getCartTotalNum = (value) => {
  const cartTotal = uni.getStorageSync('cartTotalNum') || 0
  return checkLogin() ? cartTotal : 0
}

/**
 * 记录购物车商品总数量
 * @param {*} value 
 */
export const setCartTotalNum = (value) => {
  uni.setStorageSync('cartTotalNum', Number(value))
}

/**
 * 设置购物车tabbar的角标
 * 该方法只能在tabbar页面中调用, 其他页面调用会报错
 */
export const setCartTabBadge = () => {
  const cartTabbarIndex = 2
  const cartTotal = getCartTotalNum()
  if (cartTotal > 0) {
    uni.setTabBarBadge({
      index: cartTabbarIndex,
      text: `${cartTotal}`
    })
  } else {
    uni.removeTabBarBadge({
      index: cartTabbarIndex
    })
  }
  return
}

/**
 * 验证是否已登录
 */
export const checkLogin = () => {
  return !!store.getters.userId
}

/**
 * 发起支付请求
 * @param {Object} 参数
 */
export const wxPayment = (option) => {
  const options = {
    timeStamp: '',
    nonceStr: '',
    prepay_id: '',
    paySign: '',
    ...option
  }
  return new Promise((resolve, reject) => {
    uni.requestPayment({
      provider: 'wxpay',
      timeStamp: options.timeStamp,
      nonceStr: options.nonceStr,
	  package: options.package,
      signType: options.signType,
      paySign: options.paySign,
      success: res => resolve(res),
      fail: res => reject(res)
    })
  })
}

/**
 * 加载更多列表数据
 * @param {Object} resList 新列表数据
 * @param {Object} oldList 旧列表数据
 * @param {int} pageNo 当前页码
 */
export const getEmptyPaginateObj = () => {
  return util.cloneObj(paginate)
}

/**
 * 加载更多列表数据
 * @param {Object} resList 新列表数据
 * @param {Object} oldList 旧列表数据
 * @param {int} pageNo 当前页码
 */
export const getMoreListData = (resList, oldList, pageNo) => {
  // 如果是第一页需手动制空列表
  if (pageNo == 1) oldList.data = []
  // 合并新数据
  
  return oldList.data.concat(resList)
}
