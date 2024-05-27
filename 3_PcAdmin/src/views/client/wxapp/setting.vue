<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit">
                <a-form-item class="mt-30" label="小程序 AppID" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input v-decorator="['appId', { rules: [{ required: true, message: '请输入小程序AppID' }] }]" />
                </a-form-item>
                <a-form-item label="小程序 AppSecret" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input type="password" v-decorator="['appSecret', { rules: [{ required: true, message: '请输入小程序AppSecret' }] }]" />
                </a-form-item>
                <a-divider orientation="left">微信小程序支付</a-divider>
                <a-form-item label="微信商户号 partnerId" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input v-decorator="['partnerId', { rules: [{ required: true, message: '请输入微信商户号partnerId' }] }]" />
                </a-form-item>
                <a-form-item label="微信支付密钥 partnerAppKey" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input type="password" v-decorator="['partnerAppKey', { rules: [{ required: true, message: '请输入微信支付密钥' }] }]" />
                </a-form-item>
                <a-form-item class="mt-40" label="证书文件cert" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-textarea :autoSize="{ minRows: 4, maxRows: 6 }" v-decorator="['certPem']" />
                    <div class="form-item-help">
                        <small>使用文本编辑器打开apiclient_cert.pem文件，将文件的全部内容复制进来</small>
                    </div>
                </a-form-item>
                <a-form-item label="证书文件key" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-textarea :autoSize="{ minRows: 4, maxRows: 6 }" v-decorator="['keyPem']" />
                    <div class="form-item-help">
                        <small>使用文本编辑器打开apiclient_key.pem文件，将文件的全部内容复制进来</small>
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
            // 当前设置项key
            key: 'basicWxApp',
            // 标签布局属性
            labelCol: { span: 4 },
            // 输入框布局属性
            wrapperCol: { span: 10 },
            // loading状态
            isLoading: false,
            // 当前表单元素
            form: this.$form.createForm(this),

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
                    form.setFieldsValue(
                        pick(record, [
                            'appId',
                            'appSecret',
                            'partnerId',
                            'partnerAppKey',
                            'certPem',
                            'keyPem',
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
    margin-bottom: 15px;
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
