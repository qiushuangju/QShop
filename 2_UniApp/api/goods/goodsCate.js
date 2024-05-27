import request from '@/utils/request'

// api地址
const api = {
  ListByWhere: 'GoodsCate/ListByWhere'
}

// 页面数据
export function ListByWhere() {
  return request.get(api.ListByWhere)
}
