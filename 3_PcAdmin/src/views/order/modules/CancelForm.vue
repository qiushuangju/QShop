<template>
    <a-modal :title="title" :width="560" :visible="visible" :isLoading="isLoading" :confirmLoading="isLoading" :maskClosable="false" @ok="handleSubmit"
        @cancel="handleCancel">
        <a-spin :spinning="isLoading">
            <a-form :form="form">
                <a-form-item label="实付款金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <span>￥{{ record.payPrice }}</span>
                </a-form-item>
                <a-form-item label="审核状态" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="同意后将退回付款金额并关闭订单">
                    <a-radio-group v-decorator="['orderStatus', { initialValue: -30, rules: [{ required: true }] }]">
                        <a-radio :value="-30">同意</a-radio>
                        <a-radio :value="20">拒绝</a-radio>
                    </a-radio-group>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-modal>
</template>

<script>
import * as Api from '@/api/order/order'

export default {
    data() {
        return {
            // 对话框标题
            title: '审核取消订单',
            // 标签布局属性
            labelCol: { span: 7 },
            // 输入框布局属性
            wrapperCol: { span: 13 },
            // modal(对话框)是否可见
            visible: false,
            // modal(对话框)确定按钮 loading
            isLoading: false,
            // 当前表单元素
            form: this.$form.createForm(this),
            // 当前记录
            record: {},
        }
    },
    created() {},
    methods: {
        /**
         * 显示对话框
         */
        show(record) {
            // 显示窗口
            this.visible = true
            // 当前记录
            this.record = record
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
            this.isLoading = true
            Api.auditCancel({ orderId: this.record.order_id, form: values })
                .then((result) => {
                    // 显示成功
                    this.$message.success(result.message, 1.5)
                    // 关闭对话框事件
                    this.handleCancel()
                    // 通知父端组件提交完成了
                    this.$emit('handleSubmit', values)
                })
                .finally(() => {
                    this.isLoading = false
                })
        },
    },
}
</script>
