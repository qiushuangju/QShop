<template>
  <view class="container">
    <!-- 订单页列表 -->
    <view class="order-list">
      <view class="order-item" v-for="(item, index) in orderList" :key="index" @click="$navTo(item.path)">
        <view class="order-item-icon" :style="{ color: item.color }">
          <text class="iconfont" :class="item.icon"></text>
        </view>
        <view class="order-item-name">{{ item.name }}</view>
        <view class="icon-arrow">
          <text class="iconfont icon-arrow-right"></text>
        </view>
      </view>
    </view>
    <!-- 操作按钮 -->
    <view class="footer">
      <view class="btn-wrapper">
        <view class="btn-item btn-item-main" @click="$navTo('pages/index/index')">
          <view class="btn-item-icon">
            <text class="iconfont icon-shouye2"></text>
          </view>
          <view class="btn-item-name">
            <text>返回首页</text>
          </view>
        </view>
      </view>
    </view>
  </view>
</template>

<script>
  import * as UserApi from '@/api/user/user'

  // 订单页数据
  const orderList = [{
      name: '商城订单',
      icon: 'icon-quanbudingdan',
      color: 'rgb(253 65 0)',
      path: 'pages/order/index'
    },
    {
      name: '充值订单',
      icon: 'icon-jifen',
      color: 'rgb(191, 150, 7)',
      path: 'pages/wallet/recharge/order'
    }
  ]

  export default {
    data() {
      return {
        orderList
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {
      // 获取当前用户信息
      this.getUserInfo()
    },

    methods: {

      // 获取当前用户信息（验证是否登录）
      getUserInfo() {
        const app = this
        UserApi.getDetail()
      },

    }
  }
</script>

<style lang="scss" scoped>
  .order-list {
    padding: 0 25rpx;
    background-color: #fff;

    .order-item {
      position: relative;
      padding: 26rpx 16rpx;
      border-bottom: 1rpx solid #eee;
      display: flex;
      justify-content: flex-start;
      align-items: center;
      font-size: 30rpx;

      &:last-child {
        border-bottom: none;
      }
    }

    .order-item-icon {
      font-size: 34rpx;
      margin-right: 18rpx;
    }

    .icon-arrow {
      position: absolute;
      top: 26rpx;
      right: 6rpx;
    }
  }

  // 底部操作栏
  .footer {
    position: fixed;
    bottom: var(--window-bottom);
    left: 0;
    right: 0;
    z-index: 11;
    box-shadow: 0 -4rpx 40rpx 0 rgba(151, 151, 151, 0.24);
    background: #fff;

    // 设置ios刘海屏底部横线安全区域
    padding-bottom: constant(safe-area-inset-bottom);
    padding-bottom: env(safe-area-inset-bottom);

    .btn-item {
      font-size: 30rpx;
      height: 90rpx;
      border-radius: 50rpx;
      display: flex;
      justify-content: center;
      align-items: center;
    }

    .btn-item-icon {
      font-size: 34rpx;
      margin-right: 18rpx;
    }

  }
</style>
