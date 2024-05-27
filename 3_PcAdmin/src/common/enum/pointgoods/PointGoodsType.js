import Enum from '../enum'

/**
 * 枚举类：积分商品类型
 * PointGoodsType
 */
export default new Enum([
  { key: 'NOTDISTRIBUTION', name: '无需配送', value: 10 },
  { key: 'DISTRIBUTION', name: '需要配送', value: 20 },
])