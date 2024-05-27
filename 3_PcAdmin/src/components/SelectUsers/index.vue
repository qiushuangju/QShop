<template>
  <div>
    <el-input @click.native="selectDialog = true" :readonly="true" v-model="names" :placeholder="placeholder"></el-input>
    <el-dialog :destroy-on-close="true" class="dialog-mini custom-dialog user-dialog" width="850px" title="选择用户"
      :visible.sync="selectDialog">
      <!-- 使用v-if的原因：dialog在关闭的时候会执行组件里面的mounted，所以需要关闭dialog时销毁子组件 -->
      <selectUsersCom ref="selectUser" :hiddenFooter="true" v-if="selectDialog" :show.sync="selectDialog" :ignore-auth="ignoreAuth" :users.sync="selectUsers" :loginKey="'loginUser'" :userNames.sync="names"></selectUsersCom>
      <div slot="footer" style="text-align:right;">
        <el-button size="small" type="cancel" @click="selectDialog = false">取消</el-button>
        <el-button size="small" type="primary" @click="handleSaveUsers">确定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
  import selectUsersCom from '@/components/SelectUsersCom'

  export default {
    name: 'select-users',
    components: {
      selectUsersCom
    },
    props: ['users', 'userNames', 'placeholder','ignoreAuth'],
    data() { // todo:兼容layui的样式、图标
      return {
        defaultSelectUsers: this.users,
        selectDialog: false
      }
    },
    computed: {
      selectUsers:{
        get(){
          return this.users
        },
        set(val){
          this.$emit('users-change', 'users', val)
        }
      },
      names:{
        get(){
          return this.userNames
        },
        set(val){
          this.$emit('users-change', 'Texts', val)
        }
      }
    },
    watch: {
      userNames() {
        this.names = this.userNames
      }
    },
    methods: {
      handleSaveUsers() {
        this.$refs.selectUser.handleSaveUsers()
      }
    }
  }

</script>

<style lang="scss">
 .el-transfer{
   margin-top:10px;
 }
 .user-dialog{
  &.custom-dialog{
    .el-dialog{
      height: 70%;
      .el-icon-close{
        padding-top: 10px;
      }
      .el-dialog__body{
        height: calc(100% - 35px - 40px);
      }
      .el-dialog__headerbtn {
        top: 0;
      }
    }
  }
 }
 
</style>
