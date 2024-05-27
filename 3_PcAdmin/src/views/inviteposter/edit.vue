<template>

    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <el-divider></el-divider>
            <a-form :form="form" @submit="handleSubmit" :selfUpdate="true">
                <div class="tabs-content">
                  <a-form-item label="海报名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input v-decorator="['posterName', {rules:[{ required: true,message: '请输入海报名称'  }]}]" />
                    </a-form-item>
                    <a-form-item label="海报" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议尺寸：750*750像素">
                        <SelectImage :maxNum="1" :defaultList="temp.file" v-decorator="['imageId', { rules: [{ required: true, message: '请选择/上传海报' }] }]" />
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
import * as invitePoster from '@/api/inviteposter'
import { SelectImage } from '@/components'
import _ from 'lodash'
import { debuglog, debug } from 'util'

export default {
    components: { SelectImage },
    data() {
        return {
            // loading状态
            isLoading: false,
            form: this.$form.createForm(this),
            tabKey: 0,
            labelCol: { span: 3 },
            wrapperCol: { span: 10 },
            isBtnLoading: false,
            inviteposterId: null,
            temp: {
              id : '',
              posterName : '',//图片名称
              imageId : '',//图片id
              countRegUser : '',//已邀请人数
              createTime : '',//
              urlThumbnail : '',//缩略图
              urlImg : '',//图片地址
              file: [], //图片列表
            },
        }
    },
    created() {
        this.inviteposterId = this.$route.params && this.$route.params.id
        if (this.inviteposterId != null && this.inviteposterId.trim() != '') 
          this.loadData()
    },
    methods: {
        //加载数据
        loadData() {
            invitePoster.get({ id: this.inviteposterId }).then((res) => {
                this.form.resetFields()
                this.$nextTick(() => {
                    if (res.result.imageId != null && res.result.imageId != '') {
                        res.result.file = [
                            {
                                id: res.result.imageId,
                                thumbnail: res.result.urlThumbnail,
                            },
                        ]
                        this.temp = res.result
                    }
                    this.form.setFieldsValue(
                        _.pick(res.result, 'posterName','imageId')
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
                const formData = Object.assign({}, this.temp)
                formData.posterName=values.posterName
                formData.imageId=values.imageId
                invitePoster
                    .addOrUpdate(formData)
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
        loadList() {
            
        },
        onTargetTabError(errors) {
            // 表单字段与tabKey对应关系
            // 只需要必填字段就可
            const tabsFieldsMap = [['posterName','imageId']]
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
@import './style.less';
</style>