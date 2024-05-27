import request from '@/utils/request'

export function assign(data) {
  var obj = data
  return request({
    url: '/accessobjs/unassign',
    method: 'post',
    data: {
      type: obj.type,
      firstId: obj.firstId
    }
  }).then(function() {
    request({
      url: '/accessobjs/assign',
      method: 'post',
      data
    })
  })
}

