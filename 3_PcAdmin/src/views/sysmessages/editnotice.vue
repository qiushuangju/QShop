<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <el-divider></el-divider>
              <a-form :form="form" @submit="handleSubmit" :selfUpdate="true">
                <div class="tabs-content">
                  <a-form-item label="标题" :labelCol="labelCol" :wrapperCol="{span: 16}">
                     <a-input placeholder="请输入标题" v-decorator="['title', { rules: [{ required: true, message: '请输入标题' }] }]" />
                  </a-form-item>
              <a-form-item label="公告内容" :labelCol="labelCol" :wrapperCol="{span: 16}">
                <Ueditor v-decorator="['content', {rules: [{required: true, message: '公告不能为空'}]}]" />
              </a-form-item>
                </div>
                <a-form-item class="mt-20" :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
                    <a-button type="primary" html-type="submit" :loading="isBtnLoading">提交</a-button>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-card>
</template>
<script>
  import * as sysMessages from '@/api/sysmessages'
import { Ueditor } from '@/components'
import _ from 'lodash'
import { debuglog, debug } from 'util'

export default {
    components: { Ueditor },
    data() {
        return {
            // loading状态
            isLoading: false,
            form: this.$form.createForm(this),
            tabKey: 0,
            labelCol: { span: 3 },
            wrapperCol: { span: 10 },
            isBtnLoading: false,
            noticeId: null,
            dialogFormVisible: false,
            content:"",//内容
            temp: {
                id: '', // Id
                title:'',//标题
                content:'',//内容
            },
        }
    },
    created() {
        this.noticeId = this.$route.params && this.$route.params.id
        if (this.noticeId != null && this.noticeId.trim() != '') this.loadData()
    },
    methods: {
        //加载数据
        loadData() {
            sysMessages.get({ id: this.noticeId }).then((res) => {
                this.form.resetFields()
                this.$nextTick(() => {
                    this.temp = res.result
                    this.form.setFieldsValue(
                        _.pick(
                            res.result,
                            'title',
                            'content',
                        )
                    )
                })
            })
        },
        handleSubmit(e) {
            e.preventDefault()
            // 表单验证
            const {
                form: { validateFields },
            } = this
            validateFields((errors, values) => {
                if (errors) {
                    this.onTargetTabError(errors)
                    return false
                }
                const formData = Object.assign({}, values)
                formData.id = this.noticeId
                sysMessages.addOrUpdateNotice(formData)
                    .then((result) => {
                        // 显示提示信息
                        this.$message.success(result.message, 1.5)
                        // 跳转到列表页
                        setTimeout(() => {
                            this.$store.state.tagsView.visitedViews.splice(
                                this.$store.state.tagsView.visitedViews.findIndex(
                                    (item) => item.path === this.$route.path
                                ),
                                1
                            )
                            this.$router.push(
                                this.$store.state.tagsView.visitedViews[
                                    this.$store.state.tagsView.visitedViews
                                        .length - 1
                                ].path
                            )
                        }, 1500)
                    })
                    .catch(() => {
                        this.isBtnLoading = false
                    })
                    .finally(() => {
                        this.isLoading = false
                    })
            })
        },
        onTargetTabError(errors) {
            // 表单字段与tabKey对应关系
            // 只需要必填字段就可
            const tabsFieldsMap = [['title', 'content']]
            const field = Object.keys(errors).shift()
            for (const key in tabsFieldsMap) {
                if (tabsFieldsMap[key].indexOf(field) > -1) {
                    this.tabKey = parseInt(key)
                    break
                }
            }
        },
    },
}
</script>

<style>
</style>