<template>
  <a-card :bordered="false">
    <div class="card-title">{{ $route.meta.title }}</div>
    <a-spin :spinning="isLoading">
      <a-form :form="form" @submit="handleSubmit">
        <a-form-item label="优惠券名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input placeholder="请输入优惠券名称" v-decorator="['name', { rules: [{ required: true, min: 2, message: '请输入至少2个字符' }] }]" />
        </a-form-item>
        <a-form-item label="优惠券类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-radio-group v-decorator="['coupon_type', { initialValue: 10, rules: [{ required: true }] }]">
            <a-radio :value="10">满减券</a-radio>
            <a-radio :value="20">折扣券</a-radio>
          </a-radio-group>
        </a-form-item>
        <a-form-item v-show="form.getFieldValue('coupon_type') == CouponTypeEnum.FULL_DISCOUNT.value" label="减免金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input-number :min="0.01" :precision="2" v-decorator="['reduce_price', { rules: [{ required: true, message: '请输入减免金额' }] }]" />
          <span class="ml-5">元</span>
        </a-form-item>
        <a-form-item v-show="form.getFieldValue('coupon_type') == CouponTypeEnum.DISCOUNT.value" label="折扣率" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input-number :min="0" :max="9.9" :precision="1" v-decorator="['discount', { initialValue: 9.9, rules: [{ required: true, message: '请输入折扣率' }] }]" />
          <span class="ml-5">%</span>
          <p class="form-item-help">
            <small>折扣率范围 0-9.9，8代表打8折，0代表不折扣</small>
          </p>
        </a-form-item>
        <a-form-item label="最低消费金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input-number :min="1" :precision="2" v-decorator="['min_price', { rules: [{ required: true, message: '请输入最低消费金额' }] }]" />
          <span class="ml-5">元</span>
        </a-form-item>
        <a-form-item label="到期类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-radio-group v-decorator="['expire_type', { initialValue: 10, rules: [{ required: true }] }]">
            <a-radio :value="10">领取后生效</a-radio>
            <a-radio :value="20">固定时间</a-radio>
          </a-radio-group>
          <a-form-item v-show="form.getFieldValue('expire_type') == 10" class="expire_type-10">
            <InputNumberGroup addonBefore="有效期" addonAfter="天" :inputProps="{ min: 1, precision: 0 }" v-decorator="['expire_day', { initialValue: 7, rules: [{ required: true, message: '请输入有效期天数' }] }]" />
          </a-form-item>
          <a-form-item v-show="form.getFieldValue('expire_type') == 20" class="expire_type-20">
            <a-range-picker format="YYYY-MM-DD" v-decorator="['betweenTime', { initialValue: defaultDate, rules: [{ required: true, message: '请选择有效期范围' }] }]" />
          </a-form-item>
        </a-form-item>
        <a-form-item label="券适用范围" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-radio-group v-decorator="['apply_range', { initialValue: 10, rules: [{ required: true }] }]">
            <a-radio :value="10">全场通用</a-radio>
            <a-radio :value="20">指定商品</a-radio>
          </a-radio-group>
          <a-form-item v-if="form.getFieldValue('apply_range') == 20">
            <SelectGoods :defaultList="containGoodsList" v-decorator="['apply_range_config.applyGoodsIds', { rules: [{ required: true, message: '请选择指定的商品' }] }]" />
          </a-form-item>
        </a-form-item>
        <a-form-item label="发放总数量" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input-number :min="-1" :precision="0" v-decorator="['total_num', { initialValue: -1, rules: [{ required: true, message: '请输入发放总数量' }] }]" />
          <span class="ml-5">张</span>
          <p class="form-item-help">
            <small>发放的优惠券总数量，-1为不限制</small>
          </p>
        </a-form-item>
        <a-form-item label="显示状态" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-radio-group v-decorator="['status', { initialValue: 1, rules: [{ required: true }] }]">
            <a-radio :value="1">显示</a-radio>
            <a-radio :value="0">隐藏</a-radio>
          </a-radio-group>
          <p class="form-item-help">
            <small>如果设为隐藏将不会展示在用户端页面</small>
          </p>
        </a-form-item>
        <a-form-item label="优惠券描述" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-textarea :autoSize="{ minRows: 4 }" v-decorator="['describe']" />
        </a-form-item>
        <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="数字越小越靠前">
          <a-input-number :min="0" v-decorator="['sort', {initialValue: 100, rules:[{required: true, message: '请输入排序值'}]}]" />
        </a-form-item>
        <a-form-item class="mt-20" :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
          <a-button type="primary" html-type="submit" :loading="isBtnLoading">提交</a-button>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-card>
