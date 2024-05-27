import request from '@/utils/request'

// api地址
const api = {
	load: 'rechargeOrder/load',
  submit: 'rechargeOrder/rechargeBefore'
}
// 积分明细列表
export const load = (data) => {
  return request.get(api.load, data)
}
// 积分明细列表
export const rechargeBefore = (data) => {
  return request.post(api.submit, data)
}
