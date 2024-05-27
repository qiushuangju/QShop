<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-date-picker size="mini" style="width: 200px;" value-format="yyyy-MM-dd" format="yyyy-MM-dd" v-model="listQuery.dateTime" type="daterange" align="right" unlink-panels range-separator="-" start-placeholder="开始日期"
          end-placeholder="结束日期" :picker-options="pickerOptions">
        </el-date-picker>
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'订单号'" v-model="listQuery.key">
        </el-input>
        <el-select size="mini" style="width: 200px;" v-model="listQuery.payStatus" placeholder="支付状态">
          <el-option v-for="item in  PayStatusList" :key="item.value" :label="item.text" :value="item.value">
          </el-option>
        </el-select>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('rechargeOrder', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('rechargeOrder', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container ">

      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">

          <el-table-column type="selection" align="center" width="55"></el-table-column>

          <el-table-column fixed="left" min-width="150" prop="orderNo" label="订单号" show-overflow-tooltip></el-table-column>

          <el-table-column min-width="130" label="用户信息" show-overflow-tooltip>
            <template slot-scope="item">
              <el-row>昵称：<span class="c-p">{{item.row.userInfo.name}}</span></el-row>
              <el-row>电话：<span class="c-p">{{item.row.userInfo.phone}}</span></el-row>
            </template>
          </el-table-column>
          <el-table-column min-width="130" label="套餐信息" show-overflow-tooltip>
            <template slot-scope="item">
              <el-row>名称：<span class="c-p">{{item.row.planInfo.planName}}</span></el-row>
              <el-row>充值：<span class="c-p">{{item.row.planInfo.money}}</span></el-row>
              <el-row>赠送：<span class="c-p">{{item.row.planInfo.giftMoney}}</span></el-row>
            </template>
          </el-table-column>

          <el-table-column prop="rechargeType" label="充值方式" show-overflow-tooltip>
            <template slot-scope="scope">
              <span class="c-p">{{RechargeTypeList.find(p=>p.value==scope.row.rechargeType)==null?"":
            RechargeTypeList.find(p=>p.value==scope.row.rechargeType).text}}</span>
            </template>
          </el-table-column>

          <el-table-column prop="payPrice" label="支付金额" show-overflow-tooltip></el-table-column>

          <el-table-column prop="giftMoney" label="赠送金额" show-overflow-tooltip></el-table-column>

          <el-table-column prop="actualMoney" label="到账金额" show-overflow-tooltip></el-table-column>

          <el-table-column prop="createTime" label="创建时间" show-overflow-tooltip></el-table-column>

          <el-table-column prop="payStatus" label="支付状态" show-overflow-tooltip>
            <template slot-scope="scope">
              <el-tag size="mini" :type="scope.row.payStatus==10?'danger':'success'">
                {{PayStatusList.find(p=>p.value==scope.row.payStatus)==null?"":
             PayStatusList.find(p=>p.value==scope.row.payStatus).text}}
              </el-tag>
            </template>
          </el-table-column>

          <el-table-column prop="payTime" label="付款时间" show-overflow-tooltip>
            <template slot-scope="item">
              {{item.row.payStatus==10?"":item.row.payTime}}
            </template>
          </el-table-column>
          <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('rechargeOrder', 'btnEdit')" @click="openEdit(scope.row)">编辑</el-button>
              <el-button type="text" v-if="checkPermission('rechargeOrder', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
            </template>
          </el-table-column>

        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>

      <el-dialog v-el-drag-dialog class="dialog-mini" width="500px" :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
        <el-form :rules="rules" ref="dataForm" :model="temp" label-position="right" label-width="100px">

          <el-form-item size="small" label="订单Id" prop="id">
            <el-input v-model="temp.id"></el-input>
          </el-form-item>
          <el-form-item size="small" label="订单号" prop="orderNo">
            <el-input v-model="temp.orderNo"></el-input>
          </el-form-item>
          <el-form-item size="small" label="用户Id" prop="userId">
            <el-input v-model="temp.userId"></el-input>
          </el-form-item>
          <!-- <el-form-item size="small" label="充值方式(10自定义金额 20套餐充值)">
              <el-select class="filter-item" v-model="temp.rechargeType" placeholder="Please select">
                 <el-option v-for="item in  RechargeTypeList" :key="item.key" :label="item.display_name" :value="item.key">
                </el-option>
              </el-select>
            </el-form-item> -->
          <el-form-item size="small" label="充值套餐Id" prop="planId">
            <el-input v-model="temp.planId"></el-input>
          </el-form-item>
          <el-form-item size="small" label="用户支付金额" prop="payPrice">
            <el-input v-model="temp.payPrice"></el-input>
          </el-form-item>
          <el-form-item size="small" label="赠送金额" prop="giftMoney">
            <el-input v-model="temp.giftMoney"></el-input>
          </el-form-item>
          <el-form-item size="small" label="实际到账金额" prop="actualMoney">
            <el-input v-model="temp.actualMoney"></el-input>
          </el-form-item>
          <el-form-item size="small" label="支付状态">
            <el-select class="filter-item" v-model="temp.payStatus" placeholder="Please select">
              <el-option v-for="item in  statusOptions" :key="item.key" :label="item.display_name" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
          <el-form-item size="small" label="付款时间" prop="payTime">
            <el-input v-model="temp.payTime"></el-input>
          </el-form-item>
          <el-form-item size="small" label="微信支付交易号" prop="transactionId">
            <el-input v-model="temp.transactionId"></el-input>
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
import * as rechargeOrder from '@/api/rechargeorder'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import moment from 'moment'
import enums from '@/api/enumList'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import { initDate, initPickerOptions } from '@/utils/util'
export default {
  components: { Sticky, Pagination },
  directives: {
    waves,
    elDragDialog
  },
  data() {
    return {
      PayStatusList: [],//充值状态
      RechargeTypeList: [],//充值方式
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: { // 查询条件
        page: 1,
        limit: 20,
        key: undefined,
        dateTime: initDate(),
        startTime: undefined,
        endTime: undefined,
        payStatus: undefined,//支付状态
        rechargeType: undefined,//充值方式
      },
      pickerOptions: initPickerOptions(),
      temp: {
        id: '', // 订单Id
        orderNo: '', // 订单号
        userId: '', // 用户Id
        rechargeType: '', // 充值方式(10自定义金额 20套餐充值)
        planId: '', // 充值套餐Id
        payPrice: '', // 用户支付金额
        giftMoney: '', // 赠送金额
        actualMoney: '', // 实际到账金额
        payStatus: '', // 支付状态(10待支付 20已支付)
        payTime: '', // 付款时间
        transactionId: '', // 微信支付交易号
        storeId: '', // 商城Id
        extendInfo: '', // 其他信息,防止最后加逗号，可以删除
        userInfo: {//用户信息
          account: '',
          name: '',
          phone: '',
        },
        plan: {//套餐信息
          planName: '',
          money: '',
          giftMoney: ''
        }
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
        appId: [{ required: true, message: '必须选择一个应用', trigger: 'change' }],
        name: [{ required: true, message: '名称不能为空', trigger: 'blur' }]
      },
      downloadLoading: false
    }
  },
  created() {
    enums.getRechargeTypeList(true).then((res) => {
      this.RechargeTypeList = res
    })
    enums.getPayStatusList(true).then((res) => {
      this.PayStatusList = res
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
    loadList() {
      if (this.listQuery.dateTime != undefined) {
        this.listQuery.startTime = moment(new Date(this.listQuery.dateTime[0])).startOf("day").format("YYYY-MM-DD HH:mm:ss");
        this.listQuery.endTime = moment(new Date(this.listQuery.dateTime[1])).endOf("day").format("YYYY-MM-DD HH:mm:ss");
      }
      else {
        this.listQuery.startTime = undefined
        this.listQuery.endTime = undefined
      }
      this.listLoading = true
      rechargeOrder.load(this.listQuery).then(res => {
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
        orderNo: '',
        userId: '',
        rechargeType: '',
        planId: '',
        payPrice: '',
        giftMoney: '',
        actualMoney: '',
        payStatus: '',
        payTime: '',
        transactionId: '',
        storeId: '',
        createTime: '',
        updateTime: '',
        extendInfo: ''
      }
    },
    openEdit(row) { // 弹出编辑框
      if (row == null) {
        this.resetTemp();
        this.dialogStatus = 'create'
      } else {
        this.temp = Object.assign({}, row) // copy obj
        this.dialogStatus = 'update'
      } this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    saveData() { // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          rechargeOrder.addOrUpdate(tempData).then(() => {
            this.loadList();
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '保存成功',
              type: 'success',
              duration: 2000
            })
          })
        }
      })
    },
    handleDelete(rows) { // 多行删除
      rechargeOrder.del(rows.map(u => u.id)).then(() => {
        this.$notify({
          title: '成功',
          message: '删除成功',
          type: 'success',
          duration: 2000
        })
        rows.forEach(row => {
          const index = this.list.indexOf(row)
          this.list.splice(index, 1)
        })
      })
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
/* .mlr-2 {
    margin-left: 2px;
    margin-right: 2px;
} */
</style>
