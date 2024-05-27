<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="dvLeftFloat" style="">
        <span>售后状态：</span>

        <el-radio-group v-model="searchTypeText" size="mini" @change="clickSearchType">
          <el-radio-button :label=item.text v-for="item in listRefundStatus" :key="item.key" size="mini"></el-radio-button>
        </el-radio-group>
      </div>
      <div class="filter-container">
        <el-date-picker size="mini" style="width: 200px;" value-format="yyyy-MM-dd" format="yyyy-MM-dd" v-model="listQuery.dateTime" type="daterange" align="right" unlink-panels range-separator="-" start-placeholder="开始日期"
          end-placeholder="结束日期" :picker-options="pickerOptions">
        </el-date-picker>
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'订单号'" v-model="listQuery.key">
        </el-input>
        <el-button class="filter-item" size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
      </div>
    </sticky>
    <div class="app-container ">
      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column prop="orderNo" width="190" label="订单号" show-overflow-tooltip>
          </el-table-column>
          <el-table-column prop="userName" width="180" label="用户" show-overflow-tooltip>
          </el-table-column>
          <el-table-column width="300" label="商品信息" show-overflow-tooltip>
            <template slot-scope="scope">
              <el-row>
                <el-col :span="8">
                  <el-image style="width: 50px; height: 50px" :src="scope.row.urlSkuThumbnail" fit></el-image>
                </el-col>
                <el-col :span="16">
                  <el-row>
                    <el-col :span="24">{{scope.row.goodsName}}</el-col>
                  </el-row>
                  <el-row>
                    <el-col :span="24">{{scope.row.skuName}}</el-col>
                  </el-row>
                  <el-row>
                    <el-col :span="24">￥{{scope.row.amountExpectRefund}}x{{scope.row.quantity}}</el-col>
                  </el-row>
                  <el-row>
                  </el-row>
                </el-col>
              </el-row>
            </template>
          </el-table-column>
          <el-table-column prop="type" label="售后类型" show-overflow-tooltip>
            <template slot-scope="item">
              {{RefundTypeList.find(p=>p.value==item.row.refundType)==null?"":RefundTypeList.find(p=>p.value==item.row.refundType).text}}
            </template>
          </el-table-column>
          <el-table-column width="120" label="状态" show-overflow-tooltip>
            <template slot-scope="scope">
              <el-row style="margin-bottom:5px"> 订单: <el-tag>{{ scope.row.strOrderStatus }}</el-tag></el-row>
              <el-row> 售后:<el-tag>{{ scope.row.strStatus }}</el-tag></el-row>
            </template>
          </el-table-column>
          <el-table-column width="170" prop="createTime" label="售后发起时间" show-overflow-tooltip>
          </el-table-column>
          <el-table-column prop="payPrice" label="售后凭证" show-overflow-tooltip>
            <template slot-scope="scope">
              <el-image :preview-src-list="[item]" style="width:90px; padding:0px 30px; " v-for="(item,index) in scope.row.listRefundProof" :key="index" :src="item" fit></el-image>
            </template>
          </el-table-column>

          <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" @click="openDetails(scope.row)">详情</el-button>
              <el-button type="text" v-if="checkPermission('AfterSales','btnAuditCancel')&&scope.row.status==10" @click="handleAudit(scope.row)">审核</el-button>
              <el-button type="text" v-if="checkPermission('AfterSales','btnRefundMoney')&&(scope.row.status==30||scope.row.status==40)" @click="handleRefundMoney(scope.row)">退款</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
    <AuditForm ref="AuditForm" @handleSubmit="loadList" />
    <ReceiptForm ref="ReceiptForm" @handleSubmit="loadList" />
  </div>
</template>

