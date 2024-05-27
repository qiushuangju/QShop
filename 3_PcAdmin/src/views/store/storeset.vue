<template>
  <a-card :bordered="false">
    <div class="card-title">{{ $route.meta.title }}</div>
    <a-spin :spinning="isLoading">
      <a-form :form="form" @submit="handleSubmit">
        <a-form-item label="商城名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-decorator="['storeName', {rules: [{required: true, min: 2, message: '请输入至少2个字符'}]}]" />
        </a-form-item>
        <!-- <a-form-item label="商城简介" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-textarea v-decorator="['describe']" />
                </a-form-item> -->
        <a-form-item label="商城Logo" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议尺寸: 300*300">
          <SelectImage :defaultList="record.logoImage ? [record.logoImage] : []" v-decorator="['logoImageId']" />
        </a-form-item>
        <a-form-item :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
          <a-button type="primary" html-type="submit">提交</a-button>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-card>
</template>
  
  <script>
import lodash from 'lodash'
import * as Api from '@/api/store/store'
import { SelectImage } from '@/components'

export default {
  components: {
    SelectImage,
  },
  data() {
    return {
      // 标签布局属性
      labelCol: { span: 4 },
      // 输入框布局属性
      wrapperCol: { span: 10 },
      // loading状态
      isLoading: false,
      // 当前表单元素
      form: this.$form.createForm(this),
      // 当前记录详情
      record: {},
    }
  },
  // 初始化数据
  created() {
    // 获取当前详情记录
    this.getDetail()
  },
  methods: {
    // 获取当前详情记录
    getDetail() {
      this.isLoading = true
      Api.listByWhere()
        .then((res) => {
          // 当前记录
          this.record = res.result[0]
          // 设置默认值
          this.setFieldsValue()
        })
        .finally((result) => {
          this.isLoading = false
        })
    },

    /**
     * 设置默认值
     */
    setFieldsValue() {
      const {
        record,
        form: { setFieldsValue },
      } = this
      // 表单内容
      this.$nextTick(() => {
        setFieldsValue(
          lodash.pick(record, [
            'id',
            'storeName',
            'describe',
            'logoImageId',
          ])
        )
      })
    },

    /**
     * 确认按钮
     */
    handleSubmit(e) {
      e.preventDefault()
      // 表单验证
      const {
        form: { validateFields },
      } = this
      validateFields((errors, values) => {
        // 提交到后端api
        !errors && this.onFormSubmit(values)
      })
    },

    /**
     * 提交到后端api
     */
    onFormSubmit(values) {
      values.id = this.record.id
      this.isLoading = true
      Api.updateStore(values)
        .then((res) => {
          // 显示提示信息
          this.$message.success(res.message, 1.5)
        })
        .finally((result) => {
          this.isLoading = false
        })
    },
  },
}
</script>
  <style lang="less" scoped>
/deep/.ant-form-item-control {
  padding-left: 10px;
  .ant-form-item-control {
    padding-left: 0;
  }
}
</style>
  