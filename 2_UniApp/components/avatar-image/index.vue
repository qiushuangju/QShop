<template>
  <view class="avatar-image">
    <image class="image"
      :style="{ width: `${width}rpx`, height: `${width}rpx`, borderWidth: `${borderWidth}rpx`, borderColor: borderColor }"
      :src="url ? url : '/static/default-avatar.png'"></image>
  </view>
</template>

<script>
  export default {

    /**
     * 组件的属性列表
     * 用于组件自定义设置
     */
    props: {
      url: {
        type: String,
        default: ''
      },
      width: {
        type: Number,
        default: 90
      },
      borderWidth: {
        type: Number,
        default: 0
      },
      borderColor: {
        type: String,
        default: '#000000'
      }
    },

    data() {
      return {

      }
    },

    methods: {

    }
  }
</script>

<style lang="scss" scoped>
  .avatar-image {
    .image {
      display: block;
      width: 60rpx;
      height: 60rpx;
      border-radius: 50%;
      border-style: solid;
    }
  }
</style>
