<template>
  <view class="container" v-if="userInfo">
	  
	  
    <view class="account-panel dis-flex flex-y-center">
      <view class="panel-lable">
        <text>账户余额</text>
      </view>
      <view class="panel-balance flex-box">
        <text>￥{{ userInfo.balance }}</text>
      </view>
    </view>
	
	
    <view class="recharge-panel">
      <view class="recharge-label">
        <text>充值金额</text>
      </view>
      <view class="recharge-plan clearfix">
        <block v-for="(item, index) in planList" :key="index">
          <view class="recharge-plan_item" :class="{ active: selectedPlanId == item.id }" @click="onSelectPlan(item.id)">
            <view class="plan_money">
              <text>{{ item.money }}</text>
            </view>
            <view class="plan_gift" v-if="item.giftMoney > 0">
              <text>送{{ item.giftMoney }}</text>
            </view>
          </view>
        </block>
      </view>
      <!-- 手动充值输入框 -->
      <view class="recharge-input" v-if="setting.isCustom == true">
        <input type="digit" placeholder="请输入充值金额" v-model="inputMoney"  @input="onChangeMoney" />
      </view>
	  
	  <view class="plan_gift" v-if="customGiftMoney > 0">
	    <text>送{{ customGiftMoney }}</text>
	  </view>
      <!-- 确认按钮 -->
      <view class="recharge-submit btn-submit">
        <form @submit="onSubmit">
          <button class="button" formType="submit" :disabled="disabled">立即充值</button>
        </form>
      </view>
    </view>
    <!-- 充值描述 -->
    <view class="recharge-describe">
      <view class="recharge-label">
        <text>充值说明</text>
      </view>
      <view class="content">
        <text space="ensp">{{ setting.describe }}</text>
      </view>
    </view>
  </view>
</template>

