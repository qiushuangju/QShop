<template>
  <a-modal
    class="noborder"
    :title="title"
    :width="520"
    :visible="visible"
    :confirmLoading="confirmLoading"
    :maskClosable="false"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="会员ID" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <span>{{ record.user_id }}</span>
        </a-form-item>
        <a-form-item label="会员等级" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select
            placeholder="请选择会员等级"
            v-decorator="['grade_id', { rules: [{ required: true }] }]"
          >
            <a-select-option :value="0">无等级</a-select-option>
            <a-select-option
              v-for="(item, index) in gradeList"
              :key="index"
              :value="item.grade_id"
            >{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="管理员备注" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-textarea
            placeholder="请输入管理员备注"
            :rows="3"
            v-decorator="['remark', { rules: [{ required: true, message: '请输入管理员备注' }] }]"
          />
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import _ from 'lodash'
import * as Api from '@/api/user/memberUser'
import * as GradeApi from '@/api/user/usergrade'

export default {
  components: {
  },
  props: {
    // 会员等级列表
    gradeList: {
      type: Array,
      required: true
    }
  },
  data () {
    return {
      // 对话框标题
      title: '设置会员等级',
      // 标签布局属性
      labelCol: { span: 7 },
      // 输入框布局属性
      wrapperCol: { span: 13 },
      // modal(对话框)是否可见
      visible: false,
      // modal(对话框)确定按钮 loading
      confirmLoading: false,
      // 当前表单元素
      form: this.$form.createForm(this),
      // 当前记录
      record: {}
    }
  },
  methods: {

    // 显示对话框
    handle (record) {
      // 显示窗口
      this.visible = true
      // 当前记录
      this.record = record
      // 设置默认值
      this.setFieldsValue()
    },

    /**
     * 设置默认值
     */
    setFieldsValue () {
      const { record, form: { setFieldsValue } } = this
      this.$nextTick(() => {
        setFieldsValue(_.pick(record, ['grade_id']))
      })
    },

    /**
     * 确认按钮
     */
    handleSubmit (e) {
      e.preventDefault()
      // 表单验证
      const { form: { validateFields } } = this
      validateFields((errors, values) => {
        // 提交到后端api
        !errors && this.onFormSubmit(values)
      })
    },

    /**
     * 关闭对话框事件
     */
    handleCancel () {
      this.visible = false
      this.form.resetFields()
    },

    /**
    * 提交到后端api
    */
    onFormSubmit (values) {
      this.confirmLoading = true
      Api.addOrUpdate({ userId: this.record.user_id, form: values })
        .then((result) => {
          // 显示成功
          this.$message.success(result.message, 1.5)
          // 关闭对话框事件
          this.handleCancel()
          // 通知父端组件提交完成了
          this.$emit('handleSubmit', values)
        })
        .finally(() => {
          this.confirmLoading = false
        })
    }

  }
}
</script>
<style lang="less">
.ant-form-item {
  margin-bottom: 15px;
}
</style>
