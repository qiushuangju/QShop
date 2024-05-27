<template>
  <!-- 微信授权手机号一键登录 -->
  <view class="wechat-auth">
    <button class="btn-normal" open-type="getPhoneNumber" @getphonenumber="handelMpWeixinMobileLogin($event)" @click="clickPhoneNumber">
      <view class="wechat-auth-container">
        <image class="icon" src="../../../static/channel/wechat.png"></image>
        <text class="title">手机号快捷登录</text>
      </view>
    </button>
  </view>
</template>

<script>
  import store from '@/store'
  import { isEmpty } from '@/utils/util'

  export default {
    props: {
      // 是否存在第三方用户信息
      isParty: {
        type: Boolean,
        default: () => false
      },
      // 第三方用户信息数据
      partyData: {
        type: Object
      }
    },

    data() {
      return {
        // 微信小程序登录凭证 (code)
        // 提交到后端，用于换取openid
        code: ''
      }
    },

    methods: {

      // 按钮点击事件: 获取微信手机号按钮
      // 实现目的: 在getphonenumber事件触发之前获取微信登录code
      // 因为如果在getphonenumber事件中获取code的话,提交到后端的encryptedData会存在解密不了的情况
      async clickPhoneNumber() {
        this.code = await this.getCode()
      },

      // 微信授权获取手机号一键登录
      // getphonenumber事件的回调方法
      async handelMpWeixinMobileLogin({ detail }) {
        const app = this
        if (detail.errMsg != 'getPhoneNumber:ok') {
          console.log('微信授权获取手机号失败', detail.errMsg)
          // app.$error(detail.errMsg)
          return
        }
		console.log("getphonenumber",detail)
        app.isLoading = true
        store.dispatch('bindWxPhone', {
            wxCode: detail.code,
            // encryptedData: detail.encryptedData,
            // iv: detail.iv,
            // isParty: app.isParty,
            // partyData: app.partyData,
            refereeId: store.getters.refereeId
          })
          .then(result => {
            // 显示登录成功
            app.$toast(result.message)
            // 相应全局事件订阅: 刷新上级页面数据
            uni.$emit('syncRefresh', true)
            // 跳转回原页面
            setTimeout(() => app.onNavigateBack(1), 2000)
          })
          .catch(err => {
            const resultData = err.result.data
            // 显示错误信息
            if (isEmpty(resultData)) {
              app.$toast(err.result.message)
            }
            // 跳转回原页面
            if (resultData.isBack) {
              setTimeout(() => app.onNavigateBack(1), 2000)
            }
          })
          .finally(() => app.isLoading = false)
      },

      // 获取微信登录的code
      // https://developers.weixin.qq.com/miniprogram/dev/api/open-api/login/wx.login.html
      getCode() {
        return new Promise((resolve, reject) => {
          uni.login({
            provider: 'weixin',
            success: res => {
              console.log('code', res.code)
              resolve(res.code)
            },
            fail: reject
          })
        })
      },

      /**
       * 登录成功-跳转回原页面
       */
      onNavigateBack(delta = 1) {
        const pages = getCurrentPages()
        if (pages.length > 1) {
          uni.navigateBack({
            delta: Number(delta || 1)
          })
        } else {
          this.$navTo('pages/index/index')
        }
      }

    }
  }
</script>

<style lang="scss" scoped>
  // 微信授权登录
  .wechat-auth {
    width: 320rpx;
    margin: 50rpx auto 0 auto;

    .wechat-auth-container {
      display: flex;
      justify-content: center;
    }

    .icon {
      width: 38rpx;
      height: 38rpx;
      margin-right: 15rpx;
    }

    .title {
      font-size: 28rpx;
      color: #666666;
    }
  }
</style>
