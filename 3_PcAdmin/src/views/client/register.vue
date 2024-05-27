<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit">
                <a-form-item label="默认登录/注册方式" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['registerMethod', { rules: [{ required: true }] }]">
                        <a-radio :value=10>手机号 + 短信验证码</a-radio>
                    </a-radio-group>
                    <div class="form-item-help">
                        <p class="extra">
                            <!-- <small>
             
            </small> -->
                            发送短信服务需要先配置
                            <router-link target="_blank" :to="{ path: '/setting/sms' }">短信通知设置</router-link>
                        </p>
                        <p class="extra">
                            <!-- <small> -->
                            使用手机号作为主账户可以实现多种客户端的账户统一，例如H5和微信小程序
                            <!-- </small> -->
                        </p>
                    </div>
                </a-form-item>

                <a-divider orientation="left">微信小程序授权登录</a-divider>

                <a-form-item label="一键授权登录/注册" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['isOauthMpweixin', { rules: [{ required: true }] }]">
                        <a-radio :value=1>
                            <span>开启</span>
                            <a-tag class="ml-5" color="green">推荐</a-tag>
                        </a-radio>
                        <a-radio :value=0>关闭</a-radio>
                    </a-radio-group>
                    <div class="form-item-help">
                        开启后在微信小程序端一键获取用户授权并登录和注册
                        <!-- <small></small> -->
                    </div>
                </a-form-item>

                <a-form-item v-show="form.getFieldValue('isOauthMpweixin') == 1" label="注册时绑定手机号" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['isForceBindMpweixin', { rules: [{ required: true }] }]">
                        <a-radio :value=1>
                            <span>强制绑定</span>
                            <a-tag class="ml-5" color="green">推荐</a-tag>
                        </a-radio>
                        <a-radio :value=0>不绑定</a-radio>
                    </a-radio-group>
                    <div class="form-item-help">
                        <p class="extra">开启后在微信小程序端一键授权注册时强制绑定手机号，仅首次注册时弹出</p>
                        <p v-show="form.getFieldValue('isForceBindMpweixin') == 0" class="extra c-red">如果不强制绑定手机号，会造成多端情况下同一个用户注册多个账户，强烈推荐绑定手机号</p>
                    </div>
                </a-form-item>

                <a-form-item v-show="form.getFieldValue('isOauthMpweixin') == 1 && form.getFieldValue('isForceBindMpweixin') == 0" label="手动绑定手机号" :labelCol="labelCol"
                    :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['isManualBind', { rules: [{ required: true }] }]">
                        <a-radio :value=1>显示</a-radio>
                        <a-radio :value=0>不显示</a-radio>
                    </a-radio-group>
                    <div class="form-item-help">
                        <small>用户在个人中心页可以手动操作绑定手机号</small>
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
// import pick from 'lodash.pick'

import { pick } from 'lodash'
import { isEmpty } from '@/utils/util'
// import * as Api from '@/api/setting/store'
import * as Api from '@/api/setting/storesetting'

import SettingSmsSceneEnum from '@/common/enum/setting/sms/Scene'

export default {
    data() {
        return {
            SettingSmsSceneEnum,
            // 当前设置项的key
            key: 'register',
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
                            'registerMethod',
                            'isManualBind',
                            'isOauthMpweixin',
                            'isForceBindMpweixin',
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
    margin-top: 50px !important;
}
</style>
