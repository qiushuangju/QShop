const getters = {
    sidebar: state => state.app.sidebar,
    device: state => state.app.device,
    token: state => state.user.token,
    name: state => state.user.name,
    modules: state => state.user.modules,
    visitedViews: state => state.tagsView.visitedViews,
    cachedViews: state => state.tagsView.cachedViews,
    addRouters: state => state.permission.addRouters,
    permission_routers: state => state.permission.routers,
    tenantid: state => state.tenant.id,
    publicConfig: () => window.publicConfig
}
export default getters