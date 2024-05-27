<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('childUserDetail', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('childUserDetail', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container ">

      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column align="center" :label="'头像'">
            <template slot-scope="scope">
              <el-avatar :size="50" :src="scope.row.urlAvater"></el-avatar>
            </template>
          </el-table-column>

          <el-table-column min-width="260" label="会员信息">
            <template slot-scope="scope">
              <el-row>
                <el-col>昵称：<span class="c-p">{{scope.row.name}}</span></el-col>
              </el-row>
              <el-row>
                <el-col>电话：<span class="c-p">{{scope.row.phone}}</span></el-col>
              </el-row>
              <el-row>
                <el-col>账号：<span class="c-p">{{scope.row.account}}</span></el-col>
              </el-row>
            </template>
          </el-table-column>
          <el-table-column min-width="160" label="订单数据汇总">
            <template slot-scope="item">
              <el-col :span="18">
                <el-col>订单总数：{{item.row.dataAggregation.orderTotal}}</el-col>
                <el-col>订单总额：{{item.row.dataAggregation.orderMoneyTotal}}</el-col>
              </el-col>
            </template>
          </el-table-column>

          <el-table-column label="用户类型" align="center" width="100">
            <template slot-scope="scope">
              <span class="c-p">{{UserTypeList.find(p=>p.value==scope.row.userType)==null?
                '':UserTypeList.find(p=>p.value==scope.row.userType).text}}</span>
            </template>
          </el-table-column>
          <el-table-column width="55" label="状态" align="center">
            <template slot-scope="scope">
              <span class="c-p">{{UserStatusList.find(p=>p.value==scope.row.status)==null?'': UserStatusList.find(p=>p.value==scope.row.status).text}}</span>
            </template>
          </el-table-column>
          <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('childUserDetail', 'btnEdit')" @click="handleUpdate(scope.row)">编辑</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
  </div>

</template>

<script>
import _ from 'lodash'
import * as user from '@/api/user/memberUser'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import enums from '@/api/enumList'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import { RechargeForm } from './modules'
export default {
  components: {
    Sticky,
    Pagination,
  },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      UserTypeList: [], //用户类型
      UserStatusList: [], //用户状态
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      businessUserId: '', //业务员id
      listQuery: {
        // 查询条件
        page: 1,
        limit: 20,
        key: undefined,
        appId: undefined,
        isTotalOrder: true,
        minUserType: 10, //最小用户类型
        maxUserType: 15, //最大用户类型
        businessUserId: undefined, //业务员id
      },
      temp: {
        id: '',
        account: '',
        phone: '',
        password: '',
        userType: 40,
        status: 1,
        name: '',
        wxOpenId: '',
        urlAvater: '',
        tokenTime: '',
        token: '',
        sex: 1,
        isInsideBusiness: true, //是否内部业务员
        address: '',
        lastLoginTime: '',
        creator: '',
        createTime: '',
        extendInfo: '',
      },
      changePwd: {
        //修改密码
        account: '',
        oldPassword: '',
        password: '',
      },
      dialogFormVisible: false,
      dialogFormVisibleChangePwd: false, //修改密码弹框
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '添加',
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        account: [
          {
            required: true,
            message: '账号不能为空',
            trigger: 'blur',
          },
        ],
        name: [
          {
            required: true,
            message: '昵称不能为空',
            trigger: 'blur',
          },
        ],
        phone: [
          {
            required: true,
            message: '手机号不能为空',
            trigger: 'blur',
          },
        ],
        password: [
          {
            required: true,
            message: '密码不能为空',
            trigger: 'blur',
          },
        ],
      },
      rulesPwd: {
        account: [
          {
            required: true,
            message: '账号不能为空',
            trigger: 'blur',
          },
        ],
        password: [
          {
            required: true,
            message: '新密码不能为空',
            trigger: 'blur',
          },
        ],
      },
      downloadLoading: false,
    }
  },
  created() {
    this.businessUserId = this.$route.params && this.$route.params.id
    this.listQuery.businessUserId = this.businessUserId
    new Promise((resolve, reject) => {
      Promise.all([
        enums.getUserStatusList(true).then((res) => {
          this.UserStatusList = res
        }),
        enums.getUserTypeList().then((res) => {
          this.UserTypeList = res
        }),
      ]).then(() => {
        if (
          this.businessUserId != null &&
          this.businessUserId.trim() != ''
        ) {
          this.loadList()
        }
      })
    })
  },
  methods: {
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    /**
     * 刷新列表
     * @param Boolean bool 强制刷新到第一页
     */
    handleRefresh(bool = false) {
      //this.$refs.table.refresh(bool)
      this.loadList()
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
    //加载列表
    loadList() {
      this.listLoading = true
      const query = Object.assign({}, this.listQuery)
      user.load(query).then((res) => {
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
.c-a {
  margin: 5px;
}
</style>
