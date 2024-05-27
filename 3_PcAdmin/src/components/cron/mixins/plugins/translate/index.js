import ZHCN from './zh-CN'
export default {
  install(Vue) {
    Vue.prototype.$t = (type) => {
      const area = (type && type.split('.')) || []
      let name = ''
      if (area.length > 0) {
        name = ZHCN[area[0]][area[1]]
      }
      return name
    }
  }
}
