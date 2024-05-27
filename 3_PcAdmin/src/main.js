import Vue from 'vue'

import 'normalize.css/normalize.css' // A modern alternative to CSS resets

import ElementUI from 'element-ui'

import 'element-ui/lib/theme-chalk/index.css'
// import '@/assets/custom-theme/index.css'
import locale from 'element-ui/lib/locale/lang/zh-CN' // lang i18n
import VueContextMenu from 'vue-contextmenu'

import '@/styles/index.scss' // global css

import App from './App'
import router from './router'
import store from './store'

import '@/permission' // permission control

import '@/assets/public/css/comIconfont/iconfont.css'
import '@/assets/public/css/comIconfont/iconfont.js'

//工作流使用的图标
import '@/assets/public/css/workflowicon/iconfont.css'
import '@/assets/public/css/workflowicon/iconfont.js'

//自定义的图标
import '@/assets/public/css/myIconFont/iconfont.css'
import '@/assets/public/css/myIconFont/iconfont.js'

import '../public/ueditor/ueditor.config.js'
import '../public/ueditor/ueditor.all.js'
import '../public/ueditor/lang/zh-cn/zh-cn.js'
import '../public/ueditor/formdesign/leipi.formdesign.v4.js'

//表单验证
import * as validater from '@/utils/validater.js'
Vue.prototype.$validater = validater


// 请假条表单和详情

//富文本编辑
import VueQuillEditor from 'vue-quill-editor'
import 'quill/dist/quill.core.css'
import 'quill/dist/quill.snow.css'
import 'quill/dist/quill.bubble.css'
Vue.use(VueQuillEditor)

import iView from 'iview';
import 'iview/dist/styles/iview.css';
// Vue.use(VueRouter);
Vue.use(iView);

// 引入echarts
// import echarts from 'echarts'
// npm install echarts@5.2.2 --save
import * as echarts from 'echarts'
Vue.prototype.$echarts = echarts

//树状列表
// npm i vue-table-with-tree-grid -S
import TreeTable from 'vue-table-with-tree-grid'
Vue.config.productionTip = false
    //全局注册组件
Vue.component('tree-table', TreeTable)

//视频播放
import Video from 'video.js'
import 'video.js/dist/video-js.css'
Vue.prototype.$video = Video

import tools from '@/utils/tools'
Vue.prototype.tools = tools

Vue.use(ElementUI, { locale })
Vue.use(VueContextMenu)

import {
    // message,
    notification,
} from 'ant-design-vue'
// Vue.use(message)
Vue.use(notification)

//按钮权限
import { checkPermission } from '@/utils/util.js'
Vue.prototype.checkPermission = checkPermission

// // 引入组件库
import ant from 'ant-design-vue'
//import './core/lazy_use'
// 引入样式表
// Vue.prototype.$message = message

import 'ant-design-vue/dist/antd.css'
Vue.use(ant)

Vue.prototype.$notification = notification
Vue.prototype.$confirm = ElementUI.MessageBox.confirm

//打印
import Print from 'vue-print-nb'
Vue.use(Print)
Vue.prototype.$Print = Print

import {
    Message
} from 'element-ui'
Vue.use(Message)
Vue.prototype.$message = Message;

Vue.config.productionTip = false

new Vue({
    el: '#app',
    router,
    store,
    render: h => h(App)
})