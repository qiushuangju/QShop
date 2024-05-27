import { login, logout, getInfo, getModules, getModulesTree } from '@/api/login'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { local } from '@/utils'

const user = {
    state: {
        token: getToken(),
        name: '',
        avatar: '',
        modules: null,
    },

    mutations: {
        SET_TOKEN: (state, token) => {
            state.token = token
        },
        SET_NAME: (state, name) => {
            state.name = name
        },
        SET_MODULES: (state, modules) => {
            state.modules = modules
        }
    },

    actions: {
        // 登录
        Login({ commit }, userInfo) {
            const username = userInfo.username.trim()
            const verifyCodeId = userInfo.verifyCodeId.trim()
            const verifyCode = userInfo.verifyCode.trim()

            return new Promise((resolve, reject) => {
                login(username, userInfo.password, verifyCodeId, verifyCode).then(response => {
                    const data = response.result;
                    setToken(data.token)
                    local.removeItem('visitedViews')
                    commit('SET_TOKEN', data.token)
                    resolve()
                }).catch(error => {
                    reject(error)
                })
            })
        },

        // 获取用户信息
        GetInfo({ commit, state }) {
            return new Promise((resolve, reject) => {
                getInfo(state.token).then(response => {
                    commit('SET_NAME', response.result)
                    resolve(response)
                }).catch(error => {
                    reject(error)
                })
            })
        },
        
        // 获取用户模块
        GetModules({ commit, state }) {
            return new Promise((resolve, reject) => {
                getModules(state.token).then(response => {
                    commit('SET_MODULES', response.result)
                    resolve(response)
                }).catch(error => {
                    reject(error)
                })
            })
        },
        // 获取用户模块
        GetModulesTree({ commit, state }) {
            return new Promise((resolve, reject) => {
                if (state.modules != null) {
                    resolve(state.modules)
                    return
                }
                getModulesTree(state.token).then(response => {
                    commit('SET_MODULES', response.result)
                    resolve(response.result)
                }).catch(error => {
                    reject(error)
                })
            })
        },
        // 登出
        LogOut({ commit, state }) {
            return new Promise((resolve, reject) => {
                logout(state.token).then(() => {
                    commit('SET_TOKEN', '')
                    commit('SET_MODULES', [])
                    removeToken()
                    local.removeItem('visitedViews')
                    resolve()
                }).catch(error => {
                    reject(error)
                })
            })
        },

        // 前端 登出
        FedLogOut({ commit }) {
            return new Promise(resolve => {
                commit('SET_TOKEN', '')
                removeToken()
                local.removeItem('visitedViews')
                resolve()
            })
        }
    }
}

export default user