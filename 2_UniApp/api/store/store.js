import request from '@/utils/request'

// api地址
const api = {
	 listByWhere: 'Store/listByWhere',
  getCurrentStore: 'Store/getCurrentStore'
}
// 设置项详情
export function listByWhere(param) {
  return request.get(api.listByWhere,param)
}
// 设置项详情
export function getCurrentStore(param) {
  return request.get(api.getCurrentStore,param)
}