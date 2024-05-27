<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="dvLeftFloat" style="">
        <span>状态：</span>
        <Button-group>
          <Button v-for="item in ComStatusList" :key="item.key" @click="clickStatus(item.value)">{{item.text}}</Button>
        </Button-group>
      </div>
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('CouponList','btnAdd')" icon="el-icon-plus" @click="handleUpdate()">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('CouponList','btnDel')" icon="el-icon-delete" @click="handleFilter">删除</el-button>
      </div>
    </sticky>
    <div class="app-container ">

      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column align='center' prop="name" label="名称" show-overflow-tooltip></el-table-column>
          <el-table-column align='center' prop="sendType" label="类型" show-overflow-tooltip>
            <template slot-scope="item">
              <el-tag size="mini">{{SendTypeEnum.getNameByValue(item.row.sendType)}}</el-tag>
            </template>
          </el-table-column>
          <el-table-column align='center' prop="isShowHome" label="首页弹出" show-overflow-tooltip>
            <template slot-scope="scope">
              {{scope.row.isShowHome==10?"是":"否"}}
            </template>
          </el-table-column>
          <el-table-column align='center' label="最低消费金额" show-overflow-tooltip>
            <template slot-scope="item">
              <span class="c-p">{{item.row.minPrice}}</span>
            </template>
          </el-table-column>

          <el-table-column align='center' label="优惠方式 " show-overflow-tooltip>
            <template slot-scope="item">
              <template v-if="item.row.sendType == 10">
                <span>立减</span>
                <span class="c-p mlr-2">{{ item.row.reducePrice }}</span>
                <span>元</span>
              </template>
              <template v-if="item.row.sendType == 20">
                <span>打</span>
                <span class="c-p mlr-2">{{ item.row.discount }}</span>
                <span>折</span>
              </template>
            </template>
          </el-table-column>
          <el-table-column align='center' prop="receiveNum" label="已领取数量" show-overflow-tooltip></el-table-column>
          <el-table-column align='center' prop="totalNum" label="发放总数" show-overflow-tooltip>
            <template slot-scope="item">
              {{item.row.totalNum==-1?"不限":item.row.totalNum}}
            </template>
          </el-table-column>
          <el-table-column align='center' prop="limitQuantity" label="限领次数" show-overflow-tooltip>
          </el-table-column>
          <el-table-column align='center' label="状态" show-overflow-tooltip>
            <template slot-scope="scope">
              <el-tag size="mini" :type="scope.row.status==10?'success':'danger'">
                {{ComStatusList.find(p=>p.value==scope.row.status)==null?
           '':ComStatusList.find(p=>p.value==scope.row.status).text}}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column align='center' prop="sort" label="排序" show-overflow-tooltip></el-table-column>
          <el-table-column align='center' prop="createTime" width="180" label="创建时间" show-overflow-tooltip></el-table-column>
          <el-table-column fixed="right" align="center" label="操作" width="80" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" @click="handleUpdate(scope.row.id)">编辑</el-button>
            </template>
          </el-table-column>

        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
  </div>

</template>

