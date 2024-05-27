<template>
    <a-modal :title="title" :width="565" :visible="visible" :isLoading="isLoading" :confirmLoading="isLoading" :maskClosable="false" @ok="handleSubmit"
        @cancel="handleCancel">
        <a-spin :spinning="isLoading">
            <a-alert v-if="record.type == RefundTypeEnum.RETURN.value" message="请确认已收到寄回的商品，确认后自动退回付款金额（原路退款）并关闭当前售后单" banner />
            <a-form v-if="visible" :form="form">
                <a-form-item label="售后类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-tag>{{ RefundTypeEnum[record.type].name }}</a-tag>
                </a-form-item>
                <a-form-item label="订单付款的总金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <span>￥{{ record.orderData.pay_price }}元</span>
                </a-form-item>
                <a-form-item label="退款的金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input-number :min="0.01" :precision="2" v-decorator="['refund_money', { rules: [{ required: true, message: '请输入退款金额' }] }]" />
                    <span class="ml-10">元</span>
                    <div class="form-item-help">
                        <p class="extra">请输入退款金额，最多{{ Math.min(record.orderGoods.total_pay_price, record.orderData.pay_price) }}元，最多不能大于订单实际付款的总金额</p>
                    </div>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-modal>
</template>

<script>
import { assignment } from '@/utils/util'
import * as Api from '@/api/order/orderRefundSku'
import { AuditStatusEnum, RefundTypeEnum } from '@/common/enum/order/refund'

export default {
    data() {
        return {
            // 对话框标题
            title: '确认收货并退款',
            // 标签布局属性
            labelCol: { span: 8 },
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
    beforeCreate() {
        // 批量给当前实例赋值
        assignment(this, { AuditStatusEnum, RefundTypeEnum })
    },
    methods: {
        /**
         * 显示对话框
         */
        show(record) {
            // 显示窗口
            this.visible = true
            // 当前记录
            this.record = record
            // 更新form元素
            this.$nextTick(() => {
                this.$forceUpdate()
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
            Api.refundMoney({
                orderRefundId: this.record.order_refund_id,
                form: values,
            })
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
<style lang="less" scoped>
.ant-form-item {
    margin-bottom: 10px;
}
</style>
