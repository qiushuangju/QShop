<template>
    <div class="image-list clearfix" :class="{ multiple }">
        <!-- 文件列表 -->
        <!-- draggable是拖拽组件 -->
        <draggable v-if="selectedItems.length" v-model="selectedItems" @start="drag=true" @end="drag=false" @update="onUpdate">
            <transition-group class="draggable-item" type="transition" :name="'flip-list'">
                <div v-for="(item, index) in selectedItems" :key="item.id" class="file-item" :style="{ width: `${width}px`, height: `${width}px` }">
                    <!-- 预览图 -->
                    <a :href="item.thumbnail" target="_blank">
                        <div class="img-cover" :style="{ backgroundImage: `url('${item.thumbnail}')` }"></div>
                    </a>
                    <!-- 删除文件 -->
                    <a-icon class="icon-close" theme="filled" type="close-circle" @click="handleDeleteFileItem(index)" />
                </div>
            </transition-group>
        </draggable>
        <!-- 图片选择器 -->
        <!-- 如果单选, selectedItems无内容时 显示 -->
        <!-- 如果多选, selectedItems数量小于 maxNum 显示 -->
        <div v-show="(!multiple && selectedItems.length <= 0) || (multiple && selectedItems.length < maxNum)" class="selector"
            :style="{width: `${width}px`, height: `${width}px`}" title="点击选择图片" @click="handleSelectImage">
            <a-icon class="icon-plus" :style="{ fontSize: `${width * 0.4}px` }" type="plus" />
        </div>
        <!-- 文件选择器 -->
        <FilesModal ref="FilesModal" :multiple="multiple" :maxNum="maxNum" :permission="permission" :selectedNum="selectedItems.length"
            @handleSubmit="handleSelectImageSubmit" />
    </div>
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import draggable from 'vuedraggable'
import _ from 'lodash'
import { FilesModal } from '@/components/Modal'

// 图片选择器组件
export default {
    name: 'SelectImage',
    components: {
        FilesModal,
        draggable,
    },
    model: {
        prop: 'value',
        event: 'change',
    },
    props: {
        // 多选模式, 如果false为单选
        multiple: PropTypes.bool.def(false),
        // 最大选择的数量限制, multiple模式下有效
        maxNum: PropTypes.integer.def(100),
        // 默认选中的文件
        defaultList: PropTypes.array.def([]),
        // 元素的尺寸(宽)
        width: PropTypes.integer.def(80),
        // 数据权限(self:自己 role:同一角色 空串:所有 )
        permission: PropTypes.string.def(''),
    },
    data() {
        return {
            // 选择的图片列表
            selectedItems: [],
            // 禁止传参 (防止selectedItems为空时defaultList重新赋值)
            allowProps: true,
        }
    },
    watch: {
        // 设置默认数据
        defaultList: {
            // 首次加载的时候执行函数
            immediate: true,
            handler(val) {
                const { selectedItems, allowProps } = this
                if (val.length && !selectedItems.length && allowProps) {
                    this.selectedItems = _.cloneDeep(val, true, true)
                    this.onChange()
                }
            },
        },
    },
    created() {},
    methods: {
        // 拖动排序后的回调事件
        onUpdate() {
            this.onChange()
        },
        // 打开文件选择器
        handleSelectImage() {
            this.$refs.FilesModal.show()
        },
        // 文件库modal确认回调
        handleSelectImageSubmit(result) {
            if (result.length > 0) {
                // 记录选中的图片列表
                const { multiple, selectedItems } = this
                this.selectedItems = multiple
                    ? selectedItems.concat(result)
                    : result
                this.onChange()
            }
        },
        // 删除文件
        handleDeleteFileItem(index) {
            this.selectedItems.splice(index, 1)
            if (this.selectedItems.length === 0) {
                this.allowProps = false
            }
            this.onChange()
        },

        // 触发change事件
        onChange() {
            const { multiple, selectedItems } = this
            if (selectedItems.length <= 0) {
                return this.$emit('change', multiple ? [] : '')
            }
            // 生成fileId
            const id = multiple
                ? selectedItems.map((item) => item.id)
                : selectedItems[0].id
            // 触发change事件
            return this.$emit('change', id, selectedItems)
        },
    },
}
</script>

<style lang="less" scoped>
:v-deep(.flip-list-move) {
    transition: transform 0.3s !important;
}
:v-deep(.no-move) {
    transition: transform 0s;
}

.image-list {
    // 多选模式下margin
    &.multiple {
        .file-item,
        .selector {
            margin-right: 10px;
            margin-bottom: 10px;
        }
    }
}

// 文件元素
.file-item {
    position: relative;
    float: left;
    width: 80px;
    height: 80px;
    position: relative;
    padding: 2px;
    border: 1px solid #ddd;
    background: #fff;
    .img-cover {
        display: block;
        width: 100%;
        height: 100%;
        background: no-repeat center center / 100%;
    }
    &:hover {
        .icon-close {
            display: block;
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
    &:hover {
        border: 1px solid #a7c3de;
    }
}

// 选择器
.selector {
    width: 80px;
    height: 80px;
    float: left;
    border: 1px dashed #e2e2e2;
    text-align: center;
    color: #dad9d9;
    cursor: pointer;
    display: flex;
    justify-content: center;
    align-items: center;
    &:hover {
        border: 1px dashed #40a9ff;
        color: #40a9ff;
    }
    .icon-plus {
        font-size: 32px;
    }
}
</style>
