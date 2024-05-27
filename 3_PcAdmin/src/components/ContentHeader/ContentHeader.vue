<template>
  <div class="content-header">
    <div class="widget-head">
      <div class="widget-title">{{ title }}</div>
    </div>
  </div>
</template>

<script>

export default {
  name: 'ContentHeader',
  props: {
    title: {
      type: String,
      default: null
    }
  },
  data() {
    return {}
  },
  mounted() {
  },
  methods: {
  }
}
</script>

<style lang="less" scoped>
@import "../index.less";

.widget-head {
  width: 100%;
  padding: 5px 0 14px 20px;
  border-bottom: 1px solid #eef1f5;
  margin-bottom: 20px;
  .widget-title {
    position: relative;
    font-size: 14px;
    &::before {
      content: "";
      position: absolute;
      width: 4px;
      height: 16px;
      background: @primary-color;
      top: 4px;
      left: -15px;
    }
  }
}
</style>
