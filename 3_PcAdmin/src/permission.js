import router from './router'
import store from './store'
import NProgress from 'nprogress' // Progress 进度条
import 'nprogress/nprogress.css' // Progress 进度条样式
import { Message } from 'element-ui'
import { getToken } from '@/utils/auth' // 验权

const whiteList = ['/login', '/oidc-callback', '/swagger', '/employee/profile'] // 不重定向白名单
router.beforeEach((to, from, next) => {
    NProgress.start()
    if (getToken()) {
        if (to.path === '/login') { // 登录后login自动跳转
            next({ path: '/' })
            return
        }
        if (store.getters.modules != null) {
            next()
            return
        }

        store.dispatch('GetInfo').then(() => { // 拉取用户信息
            store.dispatch('GetModulesTree').then(modules => { // 获取用户可访问的模块
                store.dispatch('GenerateRoutes', { modules }).then(() => { // 根据权限生成可访问的路由表
                    // console.log('store.getters.addRouters', JSON.stringify(store.getters.addRouters))
                    router.addRoutes(store.getters.addRouters) // 动态添加可访问路由表
                    next({...to, replace: true }) // hack方法 确保addRoutes已完成 ,set the replace: true so the navigation will not leave a history record
                })
            })
        }).catch((err) => {
            store.dispatch('FedLogOut').then(() => {
                Message.error(err || '获取用户信息失败')
                next({ path: '/' })
            })
        })

        return
    }
    if (whiteList.indexOf(to.path) !== -1) { // 没登录情况下过滤白名单
        next()
    } else {
        next('/login')
    }
})

router.afterEach(() => {
    NProgress.done() // 结束Progress
})