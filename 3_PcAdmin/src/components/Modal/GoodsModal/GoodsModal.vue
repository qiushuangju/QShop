<!-- 商品库选择 -->

<template>
    <a-modal class="noborder" :title="title" :width="820" :visible="visible" :isLoading="isLoading" :maskClosable="false" @ok="handleSubmit" @cancel="handleCancel">
        <!-- 搜索板块 -->
        <div class="table-operator">
            <a-row class="row-item-search">
                <a-form class="search-form" :form="searchForm" layout="inline" @submit="handleSearch">
                    <a-form-item label="商品名称">
                        <a-input v-decorator="['goodsName']" placeholder="请输入商品名称" />
                    </a-form-item>
                    <a-form-item label="商品分类">
                        <a-tree-select :treeData="categoryListTree" :dropdownStyle="{ maxHeight: '500px', overflow: 'auto' }" allowClear
                            v-decorator="['CateId', {initialValue: 0}]"></a-tree-select>
                    </a-form-item>
                    <a-form-item class="search-btn">
                        <a-button type="primary" icon="search" html-type="submit">搜索</a-button>
                    </a-form-item>
                </a-form>
            </a-row>
        </div>
        <!-- :data="loadData" -->
        <a-table ref="table" :scroll="{ y: '420px', scrollToFirstRowOnChange: true }" :rowKey="fieldName" :loading="isLoading" :columns="columns" :dataSource="dataList"
            :rowSelection="rowSelection" :pageSize="15">
            <!-- 商品信息 -->
            <template slot="item" slot-scope="item">
                <GoodsItem :data="{
            image: item.urlImageMain,
            imageAlt: '商品图片',
            title: item.goodsName,
            subtitle: `¥${item.salePrice}`
          }" :subTitleColor="true" />
            </template>
            <!-- 商品状态 -->
            <span slot="status" slot-scope="text">
                <a-tag :color="text == 10 ? 'green' : 'red'">{{ text == 10 ? '上架' : '下架' }}</a-tag>
            </span>
        </a-table>
    </a-modal>
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import cloneDeep from 'lodash'
import * as GoodsApi from '@/api/goods/goods'
import CategoryModel from '@/common/model/Category'
import { STable, GoodsItem } from '@/components/Table'

// table表头
const columns = [
    // {
    //     title: '商品ID',
    //     dataIndex: 'id',
    // },
    {
        title: '商品信息',
        width: '450px',
        scopedSlots: { customRender: 'item' },
    },
    // {
    //     title: '划线价',
    //     width: '90px',
    //     dataIndex: 'linePrice',
    //     scopedSlots: { customRender: 'linePrice' },
    // },
    // {
    //     title: '售价',
    //     width: '70px',
    //     dataIndex: 'salePrice',
    //     scopedSlots: { customRender: 'salePrice' },
    // },
    {
        title: '库存总量',
        dataIndex: 'stockTotal',
    },
    {
        title: '状态',
        dataIndex: 'status',
        scopedSlots: { customRender: 'status' },
    },
]