<script>
import Layout from '@/views/layout/Layout'
import * as refund from '@/api/order/orderRefundSku'
import enums from '@/api/enumList'
import moment from 'moment'
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import { AuditForm, ReceiptForm } from './modules'
import { setTimeout } from 'timers';
import { initDate, initPickerOptions } from '@/utils/util'
export default {
  components: {
    Sticky,
    Pagination,
    AuditForm,
    ReceiptForm,
  },
  directives: {
    elDragDialog,
  },
  data() {
    return {
      searchTypeText: "全部",
      listRefundStatus: [], //售后状态
      RefundTypeList: [], //退款类型
      DeliveryTypeList: [], //配送方式
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
        status: undefined, //订单状态
        onlyMy: false, //是否只查询自己
        dateTime: initDate(),
        startDate: undefined,
        endDate: undefined,
      },
      pickerOptions: initPickerOptions(),
    }
  },
  activated() {
    this.loadList()
  },
  created() {
    const status = this.$route.query && this.$route.query.status
    if (status != undefined && status > 0) {
      this.listQuery.status = status
      this.listQuery.dateTime = [
        moment(new Date()).format('YYYY-MM-DD'),
        moment(new Date()).format('YYYY-MM-DD'),
      ]
    }
    new Promise((resolve, reject) => {
      Promise.all([
        enums.getRefundStatusList(true).then((res) => {
          this.listRefundStatus = res
        }),
        enums.getRefundTypeList(true).then((res) => {
          this.RefundTypeList = res
        }),
      ]).then(() => {
        this.loadList()
        this.addRoutes()
      })
    })
  },
  methods: {
    addRoutes() {
      var addRouter = [
        {
          path: '/aftersales',
          component: Layout,
          redirect: 'noredirect',
          name: 'aftersales',
          meta: {
            title: '业务员',
            icon: 'eye',
          },
          children: [
            {
              path: 'details',
              component: () =>
                import('@/views/aftersales/details'),
              name: 'aftersalesDetails',
              hidden: true,
              meta: {
                notauth: true,
                title: '详情',
                noCache: true,
                icon: 'list',
              },
            },
            {
              path: 'refundmoney',
              component: () =>
                import('@/views/aftersales/refundmoney'),
              name: 'aftersalesReFundMoney',
              hidden: true,
              meta: {
                notauth: true,
                title: '退款',
                noCache: true,
                icon: 'list',
              },
            },
          ],
        },
      ]
      this.$router.addRoutes(addRouter)
    },
    // 商家审核
    handleAudit(record) {
      this.$refs.AuditForm.show(record)
    },
    // 退款
    handleRefundMoney(record) {
      this.$refs.ReceiptForm.show(record)
    },
    //选择订单状态
    clickSearchType() {
      var searchType = this.listRefundStatus.filter(
        (p) => p.text == this.searchTypeText
      ).value
      this.listQuery.status = searchType
      this.loadList()
    },
    //支付方式
    clickPayType(payType) {
      this.listQuery.payType = payType
      this.loadList()
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
        case 'btnRefundMoney':
          setTimeout(() => {
            if (this.multipleSelection.length !== 1) {
              this.$message({
                message: '只能选中一个',
                type: 'error',
              })
              return
            }
            this.openReFundMoney(this.multipleSelection[0])
          }, 100)
          break
        default:
          break
      }
    },
    loadList() {
      //获取列表
      if (this.listQuery.dateTime != undefined) {
        this.listQuery.startDate = moment(
          new Date(this.listQuery.dateTime[0])
        )
          .startOf('day')
          .format('YYYY-MM-DD HH:mm:ss')
        this.listQuery.endDate = moment(
          new Date(this.listQuery.dateTime[1])
        )
          .endOf('day')
          .format('YYYY-MM-DD HH:mm:ss')
      } else {
        this.listQuery.startDate = undefined
        this.listQuery.endDate = undefined
      }
      this.listLoading = true
      refund.load(this.listQuery).then((res) => {
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
    openDetails(row) {
      //弹出详情
      var id = ''
      if (row != null) {
        id = row.orderId
      }
      this.$router.push({
        name: 'aftersalesDetails',
        params: { id: id },
      })
    },
    //退款
    openReFundMoney(row) {
      var id = ''
      if (row != null) {
        id = row.orderId
      }
      this.$router.push({
        name: 'aftersalesReFundMoney',
        params: { id: id },
      })
    },
    handleDelete(rows) {
      // 多行删除
      refund.del(rows.map((u) => u.id)).then(() => {
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
<style scoped>
.dialog-mini .el-select {
  width: 100%;
}
.c-p {
  color: #0076c8;
}
.goodsDetails {
  float: left;
  align: center;
}
.dropdown span {
  padding-left: 12px !important;
}
.details-content {
  padding: 0px 20px;
}
.details-content-content {
  font-size: 12px;
}
.details-content .el-row {
  margin-top: 20px;
}
</style>
