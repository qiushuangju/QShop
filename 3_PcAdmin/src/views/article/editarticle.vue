<template>

    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <el-divider></el-divider>
              <a-form :form="form" @submit="handleSubmit" :selfUpdate="true">
                <a-tabs :activeKey="tabKey" :tabBarStyle="{marginBottom: '30px'}" @change="handleTabs">
                <a-tab-pane :key="0" tab="基本信息"></a-tab-pane>
                <a-tab-pane :key="1" tab="文章内容"></a-tab-pane>
                </a-tabs>
                <div class="tabs-content">
                    <div class="tab-pane" v-show="tabKey == 0">
                        <a-form-item label="标题" :labelCol="labelCol" :wrapperCol="{span: 16}">
                            <a-input placeholder="请输入标题" v-decorator="['title', { rules: [{ required: true, message: '请输入标题' }] }]" />
                        </a-form-item>
                         <a-form-item label="文章类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-select style="width: 300px" v-decorator="['categoryId', { rules: [{ required: true, message: '请选择文章类型' }] }]" placeholder="请选择文章类型">
                            <a-select-option v-for="item in articleCategoryList" :key="item.id" :value="item.id">{{ item.name }}</a-select-option>
                            </a-select>
                        </a-form-item>
                         <a-form-item label="封面" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议尺寸：750*750像素">
                            <SelectImage multiple :maxNum="1" 
                            :defaultList="temp.listImg" 
                            v-decorator="['imageId', { rules: [{ required: true, message: '请至少上传1张商品图片' }] }]" />
                        </a-form-item>
                        <a-form-item label="文章状态" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-radio-group v-decorator="['status', {initialValue: 10, rules: [{ required: true }]}]" >
                                <a-radio v-for="item in ArticleStatusList" :key="item.value" :value="item.value">{{item.text}}</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item label="是否删除" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-radio-group v-decorator="['isDelete', {initialValue: 0, rules: [{ required: true }]}]" >
                                <a-radio  value="0">否</a-radio>
                                <a-radio  value="1">是</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="数字越小越靠前">
                            <a-input-number :min="1"  v-decorator="['sort', {initialValue: 1, rules:[{ required: true }]}]" />
                        </a-form-item>
                    </div>
                    <div class="tab-pane" v-show="tabKey == 1">
                        <a-form-item label="公告内容" :labelCol="labelCol" :wrapperCol="{span: 16}">
                            <Ueditor v-decorator="['content', {rules: [{required: true, message: '公告不能为空'}]}]" />
                        </a-form-item>
                    </div>
                </div>
                <a-form-item class="mt-20" :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
                    <a-button type="primary" html-type="submit" :loading="isBtnLoading">提交</a-button>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-card>
</template>
<script>
import * as article from "@/api/article/article";
import * as articlecategory from "@/api/article/articlecategory";
import enums from'@/api/enumList';
import { Ueditor,SelectImage } from '@/components'
import _ from 'lodash'
import { debuglog, debug } from 'util'

export default {
    components: { Ueditor,SelectImage },
    data() {
        return {
            // loading状态
            isLoading: false,
            ArticleStatusList:[],//文章状态
            form: this.$form.createForm(this),
            tabKey: 0,
            labelCol: { span: 3 },
            wrapperCol: { span: 10 },
            isBtnLoading: false,
            articleId: null,
            dialogFormVisible: false,
            articleCategoryList:[],//文章类型
            content:"",//内容
            temp: {
                id: '', // Id
                title:'',//标题
                imageId:'',//封面
                categoryId:'',
                status:'',
                isDelete:'',
                content:'',//内容
                listImg:[],//图片
            },
        }
    },
    created() {
        new Promise((resolve, reject) => {
                Promise.all([
                        this.loadArticleCategory().then((res)=>{
                            this.articleCategoryList=res
                        }),
                        enums.getArticleStatusList().then((res)=>{
                            this.ArticleStatusList=res
                        }),        
                    ]).then(() => {
                    this.articleId = this.$route.params && this.$route.params.id
                    if (this.articleId != null && this.articleId.trim() != '') {
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
            article.get({ id: this.articleId }).then((res) => {
                this.form.resetFields()
                this.$nextTick(() => {
                    if (res.result.imageId != null && res.result.imageId != '') {
                        res.result.listImg = [
                            {
                                id: res.result.imageId,
                                thumbnail: res.result.urlCover,
                            },
                        ]
                    }
                    this.temp = res.result
                    this.form.setFieldsValue(
                        _.pick(
                            res.result,
                            'title',
                            'content',
                            'categoryId',
                            'imageId',
                            'sort',
                            'status',
                            'isDelete'
                        )
                    )
                })
            })
        },
        //加载文章类型
        loadArticleCategory(){
            return new Promise((resolve, reject) => {
                    articlecategory.listByWhere().then(res => {
                    resolve(res.result)
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
                formData.id = this.articleId
                formData.imageId=formData.imageId.join(',')
                article.addOrUpdate(formData)
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
            const tabsFieldsMap = [['title','categoryId','imageId','isDelete','sort','status'],
            ['content']]
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