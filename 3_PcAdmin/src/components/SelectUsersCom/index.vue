<!--
 * @description: 用户及角色选择共用组件
 * @author: liyubao | xufuxing
 * @version: 1.0
 * @updateDate:2021-07-04
-->
<template>
  <div style="height: 100%;" class="select-users-wrap">
    <div class="flex-row" style="height: 100%;">
      <div class="flex-item table-box">
        <div class="flex-row" style="align-items: center;" @keyup.13="handleSearch">
          <el-input size="mini" style="margin: 10px;width: 200px;" placeholder="请输入内容" v-model="searchKey">
            <i slot="prefix" class="el-input__icon el-icon-search"></i>
          </el-input>
          <el-button type="primary" icon="el-icon-search" size="mini" @click="handleSearch">查询</el-button>
          <div style="text-align: right;padding: 5px 10px;" :title="names" v-if="names" class="flex-item ellipsis">当前选中：{{ names }}</div>
        </div>
        <el-table ref="multipleTable" height="calc(100% - 52px - 36px)" v-if="loginKey === 'loginUser'" :data="tableData.datas" tooltip-effect="dark" v-loading="tableData.loading" style="width: 100%;border-top: 1px solid #e4e4e4;"
          @row-click="rowClick" @select="handleSelectionUser" @select-all="handleSelectionUser">
          <el-table-column align="center" type="selection" width="55"> </el-table-column>

          <el-table-column align="center" min-width="80px" :label="'账号'">
            <template slot-scope="scope">
              <span class="link-type">{{ scope.row.account }}</span>
            </template>
          </el-table-column>

          <el-table-column align="center" min-width="80px" :label="'姓名'">
            <template slot-scope="scope">
              <span>{{ scope.row.name }}</span>
            </template>
          </el-table-column>

          <el-table-column align="center" class-name="status-col" :label="'状态'" width="100">
            <template slot-scope="scope">
              <span :class="scope.row.status | userStatusFilter">{{ statusOptions.find((u) => u.key == scope.row.status).display_name }}</span>
            </template>
          </el-table-column>
        </el-table>

        <!--角色选择-->
        <el-table ref="multipleTable" height="calc(100% - 52px - 36px)" v-else :data="tableData.datas" tooltip-effect="dark" v-loading="tableData.loading" border style="width: 100%;" @row-click="rowClick" @select="handleSelectionUser"
          @select-all="handleSelectionUser">
          <el-table-column align="center" type="selection" width="55"> </el-table-column>

          <el-table-column align="center" min-width="50px" :label="'角色名称'">
            <template slot-scope="scope">
              <span>{{ scope.row.name }}</span>
            </template>
          </el-table-column>

          <el-table-column align="center" class-name="status-col" :label="'状态'" width="100">
            <template slot-scope="scope">
              <span :class="scope.row.status | userStatusFilter">{{ statusOptions.find((u) => u.key == scope.row.status).display_name }}</span>
            </template>
          </el-table-column>
        </el-table>

        <el-pagination :background="false" layout="prev, pager, next" :total="tableData.total" :page-size="tableData.listQuery.limit" @current-change="handlePageSearch" style="margin-top: 3px;text-align: right;">
        </el-pagination>
      </div>
    </div>
    <div style="text-align:right;margin-top: 10px;" v-if="!hiddenFooter">
      <el-button size="small" type="cancel" @click="handleClose">取消</el-button>
      <el-button size="small" type="primary" @click="handleSaveUsers">确定</el-button>
    </div>
  </div>
