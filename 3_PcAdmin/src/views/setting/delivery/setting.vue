<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit">
                <a-form-item label="配送方式" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['default', { rules: [{ required: true }] }]">
                        <a-radio v-for="(item, index) in DeliveryEnum.data" :key="index" :value="item.value">{{ item.name  }}
                            {{ item.value == DeliveryEnum.KuaiDiNiao.value ? '(推荐)' : '' }}</a-radio>
                    </a-radio-group>
                </a-form-item>
                <a-divider orientation="left">物流查询API</a-divider>
                <div v-show="form.getFieldValue('default') == DeliveryEnum.KuaiDi100.value">
                    <a-form-item label="快递100 Customer" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${DeliveryEnum.KuaiDi100.value}.customer`]" />
                        <div class="form-item-help">
                            <small style="margin-right: 6px;">用于查询快递/物流信息，需快递100企业版API</small>
                            <a href="https://api.kuaidi100.com/home" target="_blank">去申请</a>
                        </div>
                    </a-form-item>
                    <a-form-item label="快递100 Key" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${DeliveryEnum.KuaiDi100.value}.key`]" />
                    </a-form-item>
                </div>

                <div v-show="form.getFieldValue('default') == DeliveryEnum.KuaiDiNiao.value">
                    <a-form-item label="快递鸟商户ID" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${DeliveryEnum.KuaiDiNiao.value}.eBusinessID`]" />
                        <div class="form-item-help">
                            <small style="margin-right: 6px;">用于查询快递/物流信息，需快递鸟企业版API</small>
                            <a href="https://www.kdniao.com/" target="_blank">去申请</a>
                        </div>
                    </a-form-item>
                    <a-form-item label="快递鸟API key" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="[`engine.${DeliveryEnum.KuaiDiNiao.value}.apiKey`]" />
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
import { isEmpty } from '@/utils/util'
import DeliveryEnum from '@/common/enum/setting/Delivery'
export default {
    data() {
        return {
            // 当前设置项的key
            key: 'delivery',
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
            // 枚举类
            DeliveryEnum,
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
.ant-divider {
    margin-top: 50px !important;
}
</style>
