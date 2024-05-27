<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('StorePage', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('StorePage', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>

      </div>
    </sticky>
    <div class="app-container">
      <div class="bg-white">
        <el-table ref="mainTable" :key="tableKey" :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column prop="id" width="300" label="页面ID" show-overflow-tooltip></el-table-column>
          <el-table-column prop="pageName" min-width="150" label="页面名称" show-overflow-tooltip></el-table-column>
          <el-table-column width="120" label="页面类型" show-overflow-tooltip>
            <template slot-scope="scope">
              <el-tag size="mini">{{scope.row.strPageType}}</el-tag>
            </template>
          </el-table-column>

          <el-table-column prop="createTime" min-width="170" label="创建时间" show-overflow-tooltip></el-table-column>
          <el-table-column align="center" label="操作" width="160" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('StorePage', 'btnEdit')" @click="handleUpdate(scope.row)">编辑</el-button>
              <el-button type="text" v-if="checkPermission('StorePage', 'btnSetHome')" @click="handleSetHome(scope.row)">设为首页</el-button>
              <el-button type="text" v-if="checkPermission('StorePage', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total > 0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
  </div>
</template>




<script>
import Layout from '@/views/layout/Layout'
import * as api from '@/api/store/storepage'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import { PageTypeEnum } from '@/common/enum/page'
import { getToken } from '@/utils/auth'
export default {
  components: { Sticky, Pagination },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      PageTypeEnum,
      dialogUpload: false,
      fileList: [],
      tokenHeader: { 'X-Token': getToken() },
      baseURL: process.env.VUE_APP_BASE_API, // api的base_url

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
        appId: undefined,
      },
      temp: {
        id: '', // Id
        cateName: '', // 分类名
        level: '', // 深度
        parentId: '', // 父节点
        urlIcon: '', // 分类图
        sortNo: '', // 排序
        creator: '', // 创建人Id
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
        appId: [
          {
            required: true,
            message: '必须选择一个应用',
            trigger: 'change',
          },
        ],
        name: [
          {
            required: true,
            message: '名称不能为空',
            trigger: 'blur',
          },
        ],
      },
      downloadLoading: false,
    }
  },
  filters: {
    statusFilter(disable) {
      const statusMap = {
        false: 'color-success',
        true: 'color-danger',
      }
      return statusMap[disable]
    },
  },
  created() {
    var addRouter = [
      {
        path: '/storepage',
        component: Layout,
        redirect: 'noredirect',
        name: 'storepage',
        meta: {
          title: '页面设置',
          icon: 'eye',
        },
        children: [
          {
            path: 'edit/:id',
            component: () => import('@/views/storepage/edit'),
            name: 'edit',
            hidden: true,
            meta: {
              notauth: true,
              title: '编辑',
              noCache: true,
              icon: 'list',
            },
          },
          {
            path: 'create/',
            component: () => import('@/views/storepage/create'),
            name: 'create',
            hidden: true,
            meta: {
              notauth: true,
              title: '新增',
              noCache: true,
              icon: 'list',
            },
          },
        ],
      },
    ]
    this.$router.addRoutes(addRouter)

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
    getList() {
      this.listLoading = true
      api.load(this.listQuery).then((res) => {
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

    resetTemp() {
      this.temp = {
        id: '',
        cateName: '',
        level: '',
        parentId: '',
        urlIcon: '',
        sortNo: '',
        creator: '',
        createTime: '',
        extendInfo: '',
      }
    },
    handleCreate() {
      this.$router.push('/storepage/create')
    },
    handleUpdate(row) {
      this.$router.push('/storepage/edit/' + row.id)
    },
    handleSetHome(row) {
      api.setHome({ id: row.id }).then((res) => {
        this.listLoading = false
        this.$notify({
          title: '成功',
          message: '删除成功',
          type: 'success',
          duration: 2000,
        })
      })
    },
    handleDelete(rows) {
      // 多行删除
      api.del(rows.map((u) => u.id)).then(() => {
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
<style lang="less">
.input-search {
  max-width: 300px;
  min-width: 150px;
  float: right;
}
</style>
