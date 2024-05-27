<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit">
                <a-form-item label="未支付订单" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <div class="clearfix">
                        <a-input-number class="fl-l" :min="0" :precision="0" v-decorator="['order.closeHours', { rules: [{required: true, message: '不能为空'}] }]" />
                        <span class="input-text_right">小时后自动关闭</span>
                    </div>
                    <div class="form-item-help">
                        <p class="extra">如果在期间订单未付款，系统自动关闭，设置0小时不自动关闭</p>
                    </div>
                </a-form-item>
                <a-form-item label="已发货订单" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <div class="clearfix">
                        <a-input-number class="fl-l" :min="0" :precision="0" v-decorator="['order.receiveDays', { rules: [{required: true, message: '不能为空'}] }]" />
                        <span class="input-text_right">天后自动确认收货</span>
                    </div>
                    <div class="form-item-help">
                        <p class="extra">如果在期间未确认收货，系统自动完成收货，设置0天不自动收货</p>
                    </div>
                </a-form-item>
                <a-form-item label="已完成订单" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <div class="clearfix">
                        <a-input-number class="fl-l" :min="0" :precision="0" v-decorator="['order.refundDays', { rules: [{required: true, message: '不能为空'}] }]" />
                        <span class="input-text_right">天内允许申请售后</span>
                    </div>
                    <div class="form-item-help">
                        <p class="extra">订单完成后，用户在指定期限内可申请售后，设置0天不允许申请</p>
                    </div>
                </a-form-item>

                <a-form-item label="库存计算方式" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['order.deductStockType', {initialValue: 10, rules: [{ required: true }]}]">
                        <a-radio v-for="item in listDeductStockType" :key=item.value :value=item.value>{{item.text}}</a-radio>
                    </a-radio-group>
                </a-form-item>

                <a-divider orientation="left">运费设置</a-divider>
                <a-form-item label="运费组合策略" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['freightRule', { rules: [{required: true}] }]">
                        <div class="radio-item">
                            <a-radio :style="radioStyle" :value=10>叠加</a-radio>
                            <div class="form-item-help">
                                <p class="extra">订单中的商品有多个运费模板时，将每个商品的运费之和订为订单总运费</p>
                            </div>
                        </div>
                        <div class="radio-item">
                            <a-radio :style="radioStyle" :value=20>以最低运费结算</a-radio>
                            <div class="form-item-help">
                                <p class="extra">订单中的商品有多个运费模板时，取订单中运费最少的商品的运费计为订单总运费</p>
                            </div>
                        </div>
                        <div class="radio-item">
                            <a-radio :style="radioStyle" :value=30>以最高运费结算</a-radio>
                            <div class="form-item-help">
                                <p class="extra">订单中的商品有多个运费模板时，取订单中运费最多的商品的运费计为订单总运费</p>
                            </div>
                        </div>
                    </a-radio-group>
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
import * as Api from '@/api/setting/storesetting'
import { isEmpty } from '@/utils/util'
import * as commApi from '@/api/commApi'
export default {
    data() {
        return {
            // 当前设置项的key
            key: 'trade',
            // 标签布局属性
            labelCol: { span: 4 },
            // 输入框布局属性
            wrapperCol: { span: 10 },
            // loading状态
            isLoading: false,
            // 当前表单元素
            form: this.$form.createForm(this),
            // 单选框样式
            radioStyle: {
                display: 'block',
                height: '30px',
                lineHeight: '30px',
            },
            // 当前记录详情
            record: {},
            listDeductStockType: [], //库存计算方式
        }
    },
    // 初始化数据
    created() {
        // 获取当前详情记录
        this.getDetail()
        this.getListDeductStockType()
    },
    methods: {
        getListDeductStockType() {
            //库存计算方式
            return new Promise((resolve, reject) => {
                commApi.ListEnum({ name: 'DeductStockType' }).then((res) => {
                    this.listDeductStockType = res.result
                    resolve()
                })
            })
        },
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
                    form.setFieldsValue({
                        freightRule: record.freightRule,
                        order: pick(record.order, [
                            'closeHours',
                            'receiveDays',
                            'refundDays',
                            'deductStockType',
                        ]),
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
/deep/.ant-form-item-control {
    padding-left: 10px;

    .ant-form-item-control {
        padding-left: 0;
    }
}
.ant-divider {
    margin-top: 60px !important;
}
.ant-input-number {
    width: 160px;
}
.input-text_right {
    margin-left: 10px;
}
.radio-item {
    margin-bottom: 16px;
}
</style>
