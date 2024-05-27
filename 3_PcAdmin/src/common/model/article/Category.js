import * as CategoryApi from '@/api/article/articlecategory'

/**
 * 文章分类模型类
 * CategoryModel
 */
export default {

  /**
   * 向服务端获取分类列表并格式化
   */
  getCategorySelect () {
    return new Promise((resolve, reject) => {
      CategoryApi.list()
        .then(result => {
          const categoryList = result.data.list
          console.log(categoryList)

          // 格式化分类列表
          const selectList = this.formatData(categoryList)
          resolve(selectList)
        })
        .catch(error => reject(error))
    })
  },

  /**
   * 格式化分类列表
   * @param {*} list 分类数据源
   * @param {*} disabled
   */
  formatData (list, disabledId = null) {
    const data = []
    list.forEach(item => {
      // 新的元素
      const netItem = {
        title: item.name,
        key: item.category_id,
        value: item.category_id
      }
      // 禁用的分类
      // netItem.disabled = item.category_id === disabledId
      data.push(netItem)
    })
    return data
  }

}
