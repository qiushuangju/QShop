<template>
	<view class="container">
		<view class="wechatapp">
			<view class="header">
				<!-- <open-data class="avatar" type="userAvatarUrl"></open-data> -->
				<image class="image"
					:src="storeInfo && storeInfo.logoImageUrl ? storeInfo.logoImageUrl : '/static/default-avatar.png'">
				</image>
			</view>
		</view>
		<view class="auth-title">申请获取以下权限</view>
		<view class="auth-subtitle">获得你的公开信息（昵称、头像等）</view>
		<view class="login-btn">
			<!-- 获取微信用户信息 -->
			<button class="button btn-normal" @click.stop="getUserProfile">授权登录</button>
		</view>
		<view class="no-login-btn">
			<button class="button btn-normal" @click="handleCancel">暂不登录</button>
		</view>
		
	</view>
</template>

<script>
	  import * as StoreApi from '@/api/store/store'
	import store from '@/store'
	import {
		isEmpty
	} from '@/utils/util'
	import SettingModel from '@/common/model/Setting'

	export default {

		data() {
			return {
				// 商城基本信息
				storeInfo: undefined,
				// 微信小程序登录凭证 (code)
				// 提交到后端，用于换取openid
				code: ''
			}
		},

		created() {
			// 获取商城基本信息
			this.getStoreInfo()
		},

		methods: {

			// 获取商城基本信息
			getStoreInfo() {
				StoreApi.getCurrentStore()
				.then(res => {
				  this.storeInfo =res.result
				})
			},

			// 获取code
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

			// 获取微信用户信息(新版)
			getUserProfile() {
				const app = this
				wx.canIUse('getUserProfile') && wx.getUserProfile({
					lang: 'zh_CN',
					desc: '获取用户相关信息',
					success({
						userInfo
					}) {
						console.log('用户同意了授权')
						console.log('userInfo：', userInfo)
						// 授权成功事件
						app.onAuthSuccess(userInfo)
					},
					fail() {
						console.log('用户拒绝了授权')
					}
				})
			},

			// 授权成功事件
			// 这里分为两个逻辑:
			// 1.将code和userInfo提交到后端，如果存在该用户 则实现自动登录，无需再填写手机号
			// 2.如果不存在该用户, 则显示注册页面, 需填写手机号
			// 3.如果后端报错了, 则显示错误信息
			async onAuthSuccess(userInfo) {
				const app = this
				var reqData = {

					wxCode: await app.getCode(),
					oauth: 'MP-WEIXIN',
					// userInfo,
					refereeId: store.getters.refereeId
				};
				console.log("111", reqData);
				// return
				// 提交到后端
				store.dispatch('loginWx', reqData)
					.then(data => {
						// 一键登录成功
						app.$toast(data.message)
						
						console.log("data",data)
						console.log("data",data.isBindPhone)
						if (!data.isBindPhone) {//未绑定手机号
						console.log("11111")
						app.onEmitSuccess(userInfo)
						}else{
							// 相应全局事件订阅: 刷新上级页面数据
							uni.$emit('syncRefresh', true)
							// 跳转回原页面
							setTimeout(() => app.onNavigateBack(), 2000)
						}
					})
					.catch(err => {
						console.log("err", err)
						const resultData = err.result
						// 显示错误信息
						if (isEmpty(err.message)) {
							app.$toast(err.message)
						}
					
						// 跳转回原页面
						if (resultData.isBack) {
							setTimeout(() => app.onNavigateBack(1), 2000)
						}
						// 判断还需绑定手机号
						if (!resultData.isBindPhone) {
							app.onEmitSuccess(userInfo)
						}
					})
			},

			// 将oauth提交给父级
			// 这里要重新获取code, 因为上一次获取的code不能复用(会报错)11
			async onEmitSuccess(userInfo) {
				const app = this
				app.$emit('success', {
					oauth: 'MP-WEIXIN', // 第三方登录类型: MP-WEIXIN
					code: await app.getCode(), // 微信登录的code, 用于换取openid
					userInfo // 微信用户信息
				})
			},

			/**
			 * 暂不登录
			 */
			handleCancel() {
				// 跳转回原页面
				this.onNavigateBack()
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
		padding: 0 60rpx;
		font-size: 32rpx;
		background: #fff;
		min-height: 100vh;
	}

	.wechatapp {
		padding: 80rpx 0 48rpx;
		border-bottom: 1rpx solid #e3e3e3;
		margin-bottom: 72rpx;
		text-align: center;

		.header {
			width: 190rpx;
			height: 190rpx;
			border: 4rpx solid #fff;
			margin: 0 auto 0;
			border-radius: 50%;
			overflow: hidden;
			box-shadow: 2rpx 0 10rpx rgba(50, 50, 50, 0.3);

			.image {
				display: block;
				width: 100%;
				height: 100%;
			}
		}
	}

	.auth-title {
		color: #585858;
		font-size: 34rpx;
		margin-bottom: 40rpx;
	}

	.auth-subtitle {
		color: #888;
		margin-bottom: 88rpx;
		font-size: 28rpx;
	}

	.login-btn {
		padding: 0 20rpx;

		.button {
			height: 88rpx;
			background: #04be01;
			color: #fff;
			font-size: 30rpx;
			border-radius: 999rpx;
			display: flex;
			justify-content: center;
			align-items: center;
		}
	}


	.no-login-btn {
		margin-top: 20rpx;
		padding: 0 20rpx;

		.button {
			height: 88rpx;
			background: #dfdfdf;
			color: #fff;
			font-size: 30rpx;
			border-radius: 999rpx;
			display: flex;
			justify-content: center;
			align-items: center;
		}
	}
</style>