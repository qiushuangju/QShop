<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>

        <el-button class="filter-item" size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('StoreSettingPay', 'btnAdd')" icon="el-icon-plus" @click="openEdit">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('StoreSettingPay', 'btnDel')" icon="el-icon-delete" @click="handleDeleteArr">删除</el-button>
      </div>
    </sticky>
    <div class="app-container ">
      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">
          <!-- <el-table-column type="selection" align="center" width="55"></el-table-column> -->
          <el-table-column prop="code" label="编号" show-overflow-tooltip></el-table-column>
          <el-table-column prop="name" label="名字" show-overflow-tooltip></el-table-column>
          <el-table-column align="center" class="c-p" prop="status" label="状态" show-overflow-tooltip width="130">
            <template slot-scope="scope">
              <el-switch v-model="scope.row.status" active-color="#13ce66" inactive-color="#ff4949" :active-value=10 :inactive-value=-10 @change="setStatus(scope.row)">
              </el-switch>
            </template>
          </el-table-column>
          <el-table-column align="center" class="c-p" prop="isDefault" label="是否默认" show-overflow-tooltip width="130">
            <template slot-scope="scope">
              <el-switch v-model="scope.row.isDefault" active-color="#13ce66" inactive-color="#ff4949" :active-value=true :inactive-value=false @change="setDefault(scope.row)">
              </el-switch>
            </template>
          </el-table-column>
          <el-table-column prop="sortNo" label="排序" show-overflow-tooltip></el-table-column>
          <el-table-column prop="createTime" label="创建时间" show-overflow-tooltip></el-table-column>
          <el-table-column align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('StoreSettingPay','btnSetting')" @click="openEdit(scope.row)">设置</el-button>
              <el-button type="text" v-if="checkPermission('StoreSettingPay', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
    <EditFrom ref="EditFrom" @handleSubmit="loadList" />
  </div>
</template>

<script>
import * as ApiStoreSettingPay from '@/api/storesettingpay'
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import EditFrom from './modules/EditFrom'
export default {
  components: { Sticky, EditFrom, Pagination },
  directives: {},
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
      }
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
    loadList() {
      this.listLoading = true
      ApiStoreSettingPay.load(this.listQuery).then(res => {
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
    setStatus(row) {
      ApiStoreSettingPay.setStatus({ id: row.id, status: row.status }).then(res => {
        this.loadList()
      })
    },
    setDefault(row) {
      ApiStoreSettingPay.setDefault({ id: row.id, isDefault: row.isDefault }).then(res => {
        this.loadList()
      })
    },
    // 打开编辑框
    openEdit(row) {
      this.$refs.EditFrom.show(row)
    },
    handleDeleteArr() {//列表选中删除
      if (this.multipleSelection.length < 1) {
        this.$message({
          message: '至少删除一个',
          type: 'error'
        })
        return
      }
      this.handleDelete(this.multipleSelection)
    },
    handleDelete(rows) { // 多行删除
      ApiStoreSettingPay.del(rows.map(u => u.id)).then(() => {
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
    },

  }
}
</script>
<style scoped>
.dialog-mini .el-select {
  width: 100%;
}
</style>
