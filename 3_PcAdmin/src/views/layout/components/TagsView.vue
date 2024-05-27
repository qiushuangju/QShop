<template>
  <div class="tags-view-container">
    <scroll-pane class='tags-view-wrapper' ref='scrollPane'>
      <router-link ref='tag' class="tags-view-item" :class="isActive(tag)?'active':''" v-for="tag in Array.from(visitedViews)" :to="tag" :key="tag.path" @contextmenu.prevent.native="openMenu(tag,$event)">
        {{editTitle(tag)}}
        <span class='el-icon-close' @click.prevent.stop='closeSelectedTag(tag)'></span>
      </router-link>
    </scroll-pane>
    <ul class='contextmenu' v-show="visible" :style="{left:left+'px',top:top+'px'}">
      <li @click="closeSelectedTag(selectedTag)">关闭</li>
      <li @click="closeOthersTags">关闭其他</li>
      <li @click="closeAllTags">全部关闭</li>
    </ul>
  </div>
</template>

<script>
import ScrollPane from "@/components/ScrollPane";
import { mapGetters } from "vuex";
import $ from "jquery";
export default {
  components: { ScrollPane },
  data() {
    return {
      visible: false,
      title: "",
      top: 0,
      left: 0,
      selectedTag: {},
    };
  },
  computed: {
    ...mapGetters(["iframeViews", "visitedViews"]),
  },
  watch: {
    $route() {
      // console.log("route1....");
      this.addViewTags();
      this.moveToCurrentTag();
    },
    visible(value) {
      if (value) {
        document.body.addEventListener("click", this.closeMenu);
      } else {
        document.body.removeEventListener("click", this.closeMenu);
      }
    },
  },
  mounted() {
    this.addViewTags();
  },
  methods: {
    generateRoute() {
      if (this.$route.name) {
        return this.$route;
      }
      return false;
    },
    editTitle(route) {
      var oldTitle = route.title;
      var path = route.path;
      var title = oldTitle;
      if (path.indexOf("dashboard") >= 0) {
        return title;
      }
      return title;
    },
    isActive(route) {
      return route.path === this.$route.path;
    },
    addViewTags() {
      const route = this.generateRoute();
      if (!route) {
        return false;
      }

      this.$store.dispatch("addVisitedViews", route);
    },
    RemoveChinese(strValue) {
      if (strValue != null && strValue != "") {
        var reg = /[\u4e00-\u9fa5]/g;
        return strValue.replace(reg, "");
      } else return "";
    },
    moveToCurrentTag() {
      const tags = this.$refs.tag;
      this.$nextTick(() => {
        for (const tag of tags) {
          if (tag.to.path === this.$route.path) {
            this.$refs.scrollPane.moveToTarget(tag.$el);
            break;
          }
        }
      });
    },
    closeSelectedTag(view) {
      // console.log(JSON.stringify(view));
      this.$store.dispatch("delVisitedViews", view).then((views) => {
        if (this.isActive(view)) {
          const latestView = views.slice(-1)[0];
          if (latestView) {
            this.$router.push(latestView);
          } else {
            this.$router.push("/");
          }
        }
      });
    },
    closeOthersTags() {
      this.$router.push(this.selectedTag);
      this.$store
        .dispatch("delOthersViews", this.selectedTag)
        .then(() => {
          this.moveToCurrentTag();
        });
    },
    closeAllTags() {
      this.$store.dispatch("delAllViews");
      this.$router.push("/");
    },
    openMenu(tag, e) {
      this.visible = true;
      this.selectedTag = tag;
      const offsetLeft = this.$el.getBoundingClientRect().left; // container margin left
      this.left = e.clientX - offsetLeft + 15; // 15: margin right
      this.top = 35;
    },
    closeMenu() {
      this.visible = false;
    },
  },
};
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.custom-theme {
  .tags-view-container {
    .tags-view-wrapper {
      .tags-view-item {
        &.active {
          color: #333;
          background-color: #efefef;
          border-color: #dcdfe6;
          &::before {
            background: #606266;
          }
        }
      }
    }
  }
}
.tags-view-container {
  margin-top: 45px;
  .tags-view-wrapper {
    background: #fff;
    height: 40px;
    border-bottom: 1px solid #d8dce5;
    box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.12), 0 0 3px 0 rgba(0, 0, 0, 0.04);
    .tags-view-item {
      display: inline-block;
      position: relative;
      height: 34px;
      line-height: 34px;
      border: 1px solid #d8dce5;
      color: #495060;
      background: #fff;
      padding: 0 10px;
      font-size: 12px;
      font-weight: 600;
      margin-left: 5px;
      margin-top: 6px;
      &.active {
        color: #fff;
        background-color: #409eff;
        border-color: #409eff;
        &::before {
          content: "";
          background: #fff;
          display: inline-block;
          width: 8px;
          height: 8px;
          border-radius: 50%;
          position: relative;
          margin-right: 2px;
        }
      }
    }
  }
  .contextmenu {
    margin: 0;
    background: #fff;
    z-index: 10000;
    position: absolute;
    list-style-type: none;
    padding: 5px 0;
    border-radius: 4px;
    font-size: 12px;
    font-weight: 400;
    color: #333;
    box-shadow: 2px 2px 3px 0 rgba(0, 0, 0, 0.3);
    li {
      margin: 0;
      padding: 7px 16px;
      cursor: pointer;
      &:hover {
        background: #eee;
      }
    }
  }
}
</style>

<style rel="stylesheet/scss" lang="scss">
.tags-view-wrapper {
  .tags-view-item {
    .el-icon-close {
      width: 16px;
      height: 16px;
      font-size: 16px;
      vertical-align: -2px;
      border-radius: 50%;
      text-align: center;
      transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
      transform-origin: 100% 50%;
      &:before {
        transform: scale(0.6);
        display: inline-block;
        // vertical-align: -3px;
      }
      &:hover {
        background-color: #b4bccc;
        color: #fff;
      }
    }
  }
}
</style>