</template>
<script>
import { listToTreeSelect, unique } from "@/utils";
import * as login from "@/api/login";
import * as users from "@/api/user/users";
import * as roles from "@/api/roles";
export default {
  props: {
    /**
     * 是否忽略登录用户权限，直接获取全部数据
     * 用于可以跨部门选择用户、角色的场景
     */
    ignoreAuth: Boolean,
    show: Boolean,
    /**
     * 如果为loginUser，则表示选择用户
     */
    loginKey: String,
    /**`
     * 如果不为空则显示左边树状结构
     */
    /**
     * 是否隐藏【确定】按钮
     * 如果本页面的【确定】按钮被隐藏，父组件可以通过调用$ref.xxx.handleSaveUsers方法获取改变后的值
     */
    hiddenFooter: Boolean,
    /**
     * 初始选中的显示文本（由逗号组成的串）
     */
    userNames: String,
    /**
     * 初始选中项Id列表
     */
    users: Array,
  },
  data() {
    return {
      searchKey: "",
      statusOptions: [
        {
          key: -10,
          display_name: "停用",
        },
        {
          key: 10,
          display_name: "正常",
        },
      ],
      tableData: {
        datas: [],
        total: 0,
        /**
         * 外部传进来的已选中项显示文本
         */
        selectTexts: [],
        /**
         * 外部传进来的已选中项Id
         */
        selectIds: [],

        /**
         * 当前页选择的Id列表
         * 为解决删除选中项的时候使用
         */
        currentPageIds: [],

        /**
         * 当前页选择的文本信息
         * 为解决删除选中项的时候使用
         */
        currentPageTexts: [],

        loading: false,
        listQuery: {
          page: 1,
          limit: 10,
          key: undefined,
          maxUserType: 20, //后端用户
        },
      },
    };
  },
  computed: {
    names() {
      let names = "";
      if (
        this.tableData.selectTexts &&
        this.tableData.selectTexts.length > 0
      ) {
        names = [...this.tableData.selectTexts].join(",");
      }
      return names;
    },
  },
  filters: {
    userStatusFilter(status) {
      var res = "color-success";
      switch (status) {
        case 1:
          res = "color-danger";
          break;
        default:
          break;
      }
      return res;
    },
  },
  mounted() {
    if (this.users) {
      this.tableData.selectIds = [...this.users];
      this.tableData.selectTexts =
        this.userNames && this.userNames.split(",");
    }
    this.loadData();
  },
  methods: {
    // 加载数据
    loadData(page) {
      this.tableData.listQuery.page = page || 1;
      if (this.loginKey === "loginUser") {
        this.getUserList();
        return;
      }
      this.getRoleList();
    },

    // 搜索用户/角色
    handleSearch() {
      this.loadData();
    },
    // 获取全部用户
    getAllUsers() {
      this.tableData.listQuery.page = 1;
      this.getUserList();
    },
    // 分页查询用户/角色
    handlePageSearch(val) {
      this.loadData(val);
    },

    // 获取用户
    getUserList() {
      this.tableData.loading = true;
      this.tableData.listQuery.key = this.searchKey;

      let queryFn = this.ignoreAuth ? users.loadAll : users.getList;

      queryFn(this.tableData.listQuery).then((response) => {
        this.tableData.datas = response.result;
        this.tableData.total = response.count;
        this.tableData.loading = false;

        this.initCurrentPageData();
        this.setSelectTable();
      });
    },
    /**
     * 获取角色
     */
    getRoleList() {
      this.tableData.loading = true;
      this.tableData.listQuery.key = this.searchKey;
      this.tableData.total = 0;

      if (this.ignoreAuth != undefined && this.ignoreAuth) {
        roles.loadAll(this.tableData.listQuery).then((response) => {
          this.tableData.datas = response.result;
          this.tableData.total = response.count;
          this.initCurrentPageData();
          this.setSelectTable();
        });
      } else {
        roles.loadAll(this.tableData.listQuery).then((response) => {
          this.tableData.datas = response.result;
          this.initCurrentPageData();
          this.setSelectTable();
        });
      }
    },
    /**
     * 用后端返回的当前列表数据计算当前页面的选中项信息
     */
    initCurrentPageData() {
      this.tableData.currentPageIds = [...this.tableData.datas]
        .filter((x) => this.tableData.selectIds.indexOf(x.id) > -1)
        .map((item) => item.id);
      this.tableData.currentPageTexts = [...this.tableData.datas]
        .filter((x) => this.tableData.selectTexts.indexOf(x.name) > -1)
        .map((item) => item.name);
    },

    /**
     * 设置界面列表选中
     */
    setSelectTable() {
      this.$nextTick(() => {
        const rows = [...this.tableData.datas].filter((x) =>
          [...this.tableData.currentPageIds].some((y) => y === x.id)
        );
        rows.forEach((row) => {
          this.$refs.multipleTable.toggleRowSelection(row);
        });
        this.tableData.loading = false;
      });
    },

    handleClose() {
      this.$emit("update:show", false);
    },

    // 确认用户选择
    handleSaveUsers() {
      const names =
        this.tableData.selectTexts &&
        this.tableData.selectTexts.join(",");
      this.$emit("update:userNames", names);
      this.$emit("update:users", this.tableData.selectIds);
      this.$emit("update:show", false);
    },
    // 选择用户
    handleSelectionUser(val) {
      //先判定被删除的
      var delIds = this.tableData.currentPageIds.filter(
        (x) => val.map((u) => u.id).indexOf(x) < 0
      );
      if (delIds.length > 0) {
        this.tableData.selectIds = this.tableData.selectIds.filter(
          (x) => delIds.indexOf(x) < 0
        );
      }

      var delTexts = this.tableData.currentPageTexts.filter(
        (x) => val.map((u) => u.name).indexOf(x) < 0
      );
      if (delTexts.length > 0) {
        this.tableData.selectTexts = this.tableData.selectTexts.filter(
          (x) => delTexts.indexOf(x) < 0
        );
      }

      this.tableData.currentPageIds = val.map((item) => item.id);
      this.tableData.currentPageTexts = val.map((item) => item.name);

      //合并已选中的项和新增的项
      this.tableData.selectIds = unique([
        ...this.tableData.selectIds,
        ...this.tableData.currentPageIds,
      ]);
      this.tableData.selectTexts = unique([
        ...this.tableData.selectTexts,
        ...this.tableData.currentPageTexts,
      ]);
    },

    rowClick(row) {
      this.$refs.multipleTable.clearSelection();
      this.$refs.multipleTable.toggleRowSelection(row);
      this.tableData.selectTexts = [row].map(
        (item) => item.name || item.account
      );
      this.tableData.currentPageTexts = [row].map(
        (item) => item.name || item.account
      );
      this.tableData.selectIds = [row].map((item) => item.id);
      this.tableData.currentPageIds = [row].map((item) => item.id);
    },
  },
};
</script>
<style lang="scss">
.select-users-wrap {
  .part-box {
    border: none;
  }

  .table-box {
    border: 1px solid #e4e4e4;
    border-left: 0;
  }

  .custom-card {
    height: 100%;

    .el-card__body {
      height: calc(100% - 34px);
      overflow: auto;
    }
  }

  .flex-item {
    overflow: hidden;
  }
}
</style>
