import request from '@/utils/request'

// api地址
const api = {
  list: 'article.category/list'
}

// 页面数据
export function list() {
  return request.get(api.list)
}
