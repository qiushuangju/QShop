<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>

        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
      </div>
    </sticky>
    <div class="app-container ">

      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column prop="id" label="主键Id" show-overflow-tooltip></el-table-column>
          <el-table-column prop="userId" label="用户Id" show-overflow-tooltip></el-table-column>
          <el-table-column prop="orderId" label="订单ID(可选)" show-overflow-tooltip></el-table-column>
          <el-table-column prop="scene" label="余额变动场景" show-overflow-tooltip></el-table-column>
          <el-table-column prop="points" label="变动积分" show-overflow-tooltip></el-table-column>
          <el-table-column prop="balancePoints" label="操作后积分" show-overflow-tooltip></el-table-column>
          <el-table-column prop="describe" label="描述/说明" show-overflow-tooltip></el-table-column>
          <el-table-column prop="remark" label="管理员备注" show-overflow-tooltip></el-table-column>
          <el-table-column prop="storeId" label="商城Id" show-overflow-tooltip></el-table-column>
          <el-table-column prop="createTime" label="创建时间" show-overflow-tooltip></el-table-column>

          <el-table-column align="center" label="操作" width="80" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('userPointsLog', 'btnEdit')" @click="openEdit(scope.row)">编辑</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>

      <el-dialog v-el-drag-dialog class="dialog-mini" width="500px" :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
        <el-form :rules="rules" ref="dataForm" :model="temp" label-position="right" label-width="100px">

          <el-form-item size="small" label="主键Id" prop="id">
            <el-input v-model="temp.id"></el-input>
          </el-form-item>
          <el-form-item size="small" label="用户Id" prop="userId">
            <el-input v-model="temp.userId"></el-input>
          </el-form-item>
          <el-form-item size="small" label="订单ID(可选)" prop="orderId">
            <el-input v-model="temp.orderId"></el-input>
          </el-form-item>
          <el-form-item size="small" label="余额变动场景(10:用户充值 11:管理员充值 20:用户消费 21:管理员扣减 31:管理员设置余额 40:订单退款)">
            <el-select class="filter-item" v-model="temp.scene" placeholder="Please select">
              <el-option v-for="item in  statusOptions" :key="item.key" :label="item.display_name" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
          <el-form-item size="small" label="变动积分" prop="points">
            <el-input v-model="temp.points"></el-input>
          </el-form-item>
          <el-form-item size="small" label="操作后积分" prop="balancePoints">
            <el-input v-model="temp.balancePoints"></el-input>
          </el-form-item>
          <el-form-item size="small" label="描述/说明" prop="describe">
            <el-input v-model="temp.describe"></el-input>
          </el-form-item>
          <el-form-item size="small" label="管理员备注" prop="remark">
            <el-input v-model="temp.remark"></el-input>
          </el-form-item>
          <el-form-item size="small" label="商城Id" prop="storeId">
            <el-input v-model="temp.storeId"></el-input>
          </el-form-item>
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
import * as userPointsLog from '@/api/user/userpointslog'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
export default {
  name: 'userPointsLog',
  components: { Sticky, Pagination },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
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
        id: '', // 主键Id
        userId: '', // 用户Id
        orderId: '', // 订单ID(可选)
        scene: '', // 余额变动场景(10:用户充值 11:管理员充值 20:用户消费 21:管理员扣减 31:管理员设置余额 40:订单退款)
        points: '', // 变动积分
        balancePoints: '', // 操作后积分
        describe: '', // 描述/说明
        remark: '', // 管理员备注
        storeId: '', // 商城Id
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
      userPointsLog.load(this.listQuery).then((res) => {
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
        userId: '',
        orderId: '',
        scene: '',
        points: '',
        balancePoints: '',
        describe: '',
        remark: '',
        storeId: '',
        createTime: '',
        extendInfo: '',
      }
    },
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
          const tempData = Object.assign({}, this.temp)
          userPointsLog.addOrUpdate(tempData).then(() => {
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
      userPointsLog.del(rows.map((u) => u.id)).then(() => {
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
  ./log.vue