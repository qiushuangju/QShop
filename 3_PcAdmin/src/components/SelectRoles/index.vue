<template>
  <div>
    <el-input @click.native="selectDialog = true" readonly v-model="names" :placeholder="placeholder"></el-input>
    <el-dialog :destroy-on-close="true" class="dialog-mini custom-dialog user-dialog" width="850px" title="选择角色"
      :visible.sync="selectDialog">
      <selectUsersCom v-if="selectDialog" :ignore-auth="ignoreAuth" :show.sync="selectDialog" :loginKey="'loginRole'" :users.sync="selectRoles" :userNames.sync="names"></selectUsersCom>
    </el-dialog>
  </div>
</template>

<script>
  import selectUsersCom from '@/components/SelectUsersCom'

  export default {
    name: 'select-roles',
    components: {
      selectUsersCom
    },
    props: ['roles', 'userNames','placeholder','ignoreAuth'],
    data() { 
      return {
        selectDialog: false,
        flag: false
      }
    },
    computed: {
      selectRoles:{
        get(){
          return this.roles
        },
        set(val){
          this.$emit('roles-change', 'roles', val)
        }
      },
      names:{
        get(){
          return this.userNames
        },
        set(val){
          this.$emit('roles-change', 'Texts', val)
        }
      }
    },
    watch: {
      userNames() {
        this.names = this.userNames
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
