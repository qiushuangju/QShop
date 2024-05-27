<template>
    <a-modal :title="title" :width="720" :visible="visible" :confirmLoading="confirmLoading" :maskClosable="false" @ok="handleSubmit" @cancel="handleCancel">
        <a-spin :spinning="confirmLoading">
            <a-form :form="form">
                <a-form-item label="地址类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['type', {initialValue: 10, rules: [{required: true}]}]">
                        <a-radio :value="10">发货地址</a-radio>
                        <a-radio :value="20">退货地址</a-radio>
                    </a-radio-group>
                </a-form-item>
                <a-form-item label="联系人姓名" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input v-decorator="['name', {rules: [{required: true, message: '请输入联系人姓名'}]}]" />
                </a-form-item>
                <a-form-item label="联系电话" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input v-decorator="['phone', {rules: [{required: true, message: '请输入联系电话'}]}]" />
                </a-form-item>
                <a-form-item label="选择地区" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <SelectRegion placeholder="请选择省市区" v-decorator="['listRegion', {rules: [{required: true, message: '请选择省市区'}]}]" />
                </a-form-item>
                <a-form-item label="详细地址" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input v-decorator="['detail', {rules: [{required: true, message: '请输入详细地址'}]}]" />
                </a-form-item>
                <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="数字越小越靠前">
                    <a-input-number :min="0" v-decorator="['sort', {initialValue: 100, rules: [{required: true, message: '请输入排序值'}]}]" />
                </a-form-item>
            </a-form>
        </a-spin>
    </a-modal>
</template>
  
  <script>
import lodash from 'lodash'
import * as Api from '@/api/store/storeaddress'
import { SelectRegion } from '@/components'

export default {
    components: {
        SelectRegion,
    },
    data() {
        return {
            // 对话框标题
            title: '编辑地址',
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
            record: {},
        }
    },
    methods: {
        /**
         * 显示对话框
         */
        edit(id) {
            // 显示窗口
            this.visible = true
            if (id != '' && id != null) {
                Api.get({ id: id }).then((res) => {
                    // 当前记录
                    this.record = res.result
                    // 设置默认值
                    this.setFieldsValue()
                })
            }
        },

        /**
         * 设置默认值
         */
        setFieldsValue() {
            const {
                record,
                form: { setFieldsValue },
            } = this
            record.listRegion = [
                record.provinceId,
                record.cityId,
                record.regionId,
            ]
            this.$nextTick(() => {
                setFieldsValue(
                    lodash.pick(record, [
                        'type',
                        'name',
                        'phone',
                        'listRegion',
                        'detail',
                        // 'sort',
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
            values.id = this.record.id
            this.confirmLoading = true
            Api.addOrUpdate(values)
                .then((res) => {
                    // 显示成功
                    this.$message.success(res.message, 1.5)
                    // 关闭对话框事件
                    this.handleCancel()
                    // 通知父端组件提交完成了
                    this.$emit('handleSubmit', values)
                })
                .finally(() => {
                    this.confirmLoading = false
                })
        },
    },
}
</script>
  ./storeaddressedit.vue./StoreAddressEdit.vue