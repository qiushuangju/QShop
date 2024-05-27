<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>

        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('Store', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
      </div>
    </sticky>
    <div class="app-container ">

      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column prop="id" label="StoreId" width="270" show-overflow-tooltip></el-table-column>
          <el-table-column prop="storeName" label="商城名称" show-overflow-tooltip></el-table-column>
          <el-table-column align="center" label="状态" width="190">
            <template slot-scope="scope">
              <el-radio-group v-model="scope.row.strStatus" size="mini" @change="clickChangeStatus">
                <el-radio-button :label=item.text v-for="item in listStoreStatus" :key="item.key" size="mini"></el-radio-button>
              </el-radio-group>
            </template>
          </el-table-column>
          <el-table-column align="center" label="日期时间" width="220">
            <template slot-scope="scope">
              <el-row>创建:{{scope.row.createTime}}</el-row>
              <el-row>截止:{{scope.row.deadLineDate}}</el-row>
            </template>
          </el-table-column>
          <el-table-column prop="companyName" label="公司名称" show-overflow-tooltip></el-table-column>
          <el-table-column prop="linkMan" label="联系人" width="100" show-overflow-tooltip></el-table-column>
          <el-table-column prop="phone" label="联系手机" width="120" show-overflow-tooltip></el-table-column>

          <el-table-column align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('Store','btnEdit')" @click="handleEdit(scope.row)">编辑</el-button>
              <el-button type="text" v-if="checkPermission('Store', 'btnEnter')" @click="openEdit(scope.row)">进入商城</el-button>
            </template>
          </el-table-column>

        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>

      <el-dialog v-el-drag-dialog class="dialog-mini" width="500px" :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
        <el-form :rules="rules" ref="dataForm" :model="temp" label-position="right" label-width="100px">
          <el-form-item size="small" label="商城名称" prop="storeName">
            <el-input v-model="temp.storeName"></el-input>
          </el-form-item>
          <el-form-item size="small" label="商家用户名" prop="storeUserName">
            <el-input v-model="temp.storeUserName"></el-input>
          </el-form-item>
          <el-form-item size="small" label="用户密码" prop="storePwd">
            <el-input v-model="temp.storePwd"></el-input>
          </el-form-item>
          <el-form-item size="small" label="确认密码" prop="storeConfirmPwd">
            <el-input v-model="temp.storeConfirmPwd"></el-input>
          </el-form-item>
          <el-form-item size="small" label="公司名称" prop="companyName">
            <el-input v-model="temp.companyName"></el-input>
          </el-form-item>
          <el-form-item size="small" label="联系人" prop="linkMan">
            <el-input v-model="temp.linkMan"></el-input>
          </el-form-item>
          <el-form-item size="small" label="联系手机" prop="phone">
            <el-input v-model="temp.phone"></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer">
          <el-button size="mini" @click="dialogFormVisible = false">取消</el-button>
          <el-button size="mini" type="primary" @click="saveData">确认</el-button>
        </div>
      </el-dialog>
    </div>
    <EditForm ref="EditForm" @handleSubmit="loadList" />
  </div>

</template>

<script>
import * as store from '@/api/store/store'
import enums from '@/api/enumList'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import { EditForm } from './modules'
export default {
  name: 'store',
  components: { Sticky, EditForm, Pagination },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      listStoreStatus: [],
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        // 查询条件
        page: 1,
        limit: 20,
        key: undefined,
        appId: undefined,
      },
      temp: {
        id: '', // Id
        storeName: '', // 商城名称
        logoImageId: '', // logo 图片
        status: '', // 状态(-10:闭店 10:试用 20:正常)
        describe: '', // 商城简介
        extendInfo: '', // 其他信息,防止最后加逗号，可以删除
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '添加',
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        storeName: [
          {
            required: true,
            message: '商城名称不能为空',
            trigger: 'change',
          },
        ],
        storeUserName: [
          {
            required: true,
            message: '用户名不能为空',
            trigger: 'blur',
          },
        ],
        storePwd: [
          {
            required: true,
            message: '密码不能为空',
            trigger: 'blur',
          },
          {
            validator: this.$validater.validatePwd,
            message: '8-16位,且包含字母数字',
            trigger: 'blur',
          },
        ],
        storeConfirmPwd: [
          {
            required: true,
            message: '确认密码不能为空',
            trigger: 'blur',
          },
        ],
      },
      downloadLoading: false,
    }
  },
  created() {
    new Promise((resolve, reject) => {
      Promise.all([
        enums.getListStoreStatus().then((res) => {
          this.listStoreStatus = res
        })
      ]).then(() => {
        this.loadList()
      })
    })


  },
  methods: {
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    onBtnClicked: function (btnCode) {

      switch (btnCode) {
        case 'btnAdd':
          this.openEdit()
          break
        case 'btnEdit':
          if (this.multipleSelection.length !== 1) {
            this.$message({
              message: '只能选中一个进行编辑',
              type: 'error',
            })
            return
          }
          this.openEdit(this.multipleSelection[0])
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
        default:
          break
      }
    },
    loadList() {
      this.listLoading = true
      store.load(this.listQuery).then((res) => {
        this.list = res.result
        this.total = res.count
        this.listLoading = false
      })
    },
    handleFilter() {
      this.listQuery.page = 1
      this.loadList()
    },
    handleSizeChange(val) {
      this.listQuery.limit = val
      this.loadList()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val.page
      this.listQuery.limit = val.limit
      this.loadList()
    },
    handleModifyStatus(row, disable) {
      // 模拟修改状态
      this.$message({
        message: '操作成功',
        type: 'success',
      })
      row.disable = disable
    },
    resetTemp() {
      this.temp = {
        id: '',
        storeName: '',
        logoImageId: '',
        status: '',
        describe: '',
        createTime: '',
        extendInfo: '',
      }
    },
    handleEdit(row) {
      this.$refs.EditForm.show(row)
    },
    // 修改状态
    clickChangeStatus() { },
    openEdit(row) {
      // 弹出编辑框
      if (row == null) {
        this.resetTemp()
        this.dialogStatus = 'create'
      } else {
        this.temp = Object.assign({}, row) // copy obj
        this.dialogStatus = 'update'
      }
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    saveData() {
      // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          if (this.temp.storePwd != this.temp.storeConfirmPwd) {
            this.$notify({
              title: '错误',
              message: '2次密码不相同',
              type: 'error',
              duration: 2000,
            })
            return false
          }
          const tempData = Object.assign({}, this.temp)
          store.addStore(tempData).then(() => {
            this.loadList()
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '保存成功',
              type: 'success',
              duration: 2000,
            })
          })
        }
      })
    },
    handleDelete(rows) {
      // 多行删除
      store.del(rows.map((u) => u.id)).then(() => {
        this.$notify({
          title: '成功',
          message: '删除成功',
          type: 'success',
          duration: 2000,
        })
        rows.forEach((row) => {
          const index = this.list.indexOf(row)
          this.list.splice(index, 1)
        })
      })
    },
  },
}
</script>
<style>
.dialog-mini .el-select {
  width: 100%;
}
</style>
