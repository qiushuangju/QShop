import Vue from 'vue'
import Router from 'vue-router'
import OidcCallback from '@/views/OidcCallback'
import oidcRedirect from '@/views/OidcRedirect'


// in development-env not use lazy-loading, because lazy-loading too many pages will cause webpack hot update too slow. so only in production use lazy-loading;
// detail: https://panjiachen.github.io/vue-element-admin-site/#/lazy-loading

Vue.use(Router)

/* Layout */
import Layout from '../views/layout/Layout'

/**
* hidden: true                   if `hidden:true` will not show in the sidebar(default is false)
* alwaysShow: true               if set true, will always show the root menu, whatever its child routes length
*                                if not set alwaysShow, only more than one route under the children
*                                it will becomes nested mode, otherwise not show the root menu
* redirect: noredirect           if `redirect:noredirect` will no redirct in the breadcrumb
* name:'router-name'             the name is used by <keep-alive> (must set!!!)
* meta : {
    title: 'title'               the name show in submenu and breadcrumb (recommend set)
    icon: 'svg-name'             the icon show in the sidebar,
  }
**/
export const constantRouterMap = [{
        path: '/login',
        component: () =>
            import ('@/views/login/index'),
        meta: {
            sortNo: 0
        },
        hidden: true
    },
    {
        path: '/404',
        component: () =>
            import ('@/views/errorPage/404'),
        meta: {
            sortNo: 0
        },
        hidden: true
    },
    {
        path: '/401',
        component: () =>
            import ('@/views/errorPage/401'),
        meta: {
            sortNo: 0
        },
        hidden: true
    },
    // 把二级路由提前加到路由表，实现路由缓存

    // {
    //     path: '/oidc-callback', // Needs to match redirect_uri in you oidcSettings
    //     name: 'oidcCallback',
    //     component: OidcCallback,
    //     meta: {
    //         sortNo: 0,
    //         isOidcCallback: true,
    //         isPublic: true
    //     }
    // },
    // {
    //     path: '/oidcRedirect', // oidc临时跳转页面
    //     name: 'oidcRedirect',
    //     meta: {
    //         sortNo: 0
    //     },
    //     component: oidcRedirect
    // },
    {
        path: '/',
        component: Layout,
        redirect: 'dashboard',
        name: 'layout',
        meta: {
            sortNo: 0
        },
        children: [{
                path: '/dashboard',
                name: 'dashboard',
                meta: {
                    title: '主页',
                    icon: 'iconfont icon-zhuyeicon',
                    sortNo: 0
                },
                component: () =>
                    import ('@/views/dashboard/index')
            }, {
                path: '/profile',
                name: 'profile',
                hidden: true,
                meta: {
                    title: '个人中心',
                    icon: 'guide',
                    sortNo: 0
                },
                component: () =>
                    import ('@/views/employee/profile')
            },
            // {
            // // path: '/iframePage/:url/:name',
            // path: '/iframePage/:code',
            // name: 'iframePage',
            // hidden: true,
            // meta: {
            //     title: '接口文档',
            //     icon: 'guide',
            //     sortNo: 0
            // },
            // component: () =>
            //     import ('@/views/iframePage/index')
            // }
        ]
    }
    // {
    //   path: '/',
    //   hidden: true,
    //   component: Layout,
    //   meta: { sortNo: 0 },
    //   children: [{
    //     path: '/profile',
    //     name: 'profile',
    //     meta: { title: '个人中心', icon: 'guide', sortNo: 0 },
    //     component: () => import('@/views/employee/profile')
    //   }]
    // }
    // {
    //   path: '/swagger',
    //   component: Layout,
    //   meta: { sortNo: 0 },
    //   children: [{
    //     path: '',
    //     name: 'swagger',
    //     meta: { title: '接口文档', icon: 'guide', sortNo: 0 },
    //     component: () => import('@/views/swagger/index')
    //   }]
    // }
]

var router = new Router({
    // mode: 'history', //后端支持可开
    scrollBehavior: () => ({
        y: 0
    }),
    routes: constantRouterMap
})

export default router