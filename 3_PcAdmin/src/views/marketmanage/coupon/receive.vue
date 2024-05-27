<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'优惠券名称'" v-model="listQuery.key">
        </el-input>
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'用户昵称'" v-model="listQuery.userName">
        </el-input>
        <el-button class="filter-item" size="mini" v-waves icon="el-icon-search" @click="handleFilter">搜索</el-button>
      </div>
    </sticky>
    <div class="app-container ">

      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;">

          <el-table-column prop="userId" label="会员信息" show-overflow-tooltip>
            <template slot-scope="item">
              <UserItem :user="item.row" />
            </template>
          </el-table-column>

          <el-table-column prop="name" label="优惠券名称" show-overflow-tooltip></el-table-column>

          <el-table-column prop="sendType" label="发放方式" show-overflow-tooltip>
            <template slot-scope="item">
              <el-tag size="mini">{{SendTypeEnum.getNameByValue(item.row.sendType)}}</el-tag>
            </template>
          </el-table-column>

          <el-table-column prop="minPrice" label="最低消费金额" show-overflow-tooltip>
            <template slot-scope="item">
              <span class="c-p">{{item.row.minPrice}}</span>
            </template>
          </el-table-column>

          <el-table-column prop="minPrice" label="优惠方式" show-overflow-tooltip>
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

          <el-table-column prop="startTime" label="有效期" show-overflow-tooltip>
            <template slot-scope="item">
              <p><span class="c-p">{{item.row.startTime}}</span>~<span class="c-p">{{item.row.endTime}}</span></p>
            </template>
          </el-table-column>

          <el-table-column label="状态" show-overflow-tooltip>
            <template slot-scope="item">
              {{CouponStatusList.find(p=>p.value==item.row.status)==null?"":
        CouponStatusList.find(p=>p.value==item.row.status).text}}
            </template>
          </el-table-column>

          <el-table-column prop="createTime" label="领取时间" show-overflow-tooltip></el-table-column>

        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
  </div>

</template>

<script>
import * as userCoupon from "@/api/order/usercoupon";
import waves from "@/directive/waves"; // 水波纹指令
import Sticky from "@/components/Sticky";
import enums from "@/api/enumList";
import Pagination from "@/components/Pagination";
import elDragDialog from "@/directive/el-dragDialog";
import { UserItem } from "@/components/Table";
import { SendTypeEnum } from "@/common/enum/coupon";
export default {
  name: "userCoupon",
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
      CouponStatusList: [],
      SendTypeEnum,
      CouponRangeEnum: [],
      ExpireTypeEnum: [],
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
        userName: "",
      },
      temp: {
        id: "", // 主键
        couponId: "", // 优惠券Id
        name: "", // 优惠券名称
        sendType: "", // 优惠券类型(10满减券)
        reducePrice: "", // 满减券-减免金额
        minPrice: "", // 最低消费金额
        startTime: "", // 有效期开始时间
        endTime: "", // 有效期结束时间
        applyRange: "", // 适用范围(10全部商品 20指定商品)
        applyRangeConfig: "", // 适用范围(商品Id,隔开)
        isExpire: "", // 是否过期(-10未过期 10已过期)
        isUse: "", // 是否已使用(-10未使用 10已使用)
        userId: "", // 用户Id
        storeId: "", // 商城Id
        extendInfo: "", // 其他信息,防止最后加逗号，可以删除
      },
      dialogFormVisible: false,
      dialogStatus: "",
      textMap: {
        update: "编辑",
        create: "添加",
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        appId: [
          {
            required: true,
            message: "必须选择一个应用",
            trigger: "change",
          },
        ],
        name: [
          {
            required: true,
            message: "名称不能为空",
            trigger: "blur",
          },
        ],
      },
      downloadLoading: false,
    };
  },
  created() {
    enums.getCouponStatusList(true).then((res) => {
      this.CouponStatusList = res;
    }),
      this.loadList();
  },
  methods: {
    rowClick(row) {
      this.$refs.mainTable.clearSelection();
      this.$refs.mainTable.toggleRowSelection(row);
    },
    handleSelectionChange(val) {
      this.multipleSelection = val;
    },
    onBtnClicked: function (btnCode) {
      console.log("you click:" + btnCode);
      switch (btnCode) {
        case "btnAdd":
          this.openEdit();
          break;
        case "btnEdit":
          if (this.multipleSelection.length !== 1) {
            this.$message({
              message: "只能选中一个进行编辑",
              type: "error",
            });
            return;
          }
          this.openEdit(this.multipleSelection[0]);
          break;
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
    loadList() {
      this.listLoading = true;
      userCoupon.load(this.listQuery).then((res) => {
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
    resetTemp() {
      this.temp = {
        id: "",
        couponId: "",
        name: "",
        sendType: "",
        reducePrice: "",
        minPrice: "",
        startTime: "",
        endTime: "",
        applyRange: "",
        applyRangeConfig: "",
        isExpire: "",
        isUse: "",
        userId: "",
        storeId: "",
        createTime: "",
        updateTime: "",
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
          userCoupon.addOrUpdate(tempData).then(() => {
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
      userCoupon.del(rows.map((u) => u.id)).then(() => {
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
</style>
