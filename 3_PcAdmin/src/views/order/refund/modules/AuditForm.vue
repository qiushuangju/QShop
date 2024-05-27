<template>
    <a-modal :title="title" :width="560" :visible="visible" :isLoading="isLoading" :confirmLoading="isLoading" :maskClosable="false" @ok="handleSubmit"
        @cancel="handleCancel">
        <a-spin :spinning="isLoading">
            <a-form v-if="visible" :form="form">
                <a-form-item label="售后类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-tag>{{ RefundTypeEnum[record.type].name }}</a-tag>
                </a-form-item>
                <a-form-item label="审核状态" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['audit_status', { initialValue: 10, rules: [{ required: true }] }]">
                        <a-radio :value="10">同意</a-radio>
                        <a-radio :value="20">拒绝</a-radio>
                    </a-radio-group>
                </a-form-item>
                <a-form-item v-if="form.getFieldValue('audit_status') == AuditStatusEnum.REVIEWED.value" label="退货地址" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-select v-decorator="['address_id', { rules: [{ required: true, message: '请选择退货地址' }] }]" placeholder="请选择退货地址">
                        <a-select-option v-for="(item, index) in addressList" :key="index" :value="item.address_id">{{ item.full_address }}</a-select-option>
                    </a-select>
                    <div class="form-item-help">
                        <router-link target="_blank" :to="{ path: '/store/address/index' }">地址管理</router-link>
                    </div>
                </a-form-item>
                <a-form-item v-if="form.getFieldValue('audit_status') == AuditStatusEnum.REJECTED.value" label="拒绝原因" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-textarea :autoSize="{ minRows: 4 }" v-decorator="['refuse_desc', { rules: [{ required: true, message: '请输入拒绝原因' }] }]" />
                </a-form-item>
            </a-form>
        </a-spin>
    </a-modal>
</template>

<script>
import { assignment } from '@/utils/util'
import * as Api from '@/api/order/orderRefundSku'
import * as AddressApi from '@/api/store/storeaddress'

import { AuditStatusEnum, RefundTypeEnum } from '@/common/enum/order/refund'
import { AddressTypeEnum } from '@/common/enum/store/address'

export default {
    data() {
        return {
            // 对话框标题
            title: '售后单审核',
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
            // 退货地址列表
            addressList: [],
            // 当前记录
            record: {},
        }
    },
    beforeCreate() {
        // 批量给当前实例赋值
        assignment(this, { AuditStatusEnum, RefundTypeEnum })
    },
    created() {
        // 获取退货地址列表
        this.getAddressList()
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

        // 获取退货地址列表
        getAddressList() {
            this.isLoading = true
            AddressApi.listByWhere({ type: AddressTypeEnum.RETURN.value })
                .then((result) => (this.addressList = result.data.list))
                .finally(() => (this.isLoading = false))
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
            Api.audit({
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
