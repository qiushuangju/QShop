<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="dvLeftFloat" style="">
        <span>订单状态：</span>
        <el-radio-group v-model="searchStatusText" size="mini" @change="clickStatus">
          <el-radio-button :label=item.text v-for="item in listOrderStatus" :key="item.key" size="mini">{{ item.text }}</el-radio-button>
        </el-radio-group>
      </div>

      <div class="filter-container">
        <el-date-picker size="mini" style="width: 200px" value-format="yyyy-MM-dd" format="yyyy-MM-dd" v-model="listQuery.dateTime" type="daterange" align="right" unlink-panels range-separator="-" start-placeholder="开始日期"
          end-placeholder="结束日期" :picker-options="pickerOptions">
        </el-date-picker>
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px" class="filter-item" :placeholder="'订单号'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('Order', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('Order', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container">
      <div class="bg-white">
        <el-table ref="mainTable" :key="tableKey" :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%; height: 100%" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column width="200" label="订单号|类型">
            <template slot-scope="item">
              <p>{{ item.row.orderNo }}</p>
            </template>
          </el-table-column>
          <el-table-column width="180" label="用户昵称|手机">
            <template slot-scope="item">
              <p>{{ item.row.userInfo.nickName }}</p>
              <p>{{ item.row.userInfo.phone }}</p>
            </template>
          </el-table-column>
          <el-table-column width="400" label="商品信息">
            <template slot-scope="goods">
              <el-row :key="item.id" v-for="item in goods.row.listSku">
                <el-col :span="6">
                  <el-image :key="item.id" :src="item.skuImageUrl" fit></el-image>
                </el-col>
                <el-col :span="18">
                  <el-row>
                    <el-col :span="24">{{ item.goodsName }}</el-col>
                  </el-row>
                  <el-row>
                    <el-col :span="24"><span class="sSkuName">{{ item.skuName }}</span></el-col>
                  </el-row>
                  <el-row class="rowButtom">
                    <el-col :span="24"><span class="sMoney">￥{{ item.salePrice }}</span>×{{ item.quantity }}</el-col>
                  </el-row>
                </el-col>
              </el-row>
            </template>
          </el-table-column>
          <el-table-column width="80" label="订单状态" show-overflow-tooltip>
            <template slot-scope="item">
              {{
                listOrderStatus.find((p) => p.value == item.row.orderStatus) ==null ? "" : listOrderStatus.find((p) => p.value == item.row.orderStatus).text
              }}
            </template>
          </el-table-column>
          <el-table-column prop="payPrice" width="80" label="实际支付" show-overflow-tooltip>
          </el-table-column>
          <el-table-column width="160" label="支付方式|时间" show-overflow-tooltip>
            <template slot-scope="item">
              <p>{{ item.row.strPayType }}</p>
              <p>{{ item.row.payTime }}</p>
            </template>
          </el-table-column>

          <el-table-column label="地址来源" show-overflow-tooltip>
            <template slot-scope="item">
              <p>
                {{
                  item.row.orderAddressInfo.province +
                  item.row.orderAddressInfo.city +
                  item.row.orderAddressInfo.region
                }}
              </p>
              <p>{{ item.row.sourceUser }}</p>
            </template>
          </el-table-column>
          <el-table-column align="center" label="操作" width="150" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <router-link :to="{ path: '/order/details', query: { orderId: scope.row.id }, }">详情</router-link>
              <el-button type="text" v-if="checkPermission('order', 'btnAuditCancel') &&scope.row.orderStatus ==-20" @click="handleAudit(scope.row)">审核</el-button>
              <el-button type="text" v-if="checkPermission('order', 'btnDelivery') &&scope.row.orderStatus == 20" @click="handleDelivery(scope.row)">发货</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total > 0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
      <DeliveryForm ref="DeliveryForm" @handleSubmit="loadList" />
      <CancelForm ref="CancelForm" @handleSubmit="loadList" />
    </div>
  </div>
</template>

<script>
import Layout from "@/views/layout/Layout";
import * as orderApi from "@/api/order/order";
import { DeliveryForm, CancelForm } from "./modules";
import qs from "qs";

import enums from "@/api/enumList";
import moment from "moment";
import waves from "@/directive/waves"; // 水波纹指令
import Sticky from "@/components/Sticky";
import Pagination from "@/components/Pagination";
import elDragDialog from "@/directive/el-dragDialog";
import { OrderSourceEnum } from "@/common/enum/order";
import { initDate, initPickerOptions } from '@/utils/util'
export default {
  components: {
    Sticky,
    Pagination,
    DeliveryForm,
    CancelForm,
  },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      searchStatusText: "全部",
      baseURL: process.env.VUE_APP_BASE_API,
      storeList: [], //商城列表
      storageList: [], //运营仓列表
      OrderTypeList: [], //订单类型
      PayTypeList: [], //支付方式
      listOrderStatus: [], //订单状态
      DeliveryTypeList: [], //配送方式
      OrderSourceEnum, //订单来源
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        // 查询条件
        page: 1,
        limit: 10,
        dateTime: initDate(),
        startTime: undefined,
        endTime: undefined,
        key: undefined,
        orderStatus: undefined, //订单状态
        payType: undefined, //支付方式
        onlyMy: false, //是否只查询自己
      },
      pickerOptions: initPickerOptions(),
      dialogFormVisible: false, //编辑
      dialogDetailsFormVisible: false, //详情
      dialogStatus: "",
      textMap: {
        update: "编辑",
        details: "订单信息",
      },
      dialogPvVisible: false,
      pvData: [],
      downloadLoading: false,
    };
  },
  created() {
    const strStatus = this.$route.query && this.$route.query.strStatus;
    if (strStatus != undefined && strStatus != '') {

      this.searchStatusText = strStatus;
    }
    this.initData();
    new Promise((resolve, reject) => {
      Promise.all([
        enums.getListOrderStatus(true).then((res) => {
          this.listOrderStatus = res;
        }),
        enums.getPayTypeList(true).then((res) => {
          this.PayTypeList = res;
        }),
        enums.getDeliveryTypeList().then((res) => {
          this.DeliveryTypeList = res;
        }),
        enums.getOrderTypeList().then((res) => {
          this.OrderTypeList = res;
        }),
      ]).then(() => {
        this.clickStatus();
        this.addRoutes();
      });
    });
  },
  methods: {
    addRoutes() {
      var addRouter = [
        {
          path: "/order",
          component: Layout,
          redirect: "noredirect",
          name: "order",
          meta: {
            title: "order",
            icon: "eye",
          },
          children: [
            {
              path: "details",
              component: () => import("@/views/order/details"),
              name: "orderDetails",
              hidden: true,
              meta: {
                notauth: true,
                title: "订单详情",
                noCache: true,
                icon: "list",
              },
            },
            {
              path: "refundDetails",
              component: () => import("@/views/order/refundDetails"),
              name: "refundDetails",
              hidden: true,
              meta: {
                notauth: true,
                title: "售后订单详情",
                noCache: true,
                icon: "list",
              },
            },
          ],
        },
      ];
      this.$router.addRoutes(addRouter);
    },
    //选择订单状态
    clickSearchType() {

    },
    //更多
    handleCommand(value) {
      switch (value.command) {
        case "details": //详情
          this.openDetails(value.row);
          break;
        case "delete": //删除
          this.handleDelete([value.row]);
          break;
      }
    },
    onBtnClicked: function (btnCode) {
      switch (btnCode) {
        case "btnExport":
          this.exportOrder();
          break;
        default:
          break;
      }
    },
    // 导出订单
    exportOrder() {
      var strParams = "";
      strParams = qs.stringify(this.listQuery, {
        arrayFormat: "repeat",
      });
      window.open(`${this.baseURL}/order/ExportOrder?` + strParams);
    },
    //选择订单状态
    clickStatus() {
      var searchType = this.listOrderStatus.filter(p => p.text == this.searchStatusText)[0].value
      this.listQuery.orderStatus = searchType
      this.loadList()
    },
    //选择支付方式
    clickPayType(payType) {
      this.listQuery.orderStatus = undefined;
      this.listQuery.payType = payType;
      this.loadList();
    },
    rowClick(row) {
      this.$refs.mainTable.clearSelection();
      this.$refs.mainTable.toggleRowSelection(row);
    },
    handleSelectionChange(val) {
      this.multipleSelection = val;
    },
    initData() { },
    loadList() {
      if (this.listQuery.dateTime != undefined) {
        this.listQuery.startTime = moment(new Date(this.listQuery.dateTime[0])).startOf("day").format("YYYY-MM-DD HH:mm:ss");
        this.listQuery.endTime = moment(new Date(this.listQuery.dateTime[1])).endOf("day").format("YYYY-MM-DD HH:mm:ss");
      } else {
        this.listQuery.startTime = undefined;
        this.listQuery.endTime = undefined;
      }
      this.listLoading = true;
      orderApi.load(this.listQuery).then((res) => {
        this.list = res.result;
        this.total = res.count;
        this.listLoading = false;
      });
    },
    handleFilter() {
      this.listQuery.page = 1;
      this.loadList();
    },
    handleSizeChange(val) {
      this.listQuery.limit = val;
      this.loadList();
    },
    handleCurrentChange(val) {
      this.listQuery.page = val.page;
      this.listQuery.limit = val.limit;
      this.loadList();
    },
    handleModifyStatus(row, disable) {
      // 模拟修改状态
      this.$message({
        message: "操作成功",
        type: "success",
      });
      row.disable = disable;
    },
    openDetails(row) {
      //打开详情页
      var id = "";
      if (row != null) {
        id = row.id;
      }
      this.$router.push({ name: "orderDetails", params: { id: id } });
    },
    handleDelete(rows) {
      // 多行删除
      order.del(rows.map((u) => u.id)).then(() => {
        this.$notify({
          title: "成功",
          message: "删除成功",
          type: "success",
          duration: 2000,
        });
        rows.forEach((row) => {
          const index = this.list.indexOf(row);
          this.list.splice(index, 1);
        });
      });
    },

    // 订单发货
    handleDelivery(record) {
      this.$refs.DeliveryForm.show(record);
    },

    // 审核取消订单
    handleAuditCancel(record) {
      this.$refs.CancelForm.show(record);
    },
  },
};
</script>
<style>
.dialog-mini .el-select {
  width: 100%;
}

tr td a {
  margin-right: 8px;
}
.c-p {
  color: #0076c8;
}
.goodsDetails {
  float: left;
  align: center;
}
.sSkuName {
  font-size: 12px;
  color: #7b7b7b;
}
.sMoney {
  color: #fa2209;
}

.rowButtom {
  margin-bottom: 8px;
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
.el-image__inner {
  width: 50px;
  height: 50px;
}
</style>
