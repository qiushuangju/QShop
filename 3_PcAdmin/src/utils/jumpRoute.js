
/**
 * 用于路由强制跳转。比如添加流程模板后，需要跳转到流程列表页面
 * @param {当前vue的this} _this 
 * @param {返回的路由路径} targetUrl 
 */
export function jump(_this, targetUrl){
    _this.$store.dispatch('delVisitedViews', _this.$route).then(() =>{
        _this.$store.dispatch('delVisitedViews', {path:targetUrl}).then(() =>{
          _this.$router.push(targetUrl)
        })
      })
}