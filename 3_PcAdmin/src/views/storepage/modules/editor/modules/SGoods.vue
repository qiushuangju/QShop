<template>
    <div class="select-goods">
        <div class="data-preview clearfix">
            <draggable :list="selectedItems" v-bind="{ animation: 120, filter: 'input', preventOnFilter: false }">
                <div v-for="(item, index) in selectedItems" :key="index" class="data-item">
                    <a-icon class="icon-close" theme="filled" type="close-circle" @click="handleDeleteItem(index)" />
                    <a-tooltip>
                        <template slot="title">
                            <span class="f-12">{{ item.goodsName }}11</span>
                        </template>
                        <div class="item-inner">
                            <div class="item-image">
                                <img :src="item.urlImageMain" alt />
                            </div>
                        </div>
                    </a-tooltip>
                </div>
            </draggable>
        </div>
        <div class="data-add">
            <a-button icon="plus" @click="handleSelectGoods()">选择商品</a-button>
        </div>
        <GoodsModal ref="GoodsModal" :maxNum="maxNum" :defaultList="selectedItems" @handleSubmit="handleSelectGoodsSubmit" />
    </div>
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import draggable from 'vuedraggable'
import { pick } from 'lodash'
import { GoodsModal } from '@/components/Modal'

// 仅需要的字段
const itemColumns = [
    'id',
    'goodsName',
    'urlImageMain',
    'linePrice',
    'salePrice',
    'subTitle',
    'goodsSales',
]

// 图片选择器组件
export default {
    name: 'SelectGoods',
    components: {
        GoodsModal,
        draggable,
    },
    model: {
        prop: 'value',
        event: 'change',
    },
    props: {
        // 最大选择的数量限制, multiple模式下有效
        maxNum: PropTypes.integer.def(100),
        // 选中的商品数据
        value: PropTypes.array.def([]),
    },
    data() {
        return {
            // 已选择的商品列表
            selectedItems: [],
        }
    },
    watch: {
        value: {
            // 首次加载的时候执行函数
            immediate: true,
            handler(val) {
                this.onUpdate(val)
            },
        },
    },
    created() {},
    methods: {
        // 更新已选择的数据 selectedItems
        onUpdate(items) {
            this.selectedItems = items
            this.onChange()
        },

        // 打开商品选择器
        handleSelectGoods() {
            this.$refs.GoodsModal.handle()
        },

        // 商品库modal确认回调
        handleSelectGoodsSubmit(result) {
            const { selectedItems } = result
            this.onUpdate(this.filterItems(selectedItems))
        },

        // 过滤仅需要的数据
        filterItems(selectedItems) {
            return selectedItems.map((itm) => pick(itm, itemColumns))
        },

        // 删除商品
        handleDeleteItem(index) {
            const { selectedItems } = this
            if (selectedItems.length <= 1) {
                this.$message.warning('请至少保留1个', 1)
                return
            }
            selectedItems.splice(index, 1)
            this.onUpdate(selectedItems)
        },

        // 触发change事件
        onChange() {
            const { selectedItems } = this
            return this.$emit('change', selectedItems)
        },
    },
}
</script>

<style lang="less" scoped>
@import '~ant-design-vue/es/style/themes/default.less';
// 商品信息
.data-preview {
    .data-item {
        float: left;
        margin: 10px;
        height: 70px;
        width: 70px;
        background: #f7fafc;
        position: relative;
        border-radius: 3px;
        cursor: move;
        &:hover {
            .icon-close {
                display: block;
            }
        }
    }
    .icon-close {
        display: none;
        position: absolute;
        top: -8px;
        right: -8px;
        cursor: pointer;
        font-size: 16px;
        color: #c5c5c5;
        &:hover {
            color: #7d7d7d;
        }
    }
    .item-inner {
        padding: 10px;
        background: #f7fafc;
    }
    .item-image {
        width: auto;
        min-width: 40px;
        max-width: 220px;
        text-align: center;
        img {
            display: block;
            max-width: 100%;
            height: 50px;
        }
    }
}

// 新增数据
.data-add {
    margin-top: 10px;

    button {
        width: 100%;
        color: @primary-color;
        height: 40px;
        border-radius: 5px;
        font-size: 12px;
        border-color: #cccccc21;

        &:hover {
            border-color: @primary-color;
        }
    }
}
</style>
