
<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px" class="filter-item" :placeholder="'商品名称'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('deliveryTemplate', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('deliveryTemplate', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>

      </div>
    </sticky>
    <div class="app-container">
      <div class="bg-white">
        <el-table ref="mainTable" :key="tableKey" :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%" :height="autoTbHeight" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column :label="'模板名称'" prop="name" min-width="80"> </el-table-column>
          <el-table-column :label="'计费方式'" prop="strMethod" min-width="80"> </el-table-column>
          <el-table-column :label="'排序'" prop="sort" min-width="80"> </el-table-column>
          <el-table-column :label="'创建时间'" prop="createTime" min-width="80"> </el-table-column>

          <el-table-column align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('deliveryTemplate', 'btnEdit')" @click="handleUpdate(scope.row)">编辑</el-button>
              <el-button type="text" v-if="checkPermission('deliveryTemplate', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total > 0" :total="total" :page-sizes="[10, 20, 30, 50, 100,500]" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
  </div>
</template>

<script>
import Layout from '@/views/layout/Layout'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import * as Api from '@/api/setting/delivery'
import Pagination from '@/components/Pagination'
export default {
  components: { Sticky, Pagination },
  directives: {
    waves,
  },
  data() {
    return {
      autoTbHeight: '500px',
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        // 查询条件
        page: 1,
        limit: 10,
        status: '0',
        isShowHome: null, //是否首页推荐
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '添加',
      },
      temp: {
        id: '',
        listurlImageMain: [],
      },
      downloadLoading: false,
      listStore: [],
    }
  },
  filters: {},
  watch: {},
  activated() {
    this.getList()
  },
  created() {
    this.getList()
    this.getTbHeight()
    this.addRouter()
  },
  methods: {
    addRouter() {
      var addRouter = [
        {
          path: '/setting/delivery/template',
          component: Layout,
          redirect: 'noredirect',
          name: 'DeliveryTemplate',
          meta: {
            title: '运费模板',
            icon: 'eye',
          },
          children: [
            {
              path: 'update',
              component: () =>
                import(
                  '@/views/setting/delivery/template/update'
                ),
              name: 'DeliveryTemplateEdit',
              hidden: true,
              meta: {
                notauth: true,
                title: '新增/编辑',
                noCache: true,
                icon: 'list',
              },
            },
          ],
        },
      ]
      this.$router.addRoutes(addRouter)
    },
    searchStatus(status) {
      this.listLoading = true
      this.listQuery.status = status
      this.getList()
    },
    getTbHeight() {
      this.autoTbHeight = parseInt(window.innerHeight) - 240 + 'px'
    },
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    setStatus(row) {
      const params = {
        id: row.id,
        isShowHome: row.isShowHomeMain,
      }
      goods.setStatus(params).then((res) => {
        if (res.code === 200) {
          this.getList()
          this.$message.success('修改成功！')
        } else {
          this.$message.error(res.message)
        }
      })
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    onBtnClicked: function (btnCode) {
      switch (btnCode) {
        case 'btnAdd':
          // this.handleCreate()
          this.handleUpdate()
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
        default:
          break
      }
    },
    resetTemp() {
      this.temp = { id: '', listurlImageMain: [] }
    },
    handleUpdate(row) {
      if (row != null) {
        this.$router.push({
          path: '/setting/delivery/template/update',
          query: { id: row.id },
        })
      } else {
        this.$router.push({
          path: '/setting/delivery/template/update',
        })
      }
    },
    handleDetails(row) {
      var id = ''
      if (row != null) {
        id = row.id
      }
      this.$router.push({
        name: 'DeliveryTemplateEdit',
        params: { id: id },
      })
    },
    handleClick() {
      this.listQuery.page = 1
      this.getList()
    },
    getList() {
      this.listLoading = true
      Api.load(this.listQuery).then((res) => {
        this.list = res.result
        this.total = res.count
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
    handleDelete(rows) {
      this.$confirm(`此操作将删除商品，请确认是否继续?`, `提示`, {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        beforeClose: (action, instance, done) => {
          if (action === 'confirm') {
            // 多行删除
            Api.del(rows.map((u) => u.id)).then((result) => {
              this.$message.success(result.message, 1.5)
              this.getList()
            })
            done()
          } else {
            done()
          }
        },
      })
    },
  },

}

</script >