<script>
import Layout from "@/views/layout/Layout";
import enums from "@/api/enumList";
import * as coupon from "@/api/order/coupon";
import Sticky from "@/components/Sticky";
import Pagination from "@/components/Pagination";
import elDragDialog from "@/directive/el-dragDialog";
import * as store from "@/api/store/store";
import { SendTypeEnum } from "@/common/enum/coupon";
export default {
  components: { Sticky, Pagination },
  directives: {
    elDragDialog,
  },
  data() {
    return {
      moduleName: "CouponList",
      // 枚举类
      CouponRangeList: [],
      SendTypeEnum,
      ExpireTypeList: [],
      ComStatusList: [],
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
        status: 0,
        storeId: undefined,
      },
      dialogFormVisible: false,
      dialogStatus: "",
      downloadLoading: false,
      storeList: [], //所有商城信息
    };
  },
  activated() {
    this.loadList();
  },
  created() {
    new Promise((resolve, reject) => {
      Promise.all([
        enums.getCouponRangeList().then((res) => {
          this.CouponRangeList = res;
        }),
        enums.getExpireTypeList().then((res) => {
          this.ExpireTypeList = res;
        }),
        enums.getComStatusList(true).then((res) => {
          this.ComStatusList = res;
        }),
      ]).then(() => {
        this.loadStore();
        this.loadList();
        this.addRouter();
      });
    });
  },
  methods: {
    //动态路由
    addRouter() {
      var addRouter = [
        {
          path: "/coupon",
          component: Layout,
          redirect: "noredirect",
          name: "coupon",
          meta: {
            title: "coupon",
            icon: "eye",
          },
          children: [
            {
              path: "edit",
              component: () => import("@/views/marketmanage/coupon/edit"),
              name: "couponEdit",
              hidden: true,
              meta: {
                notauth: true,
                title: "新增/编辑优惠券",
                noCache: true,
                icon: "list",
              },
            },
          ],
        },
      ];
      this.$router.addRoutes(addRouter);
    },
    //适用范围：(10全部商品 20指定商品 30排除商品)
    applyRangeFormatter(row) {
      return this.applyRangeOptions.find((o) => o.key == row.applyRange)
        .label;
    },
    rowClick(row) {
      this.$refs.mainTable.clearSelection();
      this.$refs.mainTable.toggleRowSelection(row);
    },
    //选择订单状态
    clickStatus(status) {
      this.listQuery.status = status;
      this.loadList();
    },
    handleSelectionChange(val) {
      this.multipleSelection = val;
    },
    onBtnClicked: function (btnCode) {
      switch (btnCode) {

        case "btnDel":
          if (this.multipleSelection.length < 1) {
            this.$message({
              message: "至少删除一个",
              type: "error",
            });
            return;
          }
          this.handleDelete(this.multipleSelection);
          break;
        default:
          break;
      }
    },
    handleUpdate(id) {
      this.$router.push({ name: "couponEdit", params: { id: id } });
    },
    loadList() {
      this.listLoading = true;
      coupon.load(this.listQuery).then((res) => {
        this.list = res.result;
        this.total = res.count;
        this.listLoading = false;
      });
    },
    loadStore() {
      store.listByWhere().then((res) => {
        var list = res && res.result;
        this.storeList = list;
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
    resetTemp() {
      this.temp = {
        id: "",
        name: "",
        sendType: "",
        minPrice: "",
        reducePrice: "",
        expireType: "",
        expireDay: "",
        startTime: "",
        endTime: "",
        applyRange: "",
        applyRangeConfig: "",
        totalNum: "",
        receiveNum: "",
        describe: "",
        status: "",
        sort: "",
        storeId: "",
        createTime: "",
        extendInfo: "",
      };
    },
    openEdit(row) {
      // 弹出编辑框
      if (row == null) {
        this.resetTemp();
        this.dialogStatus = "create";
      } else {
        this.temp = Object.assign({}, row); // copy obj
        this.dialogStatus = "update";
      }
      this.dialogFormVisible = true;
      this.$nextTick(() => {
        this.$refs["dataForm"].clearValidate();
      });
    },
    saveData() {
      // 更新提交
      this.$refs["dataForm"].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp);
          coupon.addOrUpdate(tempData).then(() => {
            this.loadList();
            this.dialogFormVisible = false;
            this.$notify({
              title: "成功",
              message: "保存成功",
              type: "success",
              duration: 2000,
            });
          });
        }
      });
    },
    handleDelete(rows) {
      // 多行删除
      coupon.del(rows.map((u) => u.id)).then(() => {
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
  },
};
</script>
<style>
.dialog-mini .el-select {
  width: 100%;
}
.c-p {
  color: #0076c8;
}
.mlr-2 {
  margin-left: 2px;
  margin-right: 2px;
}
</style>
