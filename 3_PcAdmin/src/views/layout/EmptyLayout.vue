<template>
  <div class="app-wrapper" :class="classObj">
    <el-container class="flex-column">
      <el-container class="flex-row flex-item">
        <app-main></app-main>
      </el-container>
    </el-container>
  </div>
</template>

<script>
import AppMain from "./components/AppMain";

export default {
  name: "layout",
  components: {
    AppMain,
  },
  computed: {
    sidebar() {
      return this.$store.state.app.sidebar;
    },
    classObj() {
      return {
        hideSidebar: !this.sidebar.opened,
        openSidebar: this.sidebar.opened,
        withoutAnimation: this.sidebar.withoutAnimation,
        mobile: this.device === "mobile",
      };
    },
  },
  mounted() { },
  methods: {},
};
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
@import "src/styles/mixin.scss";
:v-deep(.app-main) {
  height: 100%;
}
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
  // line-height: 44px;
  background-color: #333;
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
