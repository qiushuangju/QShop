<template>
    <a-card :bordered="false">
        <div class="card-title">分类页模板</div>
        <a-spin :spinning="isLoading">
            <div class="container clearfix">
                <!-- 模板预览 -->
                <div class="preview fl-l">
                    {{ form.getFieldValue('cartStyle')==null }}
                    <img v-if="form.getFieldValue('style')"
                        :src="`static/img/category/${form.getFieldValue('style')==PageCategoryStyleEnum.COMMODITY.value?form.getFieldValue('style')+''+form.getFieldValue('cartStyle'):form.getFieldValue('style')}.png`" />
                </div>
                <!-- 表单内容 -->
                <div class="form-box fl-r">
                    <a-form :form="form" @submit="handleSubmit">
                        <a-form-item label="分类页样式" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-radio-group v-decorator="['style',{ initialValue: PageCategoryStyleEnum.COMMODITY.value,rules: [{ required: true }],}, ]">
                                <a-radio v-for="(item, index) in PageCategoryStyleEnum.data" :key="index" :value="item.value">{{ item.name }}</a-radio>
                            </a-radio-group>
                            <div class="form-item-help">
                                <p v-if="form.getFieldValue('style') ==PageCategoryStyleEnum.COMMODITY.value" class="extra">
                                    分类图尺寸：宽150像素 高150像素
                                </p>
                                <p v-if="form.getFieldValue('style') ==PageCategoryStyleEnum.ONE_LEVEL_BIG.value" class="extra">
                                    分类图尺寸：宽750像素 高度不限
                                </p>
                                <p v-if="form.getFieldValue('style') == PageCategoryStyleEnum.ONE_LEVEL_SMALL.value" class="extra">
                                    分类图尺寸：宽188像素 高度不限
                                </p>
                                <p v-if="form.getFieldValue('style') ==PageCategoryStyleEnum.TWO_LEVEL.value" class="extra">
                                    分类图尺寸：宽150像素 高150像素
                                </p>
                            </div>
                        </a-form-item>
                        <a-form-item label="分享标题" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-input v-decorator="['shareTitle',{ rules: [{ required: true, message: '请输入分享标题' }] }]" />
                        </a-form-item>
                        <a-form-item label="按钮样式" v-if="form.getFieldValue('style')==PageCategoryStyleEnum.COMMODITY.value" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-radio-group v-decorator="['cartStyle',{ initialValue:1}, ]">
                                <a-radio v-for="(item, index) in 2" :key="index" :value="index + 1">样式{{index + 1}}</a-radio>
                            </a-radio-group>
                            <div class="form-item-help">
                                <p class="extra">
                                    加入购物车图标按钮的样式
                                </p>

                            </div>
                        </a-form-item>

                        <a-form-item :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
                            <a-button type="primary" html-type="submit">提交</a-button>
                        </a-form-item>
                    </a-form>
                </div>
            </div>
        </a-spin>
    </a-card>
</template>

<script>
// import pick from "lodash.pick";
// 导入lodash
import _ from 'lodash'
import * as Api from '@/api/setting/storesetting'
import { SettingEnum } from '@/common/enum/store'
import { PageCategoryStyleEnum } from '@/common/enum/store/page/category'

export default {
    components: {},
    data() {
        return {
            // 当前设置项的key
            key: SettingEnum.PageCategoryTemplate.value,
            // 标签布局属性
            labelCol: { span: 4 },
            // 输入框布局属性
            wrapperCol: { span: 20 },
            // loading状态
            isLoading: false,
            // 枚举类
            PageCategoryStyleEnum,
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
                form: { setFieldsValue },
            } = this
            // 表单内容
            this.$nextTick(() => {
                setFieldsValue(
                    _.pick(record, ['style', 'shareTitle', , 'cartStyle'])
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
:v-deep(.ant-form-item-control) {
    padding-left: 10px;
    .ant-form-item-control {
        padding-left: 0;
    }
}

// 内容区
.container {
    max-width: 1000px;
    margin: 0 auto;

    .preview {
        width: 300px;
        img {
            display: block;
            width: 100%;
            box-shadow: 0 3px 10px #dcdcdc;
        }
    }

    .form-box {
        width: 600px;
    }
}
</style>
