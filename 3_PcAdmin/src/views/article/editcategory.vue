<template>

    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <el-divider></el-divider>
              <a-form :form="form" @submit="handleSubmit" :selfUpdate="true">
                <div class="tabs-content">
                        <a-form-item label="分类名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-input placeholder="请输入分类名称" v-decorator="['name', { rules: [{ required: true, message: '请输入分类名称' }] }]" />
                        </a-form-item>
                         <a-form-item label="图片" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议尺寸：750*750像素">
                            <SelectImage multiple :maxNum="1" 
                            :defaultList="temp.listImg" 
                            v-decorator="['imageId', { rules: [{ required: true, message: '请选择图片' }] }]" />
                        </a-form-item>
                        <a-form-item label="状态" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-radio-group v-decorator="['status', {initialValue: 10, rules: [{ required: true,message: '请选择状态' }]}]" >
                                <a-radio v-for="item in ArticleStatusList" :key="item.value" :value="item.value">{{item.text}}</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="数字越小越靠前">
                            <a-input-number :min="1"  v-decorator="['sort', {initialValue: 1, rules:[{ required: true }]}]" />
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
import * as articlecategory from "@/api/article/articlecategory";
import enums from'@/api/enumList';
import { Ueditor,SelectImage } from '@/components'
import _ from 'lodash'
import { debuglog, debug } from 'util'

export default {
    components: { SelectImage },
    data() {
        return {
            // loading状态
            isLoading: false,
            ArticleStatusList:[],//状态
            form: this.$form.createForm(this),
            tabKey: 0,
            labelCol: { span: 3 },
            wrapperCol: { span: 10 },
            isBtnLoading: false,
            categoryId: null,
            dialogFormVisible: false,
            content:"",//内容
            temp: {
                id: '', // Id
                name:'',//标题
                imageId:'',//封面
                status:'',
                sort:'',
                listImg:[],//图片
            },
        }
    },
    created() {
        new Promise((resolve, reject) => {
                Promise.all([
                        enums.getArticleStatusList().then((res)=>{
                            this.ArticleStatusList=res
                        }),        
                    ]).then(() => {
                    this.categoryId = this.$route.params && this.$route.params.id
                    if (this.categoryId != null && this.categoryId.trim() != '') {
                        this.loadData()
                    }
                })
        })
        
    },
    methods: {
        // 切换tab选项卡
        handleTabs(key) {
        this.tabKey = key;
        },
        //加载数据
        loadData() {
            articlecategory.get({ id: this.categoryId }).then((res) => {
                this.form.resetFields()
                this.$nextTick(() => {
                  if (res.result.imageId != null && res.result.imageId != '') {
                        res.result.listImg = [
                            {
                                id: res.result.imageId,
                                thumbnail: res.result.urlIcon,
                            },
                        ]
                    }
                    this.temp = res.result
                    this.form.setFieldsValue(
                        _.pick(
                            res.result,
                            'name',
                            'imageId',
                            'sort',
                            'status'
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
                formData.id = this.categoryId
                formData.imageId=formData.imageId.join(',')
                articlecategory.addOrUpdate(formData)
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
            const tabsFieldsMap = [['name','imageId','sort','status']]
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