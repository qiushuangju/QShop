<template>
    <a-dropdown v-if="currentUser" placement="bottomRight">
        <span class="ant-pro-account-avatar oneline-hide">
            <!-- <a-avatar size="small" src="https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png" class="antd-pro-global-header-index-avatar" /> -->
            <a-icon type="user" :style="{ fontSize: '16px', marginRight: '5px' }"></a-icon>
            <span>{{ currentUser.real_name || currentUser.user_name }}</span>
        </span>
        <template v-slot:overlay>
            <a-menu class="ant-pro-drop-down menu" :selected-keys="[]">
                <a-menu-item v-if="menu" key="settings" @click="handleToSettings">
                    <a-icon type="setting" />账户设置
                </a-menu-item>
                <!-- <a-menu-divider v-if="menu" /> -->
                <a-menu-item key="logout" @click="handleLogout">
                    <a-icon type="logout" />退出登录
                </a-menu-item>
            </a-menu>
        </template>
    </a-dropdown>
    <span v-else>
        <a-spin size="small" :style="{ marginLeft: 8, marginRight: 8 }" />
    </span>
</template>

<script>
import { Modal } from "ant-design-vue";

export default {
    name: "AvatarDropdown",
    props: {
        currentUser: {
            type: Object,
            default: () => null,
        },
        menu: {
            type: Boolean,
            default: true,
        },
    },
    methods: {
        handleToSettings() {
            this.$router.push({ path: "/manage/renew" });
        },
        handleLogout(e) {
            Modal.confirm({
                title: "友情提示",
                content: "真的要注销登录吗 ?",
                onOk: () => {
                    return this.$store.dispatch("Logout").then(() => {
                        setTimeout(() => {
                            window.location.reload();
                        }, 200);
                    });
                },
                onCancel() {},
            });
        },
    },
};
</script>

<style lang="less" scoped>
.ant-pro-drop-down {
    :v-deep(.action) {
        padding: 0px 24px;
    }
    :v-deep(.ant-dropdown-menu-item) {
        min-width: 130px;
        padding-left: 20px;
        font-size: 13px;
    }
}
</style>
