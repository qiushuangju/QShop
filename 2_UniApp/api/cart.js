import request from '@/utils/request'

// api地址
const api = {
  listByWhere: 'Cart/listByWhere',
  ListTotalByWhere:'Cart/ListTotalByWhere',
  load: 'Cart/load',
  addCart: 'Cart/AddCart',
  updateCartNum: 'Cart/UpdateCartNum',
  delete: 'Cart/delete'
}


// 购物车列表
export const listTotalByWhere = () => {
  return request.get(api.ListTotalByWhere, {}, { load: false })
}

// 购物车列表
export const listByWhere = () => {
  return request.get(api.listByWhere, {}, { load: false })
}

// 购物车商品总数量
export const load = () => {
  return request.get(api.load, {}, { load: false })
}

// 加入购物车
export const addCart = (goodsId, goodsSkuId, goodsNum) => {
  return request.post(api.addCart, { goodsId, goodsSkuId, goodsNum })
}

// 更新购物车商品数量
export const updateCartNum = (goodsId, goodsSkuId, goodsNum) => {
  return request.post(api.updateCartNum, { goodsId, goodsSkuId, goodsNum }, { isPrompt: false })
}

// 删除购物车中指定记录
export const deleteRecored = (cartIds = []) => {
  return request.post(api.delete,  cartIds )
}
