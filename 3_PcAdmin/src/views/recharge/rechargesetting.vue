<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit">
                <a-form-item label="是否开启会员充值" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['isEntrance', { rules: [{ required: true }] }]">
                        <a-radio :value=true>开启</a-radio>
                        <a-radio :value=false>关闭</a-radio>
                    </a-radio-group>
                    <div class="form-item-help">
                        <small>如设置关闭则用户端不显示充值按钮</small>
                    </div>
                </a-form-item>
                <a-form-item label="充值自定义金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['isCustom', { rules: [{ required: true }] }]">
                        <a-radio :value=true>允许</a-radio>
                        <a-radio :value=false>不允许</a-radio>
                    </a-radio-group>
                    <div class="form-item-help">
                        <small>是否允许用户填写自定义的充值金额</small>
                    </div>
                </a-form-item>
                <a-form-item v-show="form.getFieldValue('isCustom') == 1" label="最低充值金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input-number :min="0.01" :precision="2" v-decorator="['lowestMoney', { rules: [{ required: true, message: '请输入最低充值金额' }] }]" />
                    <span class="ml-10">元</span>
                    <div class="form-item-help">
                        <small>低于该设定金额时不允许充值</small>
                    </div>
                </a-form-item>
                <a-form-item label="自动匹配套餐" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['isMatchPlan', { rules: [{ required: true }] }]">
                        <a-radio :value=true>开启</a-radio>
                        <a-radio :value=false>关闭</a-radio>
                    </a-radio-group>
                    <div class="form-item-help">
                        <small>充值自定义金额时 是否自动匹配充值套餐，如不开启则不参与套餐金额赠送</small>
                    </div>
                </a-form-item>
                <a-form-item label="充值说明" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-textarea :autoSize="{ minRows: 4 }" v-decorator="['describe', { rules: [{ required: true, message: '请输入充值说明' }] }]" />
                </a-form-item>
                <a-form-item :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
                    <a-button type="primary" html-type="submit">提交</a-button>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-card>
</template>
  
  <script>
import lodash from 'lodash'
import * as Api from '@/api/setting/storesetting'
export default {
    components: {},
    data() {
        return {
            // 当前设置项的key
            key: 'recharge',
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
                .finally((res) => {
                    this.isLoading = false
                })
        },

        /**
         * 设置默认值
         */
        setFieldsValue() {
            const {
                record,
                $nextTick,
                form: { setFieldsValue },
            } = this
            $nextTick(() => {
                setFieldsValue(
                    lodash.pick(record, [
                        'isEntrance',
                        'isCustom',
                        'lowestMoney',
                        'isMatchPlan',
                        'describe',
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
                .then((res) => {
                    // 显示提示信息
                    this.$message.success(res.message, 1.5)
                })
                .finally((res) => {
                    this.isLoading = false
                })
        },
    },
}
</script>
  <style lang="less" scoped>
.ant-form-item {
    .ant-form-item {
        margin-bottom: 0;
    }
}

:deep(.ant-form-item-control) {
    padding-left: 10px;
    .ant-form-item-control {
        padding-left: 0;
    }
}
</style>
  