</template>

<script>
import moment from 'moment'
import { pick, get } from 'lodash'
import * as Api from '@/api/marketManage/coupon'
import * as GoodsApi from '@/api/goods/goods'
import { isEmpty } from '@/utils/util'
import { InputNumberGroup, SelectGoods } from '@/components'
import { ApplyRangeEnum, CouponTypeEnum, ExpireTypeEnum } from '@/common/enum/coupon'

export default {
  components: {
    SelectGoods,
    InputNumberGroup
  },
  data() {
    return {
      // 枚举类
      ApplyRangeEnum,
      CouponTypeEnum,
      ExpireTypeEnum,
      // 正在加载
      isLoading: false,
      isBtnLoading: false,
      // 标签布局属性
      labelCol: { span: 3 },
      // 输入框布局属性
      wrapperCol: { span: 10 },
      // 当前表单元素
      form: this.$form.createForm(this),
      // 默认日期范围
      defaultDate: [moment(), moment()],
      // 优惠券ID
      couponId: null,
      // 当前记录
      record: {},
      // 适用范围：指定的商品
      containGoodsList: []
    }
  },
  async created() {
    // 记录优惠券ID
    this.couponId = this.$route.query.couponId
    // 获取当前记录
    await this.getDetail()
    // 获取适用范围：指定的商品
    await this.getContainGoodsList()
  },
  methods: {

    // 获取当前记录
    async getDetail() {
      const { couponId } = this
      this.isLoading = true
      await Api.get({ couponId })
        .then(result => {
          // 当前记录
          this.record = result.data.detail
          // 设置表单默认值
          this.setFieldsValue()
        })
        .finally(() => {
          this.isLoading = false
        })
    },

    // 获取指定的商品列表
    async getContainGoodsList() {
      const { record } = this
      const goodsIds = get(record, 'apply_range_config.applyGoodsIds')
      if (goodsIds !== undefined && goodsIds.length) {
        this.isLoading = true
        await GoodsApi.listByWhere(goodsIds)
          .then(result => {
            this.containGoodsList = result.data.list
          })
          .finally(result => {
            this.isLoading = false
          })
      }
    },

    // 设置表单默认值
    setFieldsValue() {
      const { record, form, $nextTick } = this
      // 设置表单内容
      !isEmpty(form.getFieldsValue()) && $nextTick(() => {
        // 表单数据
        const data = pick(record, [
          'name', 'coupon_type', 'reduce_price', 'discount', 'min_price', 'status',
          'expire_type', 'expire_day', 'apply_range', 'total_num', 'describe', 'sort'
        ])
        // 时间范围
        data.betweenTime = this.getBetweenTime(record)
        form.setFieldsValue(data)
      })
    },

    // 格式化时间范围
    getBetweenTime(record) {
      if (record.expire_type === ExpireTypeEnum.FIXED_TIME.value) {
        return [moment(new Date(record.start_time)), moment(new Date(record.end_time))]
      }
      return this.defaultDate
    },

    // 确认按钮
    handleSubmit(e) {
      e.preventDefault()
      // 表单验证
      const { form: { validateFields }, onFormSubmit } = this
      validateFields((errors, values) => {
        !errors && onFormSubmit(values)
      })
    },

    /**
    * 提交到后端api
    */
    onFormSubmit(values) {
      this.isLoading = true
      this.isBtnLoading = true
      Api.addOrUpdate({ couponId: this.couponId, form: values })
        .then(result => {
          // 显示提示信息
          this.$message.success(result.message, 1.5)
          // 跳转到列表页
          setTimeout(() => {
            this.$router.push('./index')
          }, 1500)
        })
        .catch(() => {
          this.isBtnLoading = false
        })
        .finally(result => {
          this.isLoading = false
        })
    }

  }
}
</script>
<style lang="less" scoped>
.ant-form-item {
  .ant-form-item {
    margin-bottom: 0;
  }
}
</style>
