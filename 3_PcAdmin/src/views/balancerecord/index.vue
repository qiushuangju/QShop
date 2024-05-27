<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>

        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('balanceRecord', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('balanceRecord', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container ">

      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">

          <el-table-column type="selection" align="center" width="55"></el-table-column>

          <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('balanceRecord', 'btnEdit')" @click="openEdit(scope.row)">编辑</el-button>
              <el-button type="text" v-if="checkPermission('balanceRecord', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>

      <el-dialog v-el-drag-dialog class="dialog-mini" width="500px" :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
        <el-form :rules="rules" ref="dataForm" :model="temp" label-position="right" label-width="100px">

        </el-form>
        <div slot="footer">
          <el-button size="mini" @click="dialogFormVisible = false">取消</el-button>
          <el-button size="mini" type="primary" @click="saveData">确认</el-button>
        </div>
      </el-dialog>
    </div>
  </div>

</template>

<script>
import * as balanceRecord from '@/api/balancerecord'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
export default {
  name: 'balanceRecord',
  components: { Sticky, Pagination },
  directives: {
    waves,
    elDragDialog
  },
  data() {
    return {
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: { // 查询条件
        page: 1,
        limit: 20,
        key: undefined,
        appId: undefined
      },
      temp: {
        extendInfo: '' // 其他信息,防止最后加逗号，可以删除
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '添加'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        appId: [{ required: true, message: '必须选择一个应用', trigger: 'change' }],
        name: [{ required: true, message: '名称不能为空', trigger: 'blur' }]
      },
      downloadLoading: false
    }
  },
  created() {
    this.loadList()
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
              type: 'error'
            })
            return
          }
          this.openEdit(this.multipleSelection[0])
          break
        case 'btnDel':
          if (this.multipleSelection.length < 1) {
            this.$message({
              message: '至少删除一个',
              type: 'error'
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
      balanceRecord.load(this.listQuery).then(res => {
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
    handleModifyStatus(row, disable) { // 模拟修改状态
      this.$message({
        message: '操作成功',
        type: 'success'
      })
      row.disable = disable
    },
    resetTemp() {
      this.temp = {
        extendInfo: ''
      }
    },
    openEdit(row) { // 弹出编辑框
      if (row == null) {
        this.resetTemp();
        this.dialogStatus = 'create'
      } else {
        this.temp = Object.assign({}, row) // copy obj
        this.dialogStatus = 'update'
      } this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    saveData() { // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          balanceRecord.addOrUpdate(tempData).then(() => {
            this.loadList();
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '保存成功',
              type: 'success',
              duration: 2000
            })
          })
        }
      })
    },
    handleDelete(rows) { // 多行删除
      balanceRecord.del(rows.map(u => u.id)).then(() => {
        this.$notify({
          title: '成功',
          message: '删除成功',
          type: 'success',
          duration: 2000
        })
        rows.forEach(row => {
          const index = this.list.indexOf(row)
          this.list.splice(index, 1)
        })
      })
    }
  }
}
</script>
<style>
.dialog-mini .el-select {
  width: 100%;
}
</style>
