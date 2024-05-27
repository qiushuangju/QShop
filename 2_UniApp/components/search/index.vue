<template>
  <!-- 搜索框 -->
  <view class="search-wrapper">
    <view class="index-search" @click="onClick">
      <view class="index-cont-search t-c">
        <text class="search-icon iconfont icon-search"></text>
        <text class="search-text">{{ tips }}</text>
      </view>
    </view>
  </view>
</template>

<script>
  export default {
    props: {
      tips: {
        type: String,
        default: '搜索商品'
      }
    },
    data() {
      return {}
    },

    methods: {
      onClick() {
        this.$emit('event')
      }
    }
  }
</script>

<style lang="scss" scoped>
  .search-wrapper {
    background: #fff;
    padding: 13rpx;
  }

  .index-search {
    border-bottom: 0;
    background: #fff;
    border-radius: 50rpx;
    overflow: hidden;
    font-size: 28rpx;
    color: #6d6d6d;
    box-sizing: border-box;
    height: 64rpx;
    line-height: 64rpx;

    .index-cont-search {
      width: 100%;
      font-size: 28rpx;
      background: #f7f7f7;
      display: flex;
      justify-content: center;
      align-items: center;
    }

    .index-cont-search .search-icon {
      font-size: 28rpx;
    }

    .index-cont-search .search-text {
      margin-left: 14rpx;
    }

  }
</style>
