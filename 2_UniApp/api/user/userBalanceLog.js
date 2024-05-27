import request from '@/utils/request'

// api地址
const api = {
	load:'UserBalanceLog/Load',
  listByWhere: 'UserBalanceLog/listByWhere',
  getDetail: 'userAddress/getDetail',
  addOrUpdate: 'userAddress/addOrUpdate',
  delete: 'userAddress/delete'
}
// 收货地址列表
export const load = (param) => {
  return request.get(api.load, param)
}
// 收货地址列表
export const listByWhere = (param) => {
  return request.get(api.listByWhere, param)
}
// 收货地址详情
export const getDetail = (id) => {
  return request.get(api.getDetail,  {id} )
}

// 新增收货地址
export const addOrUpdate = (data) => {
  return request.post(api.addOrUpdate, data)
}
// 删除收货地址
export const remove = (id) => {
  return request.post(api.delete, [{id}])
}
