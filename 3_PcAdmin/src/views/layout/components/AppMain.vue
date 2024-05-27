<template>
  <section class="app-main" ref="appMain">
    <transition name="fade-transform" mode="out-in">
      <keep-alive :include="keepAliveDatas" v-if="keepAliveDatas.length > 0">
        <router-view :key="key"></router-view>
      </keep-alive>
      <router-view :key="key" v-else></router-view>
    </transition>
  </section>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'AppMain',
  computed: {
    ...mapGetters({
      keepAliveData: 'keepAliveData'
    }),
    keepAliveDatas() {
      console.log("kkkkk", this.keepAliveData)
      return this.keepAliveData || []
    },
    cachedViews() {
      return this.$store.state.tagsView.cachedViews
    },
    key() {
      return this.$route.fullPath
    }
  },
  watch: {
    $route() {
      this.$refs.appMain.scrollTop = 0
    }
  }
}
</script>

<style scoped>
.app-main {
  width: 100%;
  height: calc(100% - 35px);
  position: relative;
  overflow: auto;
  background-color: #efefef;
  box-sizing: border-box;
}
</style>
