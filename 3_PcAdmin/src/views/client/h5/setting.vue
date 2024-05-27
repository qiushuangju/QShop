<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit">
                <a-form-item class="mt-30" label="是否开启访问" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['enabled', { rules: [{ required: true }] }]">
                        <a-radio :value="10">开启</a-radio>
                        <a-radio :value="-10">关闭</a-radio>
                    </a-radio-group>
                    <div class="form-item-help">
                        <p class="extra">
                            <span>注：如关闭，用户则无法通过H5端访问商城</span>
                        </p>
                    </div>
                </a-form-item>
                <a-form-item label="H5站点地址" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input v-decorator="['baseUrl', { rules: [{ required: true, message: '请输入H5站点地址' }] }]" />
                    <div class="form-item-help">
                        <p class="extra">
                            <span>请填写H5端实际的访问地址，以</span>
                            <a-tag class="mlr-5">https://</a-tag>开头； 斜杠
                            <a-tag class="mlr-5">/</a-tag>
                            <span>结尾</span>
                        </p>
                        <p class="extra">
                            <span>例如：https://www.aaa.com/</span>
                        </p>
                    </div>
                </a-form-item>
                <a-form-item :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
                    <a-button type="primary" html-type="submit">提交</a-button>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-card>
</template>

<script>
import { pick } from 'lodash'
import { isEmpty } from '@/utils/util'
import * as Api from '@/api/setting/storesetting'

export default {
    data() {
        return {
            key: 'basicH5',
            // 标签布局属性
            labelCol: { span: 4 },
            // 输入框布局属性
            wrapperCol: { span: 10 },
            // loading状态
            isLoading: false,
            // 当前表单元素
            form: this.$form.createForm(this),
            // 当前设置项key

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
            Api.GetDetail({ key: this.key })
                .then((res) => {
                    // 当前记录
                    this.record = res.result
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
            const { record, $nextTick, form } = this
            !isEmpty(form.getFieldsValue()) &&
                $nextTick(() => {
                    form.setFieldsValue(pick(record, ['enabled', 'baseUrl']))
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
            this.isLoading = true
            Api.addOrUpdate({ key: this.key, data: values })
                .then((result) => {
                    // 显示提示信息
                    this.$message.success(result.message, 1.5)
                })
                .finally((result) => {
                    this.isLoading = false
                })
        },
    },
}
</script>
<style lang="less" scoped>
.ant-form-item {
    margin-bottom: 24px;
}
/deep/.ant-form-item-control {
    padding-left: 10px;

    .ant-form-item-control {
        padding-left: 0;
    }
}
.ant-divider {
    margin-top: 60px !important;
}
</style>
