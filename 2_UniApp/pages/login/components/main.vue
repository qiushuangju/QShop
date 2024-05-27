<template>
  <view class="container">

    <!-- 页面头部 -->
    <view class="header">
      <view class="title">
        <text>手机号登录</text>
      </view>
      <view class="sub-title">
        <text>未注册的手机号登录后将自动注册</text>
      </view>
    </view>
    <!-- 表单 -->
    <view class="login-form">
      <!-- 手机号 -->
      <view class="form-item">
        <input class="form-item--input" type="number" v-model="mobile" maxlength="11" placeholder="请输入手机号码" />
      </view>
      <!-- 图形验证码 -->
      <view class="form-item">
        <input class="form-item--input" type="text" v-model="imageCaptchaCode" maxlength="5" placeholder="请输入图形验证码" />
        <view class="form-item--parts">
          <view class="captcha" @click="getCaptcha()">
            <image class="image"  :src="'data:image/png;base64,'+ imageCaptcha.base64Str" ></image>
          </view>
        </view>
      </view>
      <!-- 短信验证码 -->
      <view class="form-item">
        <input class="form-item--input" type="number" v-model="smsCode" maxlength="6" placeholder="请输入短信验证码" />
        <view class="form-item--parts">
          <view class="captcha-sms" @click="handelSmsCaptcha()">
            <text v-if="!smsState" class="activate">获取验证码</text>
            <text v-else class="un-activate">重新发送({{ times }})秒</text>
          </view>
        </view>
      </view>
      <!-- 登录按钮 -->
      <view class="login-button" @click="handleLogin">
        <text>登录</text>
      </view>
    </view>

    <!-- 微信授权手机号一键登录 -->
    <!-- #ifdef MP-WEIXIN -->
    <MpWeixinMobile :isParty="true" :partyData="partyData" />
    <!-- #endif -->

  </view>
</template>

<script>
  import store from '@/store'
  import * as LoginApi from '@/api/login'
  import * as CaptchaApi from '@/api/sys/captcha'
  import * as Verify from '@/utils/verify'
  import MpWeixinMobile from './mp-weixin-mobile'

  // 倒计时时长(秒)
  const times = 60

  // 表单验证场景
  const GetCaptcha = 10
  const SubmitLogin = 20

  export default {
    components: {
      MpWeixinMobile
    },

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
        // 正在加载
        isLoading: false,
        // 图形验证码信息
        imageCaptcha: {},
		//短信验证码信息
		smsCaptcha:{},
        // 短信验证码发送状态
        smsState: false,
        // 倒计时
        times,
        // 手机号
        mobile: '',
        // 图形验证码
        imageCaptchaCode: '',
        // 短信验证码
        smsCode: ''
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    created() {
      console.log('main created')
      // 获取图形验证码
      this.getCaptcha()
    },

    methods: {

      // 获取图形验证码
      getCaptcha() {
        const app = this
        CaptchaApi.getImageCaptcha()
          .then(res => {
			  app.imageCaptcha = res.result
			  console.log("1111111111",app.imageCaptcha)
		  })
      },

      // 点击发送短信验证码
      handelSmsCaptcha() {
        const app = this
        if (!app.isLoading && !app.smsState && app.formValidation(GetCaptcha)) {
          app.sendSmsCaptcha()
        }
      },

      // 表单验证
      formValidation(scene = GetCaptcha) {
        const app = this
        // 验证获取短信验证码
        if (scene === GetCaptcha) {
          if (!app.validteMobile(app.mobile) || !app.validteImageCaptchaCode(app.imageCaptchaCode)) {
            return false
          }
        }
        // 验证提交登录
        if (scene === SubmitLogin) {
          if (!app.validteMobile(app.mobile) || !app.validteSmsCode(app.smsCode)) {
            return false
          }
        }
        return true
      },

      // 验证手机号
      validteMobile(str) {
        if (Verify.isEmpty(str)) {
          this.$toast('请先输入手机号')
          return false
        }
        if (!Verify.isMobile(str)) {
          this.$toast('请输入正确格式的手机号')
          return false
        }
        return true
      },

      // 验证图形验证码
      validteImageCaptchaCode(str) {
        if (Verify.isEmpty(str)) {
          this.$toast('请先输入图形验证码')
          return false
        }
        return true
      },

      // 验证短信验证码
      validteSmsCode(str) {
        if (Verify.isEmpty(str)) {
          this.$toast('请先输入短信验证码')
          return false
        }
        return true
      },

      // 请求发送短信验证码接口
      sendSmsCaptcha() {
        const app = this
        app.isLoading = true
        CaptchaApi.sendSmsCaptcha({
              verifyCodeId: app.imageCaptcha.verifyCodeId,
              verifyCode: app.imageCaptchaCode,
              Mobile: app.mobile
          })
          .then(res => {
            // 显示发送成功
            app.$toast(res.message)
			app.smsCaptcha=res.result;
            // 执行定时器
            app.timer()
          })
          .catch(() => app.getCaptcha())
          .finally(() => app.isLoading = false)
      },

      // 执行定时器
      timer() {
        const app = this
        app.smsState = true
        const inter = setInterval(() => {
          app.times = app.times - 1
          if (app.times <= 0) {
            app.smsState = false
            app.times = times
            clearInterval(inter)
          }
        }, 1000)
      },

      // 点击登录
      handleLogin() {
        const app = this
        if (!app.isLoading && app.formValidation(SubmitLogin)) {
          app.submitLogin()
        }
      },

      // 确认登录
      submitLogin() {
        const app = this
        app.isLoading = true
        store.dispatch('LoginByPhoneCode', {
			smsVerifyCodeId:app.smsCaptcha.verifyCodeId,
            smsCode: app.smsCode,
            Mobile: app.mobile,
            isParty: app.isParty,
            partyData: app.partyData
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
			  console.log("err",err)
            // 跳转回原页面
            if (err.result.data.isBack) {
              setTimeout(() => app.onNavigateBack(1), 2000)
            }
          })
          .finally(() => app.isLoading = false)
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
  .container {
    padding: 100rpx 60rpx;
    min-height: 100vh;
    background-color: #fff;
  }

  // 页面头部
  .header {
    margin-bottom: 60rpx;

    .title {
      color: #191919;
      font-size: 54rpx;
    }

    .sub-title {
      margin-top: 20rpx;
      color: #b3b3b3;
      font-size: 28rpx;
    }
  }

  // 输入框元素
  .form-item {
    display: flex;
    padding: 18rpx;
    border-bottom: 1rpx solid #f3f1f2;
    margin-bottom: 30rpx;
    height: 96rpx;

    &--input {
      font-size: 28rpx;
      letter-spacing: 1rpx;
      flex: 1;
      height: 100%;
    }

    &--parts {
      min-width: 100rpx;
      height: 100%;
    }

    // 图形验证码
    .captcha {
      height: 100%;

      .image {
        display: block;
        width: 192rpx;
        height: 100%;
      }
    }

    // 短信验证码
    .captcha-sms {
      font-size: 28rpx;
      line-height: 50rpx;
      padding-right: 20rpx;

      .activate {
        color: #cea26a;
      }

      .un-activate {
        color: #9e9e9e;
      }
    }
  }


  // 登录按钮
  .login-button {
    width: 100%;
    height: 86rpx;
    margin-top: 80rpx;
    background: linear-gradient(to right, #ecb53c, #ff9211);
    color: #fff;
    border-radius: 80rpx;
    box-shadow: 0px 10px 20px 0px rgba(0, 0, 0, 0.1);
    letter-spacing: 5rpx;
    display: flex;
    justify-content: center;
    align-items: center;
  }
</style>
