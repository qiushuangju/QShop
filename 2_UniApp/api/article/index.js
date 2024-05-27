import request from '@/utils/request'

// api地址
const api = {
  list: 'article/list',
  detail: 'article/detail'
}

// 文章列表
export function list(param, option) {
  return request.get(api.list, param, option)
}

// 文章详情
export function detail(articleId) {
  return request.get(api.detail, { articleId })
}
