import request from '@/utils/request'

// api地址
const api = {
  list: 'balance.log/list'
}

// 余额账单明细列表
export const list = (param) => {
  return request.get(api.list, param)
}
