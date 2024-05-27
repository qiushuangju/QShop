<template>
    <div class="select-link">
        <template v-if="sLink==null || sLink==''">

            <a class="choice" @click="handleSelectLink()">选择链接</a>
        </template>
        <div class="flex flex-x-between" v-else>
            <span class="link-title">{{ sLink.title }}</span>
            <a class="choice" @click="handleSelectLink()">修改</a>
        </div>
        <LinkModal ref="LinkModal" @handleSubmit="handleSubmit" />
    </div>
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import { LinkModal } from '@/components/Modal'

// 图片选择器组件
export default {
    name: 'SelectLink',
    components: {
        LinkModal,
    },
    model: {
        prop: 'value',
        event: 'change',
    },
    props: {
        // 选中的链接
        value: PropTypes.object.def({}),
    },
    data() {
        return {
            // 已选择的链接
            sLink: null,
        }
    },
    watch: {
        value: {
            // 首次加载的时候执行函数

            immediate: true,
            handler(val) {
                console.log({ val })
                this.onUpdate(val)
            },
        },
    },
    created() {},
    methods: {
        // 更新已选择的数据 sLink
        onUpdate(sLink) {
            this.sLink = sLink
            this.onChange()
        },

        // 打开链接选择器
        handleSelectLink() {
            const { sLink } = this
            this.$refs.LinkModal.handle(sLink)
        },

        // 链接选择器确认回调
        handleSubmit(result) {
            this.onUpdate(result)
        },

        // 触发change事件
        onChange() {
            console.log(this)
            const { sLink } = this

            return this.$emit('change', sLink)
        },
    },
}
</script>

<style lang="less" scoped>
.select-link {
    flex: 1;
    min-width: 150px;
}
.choice,
.link-title {
    display: block;
    line-height: 32px;
    white-space: nowrap;
    font-size: 12px;
}
</style>
