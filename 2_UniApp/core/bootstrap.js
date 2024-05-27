import store from '@/store'
import storage from '@/utils/storage'
import platform from '@/core/platform'
import { AccessToken, UserId } from '@/store/mutation-types'

export default function Initializer() {
  // 当前运行的终端
  store.commit('SetPlatform', platform)
  // 用户认证token
  store.commit('SetToken', storage.get(AccessToken))
  // 当前用户ID
  store.commit('SetUserId', storage.get(UserId))
}
