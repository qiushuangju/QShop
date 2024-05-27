import request from '@/utils/request'

// api地址
const api = {
  listByWhere: 'RechargePlan/listByWhere'
}

// 充值套餐列表
export const listByWhere = (param) => {
  return request.get(api.listByWhere, param)
}
