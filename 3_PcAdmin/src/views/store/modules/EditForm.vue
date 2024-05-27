<template>
  <el-dialog class="dialog-mini" width="500px" title="修改" :visible.sync="visible">
    <el-form :rules="rules" ref="dataForm" :model="temp" size="small" label-position="right" label-width="100px">
      <el-form-item size="small" label="公司名称" prop="companyName">
        <el-input v-model="temp.companyName"></el-input>
      </el-form-item>
      <el-form-item size="small" label="联系人" prop="linkMan">
        <el-input v-model="temp.linkMan"></el-input>
      </el-form-item>
      <el-form-item size="small" label="联系手机" prop="phone">
        <el-input v-model="temp.phone"></el-input>
      </el-form-item>
    </el-form>
    <div slot="footer">
      <el-button size="mini" @click="visible = false">取消</el-button>
      <el-button size="mini" type="primary" @click="handleSubmit">确认</el-button>
    </div>
  </el-dialog>
</template>

<script>
import { assignment } from "@/utils/util";
import * as ApiStore from "@/api/store/store";
import { AuditStatusEnum, RefundTypeEnum } from "@/common/enum/order/refund";

export default {
  data() {
    return {
      // modal(对话框)是否可见
      visible: false,
      // modal(对话框)确定按钮 loading
      isLoading: false,
      // 当前表单元素
      form: this.$form.createForm(this),
      // 退货地址列表
      addressList: [],
      // 当前记录
      temp: {},
      rules: {
        storeName: [
          { required: true, trigger: "change" }
        ],
        linkMan: [
          { required: true, trigger: "change", },
        ],
        phone: [
          { required: true, trigger: "change", },
        ],
      },
    };
  },
  beforeCreate() {
    // 批量给当前实例赋值
    assignment(this, { AuditStatusEnum, RefundTypeEnum });
  },
  created() {
    // 获取退货地址列表
    this.getAddressList();
  },
  methods: {
    /**
     * 显示对话框
     */
    show(record) {
      // 显示窗口
      this.visible = true;
      // 当前记录
      this.temp = record;
    },



    /**
     * 确认按钮
     */
    handleSubmit(e) {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          tempData.id = this.temp.id;
          this.isLoading = true;
          ApiStore.updateStoreInfo(tempData)
            .then((result) => {
              // 显示成功
              this.$message.success(result.message, 1.5);
              // 关闭对话框事件
              this.handleCancel();
              // 通知父端组件提交完成了
              this.$emit("handleSubmit");
            })
        }
      })
    },

    /**
     * 关闭对话框事件
     */
    handleCancel() {
      this.visible = false;
      this.form.resetFields();
    },


  },
};
</script>
<style lang="less" scoped></style>
