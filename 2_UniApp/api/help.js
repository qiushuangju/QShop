import request from '@/utils/request'

// api地址
const api = {
  list: 'help/list'
}

// 帮助中心列表
export const list = (param) => {
  return request.get(api.list, param)
}
