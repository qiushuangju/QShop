<template>
  <view class="container" :style="appThemeStyle">
    <!-- 标题 -->
    <view class="page-title">个人信息</view>
    <!-- 表单组件 -->
    <view class="form-wrapper">
      <u-form :model="form" ref="uForm" label-width="140rpx">
        <u-form-item label="头像">
          <button class="btn-normal" open-type="chooseAvatar" @click="onClickAvatar()" @chooseavatar="onChooseAvatar">
            <avatar-image :url="avatarUrl" :width="100" />
          </button>
        </u-form-item>
        <u-form-item label="昵称" prop="nickName">
          <u-input v-model="form.nickName" type="nickname" maxlength="30" placeholder="请输入昵称" @input="onInputNickName"
            @blur="onInputNickName" />
        </u-form-item>
      </u-form>
    </view>
    <!-- 操作按钮 -->
    <view class="footer">
      <view class="btn-wrapper">
        <view class="btn-item btn-item-main" :class="{ disabled }" @click="handleSubmit()">保存</view>
      </view>
    </view>
	
	<!-- 退出登录 -->
	<view v-if="isLogin" class="my-logout">
	  <view class="logout-btn" @click="handleLogout()">
	    <text>退出登录</text>
	  </view>
	</view>
  </view>
</template>

<script>
  import store from '@/store'
  import AvatarImage from '@/components/avatar-image'
  import * as UserApi from '@/api/user/user'
  import * as UploadApi from '@/api/upload'
  import { checkLogin } from '@/core/app'
  // 表单验证规则
  const rules = {
    nickName: [{
      required: true,
      message: '请输入用户昵称',
      trigger: ['blur', 'change']
    }]
  }

  export default {
    components: {
      AvatarImage
    },
    data() {
      return {
		  isLogin:false,
        // 按钮禁用
        disabled: false,
        // 头像路径 (用于显示)
        avatarUrl: '',
        // 临时图片 (用于上传)
        tempFile: null,
        // 表单数据
        form: {
          avatarId: '',
          nickName: ''
        },
        // 验证规则
        rules,
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad() {
		// 判断是否已登录
		this.isLogin = checkLogin()
		console.log("111",this.isLogin)
      // 获取当前用户信息
      this.getUserInfo()
    },

    // 必须要在onReady生命周期，因为onLoad生命周期组件可能尚未创建完毕
    onReady() {
      this.$refs.uForm.setRules(this.rules)
    },

    methods: {
		
		// 获取当前用户信息
		getUserInfo() {
		  const app = this
		  return new Promise((resolve, reject) => {
		    !app.isLogin ? resolve(null) : UserApi.getDetail({}, { load: app.isFirstload })
		      .then(res => {
		        var user = res.result
				
				      app.avatarUrl = user.urlAvater
				      
				      app.form.nickName = user.nickName
				app.form.avatarId = user.avatarId
				
				
		        resolve(user)
		      })
		      .catch(err => {
		        if (err.res && err.res.status == 401) {
		          resolve(null)
		        } else {
		          reject(err)
		        }
		      })
		  })
		},
		// 退出登录
		handleLogout() {
		  const app = this
		  uni.showModal({
		    title: '友情提示',
		    content: '您确定要退出登录吗?',
		    success(res) {
		      if (res.confirm) {
		        store.dispatch('Logout', {})
		          .then(result => app.onRefreshPage())
		      }
		    }
		  })
		},
		

      // // 获取当前用户信息
      // getUserInfo() {
      //   const app = this
      //   UserApi.getDetail()
      //     .then(result => {
      //   
      //     })
      // },

      // 点击头像按钮事件
      onClickAvatar() {
        // #ifdef MP-WEIXIN
        return
        // #endif
        this.chooseImage()
      },

      // 选择头像事件 - 仅限微信小程序
      // #ifdef MP-WEIXIN
      onChooseAvatar({ detail }) {
        const app = this
        app.avatarUrl = detail.avatarUrl
        app.tempFile = { path: app.avatarUrl }
      },
      // #endif

      // 选择图片
      chooseImage() {
        const app = this
        // 选择图片
        uni.chooseImage({
          count: 1,
          sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
          sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
          success({ tempFiles }) {
            app.tempFile = tempFiles[0]
            app.avatarUrl = app.tempFile.path
          }
        });
      },

      // 上传图片
      uploadFile() {
        const app = this
        return UploadApi.image([app.tempFile])
          .then(fileIds => {
            app.form.avatarId = fileIds[0]
            app.tempFile = null
            return true
          })
          .catch(() => {
            app.disabled = false
            return false
          })
      },

      // 确认修改
      async handleSubmit() {
        const app = this
        // 判断是否重复提交
        if (app.disabled === true) return
        app.$refs.uForm.validate(async valid => {
          if (!valid) return
          // 按钮禁用
          app.disabled = true
          // 先上传头像图片
          if (app.tempFile && !await app.uploadFile()) {
            return
          }
          // 提交保存个人信息
          UserApi.personal({ form: app.form })
            .then(result => {
              app.$toast(result.message)
              setTimeout(() => {
                app.disabled = false
                uni.navigateBack()
              }, 1500)
            })
            .catch(err => app.disabled = false)
        })
      },

      // 绑定昵称输入框 (用于微信小程序端快速填写昵称能力)
      onInputNickName(val) {
        if (val) {
          this.form.nickName = val
        }
      }

    }
  }
</script>

<style>
  page {
    background: #f7f8fa;
  }
</style>
<style lang="scss" scoped>
  .container {}

  .page-title {
    width: 94%;
    margin: 0 auto;
    padding-top: 40rpx;
    font-size: 28rpx;
    color: rgba(69, 90, 100, 0.6);
  }

  .form-wrapper {
    margin: 20rpx auto 20rpx auto;
    padding: 0 40rpx;
    width: 94%;
    box-shadow: 0 1rpx 5rpx 0px rgba(0, 0, 0, 0.05);
    border-radius: 16rpx;
    background: #fff;
  }

  /* 底部操作栏 */

  .footer {
    margin-top: 80rpx;

    .btn-wrapper {
      height: 100%;
      // display: flex;
      // align-items: center;
      padding: 0 20rpx;
    }

    .btn-item {
      flex: 1;
      font-size: 28rpx;
      height: 86rpx;
      color: #fff;
      border-radius: 50rpx;
      display: flex;
      justify-content: center;
      align-items: center;
    }

    .btn-item-wechat {
      background: #0ba90b;
      margin-bottom: 26rpx;
    }

    .btn-item-main {
      background: linear-gradient(to right, #f9211c, #ff6335);
      color: #fff;

      // 禁用按钮
      &.disabled {
        opacity: 0.6;
      }
    }

  }
  
  
  // 退出登录
  .my-logout {
    display: flex;
    justify-content: center;
    margin-top: 30rpx;
  
    .logout-btn {
		 padding: 0 20rpx;
      width: 94%;
      margin: 0 auto;
      font-size: 28rpx;
      color: #616161;
      border-radius: 40rpx;
      border: 1px solid #dcdcdc;
      padding: 16rpx 0;
      text-align: center;
    }
  }
</style>
