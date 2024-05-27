import request from '@/utils/request'

// api地址
const api = {
  listByWhere: 'userAddress/listByWhere',
  // defaultId: 'userAddress/defaultId',
  getDetail: 'userAddress/getDetail',
  addOrUpdate: 'userAddress/addOrUpdate',
  setDefault: 'userAddress/setDefault',
  delete: 'userAddress/delete'
}

// 收货地址列表
export const listByWhere = (param) => {
  return request.get(api.listByWhere, param)
}

// // 默认收货地址ID
// export const defaultId = (param) => {
//   return request.get(api.defaultId, param)
// }

// 收货地址详情
export const getDetail = (id) => {
  return request.get(api.getDetail,  {id} )
}

// 新增收货地址
export const addOrUpdate = (data) => {
  return request.post(api.addOrUpdate, data)
}

// 设置默认收货地址
export const setDefault = (id) => {
  return request.post(api.setDefault, {id})
}

// 删除收货地址
export const remove = (id) => {
  return request.post(api.delete, [{id}])
}
