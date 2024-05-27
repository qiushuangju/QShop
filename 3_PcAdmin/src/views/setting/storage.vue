<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit">
                <a-form-item label="默认上传方式" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['default', { rules: [{ required: true }] }]">
                        <a-radio v-for="(item, index) in StorageEnum.data" :key="index" :value="item.value">{{ item.name }}
                            {{ item.value == StorageEnum.LOCAL.value ? '(不推荐)' : '' }}</a-radio>
                    </a-radio-group>
                </a-form-item>
                <!-- 七牛云配置 -->
                <div v-show="form.getFieldValue('default') == StorageEnum.QINIU.value">
                    <a-form-item label="存储空间名称 Bucket" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.QINIU.value}.bucket`]" />
                    </a-form-item>
                    <a-form-item label="ACCESS_KEY AK" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.QINIU.value}.access_key`]" />
                    </a-form-item>
                    <a-form-item label="SECRET_KEY SK" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.QINIU.value}.secret_key`]" />
                    </a-form-item>
                    <a-form-item label="空间域名 Domain" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.QINIU.value}.domain`]" />
                    </a-form-item>
                </div>
                <!-- 阿里云配置 -->
                <div v-show="form.getFieldValue('default') == StorageEnum.ALIYUN.value">
                    <a-form-item label="存储空间名称 Bucket" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.ALIYUN.value}.bucket`]" />
                    </a-form-item>
                    <a-form-item label="AccessKeyId" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.ALIYUN.value}.access_key_id`]" />
                    </a-form-item>
                    <a-form-item label="AccessKeySecret" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.ALIYUN.value}.access_key_secret`]" />
                    </a-form-item>
                    <a-form-item label="空间域名 Domain" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.ALIYUN.value}.domain`]" />
                    </a-form-item>
                </div>
                <!-- 腾讯云配置 -->
                <div v-show="form.getFieldValue('default') == StorageEnum.QCLOUD.value">
                    <a-form-item label="存储空间名称 Bucket" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.QCLOUD.value}.bucket`]" />
                    </a-form-item>
                    <a-form-item label="所属地域 Region" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.QCLOUD.value}.region`]" />
                    </a-form-item>
                    <a-form-item label="SecretId" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.QCLOUD.value}.secret_id`]" />
                    </a-form-item>
                    <a-form-item label="SecretKey" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.QCLOUD.value}.secret_key`]" />
                    </a-form-item>
                    <a-form-item label="空间域名 Domain" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${StorageEnum.QCLOUD.value}.domain`]" />
                    </a-form-item>
                </div>
                <a-form-item :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
                    <a-button type="primary" html-type="submit">提交</a-button>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-card>
</template>

<script>
import { pick } from 'lodash'
import * as Api from '@/api/setting/storesetting'
import StorageEnum from '@/common/enum/file/Storage'
import { isEmpty } from '@/utils/util'

export default {
    data() {
        return {
            // 当前设置项的key
            key: 'storage',
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
            // 枚举类
            StorageEnum,
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
                    form.setFieldsValue(pick(record, ['default', 'engine']))
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
/deep/.ant-form-item-control {
    padding-left: 10px;

    .ant-form-item-control {
        padding-left: 0;
    }
}
</style>
