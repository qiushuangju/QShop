import { AccessToken, UserId } from '@/store/mutation-types'
import storage from '@/utils/storage'
import * as LoginApi from '@/api/login'

// 登陆成功后执行
const loginSuccess = (commit, userInfo) => {
	
	var userId=userInfo.userId;
	var token=userInfo.token;
  // 过期时间30天
  const expiryTime = 30 * 86400
  // 保存tokne和userId到缓存
  storage.set(UserId, userId, expiryTime)
  storage.set(AccessToken, token, expiryTime)
  // 记录到store全局变量
  commit('SetToken', token)
  commit('SetUserId', userId)
}

const user = {
  state: {
    // 用户认证token
    token: '',
    // 用户ID
    userId: null
  },

  mutations: {
    SetToken: (state, value) => {
      state.token = value
    },
    SetUserId: (state, value) => {
      state.userId = value
    }
  },

  actions: {

    // 用户登录(普通登录: 输入手机号和验证码)
    LoginByPhoneCode({ commit }, data) {
      return new Promise((resolve, reject) => {
        LoginApi.loginByPhoneCode(data)
          .then(res => {
            const result = res.result
            loginSuccess(commit, result)
            resolve(res)
          })
          .catch(reject)
      })
    },

    // 微信小程序一键授权登录(获取用户基本信息)
    loginWx({ commit }, data) {
      return new Promise((resolve, reject) => {
        LoginApi.loginWx(  data , { isPrompt: false })
          .then(res => {
			  var result=res.result
            loginSuccess(commit, result)
            resolve(result)
          })
          .catch(reject)
      })
    },

    // 微信小程序一键授权登录(授权手机号)
    bindWxPhone({ commit }, data) {
      return new Promise((resolve, reject) => {
        LoginApi.bindWxPhone(data, { isPrompt: false })
          .then(res => {
            const result = res.result
            loginSuccess(commit, result)
            resolve(response)
          })
          .catch(reject)
      })
    },

    // 退出登录
    Logout({ commit }, data) {
      const store = this
      return new Promise((resolve, reject) => {
		 // LoginApi.logout().then(res=>{
			 
				storage.remove(UserId)
				storage.remove(AccessToken)
				// 记录到store全局变量
				commit('SetToken', null)
				commit('SetUserId', null)
				resolve() 
			 

		 // })
      })
    }

  }
}

export default user
