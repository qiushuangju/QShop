import request from '@/utils/request'

// api地址
const apiUri = {
	get: 'storePage/Get',
	getTypeDetail: 'storePage/GetTypeDetail'
}
export function get(id) {
  return request.get(apiUri.get, {id})
}
// 页面数据
export function getTypeDetail(pageType) {
  return request.get(apiUri.getTypeDetail, {
    pageType
  })
}
