import request from '@/utils/request'

// api地址
const api = {
	load: 'GoodsComment/load',
  listByWhere: 'GoodsComment/listByWhere',
  addComment: 'GoodsComment/AddComment',
  totalGroup: 'GoodsComment/totalGroup'
}

// 商品评价列表
export const listByWhere = (goodsId, param, option) => {
  return request.get(api.listByWhere, { ...param, goodsId })
}


// 商品评价列表
export const load = (goodsId, param) => {
  return request.get(api.load, { ...param, goodsId })
}

// 商品评价列表
export const addComment = (data) => {
  return request.post(api.addComment, data)
}

// // 商品评价列表 (限制数量, 用于商品详情页展示)
// export const listRows = (goodsId, limit = 5) => {
//   return request.get(api.listRows, { goodsId, limit })
// }

// 商品评分总数
export const totalGroup = (goodsId) => {
  return request.get(api.totalGroup, { goodsId })
}
