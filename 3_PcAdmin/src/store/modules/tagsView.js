import { local } from '@/utils'
const tagsView = {
  state: {
    visitedViews: [],
    cachedViews: [],
    iframeViews: {}
  },
  mutations: {
    ADD_VISITED_VIEWS: (state, view) => {
      if(state.visitedViews.length <= 0) {
        state.visitedViews = local.getItem('visitedViews') || []
      }
      const route = {
        name: view.name,
        path: view.path,
        params: view.params,
        meta: view.meta
      }
      if (state.visitedViews.some(v => v.path === route.path)) return
      state.visitedViews.push(Object.assign({}, route, {
        title: route.name === 'iframePage' ? state.iframeViews[route.params.code].name : route.meta.title || 'no-name'
      }))
      if (!route.meta.noCache) {
        state.cachedViews.push(route.name)
      }
      local.setItem('visitedViews', state.visitedViews)
    },
    DEL_VISITED_VIEWS: (state, view) => {
      for (const [i, v] of state.visitedViews.entries()) {
        if (v.path === view.path) {
          state.visitedViews.splice(i, 1)
          local.setItem('visitedViews', state.visitedViews)
          break
        }
      }
      for (const i of state.cachedViews) {
        if (i === view.name) {
          const index = state.cachedViews.indexOf(i)
          state.cachedViews.splice(index, 1)
          break
        }
      }
    },
    DEL_OTHERS_VIEWS: (state, view) => {
      for (const [i, v] of state.visitedViews.entries()) {
        if (v.path === view.path) {
          state.visitedViews = state.visitedViews.slice(i, i + 1)
          local.setItem('visitedViews', state.visitedViews)
          break
        }
      }
      for (const i of state.cachedViews) {
        if (i === view.name) {
          const index = state.cachedViews.indexOf(i)
          state.cachedViews = state.cachedViews.slice(index, index + 1)
          break
        }
      }
    },
    DEL_ALL_VIEWS: (state) => {
      state.visitedViews = []
      state.cachedViews = []
      local.setItem('visitedViews', state.visitedViews)
    },
    SET_IFRAME_TAGVIEWS(state, data){
      state.iframeViews = { ...state.iframeViews, ...data }
    }
  },
  actions: {
    setIframeTagViews({ commit }, data) {
      commit('SET_IFRAME_TAGVIEWS', data)
    },
    addVisitedViews({ commit }, view) {
      commit('ADD_VISITED_VIEWS', view)
    },
    delVisitedViews({ commit, state }, view) {
      return new Promise((resolve) => {
        commit('DEL_VISITED_VIEWS', view)
        resolve([...state.visitedViews])
      })
    },
    delOthersViews({ commit, state }, view) {
      return new Promise((resolve) => {
        commit('DEL_OTHERS_VIEWS', view)
        resolve([...state.visitedViews])
      })
    },
    delAllViews({ commit, state }) {
      return new Promise((resolve) => {
        commit('DEL_ALL_VIEWS')
        resolve([...state.visitedViews])
      })
    }
  },
  getters: {
    iframeViews: state => state.iframeViews
  }
}

export default tagsView
