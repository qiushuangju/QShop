import Vue from 'vue'
import Vuex from 'vuex'
import app from './modules/app'
import tenant from './modules/tenant'
import user from './modules/user'
import tagsView from './modules/tagsView'
import permission from './modules/permission'
import storage from './modules/storage'
import getters from './getters'

Vue.use(Vuex)
const store = new Vuex.Store({
    modules: {
        app,
        user,
        permission,
        storage,
        tagsView,
        tenant,
    },
    getters
})

export default store