<template>
  <div class="flex-column">
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" prefix-icon="el-icon-search" size="small" style="width: 200px; margin-bottom: 0;" class="filter-item" :placeholder="'关键字'" v-model="listQuery.key">
        </el-input>

        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('employee', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('employee', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container flex-item">
      <div class="bg-white fh">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" height="calc(100% - 52px)" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column align="center" type="selection" width="55">
          </el-table-column>

          <el-table-column :label="'Id'" v-if="showDescription" min-width="120px">
            <template slot-scope="scope">
              <span>{{scope.row.id}}</span>
            </template>
          </el-table-column>

          <el-table-column min-width="80px" :label="'账号'">
            <template slot-scope="scope">
              <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.account}}</span>
            </template>
          </el-table-column>

          <el-table-column min-width="80px" :label="'姓名'">
            <template slot-scope="scope">
              <span>{{scope.row.name}}</span>
            </template>
          </el-table-column>
          <el-table-column prop="roleNames" label="角色" min-width="200" align="center"></el-table-column>

          <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('employee', 'btnEdit')" @click="handleUpdate(scope.row)">编辑</el-button>
              <el-button type="text" v-if="checkPermission('employee', 'btnEdit')&&scope.row.status==0" @click="handleModifyStatus(scope.row,1)">停用</el-button>
              <el-button type="text" v-if="checkPermission('employee', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>

      <el-dialog class="dialog-mini" width="500px" v-el-drag-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
        <el-form :rules="rules" ref="dataForm" :model="temp" label-position="right" label-width="100px">
          <el-form-item size="small" :label="'Id'" prop="id" v-show="dialogStatus=='update'">
            <el-input v-model="temp.id" :disabled="true" size="small" placeholder="系统自动处理"></el-input>
          </el-form-item>
          <el-form-item size="small" :label="'账号'" prop="account">
            <el-input v-model="temp.account"></el-input>
          </el-form-item>
          <el-form-item size="small" :label="'姓名'">
            <el-input v-model="temp.name"></el-input>
          </el-form-item>
          <el-form-item size="small" :label="'密码'">
            <el-input v-model="temp.password" :placeholder="dialogStatus=='update'?'如果为空则不修改密码':'如果为空则默认与账号相同'"></el-input>
          </el-form-item>
          <el-form-item size="small" :label="'退款密码'">
            <el-input v-model="temp.balancePwd" :placeholder="dialogStatus=='update'?'如果为空则不修改密码':'如果为空则默认与账号相同'"></el-input>
          </el-form-item>
          <el-form-item size="small" :label="'手机号码'">
            <el-input v-model="temp.phone" :placeholder="输入手机号"></el-input>
          </el-form-item>
          <!-- <el-form-item size="small" :label="'状态'">
                        <el-select class="filter-item" v-model="temp.status" placeholder="Please select">
                            <el-option v-for="item in  statusOptions" :key="item.key" :label="item.display_name" :value="item.key">
                            </el-option>
                        </el-select>
                    </el-form-item> -->

          <el-form-item size="small" :label="'描述'">
            <el-input type="textarea" :autosize="{ minRows: 2, maxRows: 4}" placeholder="Please input" v-model="temp.description">
            </el-input>
          </el-form-item>
        </el-form>
        <div slot="footer">
          <el-button size="mini" @click="dialogFormVisible = false">取消</el-button>
          <el-button size="mini" v-if="dialogStatus=='create'" type="primary" @click="createData">确认</el-button>
          <el-button size="mini" v-else type="primary" @click="updateData">确认</el-button>
        </div>
      </el-dialog>

      <el-dialog width="516px" class="dialog-mini body-small" v-el-drag-dialog :title="'分配角色'" :visible.sync="dialogRoleVisible">
        <el-form ref="rolesForm" size="small" v-if="dialogRoleVisible" label-position="left">
          <el-form-item>
            <select-roles :roles="selectRoles" :userNames="selectRoleNames" v-on:roles-change="rolesChange"></select-roles>
          </el-form-item>
        </el-form>
        <div slot="footer">
          <el-button size="mini" @click="dialogRoleVisible = false">取消</el-button>
          <el-button size="mini" type="primary" @click="acceRole">确认</el-button>
        </div>
      </el-dialog>

    </div>
  </div>

</template>

<script>
import { listToTreeSelect } from '@/utils'
import * as accsssObjs from '@/api/accessObjs'
import * as users from '@/api/user/users'
import * as apiRoles from '@/api/roles'
import '@riophae/vue-treeselect/dist/vue-treeselect.css'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import SelectRoles from '@/components/SelectRoles'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import extend from '@/extensions/delRows.js'

