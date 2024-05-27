const tenant = {
    state: {
        id: 'QsDBContext' // 默认租户Id
    },

    mutations: {
        SET_ID: (state, id) => {
            state.id = id
        }
    },

    actions: {
        setTenantId: ({ commit }, id) => {
            commit('SET_ID', id)
        },
    }
}

export default tenant