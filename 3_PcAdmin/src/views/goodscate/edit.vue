<template>

    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <el-divider></el-divider>
            <a-form :form="form" @submit="handleSubmit" :selfUpdate="true">
                <div class="tabs-content">

                    <a-form-item label="上级分类" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="请参照分类模板决定分类层级">
                        <a-tree-select :treeData="treeCate" :dropdownStyle="{ maxHeight: '400px', overflow: 'auto' }" allowClear v-decorator="['parentId']" />
                        <div class="form-item-help">
                            <router-link target="_blank" :to="{ path: '/CateTemplate/index' }">查看分类模板</router-link>
                        </div>
                    </a-form-item>

                    <a-form-item label="分类名" :labelCol="labelCol" :wrapperCol="wrapperCol">
                        <a-input placeholder="请输入分类名称" v-decorator="['name', { rules: [{ required: true, message: '请输入分类名称' }] }]" />
                    </a-form-item>
                    <a-form-item label="分类图" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议尺寸：200*200像素">
                        <SelectImage :maxNum="1" :defaultList="imgList" v-decorator="['imageId', { rules: [{ required: true, message: '请选择分类图' }] }]" />
                    </a-form-item>
                    <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="数字越小越靠前">
                        <a-input-number :min="1" v-decorator="['sortNo', {initialValue: 1, rules:[{ required: true }]}]" />
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
import * as goodsCate from '@/api/goods/goodscate'
import CategoryModel from '../../common/model/Category'
import { SelectImage } from '@/components'
import _ from 'lodash'

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
            cateId: null,
            imgList: [], //图片
            treeCate: [],
            listStore: [], //商户
        }
    },
    created() {
        this.cateId = this.$route.params && this.$route.params.id
        if (this.cateId != null && this.cateId.trim() != '') {
            this.getData()
        }
        this.listCateData()
        this.listStoreData()
    },
    methods: {
        //加载数据
        getData() {
            goodsCate.get({ id: this.cateId }).then((res) => {
                this.form.resetFields()
                this.$nextTick(() => {
                    if (
                        res.result.imageId != null &&
                        res.result.imageId != ''
                    ) {
                        this.imgList = [
                            {
                                id: res.result.imageId,
                                thumbnail: res.result.urlImg,
                            },
                        ]
                    }
                    this.form.setFieldsValue(
                        _.pick(
                            res.result,
                            'name',
                            'sortNo',
                            'storeId',
                            'status',
                            'imageId',
                            'parentId'
                        )
                    )
                })
            })
        },
        listCateData() {
            CategoryModel.getCategoryTreeSelect().then((list) => {
                const tempTree = list
                // 顶级分类
                this.treeCate = [
                    {
                        title: '顶级分类',
                        key: '',
                        value: '',
                    },
                ].concat(tempTree)
            })
        },
        handleSubmit(e) {
            e.preventDefault()
            // 表单验证
            const {
                form: { validateFields },
            } = this
            validateFields((errors, values) => {
                const formData = Object.assign({}, values)
                if (this.cateId != null && this.cateId != '') {
                    formData.id = this.cateId
                }
                goodsCate
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
        //获取可选商户
        listStoreData() {},
    },
}
</script>

<style>
@import './style.less';
</style>