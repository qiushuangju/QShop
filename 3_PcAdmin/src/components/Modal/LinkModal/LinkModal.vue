<template>
    <a-modal class="noborder" :title="title" :width="820" :visible="visible" :isLoading="isLoading" :maskClosable="false" :destroyOnClose="true" @ok="handleSubmit"
        @cancel="handleCancel">
        <div class="links-body">
            <a-collapse :defaultActiveKey="keys" :bordered="false" expandIconPosition="left">
                <a-collapse-panel v-for="group in linkList" :key="group.key" :header="group.title">
                    <div class="link-list">
                        <div v-for="(item, index) in group.data" :key="index" class="link-item" :class="{ active: activeKey == item.id}" @click="handleClickItem(item)">
                            <span class="link-title">{{ item.title }}</span>
                        </div>
                    </div>
                </a-collapse-panel>
            </a-collapse>
            <a-drawer :title="drawer.title" placement="right" :width="350" :closable="false" :visible="drawer.visible" :getContainer="false" :wrapStyle="{ position: 'absolute' }"
                @close="onCloseDrawer">
                <a-form v-if="drawer.visible" :form="form">
                    <a-form-item v-for="(item, index) in curItem.form" :key="index" :label="item.lable" :labelCol="{ span: 6 }" :wrapperCol="{ span: 16 }">
                        <a-input v-decorator="[`values.${index}`, {
                initialValue: item.value,
                rules: [{ required: item.required, message: `${item.lable}必须填写` }]
              }]" />
                        <div class="form-item-help">
                            <small v-html="item.tips"></small>
                        </div>
                    </a-form-item>
                </a-form>
            </a-drawer>
        </div>
    </a-modal>
</template>

<script>
import _ from "lodash";
import { linkList } from "@/common/model/Links";
import { buildUrL } from "@/utils/util";

export default {
    name: "LinkModal",
    model: {
        prop: "value",
        event: "change",
    },
    data() {
        return {
            // 对话框标题
            title: "选择链接",
            // modal(对话框)是否可见
            visible: false,
            // loading状态
            isLoading: false,
            // 当前表单元素
            form: this.$form.createForm(this),
            // 所有链接数据
            linkList,
            // 当前选中的键值
            activeKey: "",
            // 当前选中的链接
            curItem: null,
            // 抽屉组件属性
            drawer: {
                visible: false,
                title: "",
            },
        };
    },
    computed: {
        keys() {
            const { linkList } = this;
            return linkList.map((item) => item.key);
        },
    },
    created() {},
    methods: {
        // 显示对话框
        handle(record = null) {
            this.visible = true;
            if (record != null) {
                this.handleClickItem(record);
            }
        },

        // 选中事件
        handleClickItem(item) {
            // 取消选中值
            if (this.activeKey === item.id) {
                this.activeKey = "";
                this.curItem = null;
                return;
            }
            // 记录选中值
            this.activeKey = item.id;
            this.curItem = _.cloneDeep(item);
            // 显示表单
            this.onShowFrom();
        },

        // 显示表单
        onShowFrom() {
            const { curItem } = this;
            if (curItem.form && curItem.form.length) {
                // 显示抽屉
                this.onShowDrawer();
            }
        },

        // 显示抽屉
        onShowDrawer() {
            const { drawer, curItem } = this;
            drawer.visible = true;
            drawer.title = curItem.title;
        },

        // 关闭抽屉
        onCloseDrawer() {
            this.activeKey = "";
            this.curItem = null;
            this.drawer.visible = false;
        },

        // 关闭对话框事件
        handleCancel() {
            this.visible = false;
            this.onCloseDrawer();
        },

        // 确认按钮
        handleSubmit(e) {
            e.preventDefault();
            const {
                curItem,
                form: { validateFields },
            } = this;
            validateFields((errors, formData) => {
                if (!errors) {
                    // 创建返回结果
                    const result = curItem
                        ? this.buildResult(curItem, formData)
                        : null;
                    // 通知父端组件提交完成了
                    this.$emit("handleSubmit", result);
                    // 关闭对话框
                    this.handleCancel();
                }
            });
        },

        // 构建选中的link对象
        buildResult(link, formData) {
            for (const index in link.form) {
                const item = link.form[index];
                if (!item) {
                    continue;
                }
                // 记录输入的数据
                if (!formData.values[index]) {
                    formData.values[index] = "";
                }
                // 将value赋值到key指向的param对象元素
                item.value = formData.values[index];
                _.set(link.param, item.key, item.value);
            }
            // 将link对象拼接为url
            if (link.type === "PAGE") {
                link.param.url = buildUrL(link.param.path, link.param.query);
            }
            // 克隆一个新对象返回
            return _.cloneDeep(link);
        },
    },
};
</script>

<style lang="less" scoped>
@import "~ant-design-vue/es/style/themes/default.less";
.ant-modal-root {
    background: #ccc;
    :v-deep(.ant-modal-body) {
        padding: 0 24px 15px 24px;
    }
    :v-deep(.ant-modal-footer) {
        padding-top: 0;
    }
}

:v-deep(.ant-collapse-header) {
    padding: 14px 16px;
    font-size: 13px;
    font-weight: 700;
    color: #595961;
}

:v-deep(.ant-collapse-content-box) {
    padding-top: 0 !important;
}

:v-deep(.ant-collapse) > :v-deep(.ant-collapse-item) {
    border-bottom: none;
    margin-bottom: 15px;
    &:last-child {
        margin-bottom: 0;
    }
}

.links-body {
    position: relative;
    min-height: 360px;
    overflow: hidden;
}

.link-list {
    .link-item {
        float: left;
        padding: 5px 15px;
        border: 1px solid #ccc;
        border-radius: 5px;
        margin-right: 20px;
        margin-bottom: 15px;
        font-size: 12.5px;
        cursor: pointer;
        &:hover {
            border: 1px solid @primary-color;
            color: @primary-color;
        }
        &.active {
            background: @primary-color;
            border-color: @primary-color;
            color: #fff;
        }
        &:last-child {
            margin-right: 0;
        }
    }
}
</style>
