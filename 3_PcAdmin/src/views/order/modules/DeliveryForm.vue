<template>
    <a-modal :title="title" :width="580" :visible="visible" :isLoading="isLoading" :confirmLoading="isLoading" :maskClosable="false" @ok="handleSubmit"
        @cancel="handleCancel">
        <a-spin :spinning="isLoading">
            <a-form :form="form">
                <a-form-item label="物流公司" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-select v-decorator="['expressId', { rules: [{ required: true, message: '请选择物流公司' }] }]" placeholder="请选择物流公司">
                        <a-select-option v-for="(item, index) in listExpress" :key="index" :value="item.id">{{ item.expressName }}</a-select-option>
                    </a-select>
                    <div class="form-item-help">
                        <router-link target="_blank" :to="{ path: '/setting/delivery/express/index' }">物流公司管理</router-link>
                    </div>
                </a-form-item>
                <a-form-item label="物流单号" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="请手动录入物流单号或快递单号">
                    <a-input v-decorator="['expressNo', { rules: [{ required: true, message: '请输入物流单号' }] }]" />
                </a-form-item>
            </a-form>
        </a-spin>
    </a-modal>
</template>

<script>
import * as Api from '@/api/order/order'
import * as ExpressApi from '@/api/store/storeexpress'

export default {
    data() {
        return {
            // 对话框标题
            title: '订单发货',
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
            // 物流公司列表
            listExpress: [],
            // 当前记录
            record: {},
        }
    },
    created() {
        // 获取物流公司列表
        this.getListExpress()
    },
    methods: {
        // 显示对话框
        show(record) {
            // 显示窗口
            this.visible = true
            // 当前记录
            this.record = record
        },

        // 获取物流公司列表
        getListExpress() {
            this.isLoading = true
            ExpressApi.listByWhere()
                .then((res) => (this.listExpress = res.result))
                .finally(() => (this.isLoading = false))
        },

        // 确认按钮
        handleSubmit(e) {
            // 表单验证
            const {
                form: { validateFields },
            } = this
            validateFields((errors, values) => {
                // 提交到后端api
                !errors && this.onFormSubmit(values)
            })
        },

        // 关闭对话框事件
        handleCancel() {
            this.visible = false
            this.form.resetFields()
        },

        // 提交到后端api
        onFormSubmit(values) {
            this.isLoading = true
            values.orderId = this.record.id
            Api.delivery(values)
                .then((res) => {
                    // 显示成功
                    this.$message.success(res.message, 1.5)
                    // 关闭对话框事件
                    this.handleCancel()
                    // 通知父端组件提交完成了
                    this.$emit('handleSubmit', values)
                })
                .finally(() => (this.isLoading = false))
        },
    },
}
</script>
