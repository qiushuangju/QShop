<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-select size="mini" style="width: 200px;" v-model="listQuery.scene" placeholder="变动场景">
          <el-option v-for="item in  BalanceTypeList" :key="item.value" :label="item.text" :value="item.value">
          </el-option>
        </el-select>
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'手机号/用户名'" v-model="listQuery.key">
        </el-input>
        <el-button class="filter-item" size="mini" v-waves icon="el-icon-search" @click="handleFilter">搜索</el-button>
      </div>
    </sticky>
    <div class="app-container ">

      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border style="width: 100%;">
          <el-table-column prop="userId" label="用户信息" show-overflow-tooltip>
            <template slot-scope="item">
              <UserItem :user="item.row" />
            </template>
          </el-table-column>
          <el-table-column prop="scene" label="余额变动场景" show-overflow-tooltip>
            <template slot-scope="item">
              <p class="c-p" size="mini">{{BalanceTypeList.find(p=>p.value==item.row.scene)==null?'': BalanceTypeList.find(p=>p.value==item.row.scene).text}}</p>
            </template>
          </el-table-column>
          <el-table-column prop="money" label="变动金额" show-overflow-tooltip></el-table-column>
          <el-table-column prop="describe" label="描述/说明" show-overflow-tooltip></el-table-column>
          <el-table-column prop="remark" label="管理员备注" show-overflow-tooltip></el-table-column>
          <el-table-column prop="createTime" label="创建时间" show-overflow-tooltip></el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
  </div>

</template>

<script>
import * as userBalanceLog from '@/api/user/userbalancelog'
import enums from '@/api/enumList'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import { UserItem } from '@/components/Table'
export default {
  name: 'userBalanceLog',
  components: {
    Sticky,
    UserItem,
    Pagination,
  },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      url: 'http://apiserver.huacuihui.cn/wwwroot/Html/img/avatar.png',
      BalanceTypeList: [], //枚举：余额变得场景
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
        onlyMy: false,
        account: undefined, //账号
        name: undefined, //用户名
        scene: undefined, //余额变动场景
        phone: undefined, //手机号
      },
      temp: {
        id: '', // 主键Id
        userId: '', // 用户Id
        orderId: '', // 订单ID(可选)
        scene: '', // 余额变动场景(10用户充值 20用户消费 30管理员操作 40订单退款)
        money: '', // 变动金额
        describe: '', // 描述/说明
        remark: '', // 管理员备注
        storeId: '', // 商城Id
        extendInfo: '', // 其他信息,防止最后加逗号，可以删除
      },
      dialogFormVisible: false,
      dialogStatus: '',
      dialogPvVisible: false,
      downloadLoading: false,
    }
  },
  created() {
    enums.getBalanceTypeList(true).then((res) => {
      this.BalanceTypeList = res
    })
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
      userBalanceLog.load(this.listQuery).then((res) => {
        this.list = res.result.map((item) => {
          item.userName = item.name
          return item
        })
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
        money: '',
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
          userBalanceLog.addOrUpdate(tempData).then(() => {
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
      userBalanceLog.del(rows.map((u) => u.id)).then(() => {
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
.c-p {
  color: #0076c8;
}
</style>
