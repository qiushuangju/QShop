import request from '@/utils/request'

// api地址
const api = {
  load: 'UserPointsLog/Load'
}


// 收货地址列表
export const load = (param) => {
  return request.get(api.load, param)
}