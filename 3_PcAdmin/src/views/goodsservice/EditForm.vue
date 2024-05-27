<template>
    <a-modal :title="title" :width="720" :visible="visible" :confirmLoading="confirmLoading" :maskClosable="false" @ok="handleSubmit" @cancel="handleCancel">
        <a-spin :spinning="confirmLoading">
            <a-form :form="form">
                <a-form-item label="服务名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input placeholder="请输入服务与承诺的名称" v-decorator="['serviceName', {rules: [{required: true, min: 2, message: '请输入至少2个字符'}]}]" />
                </a-form-item>
                <a-form-item label="概述" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-textarea placeholder="请输入概述内容 (300个字符以内)" :autoSize="{ minRows: 4 }" v-decorator="['summary']" />
                </a-form-item>
                <a-form-item label="是否默认" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="新增商品时是否默认勾选">
                    <a-radio-group v-decorator="['isDefault', {initialValue: 10, rules: [{required: true}]}]">
                        <a-radio :value=10>是</a-radio>
                        <a-radio :value=-10>否</a-radio>
                    </a-radio-group>
                </a-form-item>
                <a-form-item label="状态" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="用户端是否展示">
                    <a-radio-group v-decorator="['status', {initialValue: 10, rules: [{required: true}]}]">
                        <a-radio :value=10>显示</a-radio>
                        <a-radio :value=-10>隐藏</a-radio>
                    </a-radio-group>
                </a-form-item>
                <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="数字越小越靠前">
                    <a-input-number :min="0" v-decorator="['sortNo', {initialValue: 100, rules: [{required: true, message: '请输入至少1个数字'}]}]" />
                </a-form-item>
            </a-form>
        </a-spin>
    </a-modal>
</template>

<script>
import lodash from 'lodash'
import * as Api from '@/api/goods/goodsservice'
export default {
    data() {
        return {
            // 对话框标题
            title: '',
            // 标签布局属性
            labelCol: {
                span: 7,
            },
            // 输入框布局属性
            wrapperCol: {
                span: 13,
            },
            // modal(对话框)是否可见
            visible: false,
            // modal(对话框)确定按钮 loading
            confirmLoading: false,
            // 当前表单元素
            form: this.$form.createForm(this),

            // 当前记录
            record: {},
        }
    },
    methods: {
        /**
         * 显示对话框
         */
        edit(record) {
            Api.get({ id: record.id }).then((res) => {
                // 显示窗口
                this.title = '编辑记录'
                this.visible = true
                // 当前管理员记录
                this.record = record
                // 设置默认值
                this.setFieldsValue()
            })
        },

        /**
         * 设置默认值
         */
        setFieldsValue() {
            this.$nextTick(() => {
                this.form.setFieldsValue(
                    lodash.pick(this.record, [
                        'id',
                        'serviceName',
                        'summary',
                        'isDefault',
                        'status',
                        'sortNo',
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
            this.confirmLoading = true
            values.id = this.record['id']
            Api.addOrUpdate(values)
                .then((result) => {
                    // 显示成功
                    this.$message.success(result.message, 1.5)
                    // 关闭对话框事件
                    this.handleCancel()
                    // 通知父端组件提交完成了
                    this.$emit('handleSubmit', values)
                })
                .finally((result) => {
                    this.confirmLoading = false
                })
        },
    },
}
</script>
