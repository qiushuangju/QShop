<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <!-- <el-radio-group v-model="searchStatusText" size="mini" @change="clickStatus">
          <el-radio-button :label=item.text v-for="item in listOrderStatus" :key="item.key" size="mini">{{ item.text }}</el-radio-button>
        </el-radio-group> -->

        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'用户名/手机号/账号'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('memberUser', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('memberUser', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
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

          <el-table-column label="昵称/手机号" show-overflow-tooltip>
            <span slot-scope="scope">
              <p>{{ scope.row.name}}</p>
              <p class="c-p">{{scope.row.phone}}</p>
            </span>
          </el-table-column>
          <el-table-column label="详细信息" show-overflow-tooltip>
            <template slot-scope="item">
              <el-col>{{item.row.realName}}</el-col>
              <el-col>{{item.row.bankName}}</el-col>
              <el-col>{{item.row.bankCardNo}}</el-col>
            </template>
          </el-table-column>
          <el-table-column label="积分余额" show-overflow-tooltip>
            <span slot-scope="scope">
              <p>余额:<span class="c-p">{{ scope.row.balance }}</span></p>
              <p>积分:<span class="c-p">{{ scope.row.points }}</span></p>
            </span>
          </el-table-column>

          <el-table-column align="center" prop="expendMoney" label="实际消费金额" show-overflow-tooltip></el-table-column>
          <el-table-column label="用户类型" show-overflow-tooltip>
            <template slot-scope="item">
              <el-col>
                <el-tag type="primary" effect="plain" size="mini" v-if="item.row.userType == 100">
                  会员
                </el-tag>
                <el-tag type="warning" effect="plain" size="mini" v-else-if="item.row.userType == 110">
                  经销商
                </el-tag>
                <el-tag type="danger" effect="plain" size="mini" v-else-if="item.row.userType == 120">
                  合伙人
                </el-tag>
              </el-col>
              <el-col>{{item.row.incomeUserPhone}}</el-col>
            </template>
          </el-table-column>

          <el-table-column prop="lastLoginTime" label="最后登录时间" show-overflow-tooltip></el-table-column>
          <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('MemberUser', 'btnRecharge')" @click="handleRecharge(scope.row)">充值</el-button>
              <el-button type="text" @click="handleDetails(scope.row)">详情</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
      <RechargeForm ref="RechargeForm" @handleSubmit="handleRefresh" />

      <el-dialog v-el-drag-dialog class="dialog-mini" width="500px" :title="dialogTitle" :visible.sync="dialogFormVisible">
        <el-form :rules="rules" ref="dataForm" :model="temp" label-position="right" label-width="100px">
          <el-form-item size="small" label="真实姓名" prop="realName">
            <el-input v-model="temp.realName"></el-input>
          </el-form-item>
          <el-form-item size="small" label="手机号码" prop="phone">
            <el-input v-model="temp.phone" disabled="disabled"></el-input>
          </el-form-item>
          <el-form-item size="small" label="开户银行" prop="bankName">
            <el-input v-model="temp.bankName"></el-input>
          </el-form-item>
          <el-form-item size="small" label="开户支行" prop="regBank">
            <el-input v-model="temp.regBank"></el-input>
          </el-form-item>
          <el-form-item size="small" label="卡号" prop="bankCardNo">
            <el-input v-model="temp.bankCardNo"></el-input>
          </el-form-item>
          <el-form-item size="small" label="详细地址" prop="fullAddress">
            <el-input v-model="temp.fullAddress"></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer">
          <el-button size="mini" @click="dialogFormVisible = false">取消</el-button>
          <el-button size="mini" type="primary" @click="onConfirm">确认</el-button>
        </div>
      </el-dialog>
    </div>
  </div>

</template>

<script>
import Layout from '@/views/layout/Layout'
import _ from 'lodash'
import enums from '@/api/enumList'
import * as user from '@/api/user/memberUser'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import { UserTypeEnum } from '@/common/enum/user'
import { RechargeForm } from './modules'
export default {
  name: 'user',
  components: { Sticky, RechargeForm, Pagination },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      UserTypeList: [],
      UserStatusList: [],
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
        minUserType: 100,
      },
      temp: {
        id: '', // 流水号
        account: '', // 账号
        phone: '', // 用户登录帐号
        password: '', // 密码
        userType: '', // 用户分类(10:用户 20:主播 30:管理用户)
        status: '', // 用户状态
        name: '', // 用户姓名
        wxOpenId: '', // 微信OpenId
        urlAvater: '', // 头像
        tokenTime: '', // token更新时间
        token: '', // Token
        sex: '', // 性别
        address: '', // 地址
        lastLoginTime: '', // 最后登录时间
        creator: '', // 创建人
        extendInfo: '', // 其他信息,防止最后加逗号，可以删除
      },
      dialogFormVisible: false,
      dialogTitle: '',
      dialogPvVisible: false,
      pvData: [],
      rules: {
        realName: [
          { required: true, message: '不能为空', trigger: 'blur' },
        ],
        phone: { required: true, message: '不能为空', trigger: 'blur' },
        bankName: [
          { required: true, message: '不能为空', trigger: 'blur' },
        ],
        regBank: [
          { required: true, message: '不能为空', trigger: 'blur' },
        ],
        bankCardNo: [
          { required: true, message: '不能为空', trigger: 'blur' },
        ],
        // detailAddress: [
        //     { required: true, message: '不能为空', trigger: 'blur' },
        // ],
      },
      downloadLoading: false,
    }
  },
  created() {
    this.addRoutes()
    this.loadList()
  },
  methods: {
    //动态路由
    addRoutes() {
      var addRouter = [
        {
          path: '/memberuser',
          component: Layout,
          redirect: 'noredirect',
          name: 'memberuser',
          meta: {
            title: 'memberuser',
            icon: 'eye',
          },
          children: [
            {
              path: 'userdetails',
              component: () =>
                import('@/views/memberuser/userdetails'),
              name: 'userDetails',
              hidden: true,
              meta: {
                notauth: true,
                title: '用户详情',
                noCache: true,
                icon: 'list',
              },
            },
          ],
        },
      ]
      this.$router.addRoutes(addRouter)
    },
    //切换用户类型
    changeUserType(type) {
      if (type == 0) {
        this.listQuery.minUserType = 100
        // this.listQuery.minUserType = 100
      } else {
        this.listQuery.minUserType = type
        this.listQuery.maxUserType = type
      }
      this.loadList()
    },
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    // 会员充值
    handleRecharge(item) {
      this.$refs.RechargeForm.handle(item)
    },
    //用户详情
    handleDetails(row) {
      var id = ''
      if (row != null) {
        id = row.id
      }
      this.$router.push({ name: 'userDetails', params: { id: id } })
    },
    /**
     * 刷新列表
     * @param Boolean bool 强制刷新到第一页
     */
    handleRefresh(bool = false) {
      //this.$refs.table.refresh(bool)
      this.loadList()
    },
    //修改用户状态
    changeStatusType(row) {
      // return false;
      // const params = {
      //     id: row.id,
      //     userType: row.userType,
      // };
      // user.changeUserType(params).then((res) => {
      //     if (res.code === 200) {
      //         this.loadList();
      //         this.$message.success("修改成功！");
      //     } else {
      //         this.$message.error(res.message);
      //     }
      // });
    },

    onBtnClicked: function (btnCode) {

      switch (btnCode) {
        case 'btnDealers':
          if (this.multipleSelection.length !== 1) {
            this.$message({
              message: '只能选中一个进行编辑',
              type: 'error',
            })
            return
          }
          this.dialogTitle = '设为经销商'
          this.userType = 110
          this.handleUpdate(this.multipleSelection[0])
          break
        case 'btnPartner':
          if (this.multipleSelection.length !== 1) {
            this.$message({
              message: '只能选中一个进行编辑',
              type: 'error',
            })
            return
          }
          this.dialogTitle = '设为合伙人'
          this.userType = 120
          this.handleUpdate(this.multipleSelection[0])
          break
        default:
          break
      }
    },
    loadList() {
      this.listLoading = true
      user.load(this.listQuery).then((res) => {
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
        account: '',
        phone: '',
        password: '',
        userType: '',
        status: '',
        name: '',
        wxOpenId: '',
        urlAvater: '',
        tokenTime: '',
        token: '',
        sex: '',
        address: '',
        lastLoginTime: '',
        creator: '',
        createTime: '',
        extendInfo: '',
      }
    },

    handleUpdate(row) {
      // 弹出编辑框
      this.temp = Object.assign({}, row) // copy obj
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    onConfirm() {
      // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          tempData.userType = this.userType
          user.changeUserType(tempData).then(() => {
            this.loadList()
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
      // 多行删除
      user.del(rows.map((u) => u.id)).then(() => {
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
        //  const index = this.list.indexOf(rows);
        //   this.list.splice(index, 1);
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
.c-a {
  margin: 5px;
}
</style>
