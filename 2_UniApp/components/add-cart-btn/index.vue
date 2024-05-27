<template>
  <view class="add-cart" @click.stop="handleAddCart">
    <text class="icon iconfont" :class="[`icon-jiagou${btnStyle}`]"></text>
  </view>
</template>

<script>
  export default {
    props: {
      // 购物车按钮样式 1 2 3
      btnStyle: {
        type: Number,
        default: 1
      },
    },
    data() {
      return {
        value: false,
        goodsInfo: {}
      }
    },
    methods: {
      handleAddCart() {
        this.$emit('click')
      }
    }
  }
</script>

<style lang="scss" scoped>
  .add-cart {
    font-size: 38rpx;
    padding: 0 20rpx;
    color: #fa2209;
  }
</style>
