<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px" class="filter-item" :placeholder="'标题'" v-model="listQuery.key">
        </el-input>

        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('article', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('article', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container">
      <div class="bg-white">
        <el-table ref="mainTable" :key="tableKey" :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column prop="title" label="标题" show-overflow-tooltip></el-table-column>
          <el-table-column width="120" label="封面" show-overflow-tooltip>
            <template slot-scope="scope">
              <el-image style="width:50px;height:50px" :src="scope.row.urlCover"></el-image>
            </template>
          </el-table-column>
          <el-table-column prop="categoryName" label="文章分类" show-overflow-tooltip></el-table-column>

          <el-table-column prop="sort" label="排序" show-overflow-tooltip></el-table-column>

          <el-table-column prop="status" label="状态" show-overflow-tooltip>
            <template slot-scope="item">
              {{ArticleStatusList.find(p=>p.value==item.row.status)==null?
              '':ArticleStatusList.find(p=>p.value==item.row.status).text}}
            </template>
          </el-table-column>

          <el-table-column prop="isDelete" label="是否删除" show-overflow-tooltip>
            <template slot-scope="item">
              {{item.row.isDelete==0?"否":"是"}}
            </template>
          </el-table-column>

          <el-table-column prop="createTime" label="创建时间" show-overflow-tooltip></el-table-column>

          <el-table-column prop="updateTime" label="更新时间" show-overflow-tooltip></el-table-column>

          <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('article', 'btnEdit')" @click="openEdit(scope.row)">编辑</el-button>
              <el-button type="text" v-if="checkPermission('article', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total > 0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>
    </div>
  </div>
</template>

<script>
import Layout from '@/views/layout/Layout'
import * as article from "@/api/article/article";
import enums from '@/api/enumList';
import waves from "@/directive/waves"; // 水波纹指令
import Sticky from "@/components/Sticky";
import Pagination from "@/components/Pagination";
import elDragDialog from "@/directive/el-dragDialog";
export default {
  name: "article",
  components: { Sticky, Pagination },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      multipleSelection: [], // 列表checkbox选中的值
      ArticleStatusList: [],//文章状态
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
        id: "", // Id
        title: "", // Title
        showType: "", // ShowType
        categoryId: "", // CategoryId
        imageId: "", // ImageId
        content: "", // Content
        sort: "", // Sort
        status: "", // Status
        virtualViews: "", // VirtualViews
        actualViews: "", // ActualViews
        isDelete: "", // IsDelete
        storeId: "", // StoreId
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
          { required: true, message: "必须选择一个应用", trigger: "change" },
        ],
        name: [{ required: true, message: "名称不能为空", trigger: "blur" }],
      },
      downloadLoading: false,
    };
  },
  activated() {
    this.loadList()
  },
  created() {
    new Promise((resolve, reject) => {
      Promise.all([
        enums.getArticleStatusList().then((res) => {
          this.ArticleStatusList = res
        }),
      ]).then(() => {
        this.loadList();
        this.addRoutes()
      })
    })
  },
  methods: {
    addRoutes() {
      var addRouter = [
        {
          path: '/article',
          component: Layout,
          redirect: 'noredirect',
          name: 'article',
          meta: {
            title: 'article',
            icon: 'eye',
          },
          children: [
            {
              path: 'editarticle',
              component: () => import('@/views/article/editarticle'),
              name: 'articleEditArticle',
              hidden: true,
              meta: {
                notauth: true,
                title: '新增/编辑文章',
                noCache: true,
                icon: 'list',
              },
            },
          ],
        },
      ]
      this.$router.addRoutes(addRouter)
    },
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
      article.load(this.listQuery).then((res) => {
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
    openEdit(row) {
      var id = ''
      if (row != null) {
        id = row.id
      }
      this.$router.push({ name: 'articleEditArticle', params: { id: id } })
    },
    handleDelete(rows) {
      // 多行删除
      article.del(rows.map((u) => u.id)).then(() => {
        this.$notify({
          title: "成功",
          message: "删除成功",
          type: "success",
          duration: 2000,
        });
        this.loadList()
      });
    },
  },
};
</script>
<style>
.dialog-mini .el-select {
  width: 100%;
}
</style>