export default {
  components: {
    Sticky,
    SelectRoles,
    Pagination,
  },
  mixins: [extend],
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      defaultProps: {
        // tree配置项
        children: 'children',
        label: 'label',
      },
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        // 查询条件
        page: 1,
        limit: 20,
        key: undefined,
        minUserType: 10,
        maxUserType: 10,
      },
      apps: [],
      statusOptions: [
        {
          key: -10,
          display_name: '停用',
        },
        {
          key: 10,
          display_name: '正常',
        },
      ],
      showDescription: false,
      selectRoles: [], // 用户分配的角色
      selectRoleNames: '',
      temp: {
        id: undefined,
        description: '',
        account: '',
        name: '',
        password: '',
        status: 10,
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '添加',
      },
      dialogRoleVisible: false, // 分配角色对话框
      rules: {
        account: [
          {
            required: true,
            message: '账号不能为空',
            trigger: 'blur',
          },
        ],
      },
      downloadLoading: false,
    }
  },
  computed: {
  },
  filters: {
    statusFilter(status) {
      var res = 'color-success'
      switch (status) {
        case 1:
          res = 'color-danger'
          break
        default:
          break
      }
      return res
    },
  },
  created() {
    this.getList()
  },
  mounted() {
    var _this = this // 记录vuecomponent

  },
  methods: {
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    handleNodeClick(data) {
      this.getList()
    },
    getAllUsers() {
      this.getList()
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    onBtnClicked: function (btnCode) {

      switch (btnCode) {
        case 'btnAdd':
          this.handleCreate()
          break
        case 'btnEdit':
          if (this.multipleSelection.length !== 1) {
            this.$message({
              message: '只能选中一个进行编辑',
              type: 'error',
            })
            return
          }
          this.handleUpdate(this.multipleSelection[0])
          break
        case 'btnDel':
          if (this.multipleSelection.length < 1) {
            this.$message({
              message: '至少删除一个',
              type: 'error',
            })
            return
          }
          this.handleDelete(this.multipleSelection)
          break
        case 'btnAccessRole':
          if (this.multipleSelection.length !== 1) {
            this.$message({
              message: '只能选中一个进行编辑',
              type: 'error',
            })
            return
          }
          this.handleAccessRole(this.multipleSelection[0])
          break
        default:
          break
      }
    },
    getList() {
      this.listLoading = true
      users.getList(this.listQuery).then((response) => {
        this.list = response.result
        this.total = response.count
        this.listLoading = false
      })
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    handleSizeChange(val) {
      this.listQuery.limit = val
      this.getList()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val.page
      this.listQuery.limit = val.limit
      this.getList()
    },
    handleModifyStatus(row, status) {
      // 模拟修改状态
      this.$message({
        message: '操作成功',
        type: 'success',
      })
      row.status = status
    },
    resetTemp() {
      this.temp = {
        id: undefined,
        description: '',
        account: '',
        name: '',
        status: 10,
      }
    },
    handleCreate() {
      // 弹出添加框
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    createData() {
      // 保存提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          users.addOrUpdateEmp(this.temp).then((response) => {
            this.temp.id = response.result
            this.getList()
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '创建成功',
              type: 'success',
              duration: 2000,
            })
          })
        }
      })
    },
    handleUpdate(row) {
      // 弹出编辑框
      this.temp = Object.assign({}, row) // copy obj
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    updateData() {
      // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          users.addOrUpdateEmp(tempData).then(() => {
            this.getList()
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '更新成功',
              type: 'success',
              duration: 2000,
            })
          })
        }
      })
    },
    handleDelete(rows) {
      // 多行删除
      this.delrows(users, rows)
    },
    handleAccessRole(row) {
      // 分配角色
      const _this = this
      this.temp = Object.assign({}, row) // copy obj
      apiRoles.loadForUser(this.temp.id).then((response) => {
        _this.dialogRoleStatus = 'update'
        _this.dialogRoleVisible = true
        _this.selectRoles = response.result
        _this.getRoleList()
        _this.$nextTick(() => {
          _this.$refs['rolesForm'].clearValidate()
        })
      })
    },

    // 获取角色
    getRoleList() {
      apiRoles.loadAll({}).then((response) => {
        this.selectRoleNames = [...response.result]
          .filter((x) => this.selectRoles.indexOf(x.id) > -1)
          .map((item) => item.name || item.account)
          .join(',')
      })
    },
    rolesChange(type, val) {
      if (type === 'Texts') {
        this.selectRoleNames = val
        return
      }
      this.selectRoles = val
    },
    acceRole() {
      accsssObjs
        .assign({
          type: 'UserRole',
          firstId: this.temp.id,
          secIds: this.selectRoles,
        })
        .then(() => {
          this.dialogRoleVisible = false
          this.$notify({
            title: '成功',
            message: '更新成功',
            type: 'success',
            duration: 2000,
          })
        })
    },
  },
}
</script>

<style>
.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both;
}

.el-card__header {
  padding: 12px 20px;
}

.body-small.dialog-mini .el-dialog__body .el-form {
  padding-right: 0px;
  padding-top: 0px;
}
</style>
