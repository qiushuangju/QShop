<template>
  <a-modal class="noborder" :title="title" :width="520" :visible="visible" :confirmLoading="confirmLoading" :maskClosable="false" @ok="handleSubmit" @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-tabs :activeKey="activeKey" @change="onChangeTabs">
          <a-tab-pane :key="RECHARGE_TYPE_BALANCE" tab="充值余额">
            <template v-if="activeKey === RECHARGE_TYPE_BALANCE">
              <a-form-item class="mb-5" label="当前余额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <span class="c-p">{{ record.balance }}</span>
              </a-form-item>
              <a-form-item label="充值方式" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-radio-group v-decorator="[`${RECHARGE_TYPE_BALANCE}.mode`, { initialValue: 'inc', rules: [{ required: true }] }]">
                  <a-radio value="inc">增加</a-radio>
                  <a-radio value="dec">减少</a-radio>
                  <a-radio value="final">最终金额</a-radio>
                </a-radio-group>
              </a-form-item>
              <a-form-item label="变更金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-input-number :min="0.01" v-decorator="[`${RECHARGE_TYPE_BALANCE}.money`, { initialValue: '', rules: [{ required: true, message: '请输入变更的金额' }] }]" />
              </a-form-item>
              <a-form-item label="管理员备注" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-textarea placeholder="请输入管理员备注" :rows="3" v-decorator="[`${RECHARGE_TYPE_BALANCE}.remark`, { rules: [{ required: true, message: '请输入管理员备注' }] }]" />
              </a-form-item>
            </template>
          </a-tab-pane>
          <!-- <a-tab-pane :key="RECHARGE_TYPE_POINTS" tab="充值积分">
            <template v-if="activeKey === RECHARGE_TYPE_POINTS">
              <a-form-item class="mb-5" label="会员ID" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <span>{{ record.user_id }}</span>
              </a-form-item>
              <a-form-item class="mb-5" label="当前积分" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <span class="c-p">{{ record.points }}</span>
              </a-form-item>
              <a-form-item label="充值方式" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-radio-group
                  v-decorator="[`${RECHARGE_TYPE_POINTS}.mode`, { initialValue: 'inc', rules: [{ required: true }] }]"
                >
                  <a-radio value="inc">增加</a-radio>
                  <a-radio value="dec">减少</a-radio>
                  <a-radio value="final">最终积分</a-radio>
                </a-radio-group>
              </a-form-item>
              <a-form-item label="变更数量" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-input-number
                  :min="0.01"
                  v-decorator="[`${RECHARGE_TYPE_POINTS}.value`, { initialValue: '', rules: [{ required: true, message: '请输入变更的金数量' }] }]"
                />
              </a-form-item>
              <a-form-item label="管理员备注" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-textarea
                  placeholder="请输入管理员备注"
                  :rows="3"
                  v-decorator="[`${RECHARGE_TYPE_POINTS}.remark`, { rules: [{ required: true, message: '请输入管理员备注' }] }]"
                />
              </a-form-item>
            </template>
          </a-tab-pane> -->
        </a-tabs>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { pick } from 'lodash'
import * as Api from '@/api/user/memberUser'

// 充值类型枚举: 余额
const RECHARGE_TYPE_BALANCE = 'balance'

// 充值类型枚举: 积分
// const RECHARGE_TYPE_POINTS = 'points'

export default {
  components: {
  },
  data() {
    return {
      // 对话框标题
      title: '会员充值',
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
      // 当前tab索引
      activeKey: RECHARGE_TYPE_BALANCE,
      // 充值类型
      RECHARGE_TYPE_BALANCE,
      // RECHARGE_TYPE_POINTS,
      // 当前记录
      record: {}
    }
  },
  methods: {

    // 显示对话框
    handle(record) {
      // 显示窗口
      this.visible = true
      // 当前记录
      this.record = record
    },

    // 切换tabs事件
    onChangeTabs(activeKey) {
      this.activeKey = activeKey
    },

    /**
     * 确认按钮
     */
    handleSubmit(e) {
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
    handleCancel() {
      this.visible = false
      this.form.resetFields()
    },

    /**
    * 提交到后端api
    */
    onFormSubmit(values) {
      const { record, activeKey } = this
      this.confirmLoading = true
      const formData = Object.assign({}, values.balance)
      formData.userId = record.id
      Api.rechargeBalance(formData)
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
<style lang="less" scoped>
.ant-modal-body {
  padding-top: 0 !important;
}
.ant-form-item {
  margin-bottom: 15px;
}
.ant-tabs-nav .ant-tabs-tab {
  padding: 10px 16px;
}
</style>
