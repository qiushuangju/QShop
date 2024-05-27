<template>
  <view v-if="!isLoading" class="empty-content" :style="customStyle">
    <view class="empty-icon">
      <image class="image" src="/static/empty.png" mode="widthFix"></image>
    </view>
    <view class="tips">{{ tips }}</view>
    <slot name="slot"></slot>
  </view>
</template>

<script>
  export default {

    /**
     * 组件的属性列表
     * 用于组件自定义设置
     */
    props: {
      // 正在加载
      isLoading: {
        type: Boolean,
        default: true
      },
      // 自定义样式
      customStyle: {
        type: Object,
        default () {
          return {}
        }
      },
      // 提示的问题
      tips: {
        type: String,
        default: '亲,暂无数据'
      }
    },

    data() {
      return {}
    },

    methods: {

    }
  }
</script>

<style lang="scss" scoped>
  .empty-content {
    box-sizing: border-box;
    width: 100%;
    padding: 180rpx 50rpx;
    text-align: center;

    .tips {
      font-size: 28rpx;
      color: gray;
      margin: 50rpx 0;
    }

    .empty-icon .image {
      width: 220rpx;
    }

  }
</style>
