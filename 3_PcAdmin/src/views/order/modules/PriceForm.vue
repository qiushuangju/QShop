<template>
    <a-modal :title="title" :width="560" :visible="visible" :isLoading="isLoading" :confirmLoading="isLoading" :maskClosable="false" @ok="handleSubmit"
        @cancel="handleCancel">
        <a-spin :spinning="isLoading">
            <a-form :form="form">
                <a-form-item label="订单金额" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="最终付款金额 = 订单金额 + 运费金额">
                    <a-input-number :min="0.01" :precision="2" v-decorator="['order_price', { rules: [{ required: true, message: '请输入订单金额' }] }]" />
                    <span class="ml-10">元</span>
                </a-form-item>
                <a-form-item label="运费金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input-number :min="0" :precision="2" v-decorator="['express_price', { rules: [{ required: true, message: '请输入运费金额' }] }]" />
                    <span class="ml-10">元</span>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-modal>
</template>

<script>
import _ from 'lodash'
import * as Api from '@/api/order/order'

export default {
    data() {
        return {
            // 对话框标题
            title: '修改订单金额',
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
            // 设置默认值
            this.setFieldsValue()
        },

        // 设置默认值
        setFieldsValue() {
            const {
                record,
                $nextTick,
                form: { setFieldsValue },
            } = this
            $nextTick(() => {
                let diffPrice = Number(record.update_price.value)
                if (record.update_price.symbol === '-') {
                    diffPrice = -diffPrice
                }
                setFieldsValue({
                    order_price: _.add(Number(record.order_price), diffPrice),
                    express_price: Number(record.express_price),
                })
            })
        },

        // 确认按钮
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
            Api.updatePrice({ orderId: this.record.order_id, form: values })
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
