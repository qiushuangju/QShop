<template>
  <div class="app-wrapper" :class="classObj">
    <el-container class="flex-column">
      <el-header style="height:55px">
        <navbar></navbar>
      </el-header>
      <el-container class="flex-row flex-item">
        <sidebar class="sidebar-container"></sidebar>
        <div class="main-container flex-item">
          <tags-view class="custom-tags-view">11</tags-view>
          <app-main></app-main>
        </div>
      </el-container>
    </el-container>
  </div>
</template>

<script>
import { Navbar, Sidebar, AppMain, TagsView } from './components'
import ResizeMixin from './mixin/ResizeHandler'

export default {
  name: 'layout',
  components: {
    Navbar,
    Sidebar,
    AppMain,
    TagsView,
  },
  mixins: [ResizeMixin],
  computed: {
    sidebar() {
      return this.$store.state.app.sidebar
    },
    device() {
      return this.$store.state.app.device
    },
    classObj() {
      return {
        hideSidebar: !this.sidebar.opened,
        openSidebar: this.sidebar.opened,
        withoutAnimation: this.sidebar.withoutAnimation,
        mobile: this.device === 'mobile',
      }
    },
  },
  methods: {
    handleClickOutside() {
      this.$store.dispatch('CloseSideBar', { withoutAnimation: false })
    },
  },
}
</script>
<style lang="less" scoped>
</style>
<style rel="stylesheet/scss" lang="scss" scoped>
@import "src/styles/mixin.scss";

.app-wrapper {
  @include clearfix;
  position: relative;
  height: 100%;
  width: 100%;
  &.mobile.openSidebar {
    position: fixed;
    top: 0;
  }
}
.drawer-bg {
  background: #000;
  opacity: 0.3;
  width: 100%;
  top: 0;
  height: 100%;
  position: absolute;
  z-index: 999;
}
.el-header {
  padding: 0;
  height: 55px;
  background-color: #001529;
  width: 100%;
  z-index: 100;
}
.el-container .sidebar-container {
  height: auto !important;
}

.el-container .main-container {
  margin-left: 0 !important;
  overflow: hidden;
}
.custom-tags-view {
  margin-top: 0 !important;
}
</style>
