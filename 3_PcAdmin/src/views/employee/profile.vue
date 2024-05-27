<template>
  <div class="flex-column">

    <div class="app-container flex-item">
      <el-row style="height: 100%;">
        <el-col :span="15" style="height: 100%;">
          <div class="bg-white" style="height: 50%;">
            <el-card shadow="never" class="body-small" style="height: 100%;overflow:auto;">
              <div slot="header" class="clearfix">
                <el-button type="text" style="padding: 0 11px">基本资料</el-button>
              </div>
              <el-form ref="dataForm" :model="temp" label-position="right" label-width="100px">
                <el-form-item size="small" :label="'账号'" prop="account">
                  <span>{{temp.account}}</span>
                </el-form-item>

                <el-form-item size="small" :label="'姓名'" prop="name">
                  <el-input v-model="temp.name"></el-input>
                </el-form-item>
                <el-form-item size="small" :label="'性别'">
                  <el-radio v-model="temp.sex" :label="0">男</el-radio>
                  <el-radio v-model="temp.sex" :label="1">女</el-radio>
                </el-form-item>
                <el-form-item size="small" :label="' '">
                  <el-button size="mini" type="primary" @click="changeProfile">确认修改</el-button>
                </el-form-item>
              </el-form>


            </el-card>
          </div>
          <div class="bg-white" style="height: 50%;">
            <el-card shadow="never" class="body-small" style="height: 100%;overflow:auto;">
              <div slot="header" class="clearfix">
                <el-button type="text" style="padding: 0 11px">修改密码</el-button>
              </div>
              <el-form ref="dataForm" :model="temp" label-position="right" label-width="100px">
                <el-form-item size="small" :label="'新密码'" prop="name">
                  <el-input v-model="newpwd" show-password></el-input>
                </el-form-item>
                <el-form-item size="small" :label="' '">
                  <el-button size="mini" type="primary" @click="changePassword">确认修改</el-button>
                </el-form-item>
              </el-form>


            </el-card>
          </div>
        </el-col>
        <el-col :span="4" style="height: 100%;">
          <div class="bg-white" style="height: 100%;">

            <el-card shadow="never" class="body-small" style="height: 100%;overflow:auto;">
              <div slot="header" class="clearfix">
                <el-button type="text" style="padding: 0 11px">可访问的模块</el-button>
              </div>

              <el-tree :data="modulesTree" :expand-on-click-node="false" default-expand-all :props="modulesTree">
              </el-tree>
            </el-card>
          </div>
        </el-col>
   

      </el-row>

    </div>
  </div>

</template>

<script>
  import {
    listToTreeSelect
  } from '@/utils'
  import * as login from '@/api/login'
  import * as users from '@/api/user/users'
  import '@riophae/vue-treeselect/dist/vue-treeselect.css'
  import waves from '@/directive/waves' // 水波纹指令
  import {
    mapGetters
  } from 'vuex'
  import store from '@/store'

  export default {
    name: 'profile',
    components: {},
    directives: {
      waves
    },
    data() {
      return {       
        listLoading: true,
        modules: [],
        modulesTree: [], // 用户可访问的所有模块组成的树
        temp: {
          account: '',
          name: '',
          sex: 0
        },
        newpwd: ''
      }
    },
    computed: {
      ...mapGetters(['name'])
    },
    filters: {

    },
    mounted() {
      this.getUserProfile()
      this.getModulesTree()
    },
    methods: {
      changeProfile() {
        const _this = this
        users.changeProfile(this.temp).then(response => {
          _this.$message({
            message: response.message,
            type: 'success'
          })
        })
      },

      changePassword() {
        const _this = this
        users.changePassword({
          account: _this.temp.account,
          password: _this.newpwd
        }).then(response => {
          _this.$message({
            message: response.message,
            type: 'success'
          })
        })
      },

    

      getUserProfile() {
        var _this = this // 记录vuecomponent
        login.getUserProfile().then(response => {
          _this.temp = response.result
        })
      },

      getModulesTree() {
        var _this = this // 记录vuecomponent
        login.getModules().then(response => {
          _this.modules = response.result.map(function(item) {
            return {
              id: item.id,
              label: item.name,
              parentId: item.parentId
            }
          })
          var tmp = JSON.parse(JSON.stringify(_this.modules))
          _this.modulesTree = listToTreeSelect(tmp)
        })
      },

    }
  }

</script>

<style scoped>
  .clearfix:before,
  .clearfix:after {
    display: table;
    content: "";
  }

  .clearfix:after {
    clear: both
  }

  .el-select.filter-item.el-select--small {
    width: 100%;
  }

</style>
