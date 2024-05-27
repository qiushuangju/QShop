<template>
  <view class="container">

    <!-- 页面头部 -->
    <view class="header">
      <view class="title">
        <text>绑定您的手机号</text>
      </view>
      <view class="sub-title">
        <text>为了更好的服务您，请绑定手机号</text>
      </view>
    </view>
    <!-- 表单 -->
    <view class="submit-form">
      <!-- 手机号 -->
      <view class="form-item">
        <input class="form-item--input" type="number" v-model="mobile" maxlength="11" placeholder="请输入手机号码" />
      </view>
      <!-- 图形验证码 -->
      <view class="form-item">
        <input class="form-item--input" type="text" v-model="captchaCode" maxlength="5" placeholder="请输入图形验证码" />
        <view class="form-item--parts">
          <view class="captcha" @click="getCaptcha()">
            <image class="image" :src="captcha.base64"></image>
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
      <!-- 确认绑定 -->
      <view class="submit-button" @click="handleSubmit()">
        <text>确认绑定</text>
      </view>
    </view>

  </view>
</template>

<script>
  import store from '@/store'
  import * as UserApi from '@/api/user/user'
  import * as CaptchaApi from '@/api/sys/captcha'
  import * as Verify from '@/utils/verify'

  // 倒计时时长(秒)
  const times = 60

  // 表单验证场景
  const GET_CAPTCHA = 10
  const FORM_SUBMIT = 20

  export default {

    data() {
      return {
        // 正在加载
        isLoading: false,
        // 图形验证码信息
        captcha: {},
        // 短信验证码发送状态
        smsState: false,
        // 倒计时
        times,
        // 手机号
        mobile: '',
        // 图形验证码
        captchaCode: '',
        // 短信验证码
        smsCode: ''
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    created() {
      // 获取图形验证码
      this.getCaptcha()
    },

    methods: {

      // 获取图形验证码
      getCaptcha() {
        const app = this
        CaptchaApi.image().then(result => app.captcha = result.data)
      },

      // 点击发送短信验证码
      handelSmsCaptcha() {
        const app = this
        if (!app.isLoading && !app.smsState && app.formValidation(GET_CAPTCHA)) {
          app.sendSmsCaptcha()
          app.getCaptcha()
        }
      },

      // 表单验证
      formValidation(scene = GET_CAPTCHA) {
        const app = this
        // 验证获取短信验证码
        if (scene === GET_CAPTCHA) {
          if (!app.validteMobile(app.mobile) || !app.validteCaptchaCode(app.captchaCode)) {
            return false
          }
        }
        // 验证表单提交
        if (scene === FORM_SUBMIT) {
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
      validteCaptchaCode(str) {
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
            form: {
              captchaKey: app.captcha.key,
              captchaCode: app.captchaCode,
              mobile: app.mobile
            }
          })
          .then(result => {
            // 显示发送成功
            app.$toast(result.message)
            // 执行定时器
            app.timer()
          })
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

      // 点击提交
      handleSubmit() {
        const app = this
        if (!app.isLoading && app.formValidation(FORM_SUBMIT)) {
          app.onSubmitEvent()
        }
      },

      // 确认提交事件
      onSubmitEvent() {
        const app = this
        app.isLoading = true
        UserApi.bindMobile({ form: { smsCode: app.smsCode, mobile: app.mobile } })
          .then(result => {
            // 显示操作成功
            app.$toast(result.message)
            // 跳转回原页面
            setTimeout(() => {
              app.onNavigateBack(1)
            }, 2000)
          })
          .finally(() => app.isLoading = false)
      },

      /**
       * 提交成功-跳转回原页面
       */
      onNavigateBack(delta) {
        uni.navigateBack({
          delta: Number(delta || 1)
        })
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
    margin-bottom: 50rpx;

    .title {
      color: #191919;
      font-size: 50rpx;
    }

    .sub-title {
      margin-top: 20rpx;
      color: #b3b3b3;
      font-size: 25rpx;
    }
  }

  // 输入框元素
  .form-item {
    display: flex;
    padding: 18rpx;
    border-bottom: 1rpx solid #f3f1f2;
    margin-bottom: 25rpx;
    height: 96rpx;

    &--input {
      font-size: 26rpx;
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
      font-size: 22rpx;
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


  // 提交按钮
  .submit-button {
    width: 100%;
    height: 86rpx;
    margin-top: 70rpx;
    // background: #cea26a;
    background: linear-gradient(to right, #ecb53c, #ff9211);
    text-align: center;
    line-height: 86rpx;
    color: #fff;
    border-radius: 80rpx;
    box-shadow: 0px 10px 20px 0px rgba(0, 0, 0, 0.1);
    letter-spacing: 5rpx;
  }
</style>
