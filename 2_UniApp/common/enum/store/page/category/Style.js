import Enum from '../../../enum'

/**
 * 枚举类：地址类型
 * PageCategoryStyleEnum
 */
export default new Enum([
  { key: 'One_Level_Big', name: '一级分类[大图]', value: 10 },
  { key: 'One_Level_Small', name: '一级分类[小图]', value: 11 },
  { key: 'Two_Level', name: '二级分类', value: 20 },
  { key: 'Commodity', name: '一级分类+商品', value: 30 }
])
