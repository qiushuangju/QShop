<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit">
                <a-form-item class="mb-20" label="短信平台" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['default', { rules: [{ required: true }] }]" @change="onChangeEngine">
                        <a-radio v-for="(engine, index) in record.engine" :key="index" :value="index">
                            <span>{{ engine.name }}{{index}}</span>
                            <a-tag v-if="index === 'aliyun'" class="ml-5" color="green">推荐</a-tag>
                        </a-radio>
                    </a-radio-group>

                    <div v-if="form.getFieldValue('default')" class="form-item-help">
                        <small>短信平台管理地址：</small>
                        <a :href="record.engine[form.getFieldValue('default')].website" target="_blank">{{ record.engine[form.getFieldValue('default')].website }}</a>
                    </div>
                </a-form-item>

                <!-- 阿里云配置 -->
                <div v-show="form.getFieldValue('default') === 'aliyun'">
                    <a-form-item label="AccessKeyId" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`engine.aliyun.accessKeyId`]" />
                    </a-form-item>
                    <a-form-item label="AccessKeySecret" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`engine.aliyun.accessKeySecret`]" />
                    </a-form-item>
                    <a-form-item label="短信签名 Sign" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`engine.aliyun.sign`]" />
                    </a-form-item>
                </div>
                <!-- 腾讯云配置 -->
                <div v-show="form.getFieldValue('default') === 'qcloud'">
                    <a-form-item label="SdkAppID" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`engine.qcloud.sdkAppID`]" />

                    </a-form-item>
                    <a-form-item label="AccessKeyId" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`engine.qcloud.accessKeyId`]" />
                    </a-form-item>
                    <a-form-item label="AccessKeySecret" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`engine.qcloud.accessKeySecret`]" />
                    </a-form-item>
                    <a-form-item label="短信签名 Sign" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`engine.qcloud.sign`]" />
                    </a-form-item>
                </div>
                <!-- 七牛云配置 -->
                <div v-show="form.getFieldValue('default') === 'qiniu'">
                    <a-form-item label="AccessKey" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`engine.qiniu.accessKey`]" />
                    </a-form-item>
                    <a-form-item label="SecretKey" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`engine.qiniu.secretKey`]" />
                    </a-form-item>
                </div>

                <!-- 短信场景配置 -->
                <div v-for="(item, index) in record['scene']" :key="index">
                    <a-divider orientation="left">{{ item.name }}</a-divider>
                    <a-form-item label="是否开启" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-radio-group v-decorator="[`scene.${index}.isEnable`, { rules: [{ required: true }] }]">
                            <a-radio :value="true">开启</a-radio>
                            <a-radio :value="false">关闭</a-radio>
                        </a-radio-group>
                    </a-form-item>
                    <a-form-item label="模板内容" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <span>{{ record.scene[index].contentPractical }}</span>
                    </a-form-item>
                    <a-form-item label="模板ID/Code" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`scene.${index}.templateCode`]" />
                        <div class="form-item-help">
                            <small>例如：SMS_139800030</small>
                        </div>
                    </a-form-item>
                    <a-form-item v-if="record.scene[index].acceptPhone !== undefined" label="接收手机号" :labelCol="labelCol" :wrapperCol="wrapperCol" required>
                        <a-input v-decorator="[`scene.${index}.acceptPhone`]" />
                        <!-- <div class="form-item-help">
              <small>
                注：如需填写多个手机号，请用英文逗号
                <a-tag>,</a-tag>隔开
              </small>
            </div>-->
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
import { pick, omit } from 'lodash'
import { isEmpty } from '@/utils/util'
import * as Api from '@/api/setting/storesetting'
import SettingSmsSceneEnum from '@/common/enum/setting/sms/Scene'

export default {
    data() {
        return {
            SettingSmsSceneEnum,
            // 当前设置项的key
            key: 'sms',
            // 标签布局属性
            labelCol: { span: 3 },
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

        // 切换短信平台事件
        onChangeEngine(e) {
            const app = this
            const engine = e.target.value
            for (const index in app.record.scene) {
                const item = app.record.scene[index]
                item.contentPractical = app.onVsprintf(
                    item.content,
                    item.variables[engine]
                )
            }
        },

        // 解析短信内容变量, 生成完整的模板内容
        onVsprintf(str, variables) {
            const reg = new RegExp('%s')
            for (var i = 0; i < variables.length; i++) {
                str = str.replace(reg, variables[i])
            }
            return str
        },

        /**
         * 设置默认值
         */
        setFieldsValue() {
            const app = this
            const { record, $nextTick, form } = app
            !isEmpty(form.getFieldsValue()) &&
                $nextTick(() => {
                    const scene = {}
                    for (const index in record.scene) {
                        const item = record.scene[index]
                        const contentPractical = app.onVsprintf(
                            item.content,
                            item.variables[record.default]
                        )
                        app.$set(item, 'contentPractical', contentPractical)
                        scene[index] = pick(item, [
                            'isEnable',
                            'templateCode',
                            'acceptPhone',
                        ])
                    }
                    const engine = {}
                    for (const index in record.engine) {
                        engine[index] = omit(record.engine[index], [
                            'name',
                            'website',
                        ])
                    }
                    form.setFieldsValue({
                        default: record.default,
                        engine,
                        scene,
                    })
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
    margin-bottom: 10px;
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
