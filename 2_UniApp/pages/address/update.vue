<template>
  <view class="container">
    <!-- 标题 -->
    <view class="page-title">收货地址</view>
    <!-- 表单组件 -->
    <view class="form-wrapper">
      <u-form :model="form" ref="uForm" label-width="140rpx">
        <u-form-item label="姓名" prop="name">
          <u-input v-model="form.name" placeholder="请输入收货人姓名" />
        </u-form-item>
        <u-form-item label="电话" prop="phone">
          <u-input v-model="form.phone" placeholder="请输入收货人手机号" />
        </u-form-item>
        <u-form-item label="地区" prop="listRegion">
          <select-region ref="sRegion" v-model="form.listRegion" />
        </u-form-item>
        <u-form-item label="详细地址" prop="detail" :border-bottom="false">
          <u-input v-model="form.detail" placeholder="街道门牌、楼层等信息" />
        </u-form-item>
      </u-form>
    </view>
    <!-- 操作按钮 -->
    <view class="footer">
      <view class="btn-wrapper">
        <view class="btn-item btn-item-main" :class="{ disabled }" @click="handleSubmit()">保存</view>
      </view>
    </view>
  </view>
</template>

<script>
  import SelectRegion from '@/components/select-region/select-region'
  import { isMobile } from '@/utils/verify'
  import * as AddressApi from '@/api/user/userAddress'

  // 表单验证规则
  const rules = {
    name: [{
      required: true,
      message: '请输入姓名',
      trigger: ['blur', 'change']
    }],
    phone: [{
      required: true,
      message: '请输入手机号',
      trigger: ['blur', 'change']
    }, {
      // 自定义验证函数
      validator: (rule, value, callback) => {
        // 返回true表示校验通过，返回false表示不通过
        return isMobile(value)
      },
      message: '手机号码不正确',
      // 触发器可以同时用blur和change
      trigger: ['blur'],
    }],
    listRegion: [{
      required: true,
      message: '请选择省市区',
      trigger: ['blur', 'change'],
      type: 'array'
    }],
    detail: [{
      required: true,
      message: '请输入详细地址',
      trigger: ['blur', 'change']
    }],
  }

  export default {
    components: {
      SelectRegion
    },
    data() {
      return {
        form: {
          name: '',
          phone: '',
          region: [],
          detail: ''
        },
        rules,
        // 加载中
        isLoading: true,
        // 按钮禁用
        disabled: false,
        // 当前收货地址ID
        addressId: null
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad({ addressId }) {
      // 当前收货地址ID
      this.addressId = addressId
      // 获取当前记录详情
      this.getDetail()
    },

    // 必须要在onReady生命周期，因为onLoad生命周期组件可能尚未创建完毕
    onReady() {
      this.$refs.uForm.setRules(this.rules)
    },

    methods: {

      // 获取当前记录详情
      getDetail() {
        const app = this
        AddressApi.getDetail(app.addressId)
          .then(res => {
            const detail = res.result
            app.createFormData(detail)
          })
      },

      // 生成默认的表单数据
      createFormData(detail) {
        const { form } = this
        form.name = detail.name
        form.phone = detail.phone
        form.detail = detail.detail
        form.listRegion = this.createRegion(detail)
      },

      createRegion(detail) {
        return [{
          label: detail.province,
          value: detail.provinceId
        }, {
          label: detail.city,
          value: detail.cityId
        }, {
          label: detail.region,
          value: detail.regionId
        }]
      },

      // 表单提交
      handleSubmit() {
        const app = this
        if (app.disabled) {
          return false
        }
        app.$refs.uForm.validate(valid => {
          if (valid) {
            app.disabled = true
            AddressApi.edit(app.addressId, app.form)
              .then(result => {
                app.$toast(result.message)
                uni.navigateBack()
              })
              .finally(() => app.disabled = false)
          }
        })
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

  // 底部操作栏
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
</style>
