<template>
  <div>
    <el-tag style="margin-right: 4px;" v-for="(item,index) in users" :key="index" size="mini">{{item}}</el-tag>
    <el-button style="height: 20px;padding: 0 10px;" @click="loadRoleUsers" type="primary" :loading="isLoading" plain title="查看更多" v-if="buttonVisible" size="mini" icon="el-icon-more"></el-button>
  </div>
</template>

<script>
import * as apiUsers from '@/api/user/users'

export default {
  name: 'role-users',
  props: ['roleId', 'selectUsers', 'userNames', 'selectUserIds'],
  data() {
    // todo:兼容layui的样式、图标
    return {
      users: [],
      page: 1,
      limit: 99999, // 每页条数
      text: '>>更多',
      buttonVisible: true,
      isLoading: false,
    }
  },
  mounted() {
    var _this = this
    _this.loadRoleUsers()
  },
  watch: {
    selectUsers(val) {
      this.users =
        (val.length > 0 &&
          val.map((item) => item.name || item.account)) ||
        []
      if (this.users.length >= this.count || this.users.length === 0) {
        this.buttonVisible = false
      }
    },
  },
  methods: {
    changeNames(names) {
      console.log('names', names)
    },
    loadRoleUsers(id) {
      var _this = this
      this.isLoading = true
      apiUsers.loadByRole({
        page: _this.page,
        limit: _this.limit,
        roleId: id || _this.roleId,
      })
        .then((response) => {
          _this.$emit('update:selectUsers', response.result || [])
          _this.isLoading = false
          if (response.result.length > 0) {
            _this.page++ // 页码加
            _this.count = response.count
          }
          if (_this.users.length >= _this.count) {
            _this.buttonVisible = false
          }
        })
    },
  },
}
</script>

<style scoped>
.el-transfer {
  margin-top: 10px;
}
</style>
