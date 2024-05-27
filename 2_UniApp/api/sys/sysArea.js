import request from '@/utils/request'

// api地址
const api = {
  listByWhere: 'sysArea/listByWhere',
  tree: 'region/tree'
}

// 获取所有地区
export const listByWhere = (param) => {
  return request.get(api.listByWhere, param)
}

// 获取所有地区(树状)
export const tree = (param) => {
  return request.get(api.tree, param)
}
