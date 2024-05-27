<template>
    <div>
        <a-button @click="handleSelectGoods">选择商品</a-button>
        <a-table v-show="selectedItems.length" class="table-goodsList" rowKey="id" :columns="columns" :dataSource="selectedItems" :pagination="false">
            <!-- 商品信息 -->
            <template slot="item" slot-scope="item">
                <GoodsSkuItem :data="{
            image: item.urlImg,
            imageAlt: '商品图片',
            title: item.goodsName,
            subtitle: `¥${item.salePrice}`,
            goodsProps:[item]
          }" :subTitleColor="true" />
            </template>
            <template slot="stockNum" slot-scope="item">
                <a-input-number :min="0" v-model="item.stockNum" />
            </template>
            <!-- 操作项 -->
            <span slot="action" slot-scope="text, item, index">
                <a @click="handleDeleteItem(index)">删除</a>
            </span>
        </a-table>
        <GoodsSkuModal ref="GoodsSkuModal" :multiple="multiple" :maxNum="maxNum" :defaultList="selectedItems" @handleSubmit="handleSelectGoodsSubmit" />
    </div>
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import _ from 'lodash'
import { GoodsSkuModal } from '@/components/Modal'
import { GoodsSkuItem } from '@/components/Table'

const columns = [
    // {
    //   title: '商品ID',
    //   dataIndex: 'id'
    // },
    {
        title: '商品信息',
        scopedSlots: { customRender: 'item' },
    },
    {
        title: '数量',
        scopedSlots: { customRender: 'stockNum' },
    },
    {
        title: '操作',
        width: '180px',
        dataIndex: 'action',
        scopedSlots: { customRender: 'action' },
    },
]

// 商品选择器组件
export default {
    name: 'SelectGoodsSku',
    components: {
        GoodsSkuModal,
        GoodsSkuItem,
    },
    model: {
        prop: 'value',
        event: 'change',
    },
    props: {
        // 多选模式, 如果false为单选
        multiple: PropTypes.bool.def(true),
        // 最大选择的数量限制, multiple模式下有效
        maxNum: PropTypes.integer.def(100),
        // 默认选中的商品
        defaultList: PropTypes.array.def([]),
    },
    data() {
        return {
            columns,
            // 已选择的商品列表
            selectedItems: [],
            // 已选择的商品ID集
            selectedGoodsIds: [],
        }
    },
    watch: {
        // 设置默认数据
        defaultList: {
            // 首次加载的时候执行函数
            immediate: true,
            handler(val) {
                const { selectedItems } = this
                if (val.length && !selectedItems.length) {
                    this.onUpdate(_.cloneDeep(val))
                }
            },
        },
    },
    created() {},
    methods: {
        // 更新数据
        onUpdate(selectedItems) {
            if (this.multiple || !selectedItems.length) {
                // 多选模式
                this.selectedItems = selectedItems
                this.selectedGoodsIds = selectedItems.map((item) => item.id)
            } else {
                // 单选模式
                const single = selectedItems[selectedItems.length - 1]
                this.selectedItems = [single]
                this.selectedGoodsIds = [single.id]
            }
            this.onChange()
        },

        // 打开商品选择器
        handleSelectGoods() {
            this.$refs.GoodsSkuModal.handle()
        },

        // 商品库modal确认回调
        handleSelectGoodsSubmit(result) {
            const { selectedItems } = result
            this.onUpdate(_.cloneDeep(selectedItems))
        },

        // 删除商品
        handleDeleteItem(index) {
            const { selectedItems } = this
            selectedItems.splice(index, 1)
            this.onUpdate(selectedItems)
        },

        // 触发change事件
        onChange() {
            const { multiple, selectedGoodsIds } = this
            const sGoodsIds = multiple
                ? selectedGoodsIds
                : selectedGoodsIds.length
                ? selectedGoodsIds[0]
                : undefined
            return this.$emit('change', {
                selectedItems: this.selectedItems,
                sGoodsIds: sGoodsIds,
            })
        },
    },
}
</script>

<style lang="less" scoped>
// 商品信息
.table-goodsList {
    margin-top: 10px;
    min-width: 620px;
}
</style>
