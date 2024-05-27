<template>
  <div class="flex-column">
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('SysMessage', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('SysMessage', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container flex-item">

      <div class="bg-white" style="height: 100%;">
        <el-table ref="mainTable" height="calc(100% - 52px)" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">

          <el-table-column min-width="50px" :label="'消息分类'">
            <template slot-scope="scope">
              <span>{{scope.row.typeName}}</span>
            </template>
          </el-table-column>

          <el-table-column min-width="50px" :label="'发送人'">
            <template slot-scope="scope">
              <span>{{scope.row.fromName}}</span>
            </template>
          </el-table-column>

          <el-table-column min-width="80px" :label="'状态'">
            <template slot-scope="scope">
              <el-tag type="danger" v-if="scope.row.toStatus ==0">未读</el-tag>
              <el-tag v-if="scope.row.toStatus ==1">已读</el-tag>
            </template>
          </el-table-column>
          <el-table-column min-width="180px" :label="'消息内容'">
            <template slot-scope="scope">
              <span>{{scope.row.content}}</span>
            </template>
          </el-table-column>
          <el-table-column min-width="80px" :label="'消息时间'">
            <template slot-scope="scope">
              <span>{{scope.row.createTime}}</span>
            </template>
          </el-table-column>

          <el-table-column align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('SysMessage', 'btnEdit')&&scope.row.toStatus ==0" @click="handleRead(scope.row)">标记已读</el-button>
              <el-button type="text" v-if="checkPermission('SysMessage', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
            </template>
          </el-table-column>

        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>

    </div>
  </div>

</template>

<script>
import * as sysMessages from '@/api/sysmessages'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import extend from "@/extensions/delRows.js"
export default {
  components: {
    Sticky,
    Pagination
  },
  directives: {
    waves,
    elDragDialog
  },
  mixins: [extend],
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
        status: 999, //999为全部，0为未读，1为已读
        key: undefined
      }
    }
  },
  filters: {
    statusFilter(disable) {
      const statusMap = {
        false: 'color-success',
        true: 'color-danger'
      }
      return statusMap[disable]
    }
  },
  created() {
    this.getList()
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
        case 'btnEdit':
          if (this.multipleSelection.length !== 1) {
            this.$message({
              message: '只能选中一个进行编辑',
              type: 'error'
            })
            return
          }
          this.handleRead(this.multipleSelection[0])
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
    getList() {
      this.listLoading = true
      sysMessages.getList(this.listQuery).then(response => {
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
    handleRead(row) { //标记已读
      sysMessages.read({
        id: row.id
      }).then(() => {
        for (const v of this.list) {
          if (v.id === row.id) {
            v.toStatus = 1 //标记界面已读
            break
          }
        }
      })
    },

    handleDelete(rows) { // 逻辑删除
      this.delrows(sysMessages, rows)
    }
  }
}
</script>
<style>
.dialog-mini .el-select {
  width: 100%;
}
</style>