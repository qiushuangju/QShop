<template>
  <div class="components">
    <a-collapse :defaultActiveKey="componentKeys" :bordered="false" expandIconPosition="right">
      <a-collapse-panel v-for="group in components" :key="group.key" :header="group.name">
        <div class="module-list">
          <div v-for="(item, index) in group.data" :key="index" class="module-item" @click="handleClickItem(item.type)">
            <!-- <span> {{ item.icon }}</span> -->
            <a-icon class="module-icon" :component="item.icon" />
            <span class="module-title">{{ item.name }}</span>
          </div>
        </div>
      </a-collapse-panel>
    </a-collapse>
  </div>
</template>

<script>
import * as Icon from "./icon";

const components = [
  {
    name: "媒体组件",
    key: "media",
    data: [
      {
        name: "轮播图",
        type: "banner",
        icon: Icon.banner,
      },
      {
        name: "图片",
        type: "image",
        icon: Icon.image,
      },
      {
        name: "图片橱窗",
        type: "window",
        icon: Icon.window,
      },
      {
        name: "视频",
        type: "video",
        icon: Icon.video,
      },
      {
        name: "文章",
        type: "article",
        icon: Icon.article,
      },
    ],
  },
  {
    name: "商城组件",
    key: "store",
    data: [
      {
        name: "搜索框",
        type: "search",
        icon: Icon.search,
      },
      {
        name: "店铺公告",
        type: "notice",
        icon: Icon.notice,
      },
      {
        name: "导航",
        type: "navBar",
        icon: Icon.navBar,
      },

      {
        name: "商品",
        type: "goods",
        icon: Icon.goods,
      },
      {
        name: "在线客服",
        type: "service",
        icon: Icon.service,
      },
    ],
  },
  {
    name: "其他组件",
    key: "other",
    data: [
      {
        name: "富文本",
        type: "richText",
        icon: Icon.richText,
      },
      {
        name: "辅助空白",
        type: "blank",
        icon: Icon.blank,
      },
      {
        name: "辅助线",
        type: "guide",
        icon: Icon.guide,
      },
    ],
  },
];

export default {
  data() {
    return {
      // Icon,
      components,
    };
  },
  computed: {
    componentKeys() {
      const { components } = this;
      return components.map((item) => item.key);
    },
  },
  methods: {
    handleClickItem(type) {
      this.$emit("handleClickItem", type);
    },
  },
};
</script>
<style lang="less" scoped>
// 组件库
.components {
  width: 310px;
  background: #f8f9fa;

  .module-list {
    display: flex;
    flex-wrap: wrap;
  }

  .module-item {
    width: 90px;
    height: 90px;
    padding: 15px 0;
    text-align: center;
    font-size: 12px;
    color: #595961;
    line-height: 20px;
    position: relative;
    user-select: none;

    &:hover {
      background: #fff;
      border-radius: 4px;
    }
    .module-icon {
      display: block;
      width: 74px;
      height: 45px;
      line-height: 45px;
      margin: 0 auto;
      font-size: 26px;
      color: #9797a1;
    }
  }
}
</style>
