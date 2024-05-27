import request from '@/utils/request'

// api地址
const api = {
  getDetail: 'StoreSetting/GetDetail'
}

// 设置项详情
export function GetDetail(param) {
  return request.get(api.getDetail,param)
}