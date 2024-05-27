const app = {
  state: {
    typeDataLists: [],
    typeIds: [],
    count: 0
  },
  mutations: {
    SAVE_TYPE_DATA_LISTS: (state, data) => {
      state.typeDataLists.push(data)
    },
    SAVE_TYPE_IDS: (state, data) => {
      state.typeIds.filter(item => item !== data)
      state.typeIds.push(data)
      state.count++
    },
    CLEAR_TYPE_DATAS: (state) => {
      state.count--
      if (state.count > 0) {
        return
      }
      state.count = 0
      state.typeIds = []
      state.typeDataLists = []
    }
  },
  actions: {
    saveTypeDataLists: ({ commit }, data) => {
      commit('SAVE_TYPE_DATA_LISTS', data)
    },
    saveTypeIds: ({ commit }, data) => {
      commit('SAVE_TYPE_IDS', data)
    },
    clearTypeDatas: ({ commit }, data) => {
      commit('CLEAR_TYPE_DATAS', data)
    }
  },
  getters: {
    typeDataLists: state => state.typeDataLists,
    typeIds: state => state.typeIds
  }
}

export default app
