<template>
  <div class="flex-column">
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('SysLog', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('SysLog', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container flex-item">

      <div class="bg-white" style="height: 100%;">
        <el-table ref="mainTable" height="calc(100% - 52px)" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">

          <el-table-column min-width="150px" :label="'日志内容'">
            <template slot-scope="scope">
              <span>{{scope.row.content}}</span>
            </template>
          </el-table-column>
          <el-table-column :label="'日志分类'">
            <template slot-scope="scope">
              <span>{{LogTypeList.find(p=>p.value==scope.row.type)==null?'':
                LogTypeList.find(p=>p.value==scope.row.type).text}}</span>
            </template>
          </el-table-column>
          <el-table-column :label="'模块'">
            <template slot-scope="scope">
              <span>{{scope.row.href}}</span>
            </template>
          </el-table-column>

          <el-table-column :label="'操作人'">
            <template slot-scope="scope">
              <span>{{scope.row.createName}}</span>
            </template>
          </el-table-column>
          <el-table-column :label="'操作结果'">
            <template slot-scope="scope">
              <el-tag type="danger" v-if="scope.row.result ==1">失败</el-tag>
              <el-tag v-else>成功</el-tag>
            </template>
          </el-table-column>
          <el-table-column :label="'所属应用'">
            <template slot-scope="scope">
              <span>{{scope.row.application}}</span>
            </template>
          </el-table-column>
          <el-table-column :label="'日志时间'">
            <template slot-scope="scope">
              <span>{{scope.row.createTime}}</span>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
  </div>

</template>

<script>
import * as sysLogs from '@/api/syslogs'
import enums from '@/api/enumList'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import extend from "@/extensions/delRows.js"
export default {
  name: 'sysLog',
  components: {
    Sticky,
    Pagination
  },
  mixins: [extend],
  directives: {
    waves,
    elDragDialog
  },
  data() {
    return {
      LogTypeList: [],//日志类型
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
        id: '', // Id
        content: '', // 日志内容
        typeName: '', // 分类名称
        typeId: '', // 分类ID
        href: '', // 操作所属模块地址
        createTime: '', // 记录时间
        createId: '', // 操作人ID
        createName: '', // 操作人
        ip: '', // 操作机器的IP地址
        result: '', // 操作的结果：0：成功；1：失败；
        application: '', // 所属应用
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
        appId: [{
          required: true,
          message: '必须选择一个应用',
          trigger: 'change'
        }],
        name: [{
          required: true,
          message: '名称不能为空',
          trigger: 'blur'
        }]
      },
      downloadLoading: false
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
    enums.getLogTypeList(true).then((res) => {
      this.LogTypeList = res
    }),
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
        case 'btnAdd':
          this.handleCreate()
          break
        case 'btnEdit':
          if (this.multipleSelection.length !== 1) {
            this.$message({
              message: '只能选中一个进行编辑',
              type: 'error'
            })
            return
          }
          this.handleUpdate(this.multipleSelection[0])
          break
        case 'btnDel':
          if (this.multipleSelection.length < 1) {
            this.$message({
              message: '至少删除一个',
              type: 'error'
            })
            return
          }
          this.delrows(sysLogs, this.multipleSelection)
          break
        default:
          break
      }
    },
    getList() {
      this.listLoading = true
      sysLogs.getList(this.listQuery).then(response => {
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
    handleModifyStatus(row, disable) { // 模拟修改状态
      this.$message({
        message: '操作成功',
        type: 'success'
      })
      row.disable = disable
    },
    resetTemp() {
      this.temp = {
        id: '',
        content: '',
        typeName: '',
        typeId: '',
        href: '',
        createTime: '',
        createId: '',
        createName: '',
        ip: '',
        result: '',
        application: '',
        extendInfo: ''
      }
    }
  }
}

</script>
<style>
.dialog-mini .el-select {
  width: 100%;
}
</style>
