import config from '@/config.js'

// 合并用户配置和默认配置
const mergeConfig = Object.assign({}, config)

/**
 * 配置文件工具类
 * mix: 如需在项目中获取配置项, 请使用本工具类的方法, 不要直接import根目录的config.js
 */
export default {
  // 获取全部配置
  all() {
    return mergeConfig
  },

  // 获取指定配置
  get(key, def = undefined) {
    if (mergeConfig.hasOwnProperty(key)) {
      return mergeConfig[key]
    }
    console.error(`检测到不存在的配置项: ${key}`)
    return def
  }
}
