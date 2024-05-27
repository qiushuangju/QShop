<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <div class="table-operator">
            <a-row>
                <a-col :span="6">
                    <a-button v-action:add type="primary" icon="plus" @click="handleEdit">新增</a-button>
                </a-col>
                <a-col :span="8" :offset="10">
                    <a-input-search style="max-width: 300px; min-width: 150px;float: right;" v-model="queryParam.search" placeholder="请输入服务名称" @search="onSearch" />
                </a-col>
            </a-row>
        </div>
        <s-table ref="table" rowKey="id" :loading="isLoading" :columns="columns" :data="loadData" :pageSize="15">
            <!-- 概述 -->
            <span slot="summary" slot-scope="text">
                <p class="summary oneline-hide">{{ text }}</p>
            </span>
            <!-- 是否默认 -->
            <span slot="isDefault" slot-scope="text">
                <a-tag :color="text ? 'green' : ''">{{ text ? '是' : '否' }}</a-tag>
            </span>
            <!-- 状态 -->
            <span slot="status" slot-scope="text">
                <a-tag :color="text ? 'green' : ''">{{ text ? '显示' : '隐藏' }}</a-tag>
            </span>
            <!-- 操作 -->
            <span slot="action" slot-scope="text, item">
                <a v-action:edit style="margin-right: 8px;" @click="handleEdit(item)">编辑</a>
                <a v-action:delete @click="handleDelete(item)">删除</a>
            </span>
        </s-table>
        <EditForm ref="EditForm" @handleSubmit="handleRefresh" />
    </a-card>
</template>

<script>
import * as Api from '@/api/goods/goodsservice'
import { STable } from '@/components'
import EditForm from './EditForm'

export default {
    name: 'Index',
    components: {
        STable,
        EditForm,
    },
    data() {
        return {
            // 查询参数
            queryParam: {},
            // 正在加载
            isLoading: false,
            // 表头
            columns: [
                // {
                //     title: '服务ID',
                //     dataIndex: 'id',
                // },
                {
                    title: '服务名称',
                    dataIndex: 'serviceName',
                    width: '300px',
                },
                {
                    title: '概述',
                    dataIndex: 'summary',
                    scopedSlots: { customRender: 'summary' },
                },
                {
                    title: '是否默认',
                    dataIndex: 'isDefault',
                    scopedSlots: { customRender: 'isDefault' },
                },
                {
                    title: '状态',
                    dataIndex: 'status',
                    scopedSlots: { customRender: 'status' },
                },
                {
                    title: '排序',
                    dataIndex: 'sortNo',
                },

                {
                    title: '操作',
                    dataIndex: 'action',
                    width: '180px',
                    scopedSlots: { customRender: 'action' },
                },
            ],
            // 加载数据方法 必须为 Promise 对象
            loadData: (param) => {
                return Api.load({ ...this.queryParam }).then((res) => {
                    res.total = 1
                    res.per_page = 15
                    res.current_page = 1
                    res.last_page = 1
                    return res
                })
            },
        }
    },
    created() {},
    methods: {
        /**
         * 编辑记录
         */
        handleEdit(item) {
            this.$refs.EditForm.edit(item)
        },

        /**
         * 删除记录
         */
        handleDelete(item) {
            const self = this

            this.$confirm('您确定要删除该记录吗?删除后不可恢复?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning',
            }).then(() => {
                // 多行删除
                Api.del([item.id]).then((result) => {
                    self.$message.success(result.message, 1.5)
                    self.handleRefresh()
                })
            })
        },

        /**
         * 刷新列表
         * @param Boolean bool 强制刷新到第一页
         */
        handleRefresh(bool = false) {
            this.$refs.table.refresh(bool)
        },

        /**
         * 检索查询
         */
        onSearch() {
            this.handleRefresh(true)
        },
    },
}
</script>

<style lang="less" scoped>
.summary {
    max-width: 400px;
}
</style>