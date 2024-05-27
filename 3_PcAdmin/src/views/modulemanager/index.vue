<template>
  <div class="flex-column">
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" prefix-icon="el-icon-search" style="width: 200px; margin-bottom: 0" class="filter-item" :placeholder="'关键字'" v-model="listQuery.key">
        </el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('module', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="warning" size="mini" v-if="checkPermission('module', 'btnEdit')" icon="el-icon-edit" @click="onBtnClicked('btnEdit')">编辑</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('module', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>

        <el-button type="primary" size="mini" v-if="checkPermission('module', 'btnAddBtn')" icon="el-icon-plus" @click="onBtnClicked('btnAddBtn')">新增按钮</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('module', 'btnDelBtn')" icon="el-icon-delete" @click="onBtnClicked('btnDelBtn')">删除按钮</el-button>
        <el-checkbox size="mini" class="m-l-15" @change="tableKey = tableKey + 1" v-model="showDescription">Id/描述
        </el-checkbox>
      </div>
    </sticky>
    <div class="app-container flex-item">
      <el-row class="fh">
        <el-col :span="14" class="fh ls-border">
          <el-card shadow="never" class="card-body-none fh" style="overflow-y: auto">
            <div slot="header" class="clearfix">
              <el-button type="text" style="padding: 0 11px" @click="getAllBtns">所有按钮>></el-button>
            </div>
            <tree-table ref="BtnTable" highlight-current-row :data="modulesTree" :columns="columns" @row-click="treeClick" border>
            </tree-table>
          </el-card>
        </el-col>
        <el-col :span="10" class="fh">
          <div class="bg-white fh">
            <el-table ref="mainTable" :key="tableKey" :data="currentPageBtns" v-loading="listLoading" border fit highlight-current-row style="width: 100%" height="calc(100% - 52px)" @row-click="rowClick"
              @selection-change="handleSelectionChange">
              <el-table-column type="selection" align="center" width="55">
              </el-table-column>

              <el-table-column :label="'Id'" v-if="showDescription" min-width="120px">
                <template slot-scope="scope">
                  <span>{{ scope.row.id }}</span>
                </template>
              </el-table-column>

              <el-table-column min-width="80px" :label="'BtnCode'">
                <template slot-scope="scope">
                  <span class="link-type" @click="handleEditBtn(scope.row)">{{
                    scope.row.btnCode
                  }}</span>
                </template>
              </el-table-column>

              <el-table-column min-width="50px" :label="'名称'">
                <template slot-scope="scope">
                  <span>{{ scope.row.name }}</span>
                </template>
              </el-table-column>

              <el-table-column min-width="30px" :label="'排序'">
                <template slot-scope="scope">
                  <span>{{ scope.row.sort }}</span>
                </template>
              </el-table-column>
              <el-table-column :label="'描述'" v-if="showDescription" min-width="120px">
                <template slot-scope="scope">
                  <span>{{ scope.row.remark }}</span>
                </template>
              </el-table-column>
              <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
                <template slot-scope="scope">
                  <el-button type="text" v-if="checkPermission('module', 'btnEditBtn')" @click="handleEditBtn(scope.row)">编辑</el-button>
                  <el-button type="text" v-if="checkPermission('module', 'btnDelBtn')" @click="handleDelBtns(scope.row)">删除</el-button>
                </template>
              </el-table-column>
            </el-table>
            <pagination v-show="total > 0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
          </div>
        </el-col>
      </el-row>

      <!--模块编辑对话框-->
      <el-dialog v-el-drag-dialog class="dialog-mini" width="500px" :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
        <el-form :rules="rules" ref="dataForm" :model="temp" label-position="right" label-width="100px">
          <el-form-item size="small" :label="'名称'" prop="name">
            <el-input v-model="temp.name"></el-input>
          </el-form-item>
          <el-form-item size="small" :label="'排序'">
            <el-input-number v-model="temp.sortNo" :min="1" :max="800"></el-input-number>
          </el-form-item>
          <el-form-item size="small" :label="'是否系统'" prop="isSys">
            <el-switch v-model="temp.isSys" active-color="#13ce66" inactive-color="#ff4949">
            </el-switch>
          </el-form-item>
          <el-form-item size="small" :label="'是否启用'" prop="status">
            <el-switch v-model="temp.status" active-color="#13ce66" inactive-color="#ff4949" :active-value="10" :inactive-value="-10">
            </el-switch>
          </el-form-item>
          <el-form-item size="small" :label="'模块标识'">
            <el-input v-model="temp.code"></el-input>
          </el-form-item>
          <el-form-item size="small" :label="'图标'">
            <el-popover placement="top-start" width="500" ref="popoverModule" trigger="click" content="">
              <el-input slot="reference" :class="
                  temp.iconName
                    ? `iconfont icon-${temp.iconName} custom-icon-input`
                    : ''
                " v-model="temp.iconName">
              </el-input>
              <el-row class="selectIcon-box">
                <el-col :class="{ active: temp.iconName === item.font_class }" :span="3" v-for="(item, index) in iconData.glyphs" :key="index">
                  <i :class="`${iconData.font_family} ${iconData.css_prefix_text}${item.font_class}`" @click="handleChangeTempIcon(item)"></i>
                </el-col>
              </el-row>
            </el-popover>
          </el-form-item>
          <el-form-item size="small" :label="'url'">
            <el-input v-model="temp.url"></el-input>
          </el-form-item>

          <el-form-item size="mini" :label="'上级机构'">
            <treeselect ref="modulesTree" :normalizer="normalizer" :disabled="treeDisabled" :options="modulesTreeRoot" :default-expand-level="3" :multiple="false" :open-on-click="true" :open-on-focus="true" :clear-on-select="true"
              v-model="dpSelectModule"></treeselect>
          </el-form-item>
        </el-form>
        <div slot="footer">
          <el-button size="mini" @click="dialogFormVisible = false">取消</el-button>
          <el-button size="mini" v-if="dialogStatus == 'create'" type="primary" @click="createData">确认</el-button>
          <el-button size="mini" v-else type="primary" @click="updateData">确认</el-button>
        </div>
      </el-dialog>
      <!--按钮编辑对话框-->
      <el-dialog v-el-drag-dialog class="dialog-mini" width="500px" :title="textMap[dialogBtnStatus]" :visible.sync="dialogBtnVisible">
        <el-form :rules="rules" ref="btnForm" :model="BtnTemp" label-position="right" label-width="100px">
          <el-form-item size="small" :label="'名称'" prop="name">
            <el-input v-model="BtnTemp.name"></el-input>
          </el-form-item>

          <el-form-item size="small" :label="'btnCode'">
            <el-input v-model="BtnTemp.btnCode"></el-input>
          </el-form-item>

          <el-form-item size="small" :label="'排序'">
            <el-input-number v-model="BtnTemp.sort" :min="0" :max="800"></el-input-number>
          </el-form-item>
          <el-form-item size="small" :label="'备注'">
            <el-input v-model="BtnTemp.remark"></el-input>
          </el-form-item>
          <el-form-item size="small" :label="'所属模块'">
            <treeselect ref="BtnModulesTree" :normalizer="normalizer" :options="modulesTree" :default-expand-level="3" :multiple="false" :open-on-click="true" :open-on-focus="true" :clear-on-select="true" v-model="BtnTemp.moduleId">
            </treeselect>
          </el-form-item>
        </el-form>
        <div slot="footer">
          <el-button size="mini" @click="dialogBtnVisible = false">取消</el-button>
          <el-button size="mini" v-if="dialogBtnStatus == 'create'" type="primary" @click="addBtn">确认</el-button>
          <el-button size="mini" v-else type="primary" @click="updateBtn">确认</el-button>
        </div>
      </el-dialog>
    </div>
  </div>
</template>

<script>
import treeTable from "@/components/TreeTable";
import Pagination from "@/components/Pagination";
import { listToTreeSelect } from "@/utils";
import extend from "@/extensions/delRows.js";
import * as modules from "@/api/modules";
import * as login from "@/api/login";
import enums from "@/api/enumList";
import Treeselect from "@riophae/vue-treeselect";
import "@riophae/vue-treeselect/dist/vue-treeselect.css";
import waves from "@/directive/waves"; // 水波纹指令
import Sticky from "@/components/Sticky";
import elDragDialog from "@/directive/el-dragDialog";
import iconData from "@/assets/public/css/comIconfont/iconfont.json";
export default {
  name: "module",
  components: {
    Sticky,
    Treeselect,
    treeTable,
    Pagination,
  },
  mixins: [extend],
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      iconData: iconData,
      classBtns: [
        {
          value: "primary",
        },
        {
          value: "danger",
        },
        {
          value: "success",
        },
        {
          value: "info",
        },
        {
          value: "warning",
        },
      ], // 按钮样式
      normalizer(node) {
        // treeselect定义字段
        return {
          label: node.name,
          id: node.id,
          children: node.children,
        };
      },
      columns: [
        // treetable的列名
        {
          text: "模块名称",
          value: "name",
          width: 180,
        },
        {
          text: "模块标识",
          value: "code",
          width: 180,
        },
        {
          text: "URL",
          value: "url",
        },
        {
          text: "排序",
          value: "sortNo",
          width: 80,
        },
      ],
      selectBtns: [], // 按钮列表选中的值
      tableKey: 0,
      list: [], // 按钮列表
      total: 0,
      currentModule: null, // 左边模块treetable当前选中的项
      listLoading: false,
      listQuery: {
        // 查询条件
        page: 1,
        limit: 20,
        key: undefined,
      },
      apps: [],
      buttonTypeList: [], //按钮类型
      showDescription: false,
      modules: [], // 用户可访问到的模块列表
      modulesTree: [], // 用户可访问到的所有模块组成的树
      temp: {
        // 模块临时值
        id: undefined,
        cascadeId: "",
        url: "",
        code: "",
        sortNo: 0,
        iconName: "",
        parentId: null,
        name: "",
        status: 0,
        isSys: false,
      },
      BtnTemp: {
        // 按钮临时值
        id: undefined,
        url: "",
        icon: "",
        code: "",
        moduleId: null,
        name: "",
        status: 0,
        sort: 0,
      },
      lastParentId: "", //最后一次选中的上级模块Id
      dialogFormVisible: false, // 模块编辑框
      dialogStatus: "",
      dialogBtnVisible: false, // 按钮编辑框
      dialogBtnStatus: "",

      chkRoot: false, // 根节点是否选中
      treeDisabled: false, // 树选择框时候可用
      textMap: {
        update: "编辑",
        create: "添加",
      },
      rules: {
        name: [
          {
            required: true,
            message: "名称不能为空",
            trigger: "blur",
          },
        ],
        buttonType: [
          {
            required: true,
            message: "按钮类型不能为空",
            trigger: "blur",
          },
        ],
      },
      downloadLoading: false,
    };
  },
  computed: {
    isRoot: {
      get() {
        return this.chkRoot;
      },
      set(v) {
        this.chkRoot = v;
        if (v) {
          this.temp.parentName = "根节点";
          this.temp.parentId = null;
        }
      },
    },
    modulesTreeRoot() {
      const root = [
        {
          name: "根节点",
          parentId: "",
          id: "",
        },
      ];
      return root.concat(this.modulesTree);
    },
    currentPageBtns: {
      get() {
        var start = (this.listQuery.page - 1) * this.listQuery.limit;
        var end = this.listQuery.page * this.listQuery.limit;
        var result = this.list.slice(start, end);
        return result;
      },
    },
    dpSelectModule: {
      // 模块编辑框下拉选中的模块
      get: function () {
        if (this.dialogStatus === "update") {
          return this.temp.parentId || "";
        } else {
          return this.lastParentId;
        }
      },
      set: function (v) {
        if (v === undefined || v === null || !v) {
          // 如果是根节点
          this.temp.parentName = "根节点";
          this.temp.parentId = null;
          return;
        }
        this.temp.parentId = v;
        this.lastParentId = v;
        var parentname = this.modules.find((val) => {
          return v === val.id;
        }).name;
        this.temp.parentName = parentname;
      },
    },
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        0: "info",
        1: "danger",
      };
      return statusMap[status];
    },
  },
  created() {
    // this.getList()
  },
  mounted() {
    this.getModulesTree();
  },
  methods: {
    handleChangeTempIcon(item) {
      this.temp.iconName = item.font_class;
      this.$refs.popoverModule.doClose();
    },
    handleChangeBtnTempIcon(item) {
      this.BtnTemp.icon = item.font_class;
      this.$refs.popoverBtn.doClose();
    },
    rowClick(row) {
      this.$refs.mainTable.clearSelection();
      this.$refs.mainTable.toggleRowSelection(row);
    },
    treeClick(data) {
      // 左侧treetable点击事件
      this.currentModule = data;
      this.currentModule.parent = null;
      this.getList();
    },
    getAllBtns() {
      this.currentModule = null;
      this.getList();
    },
    handleSelectionChange(val) {
      this.selectBtns = val;
    },
    onBtnClicked: function (btnCode) {
      switch (btnCode) {
        case "btnAdd":
          this.handleCreate();
          break;
        case "btnEdit":
          if (this.currentModule === null) {
            this.$message({
              message: "只能选中一个进行编辑",
              type: "error",
            });
            return;
          }
          this.handleUpdate(this.currentModule);
          break;
        case "btnDel":
          if (this.currentModule === null) {
            this.$message({
              message: "至少删除一个",
              type: "error",
            });
            return;
          }
          this.handleDelete(this.currentModule);
          break;
        case "btnAddBtn":
          this.handleAddBtn();
          break;
        case "btnEditBtn":
          if (this.selectBtns.length !== 1) {
            this.$message({
              message: "只能选中一个进行编辑",
              type: "error",
            });
            return;
          }
          this.handleEditBtn(this.selectBtns[0]);
          break;
        case "btnDelBtn":
          if (this.selectBtns.length < 1) {
            this.$message({
              message: "至少删除一个",
              type: "error",
            });
            return;
          }
          this.handleDelBtns(this.selectBtns);
          break;
        default:
          break;
      }
    },
    getList() {
      this.listLoading = true;
      var moduleId = this.currentModule === null ? null : this.currentModule.id;
      modules.loadBtns(moduleId).then((response) => {
        this.list = response.result;
        this.total = response.result.length;
        this.listLoading = false;
      });
    },
    getModulesTree() {
      var _this = this; // 记录vuecomponent
      login.getModules().then((response) => {
        _this.modules = response.result.map(function (item) {
          return {
            sortNo: item.sortNo,
            id: item.id,
            name: item.name,
            iconName: item.iconName,
            parentId: item.parentId || null,
            code: item.code,
            url: item.url,
            cascadeId: item.cascadeId,
            isSys: item.isSys,
            status: item.status,
          };
        });
        var modulestmp = JSON.parse(JSON.stringify(_this.modules));
        _this.modulesTree = listToTreeSelect(modulestmp).sort(
          (a, b) => a.sortNo - b.sortNo
        );
        const arr = _this.modulesTree.filter(
          (item) => item.children && item.children.length > 0
        );
        // 默认选中第一个
        _this.$refs.BtnTable.rowClick(arr[0].children[0]);
      });
    },
    handleFilter() {
      this.listQuery.page = 1;
      this.getList();
    },
    handleSizeChange(val) {
      this.listQuery.limit = val;
      this.getList();
    },
    handleCurrentChange(val) {
      this.listQuery.page = val.page;
      this.listQuery.limit = val.limit;
      this.getList();
    },
    resetTemp() {
      this.temp = {
        id: undefined,
        cascadeId: "",
        url: "",
        iconName: "",
        code: "",
        parentId: null,
        name: "",
        status: 0,
      };
    },
    resetBtnTemp() {
      this.BtnTemp = {
        id: undefined,
        cascadeId: "",
        icon: "",
        url: "",
        code: "",
        moduleId: this.currentModule ? this.currentModule.id : null,
        name: "",
        status: 0,
        sort: 0,
      };
    },

    // #region 模块管理
    handleCreate() {
      // 弹出添加框

      this.dpSelectModule = this.lastParentId;
      this.resetTemp();
      this.dialogStatus = "create";
      this.dialogFormVisible = true;
      this.temp.status = 10; //默认启用
      this.$nextTick(() => {
        this.$refs["dataForm"].clearValidate();
      });
    },
    createData() {
      // 保存提交
      this.$refs["dataForm"].validate((valid) => {
        var _this = this;
        if (valid) {
          if (_this.temp.url.indexOf("http") > -1 && !_this.temp.code) {
            _this.$message.error("请输入模块标识");
            return;
          }
          //如果打开弹框后，没有做任何选择操作，父级为空
          _this.temp.parentId = _this.temp.parentId || _this.lastParentId;

          modules.add(_this.temp).then((response) => {
            // 需要回填数据库生成的数据
            _this.temp.id = response.result.id;
            _this.temp.cascadeId = response.result.cascadeId;
            _this.list.unshift(_this.temp);
            _this.dialogFormVisible = false;
            _this.$notify({
              title: "成功",
              message: "创建成功",
              type: "success",
              duration: 2000,
            });
            _this.getModulesTree();
          });
        }
      });
    },
    handleUpdate(row) {
      // 弹出编辑框
      this.temp = Object.assign({}, row); // copy obj
      if (this.temp.children) {
        // 点击含有子节点树结构时，带有的children会造成提交的时候json死循环
        this.temp.children = null;
      }

      this.dialogStatus = "update";
      this.dialogFormVisible = true;
      this.dpSelectModule = this.temp.parentId;
      this.$nextTick(() => {
        this.$refs["dataForm"].clearValidate();
      });
    },
    updateData() {
      // 更新提交
      this.$refs["dataForm"].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp);
          if (tempData.url.indexOf("http") > -1 && !tempData.code) {
            this.$message.error("请输入模块标识");
            return;
          }
          modules.update(tempData).then(() => {
            this.dialogFormVisible = false;
            this.$notify({
              title: "成功",
              message: "更新成功",
              type: "success",
              duration: 2000,
            });

            this.getModulesTree();
            for (const v of this.list) {
              if (v.id === this.temp.id) {
                const index = this.list.indexOf(v);
                this.list.splice(index, 1, this.temp);
                break;
              }
            }
          });
        }
      });
    },
    handleDelete(row) {
      var _this = this;
      _this.delrows(modules, [row], () => {
        _this.getModulesTree();
      });
    },
    // #end region

    // #region 按钮管理
    handleAddBtn() {
      // 弹出添加框
      this.resetBtnTemp();
      this.dialogBtnStatus = "create";
      this.dialogBtnVisible = true;
      this.$nextTick(() => {
        this.$refs["btnForm"].clearValidate();
      });
    },
    addBtn() {
      // 保存提交
      this.$refs["btnForm"].validate((valid) => {
        if (valid) {
          modules.addBtn(this.BtnTemp).then((response) => {
            // 需要回填数据库生成的数据
            this.BtnTemp.id = response.result.id;
            this.list.unshift(this.BtnTemp);
            this.dialogBtnVisible = false;
            this.$notify({
              title: "成功",
              message: "创建成功",
              type: "success",
              duration: 2000,
            });
          });
        }
      });
    },
    handleEditBtn(row) {
      // 弹出编辑框
      this.BtnTemp = Object.assign({}, row); // copy obj
      this.dialogBtnStatus = "update";
      this.dialogBtnVisible = true;
      this.$nextTick(() => {
        this.$refs["btnForm"].clearValidate();
      });
    },
    updateBtn() {
      // 更新提交
      this.$refs["btnForm"].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.BtnTemp);
          modules.updateBtn(tempData).then(() => {
            this.dialogBtnVisible = false;
            this.$notify({
              title: "成功",
              message: "更新成功",
              type: "success",
              duration: 2000,
            });

            for (const v of this.list) {
              if (v.id === this.BtnTemp.id) {
                const index = this.list.indexOf(v);
                this.list.splice(index, 1, this.BtnTemp);
                break;
              }
            }
          });
        }
      });
    },
    handleDelBtns(rows) {
      // 多行删除
      modules.delBtn(rows.map((u) => u.id)).then(() => {
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
    // #end region
  },
};
</script>

<style lang="scss">
.text {
  font-size: 14px;
}

.item {
  margin-bottom: 18px;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both;
}

.el-card__header {
  padding: 12px 20px;
}

.selectIcon-box {
  text-align: center;
  border: 1px solid #eeeeee;
  border-right: 0;
  border-bottom: 0;

  .el-col {
    padding: 10px 0;
    border-right: 1px solid #eeeeee;
    border-bottom: 1px solid #eeeeee;

    &.active {
      .iconfont {
        color: #409eff;
      }
    }
  }

  .iconfont {
    cursor: pointer;
    font-size: 20px;
  }
}

.custom-icon-input::before {
  font-size: 18px;
  position: absolute;
  right: 10px;
  top: 8px;
}
.custom-select-icon {
  position: absolute;
  top: 8px;
  right: 40px;
  z-index: 10;
}
</style>
