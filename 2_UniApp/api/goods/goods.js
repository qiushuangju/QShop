import request from '@/utils/request'

// api地址
const api = {
	 load: 'goods/Load',
  listByWhere: 'goods/ListByWhere',
  detail: 'goods/Get',
  specData: 'goods/specData'
}
// 商品列表(分页)
export const load = param => {
  return request.get(api.load, param)
}

// 商品列表
export const listByWhere = param => {
  return request.get(api.listByWhere, param)
}

// 商品详情
export const detail = id => {
  return request.get(api.detail, { id })
}


// 获取商品规格数据
export const specData = (goodsId) => {
  return request.get(api.specData, { goodsId })
}
