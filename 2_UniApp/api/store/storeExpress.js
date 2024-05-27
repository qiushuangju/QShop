import request from '@/utils/request'

// api地址
const api = {
  listByWhere: 'storeExpress/listByWhere'
}

// 物流公司列表
export const listByWhere = (param) => {
  return request.get(api.listByWhere, param)
}
