import request from '@/utils/request'

export function getList() {
  return request({
    url: '/applications/load',
    method: 'get'
  })
}

