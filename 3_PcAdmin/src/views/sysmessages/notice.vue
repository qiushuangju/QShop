<template>
  <div class="flex-column">
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'标题'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('SysMessage', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container flex-item">

      <div class="bg-white" style="height: 100%;">
        <el-table ref="mainTable" height="calc(100% - 52px)" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column prop="title" min-width="50px" :label="'标题'">
          </el-table-column>
          <el-table-column align="center" min-width="50px" :label="'类型'">
            <template slot-scope="item">
              {{MessageTypeList.find(p=>p.value==item.row.type)==null?
                            '':MessageTypeList.find(p=>p.value==item.row.type).text}}
            </template>
          </el-table-column>
          <el-table-column align="center" prop="createTime" min-width="80px" :label="'创建时间'">
          </el-table-column>

          <el-table-column align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('SysMessage', 'btnEdit')" @click="openEdit(scope.row)">编辑</el-button>
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
import Layout from '@/views/layout/Layout'
import * as sysMessages from '@/api/sysmessages'
import enums from '@/api/enumList'
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
      MessageTypeList: [],//消息类型
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: { // 查询条件
        page: 1,
        limit: 20,
        type: 10, //公告
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
  activated() {
    this.getList()
  },
  created() {
    new Promise((resolve, reject) => {
      Promise.all([
        enums.getMessageTypeList().then((res => {
          console.log(res)
          this.MessageTypeList = res
        })),
      ]).then(() => {
        this.getList()
        this.addRoutes()
      })
    })
  },
  methods: {
    addRoutes() {
      var addRouter = [
        {
          path: '/sysmessages',
          component: Layout,
          redirect: 'noredirect',
          name: 'sysmessages',
          meta: {
            title: 'sysmessages',
            icon: 'eye',
          },
          children: [
            {
              path: 'editnotice',
              component: () => import('@/views/sysmessages/editnotice'),
              name: 'sysmessagesEditNotice',
              hidden: true,
              meta: {
                notauth: true,
                title: '新增/编辑公告',
                noCache: true,
                icon: 'list',
              },
            },
          ],
        },
      ]
      this.$router.addRoutes(addRouter)
    },
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
    openEdit(row) {
      var id = ''
      if (row != null) {
        id = row.id
      }
      this.$router.push({ name: 'sysmessagesEditNotice', params: { id: id } })
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
.c-p {
  color: #0076c8;
}
</style>