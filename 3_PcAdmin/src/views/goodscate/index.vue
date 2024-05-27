<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('GoodsCate', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <!-- <el-button type="danger" size="mini" v-if="checkPermission('GoodsCate', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button> -->
      </div>
    </sticky>
    <div class="app-container ">
      <div class="bg-white">
        <!-- tree 插件方法 -->
        <!-- 分类表格   参数解释 :data(设置数据源)  :columns(设置表格中列配置信息)  :selection-type(是否有复选框)  :expand-type(是否展开数据) show-index(是否设置索引列)  index-text(设置索引列头)  border(是否添加边框) 
              :is-fold:父级是否默认折叠:show-row-hover(是否鼠标悬停高亮)-->
        <tree-table :data="list" :columns="columns" :selection-type="false" :expand-type="false" :is-fold="false" show-index index-text="序号" border :show-row-hover="true">
          <!-- 是否有效区域， 设置对应的模板列： slot="slotName"(与columns中设置的template一致) -->
          <template slot-scope="scope" slot="id">
            <span>{{ scope.row.id }}</span>
          </template>
          <template slot-scope="scope" slot="img">
            <el-image class="goodsImg" width="50px" height="50px" :src="scope.row.urlImg"></el-image>
          </template>

          <template slot-scope="scope" slot="level">
            <el-tag type="primary" v-if="scope.row.level == 1">一级</el-tag>
            <el-tag type="success" v-if="scope.row.level == 2">二级</el-tag>
            <el-tag type="danger" v-if="scope.row.level == 3">三级</el-tag>
          </template>

          <template slot-scope="scope" slot="status">
            <el-switch v-model="scope.row.status" active-color="#13ce66" inactive-color="#ff4949" :active-value=10 :inactive-value=-10 @change="changeStatus(scope.row)">
            </el-switch>
          </template>
          <template slot-scope="scope" slot="opt">
            <el-button type="text" v-if="checkPermission('GoodsCate', 'btnEdit')" @click="handleUpdate(scope.row)">编辑</el-button>
            <el-button type="text" v-if="checkPermission('GoodsCate', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
          </template>
        </tree-table>
      </div>

    </div>
  </div>

</template>

<script>
import Layout from '@/views/layout/Layout'
import { listToTreeSelect } from '@/utils'
import * as goodsCate from '@/api/goods/goodscate'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import elDragDialog from '@/directive/el-dragDialog'
import { getToken } from '@/utils/auth'
export default {
  components: {
    Sticky,
  },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      dialogUpload: false,
      fileList: [],
      tokenHeader: { 'X-Token': getToken() },
      baseURL: process.env.VUE_APP_BASE_API, // api的base_url
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      list: [],
      total: 0,
      listLoading: true,
      listQuery: {
        // 查询条件
        page: 1,
        limit: 100000,
        storeId: undefined,
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
      listStore: [], //商户

      columns: [
        {
          label: '分类名称',
          prop: 'name',
        },
        {
          label: '分类图',
          type: 'template',
          template: 'img',
          width: 120,
        },
        {
          label: '分类ID',
          type: 'template',
          template: 'id',
          width: 220,
        },

        {
          label: '排序',
          prop: 'sortNo',
          width: 60,
        },
        {
          label: '层级',
          type: 'template',
          template: 'level',
        },
        {
          label: '是否显示',
          type: 'template',
          template: 'status',
        },
        {
          label: '操作',
          type: 'template',
          template: 'opt',
          width: 100,
        },
      ],
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
    this.getList()
    this.addRouter()
    this.getListStore()
  },
  activated() {
    this.getList()
  },
  methods: {
    addRouter() {
      var addRouter = [
        {
          path: '/goodscate',
          component: Layout,
          redirect: 'noredirect',
          name: 'goodsCate',
          meta: {
            title: '商品分类',
            icon: 'eye',
          },
          children: [
            {
              path: 'edit',
              component: () => import('@/views/goodscate/edit'),
              name: 'goodsCateEdit',
              hidden: true,
              meta: {
                notauth: true,
                title: '新增/编辑分类',
                noCache: true,
                icon: 'list',
              },
            },
          ],
        },
      ]
      this.$router.addRoutes(addRouter)
    },
    //获取可选商户
    getListStore() { },
    closeUpload() {
      //关闭上传对话框
      this.fileList = []
      this.dialogUpload = false
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
          //this.handleCreate();
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
    getList() {
      this.listLoading = true
      goodsCate.listByWhere(this.listQuery).then((res) => {
        this.list = listToTreeSelect(res.result)
        // this.list = res.result
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
    handleModifyStatus(row, disable) {
      // 模拟修改状态
      this.$message({
        message: '操作成功',
        type: 'success',
      })
      row.disable = disable
    },
    changeStatus(row) {
      const params = {
        id: row.id,
        status: row.status,
      }
      goodsCate.changeStatus(params).then((res) => {
        if (res.code === 200) {
          this.getList()
          this.$message.success('修改成功！')
        } else {
          this.$message.error(res.message)
        }
      })
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
          goodsCate.addOrUpdate(this.temp).then(() => {
            this.list.unshift(this.temp)
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
      var id = ''
      if (row != null) {
        id = row.id
      }
      this.$router.push({ name: 'goodsCateEdit', params: { id: id } })

      // // 弹出编辑框
      // this.temp = Object.assign({}, row); // copy obj
      // this.dialogStatus = "update";
      // this.dialogFormVisible = true;
      // this.$nextTick(() => {
      //     this.$refs["dataForm"].clearValidate();
      // });
    },
    updateData() {
      // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          goodsCate.addOrUpdate(tempData).then(() => {
            for (const v of this.list) {
              if (v.id === this.temp.id) {
                const index = this.list.indexOf(v)
                this.list.splice(index, 1, this.temp)
                break
              }
            }
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
      this.$confirm('是否确认删除?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        // 多行删除
        goodsCate.del(rows.map((u) => u.id)).then(() => {
          this.$notify({
            title: '成功',
            message: '删除成功',
            type: 'success',
            duration: 2000,
          })
          this.getList();

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
.goodsImg {
  float: left;
  width: 58px;
  height: 58px;
  border-radius: 10px;
}
</style>
