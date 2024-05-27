<template>
	<view v-if="isLoad" class="login">
		<MpWeixin v-if="isMpWeixinAuth" @success="onGetUserInfoSuccess" />
		<Main v-else :isParty="isParty" :partyData="partyData" />
	</view>
</template>

<script>
	import Main from './components/main'
	import MpWeixin from './components/mp-weixin'
	import SettingKeyEnum from '@/common/enum/setting/Key'
	import SettingModel from '@/common/model/Setting'

	export default {
		components: {
			Main,
			MpWeixin
		},

		data() {
			return {
				// 数据加载完成 [防止在微信小程序端onLoad和view渲染同步进行]
				isLoad: false,
				// 是否显示微信小程序授权登录
				isMpWeixinAuth: false,
				// 是否存在第三方用户信息
				isParty: false,
				// 第三方用户信息数据
				partyData: {}
			}
		},

		/**
		 * 生命周期函数--监听页面加载
		 */
		async onLoad(options) {
			console.log('login onLoad')
			// 设置当前是否显示微信小程序授权登录
			await this.setShowUserInfo()
			// 数据加载完成
			this.isLoad = true
			console.log('isLoad', true)
		},

		methods: {

			/**
			 * 设置当前是否显示微信小程序授权登录
			 *  - 条件1: 只有微信小程序才显示获取用户信息按钮
			 *  - 条件2: 后台设置是否已开启该选项 [后台-客户端-注册设置]
			 */
			async setShowUserInfo() {
				console.log('setShowUserInfo start')
				const app = this
				// 判断当前客户端是微信小程序, 并且支持getUserProfile接口
				const isMpWeixin = app.platform === 'MP-WEIXIN' && wx.canIUse('getUserProfile')

				console.log("111", SettingKeyEnum.register.value)
				// 获取后台设置
				await SettingModel.GetDetail(SettingKeyEnum.register.value, false)
					.then(setting => {
						app.isMpWeixinAuth = isMpWeixin && setting.isOauthMpweixin
						console.log('setShowUserInfo complete')
					})
			},

			// 获取到用户信息的回调函数
			onGetUserInfoSuccess({
				oauth,
				code,
				userInfo
			}) {
				console.log("userInfo",userInfo)
				// 记录第三方用户信息数据
				this.partyData = {
					oauth,
					code,
					userInfo
				}
				// 显示注册页面
				this.onShowRegister()
			},

			// 显示注册页面
			onShowRegister() {
				// 是否显示微信小程序授权登录
				this.isMpWeixinAuth = false
				// 已获取到了第三方用户信息
				this.isParty = true
			}
		}
	}
</script>

<style lang="scss" scoped>

</style>