export default {
    name: 'GoodsModal',
    props: {
        // 多选模式, 如果false为单选
        multiple: PropTypes.bool.def(true),
        // 最大选择的数量限制, multiple模式下有效
        maxNum: PropTypes.integer.def(100),
        // 默认选中的列表记录
        defaultList: PropTypes.array.def([]),
    },
    components: {
        // STable,
        GoodsItem,
    },
    data() {
        return {
            // 对话框标题
            title: '商品库',
            // modal(对话框)是否可见
            visible: false,
            // loading状态
            isLoading: false,
            // 当前表单元素
            searchForm: this.$form.createForm(this),
            // 查询参数
            queryParam: {},
            // table表头
            columns,
            dataList: [],
            // 加载数据方法 必须为 Promise 对象
            loadData: (param) => {
                return GoodsApi.listByWhere({
                    ...param,
                    ...this.queryParam,
                }).then((response) => {
                    console.log('response.result', response.result)
                    return response.result
                })
            },
            // 主键ID字段名
            fieldName: 'id',
            // 已选择的元素
            selectedRowKeys: [],
            // 已选择的列表记录
            selectedItems: [],
            // 商品分类列表
            categoryListTree: [],
        }
    },
    computed: {
        rowSelection() {
            return {
                selectedRowKeys: this.selectedRowKeys,
                onChange: this.onSelectChange,
            }
        },
    },
    created() {
        this.loadDataList()
        // 获取商品分类列表
        this.getCategoryList()
    },
    methods: {
        // 显示对话框
        handle() {
            // 显示窗口
            this.visible = true
            this.$nextTick(() => {
                // 刷新列表数据
                this.handleRefresh(true)
                // 设置默认数据
                this.setDefaultValue()
            })
        },

        // 设置默认数据
        setDefaultValue() {
            const { fieldName, defaultList } = this
            if (defaultList.length) {
                this.selectedItems = cloneDeep(defaultList)
                this.selectedRowKeys = defaultList.map(
                    (item) => item[fieldName]
                )
            }
        },

        // 获取分类列表
        getCategoryList() {
            this.isLoading = true
            CategoryModel.getListFromScreen()
                .then((selectList) => {
                    this.categoryListTree = selectList
                })
                .finally((result) => {
                    this.isLoading = false
                })
        },

        loadDataList() {
            if (this.queryParam.CateId != null && this.queryParam.CateId == '0')
                this.queryParam.CateId = ''
            GoodsApi.listByWhere({
                ...this.queryParam,
            }).then((response) => {
                this.dataList = response.result
            })
        },

        // 选中项发生变化时的回调
        onSelectChange(selectedRowKeys, newSelectedItems) {
            const { selectedItems } = this
            this.selectedRowKeys = selectedRowKeys
            this.selectedItems = this.createSelectedItems(
                selectedRowKeys,
                selectedItems,
                newSelectedItems
            )
        },

        /**
         * 生成已选中的元素列表
         * @param array selectedRowKeys 当前已选中的ID集
         * @param array oldSelectedItems 已选择的列表记录 (change前)
         * @param array newSelectedItems 已选择的列表记录 (change后)
         */
        createSelectedItems(
            selectedRowKeys,
            oldSelectedItems,
            newSelectedItems
        ) {
            const { fieldName } = this
            const selectedItems = []
            oldSelectedItems.forEach((item) => {
                if (selectedRowKeys.includes(item[fieldName])) {
                    selectedItems.push(item)
                }
            })
            const oldSelectedKeys = oldSelectedItems.map(
                (item) => item[fieldName]
            )
            newSelectedItems.forEach((item) => {
                if (
                    !oldSelectedKeys.includes(item[fieldName]) &&
                    selectedRowKeys.includes(item[fieldName])
                ) {
                    selectedItems.push(item)
                }
            })
            return selectedItems
        },

        /**
         * 刷新列表
         * @param Boolean bool 强制刷新到第一页
         */
        handleRefresh(bool = false) {
            this.loadDataList()
        },

        // 确认搜索
        handleSearch(e) {
            e.preventDefault()
            this.searchForm.validateFields((error, values) => {
                if (!error) {
                    this.queryParam = { ...this.queryParam, ...values }
                    this.handleRefresh(true)
                }
            })
        },

        // 关闭对话框事件
        handleCancel() {
            this.visible = false
            this.queryParam = {}
            this.searchForm.resetFields()
            this.selectedRowKeys = []
            this.selectedItems = []
        },

        // 确认按钮
        handleSubmit(e) {
            e.preventDefault()
            // 通知父端组件提交完成了
            this.$emit('handleSubmit', {
                selectedItems: this.selectedItems,
                selectedRowKeys: this.selectedRowKeys,
            })
            // 关闭对话框
            this.handleCancel()
        },
    },
}
</script>

<style lang="less" scoped>
.ant-modal-root {
    background: #ccc;
    :v-deep(.ant-modal-body) {
        padding-bottom: 8px;
    }
    :v-deep(.ant-modal-footer) {
        padding-top: 0;
    }
}
// 搜索表单
.search-form {
    :v-deep(.ant-form-item-control-wrapper) {
        min-width: 180px;
    }
}
</style>