<script>
  import * as UserApi from '@/api/user/user'
  import * as RechargeApi from '@/api/recharge/rechargeOrder'
  import * as PlanApi from '@/api/recharge/rechargePlan'
  import SettingModel from '@/common/model/Setting'
  import SettingKeyEnum from '@/common/enum/setting/Key'
  import { wxPayment } from '@/core/app'

  export default {
    data() {
      return {
        // 正在加载
        isLoading: true,
        // 会员信息
        userInfo:null,
        // 充值设置
        setting: {},
        // 充值方案列表
        planList: [],
        // 按钮禁用
        disabled: false,
        // 当前选中的套餐id
        selectedPlanId: '',
		
        // 自定义金额
        inputMoney: '',
		customGiftMoney:0.00,
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {
      // 获取页面数据
      this.getPageData()
    },

    methods: {

      /**
       * 选择充值套餐
       */
      onSelectPlan(planId) {
        this.selectedPlanId = planId
        this.inputMoney = ''
      },

      // 金额输入框
      onChangeMoney(e) {
		  const app = this
		  var inputMoney = e.target.value;
        app.inputMoney =inputMoney;
        app.selectedPlanId = ''
		console.log("inputMoney",inputMoney);
		if(app.setting.isCustom){
			var listPlan=app.planList.filter(p =>inputMoney>= p.money).sort((a, b) => b.money - a.money);
			var plan=	listPlan[0];
			if(plan!=null){
					 app.customGiftMoney=plan.giftMoney;
			}else{
					 app.customGiftMoney=0;
			}
		}
      },

      // 获取页面数据
      getPageData() {
        const app = this
        app.isLoading = true
        Promise.all([app.getUserInfo(), app.getSettingRecharge(), app.getPlanList()])
          .then(() => app.isLoading = false)
      },

      // 获取充值方案列表
      getPlanList() {
        const app = this
        return new Promise((resolve, reject) => {
          PlanApi.listByWhere()
            .then(res => {
              app.planList = res.result;
              resolve(app.planList)
            })
        })
      },

      // 获取会员信息
      getUserInfo() {
        const app = this
        return new Promise((resolve, reject) => {
          UserApi.getDetail()
            .then(res => {
              app.userInfo =res.result
              resolve(app.userInfo)
            })
        })
      },

      
	  // 获取充值设置
	  getSettingRecharge() {
	    const app = this
	    return new Promise((resolve, reject) => {
	      SettingModel.GetDetail(SettingKeyEnum.recharge.value)
	        .then(res => {
	          app.setting = res
	          resolve(res)
	        }).catch(reject)
	    })
	  },
	  

      // 立即充值
      onSubmit(e) {
		  const app = this
		  var minMoney=app.setting.lowestMoney;
		  if(app.selectedPlanId=='' && app.inputMoney <minMoney){
			  // 显示成功信息
			  app.$toast('最低充值:'+minMoney+'元')
			  return false;
		  }
        
        // 按钮禁用
        app.disabled = true
        // 提交到后端
        RechargeApi.rechargeBefore({ planId: app.selectedPlanId, customMoney: app.inputMoney })
          .then(res => app.wxPayment(res.result.wxPayParams))
          .finally(() => app.disabled = false)
      },

      // 发起微信支付
      wxPayment(option) {
        const app = this
        wxPayment(option)
          .then(() => {
            app.$success('支付成功')
            setTimeout(() => {
              // 获取页面数据
              app.getPageData()
            }, 1500)
          })
          .catch(err => app.$error('订单未支付'))
      }

    }
  }
</script>

<style lang="scss" scoped>
  page,
  .container {
    background: #fff;
  }

  .container {
    padding-bottom: 70rpx;
  }

  /* 账户面板 */
  .account-panel {
    width: 650rpx;
    height: 180rpx;
    margin: 50rpx auto;
    padding: 0 60rpx;
    box-sizing: border-box;
    border-radius: 12rpx;
    color: #fff;
    background: linear-gradient(-125deg, #a46bff, #786cff);
    box-shadow: 0 5px 22px 0 rgba(0, 0, 0, 0.26);
  }

  .panel-lable {
    font-size: 32rpx;
  }

  .recharge-label {
    color: rgb(51, 51, 51);
    font-size: 28rpx;
    margin-bottom: 25rpx;
  }

  .panel-balance {
    text-align: right;
    font-size: 46rpx;
  }

  .recharge-panel {
    margin-top: 60rpx;
    padding: 0 60rpx;
  }

  /* 充值套餐 */
  .recharge-plan {
    .recharge-plan_item {
      width: 192rpx;
      padding: 15rpx 0;
      float: left;
      text-align: center;
      color: #888;
      border: 1rpx solid rgb(228, 228, 228);
      border-radius: 10rpx;
      margin: 0 20rpx 20rpx 0;

      &:nth-child(3n + 0) {
        margin-right: 0;
      }

      &.active {
        color: #786cff;
        border: 1rpx solid #786cff;

        .plan_money {
          color: #786cff;
        }
      }
    }

  }

  .plan_money {
    font-size: 32rpx;
    color: rgb(82, 82, 82);
  }

  .plan_gift {
    font-size: 25rpx;
  }

  .recharge-input {
    margin-top: 25rpx;

    input {
      border: 1rpx solid rgb(228, 228, 228);
      border-radius: 10rpx;
      padding: 15rpx 16rpx;
      font-size: 26rpx;
    }
  }

  /* 立即充值 */
  .recharge-submit {
    margin-top: 70rpx;
  }

  .btn-submit {
    .button {
      font-size: 30rpx;
      background: #786cff;
      border: none;
      color: white;
      border-radius: 50rpx;
      padding: 0 120rpx;
      line-height: 3;
    }

    .button[disabled] {
      background: #a098ff;
      border-color: #a098ff;
      color: white;
    }
  }

  /* 充值说明 */
  .recharge-describe {
    margin-top: 50rpx;
    padding: 0 60rpx;

    .content {
      font-size: 26rpx;
      line-height: 1.6;
      color: #888;
    }
  }
</style>
