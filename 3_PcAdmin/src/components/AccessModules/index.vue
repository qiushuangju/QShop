<template>

  <div class="compent-dialog-body">
    <div class="p-m">
      <!-- 模块 -->
      <el-form ref="form" label-position="left" :class="step=='1'?'':'hide'">
        <el-form-item size="small">
          <div class="block">
            <el-tree ref="tree" :data="modules" :check-strictly="true" show-checkbox node-key="id" default-expand-all :expand-on-click-node="false">
              <span class="custom-tree-node" slot-scope="{ node }">
                <span>{{ node.label }}</span>
              </span>
            </el-tree>
          </div>
        </el-form-item>
      </el-form>
      <!-- 按钮 -->
      <div :class="step=='2'?'':'hide'">
        <div class="block">
          <el-checkbox :indeterminate="isIndeterminate" style="margin-bottom:15px" v-model="roleBtnIdsAll" @change="handleCheckAllChange">全选</el-checkbox>
          <template v-for="item in checkModules">
            <div :key="item.id">
              <h4 class="title">
                {{item.parentName}} > {{item.label}}
              </h4>
              <div class="p-l-m">
                <el-checkbox-group v-model="roleBtnIds">
                  <el-checkbox @change="onChange" v-for="btn in filterBtns(item.id)" :label="btn.id" :key="btn.Id" size="small">{{btn.name}}</el-checkbox>
                </el-checkbox-group>
              </div>
            </div>
          </template>
        </div>
      </div>
    </div>
    <div slot="footer" class="el-dialog__footer">
      <el-button size="small" @click="close">取消</el-button>
      <el-button size="small" type="primary" v-show="step > 1" @click="up">上一步</el-button>
      <el-button size="small" type="success" @click="acceRole">下一步</el-button>
    </div>
  </div>

</template>

<script>
import { listToTreeSelect } from "@/utils";
import { intersection } from '@/utils/util'
import * as login from "@/api/login";
import * as apiModules from "@/api/modules";
import * as accessObjs from "@/api/accessObjs";

export default {
  name: "access-modules",
  components: {},
  props: ["roleId"],
  data() {
    return {
      currentRoleId: this.roleId,
      modules: [], // 登录用户可以访问到的所有模块
      listBtn: [], // 登录用户可以访问到的所有按钮
      roleBtnIds: [], // 角色分配到的按钮,只存ID
      roleBtnIdsAll: false, // 角色分配到按钮全选
      isIndeterminate: false, // 角色分配到按钮全选样式
      checkModules: [], // 已选模块
      step: 1, // 步骤
    };
  },
  watch: {
    roleId(val) {
      // 因为组件只挂载一次，后期只能通过watch来改变selectusers的值
      this.currentRoleId = val;
      this.init();
    },
    step(val) {
      if (val === 1) {
        this.$emit("change-title", "为角色分配【可见模块】");
      } else if (val === 2) {
        this.$emit("change-title", "为角色分配【可见按钮】");
      }
    },
    roleBtnIds(val) {
      this.isIndeterminate = !!(
        val.length > 0 && val.length < this.listBtn.length
      );
      this.roleBtnIdsAll = val.length === this.listBtn.length;
    },
  },
  mounted() {
    var _this = this;
    login.getModules().then((res) => {
      var modules = res.result.map(function (item) {
        let lable = item.name;
        return {
          id: item.id,
          label: lable,
          parentId: item.parentId || null,
          parentName: item.parentName,
          isSys: item.isSys,
          code: item.code,
        };
      });
      var tree = listToTreeSelect(modules);
      _this.modules = tree;
    });
    // 获取用户可访问的全部的按钮
    apiModules.loadBtns("").then((res) => {
      _this.listBtn = res.result;
      _this.init();
    });
  },
  methods: {
    init() {
      this.getRoleModuleIds();
      this.getRoleBtnIds();
    },
    filterBtns(moduleId) {
      // 按模块过滤按钮
      return this.listBtn.filter(function (btn) {
        return btn.moduleId === moduleId;
      });
    },
    getRoleModuleIds() {
      // 获取角色已分配的模块
      var _this = this;
      apiModules.loadForRole(_this.roleId).then((res) => {
        _this.$refs.tree.setCheckedKeys(
          res.result.map((item) => item.id)
        );
      });
    },
    getRoleBtnIds() {
      // 获取角色已分配的按钮
      var _this = this;
      apiModules.LoadBtnsForRole(_this.roleId).then((res) => {
        var listBtn = res.result;
        _this.roleBtnIds = listBtn.map((item) => item.id);
      });
    },
    onChange(val) {
      console.log(val);
    },
    close() {
      this.$emit("close");
    },
    up() {
      this.step = this.step * 1.0 - 1;
      return;
    },
    acceRole() {
      //保存数据
      var step = this.step * 1.0;
      console.log("111", step);
      switch (step) {
        case 1://模块页面点击下一步
          var checkNodes = this.$refs.tree.getCheckedNodes(
            true,
            false
          );
          if (checkNodes.length < 1) {
            this.$notify({
              title: "提示",
              message: "请先选择模块",
              type: "warning",
              duration: 2000,
            });
            return;
          }
          // 已选模块下原数据库内已选的按钮
          this.checkModules = checkNodes;
          var btnsId = [];
          this.checkModules.forEach((module) => {
            var listModuleBtn = this.listBtn.filter(function (btn) {
              return btn.moduleId === module.id;
            });
            var moduleBtnIds = listModuleBtn.map((item) => item.id);
            btnsId = btnsId.concat(moduleBtnIds);[...btnsId, moduleBtnIds];
          })
          this.roleBtnIds = intersection(this.roleBtnIds, btnsId);
          this.step = 2;
          break;
        case 2:
          if (this.roleBtnIds.length < 1) {
            this.$notify({
              title: "提示",
              message: "请先选择按钮",
              type: "warning",
              duration: 2000,
            });
            return;
          }
          this.saveAssign()
          this.close()
          break
      }
      return;
    },
    saveAssign() {
      accessObjs.assign({
        type: "RoleModule",
        firstId: this.roleId,
        secIds: this.$refs.tree.getCheckedKeys(),
      }).then(() => {
        accessObjs.assign({
          type: "RoleElement",
          firstId: this.roleId,
          secIds: this.roleBtnIds,
        }).then(() => {
          this.$notify({
            title: "成功",
            message: "分配成功",
            type: "success",
            duration: 2000,
          });
        });
      });
    },
    handleCheckAllChange(val) {
      this.roleBtnIds = val ? this.getAllBtnIds() : [];
      this.isIndeterminate = false;
    },
    getAllBtnIds() {
      const ids = [];
      this.checkModules.forEach((item) => {
        this.listBtn.forEach((btn) => {
          if (item.id == btn.moduleId)
            ids.push(btn.id);
        });
      })
      return ids;
    },
  },
};
</script>

<style scoped>
.custom-tree-node {
  flex: 1;
  display: flex;
  align-items: left;
  justify-content: space-between;
  font-size: 14px;
  padding-right: 8px;
}

.p-m {
  padding: 10px;
}
.p-l-m {
  padding-left: 10px;
}

.hide {
  display: none;
}
</style>