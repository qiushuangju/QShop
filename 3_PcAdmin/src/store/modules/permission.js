import { constantRouterMap } from '@/router'
import { deepClone } from '@/utils/index'
import Layout from '@/views/layout/Layout'

//将模块数据转换为vue的Router结构，顶级的模块使用layout视图
const moduleToRoute = (data, dispatch) => {
    const parentPath = data.item.url.indexOf('?') > -1 ? data.item.url.split('?')[0] : data.item.url
        //查询参数,在SidebarItem再加上参数,应该是为了绕过路由
    const querydata = data.item.url.indexOf('?') > -1 ? data.item.url.split('?')[1] : '';
    const tabTitle = data.item.name
        // const parentPath = data.item.url;
    var newPath = {
        path: parentPath || '/',
        component: data.item && data.item.parentId === null ? Layout :
            () =>
            import ('@/views' + parentPath.toLowerCase()),
        meta: {
            title: data.item.name,
            sortNo: data.item.sortNo,
            icon: data.item.iconName || 'streamlist',
            elements: data.item && data.item.elements || '',
            url: data.item.url,
            code: data.item.code,
            id: data.item.cascadeId
        },
        name: data.item.name,
        hidden: false,
        children: []
    }
    if (newPath.path.indexOf('http') > -1) {
        let obj = {}
        obj[newPath.meta.code] = {
            name: newPath.name,
            url: newPath.meta.url
        }
        dispatch('setIframeTagViews', obj, { root: true })
    }
    newPath.path = newPath.path.indexOf('http') > -1 ? `/iframePage/${newPath.meta.code}` : newPath.path
    if (data.children && data.children.length > 0) {
        data.children.forEach(child => {
            newPath.children.push(moduleToRoute(child, dispatch))
        })
    }
    newPath.querydata = querydata;
    if (querydata) {
        newPath.tabTitle = tabTitle;
    }

    return newPath
}

//与初始定义的路由合并，并把根模块挂到根路由'/'下面
const mergeModuleWithConst = (data) => {
    let rootModule = []
    data.length > 0 && data.forEach(item => {
        if (item.component && item.component.name && item.component.name === 'layout') {
            let obj = {
                component: () =>
                    import ('@/views' + item.path.toLowerCase()),
                path: item.children && item.children.length > 0 ? `${item.path}/${item.meta.id}` : item.path,
                name: item.name,
                children: item.children,
                meta: item.meta
            }
            rootModule = rootModule.concat(obj)
            return
        }
        arr.push(item)
    })

    var arr = deepClone(constantRouterMap)
    arr.length > 0 && arr.forEach(item => {
        if (item.name === 'layout') { //把模块的根路由放到'/'下面
            item.children = item.children.concat(rootModule)
        }
    })
    return arr
}
const groupData = (data) => {
    if (data.children && data.children.length > 0) {
        data.children.forEach(child => {
            let code = groupData(child)
            code && (code = code.replace(code[0], code[0].toLowerCase()))
            code && code !== undefined && permission.state.keepAliveData.push(code)
        })
        return
    }
    return data.meta.code
}

// //把所有的2级以上路由都挂载到顶级layout下面，否则不能识别
const moveDeepLevelToSec = (router) => {
    router.children &&
        router.children.length > 0 &&
        (router.children = moveDeepLevelChildrenToSec(router.children, []))
    return router
}
const moveDeepLevelChildrenToSec = (children, res) => {
    if (Array.isArray(children)) {
        for (let i = 0; i < children.length; i++) {
            children[i].children && children[i].children.length > 0 &&
                (res = moveDeepLevelChildrenToSec(children[i].children, res))

            children[i].children && children[i].children.length > 0 && (children[i].children = [])
            moveDeepLevelChildrenToSec(children[i], res)
        }
    } else {
        res.push(children)
    }
    return res
}
const permission = {
    state: {
        routers: [], //仅用于渲染导航菜单Sidebar
        addRouters: [], //用于真正的路由控制
        keepAliveData: ['forms']
    },
    mutations: {
        SET_ROUTERS: (state, { addRouters, routers }) => {

            state.addRouters = addRouters
            state.routers = routers
        },
        SAVE_KEEP_ALIVE_DATA: (state, data) => {
            data && data.length > 0 && data.forEach(item => {
                let code = groupData(item)
                code && (code = code.replace(code[0], code[0].toLowerCase()))
                code && code !== undefined && state.keepAliveData.push(code)
            })
        }
    },
    actions: {
        async GenerateRoutes({ commit, dispatch }, data) {
            const moduleRoutes = [] //动态模块路由
            await data.modules.forEach((value) => {
                moduleRoutes.push(moduleToRoute(value, dispatch))
            })

            //与静态路由组合成完整路由
            const routers = mergeModuleWithConst(deepClone([...moduleRoutes]), dispatch)

            //去掉路由中与constantRouterMap相同的项
            const noConstModuleRoutes = routers.find(item => item.name === 'layout')
                .children.filter(itemF => {
                    let constantRouter = !constantRouterMap
                        .find(item => item.name === 'layout').children.some(ele => ele.path === itemF.path)
                    return constantRouter
                })

            //todo:顶级的模块路由，把带component的路由添加到自己layout的子路由中？？？
            moduleRoutes.length > 0 && moduleRoutes.forEach(item => {
                    if (item.path.indexOf('/iframePage') <= -1 && item.children.length <= 0) {
                        item.children = noConstModuleRoutes.filter(router => router.path === item.path)
                    }
                })
                //把所有的2级以上路由都挂载到顶级layout下面，否则不能识别
            moduleRoutes.forEach(router => {
                moveDeepLevelToSec(router)
            })

            // todo:可以用最流氓的方法，全部转为一维数组😂
            // var arr=[];
            // const getAllItemsPerChildren = item => {
            //   arr.push(item);
            //   if (item.children.length>0) {
            //     return item.children.map(i => getAllItemsPerChildren(i));
            //   }
            // }
            //  moduleRoutes.forEach(router => {
            //   getAllItemsPerChildren(router)
            // })
            commit('SET_ROUTERS', { addRouters: moduleRoutes, routers: routers })
            commit('SAVE_KEEP_ALIVE_DATA', moduleRoutes)
        }
    },
    getters: {
        keepAliveData: state => state.keepAliveData
    }
}

export default permission