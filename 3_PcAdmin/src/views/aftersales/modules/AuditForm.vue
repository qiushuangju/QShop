<template>
  <el-dialog class="dialog-mini" width="500px" title="审核" :visible.sync="visible">
    <el-form :rules="rules" ref="dataForm" :model="temp" size="small" label-position="right" label-width="100px">
      <el-form-item label="售后类型" prop="orderNo">
        <!-- {{temp.refundType}} -->
        <el-tag>{{ temp.strRefundType }}</el-tag>
      </el-form-item>
      <el-form-item label="审核状态" prop="status">
        <el-radio v-model="temp.status" label="20">同意</el-radio>
        <el-radio v-model="temp.status" label="-10">拒绝</el-radio>
      </el-form-item>
      <el-form-item label="退货地址" v-if="temp.status == 20" prop="status">
        <el-select v-model="temp.storeAddresId" placeholder="请选择">
          <el-option v-for="item in addressList" :key="item.id" :label="item.fullAddress" :value="item.id">
          </el-option>
        </el-select>
        <div class="form-item-help">
          <router-link target="_blank" :to="{ path: '/store/storeaddress' }">地址管理</router-link>
        </div>
      </el-form-item>
      <el-form-item label="拒绝原因" v-if="temp.status == -10" prop="sellerMark">
        <el-input type="textarea" v-model="temp.sellerMark"></el-input>
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
import * as Api from "@/api/order/orderRefundSku";
import * as AddressApi from "@/api/store/storeaddress";
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
      // 更新form元素
      this.$nextTick(() => {
        this.$forceUpdate();
      });
    },

    // 获取退货地址列表
    getAddressList() {
      this.isLoading = true;
      AddressApi.listByWhere({ type: 20 }).then(
        (res) => {
          this.addressList = res.result;
        }
      );
    },

    /**
     * 确认按钮
     */
    handleSubmit(e) {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          tempData.refundSkuId = this.temp.id;
          this.isLoading = true;
          Api.audit(tempData)
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
