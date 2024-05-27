<template>
  <el-dialog class="dialog-mini" width="500px" title="退款" :visible.sync="visible">
    <el-alert type="warning" show-icon :closable="false" title="请确认已收到寄回的商品，确认后自动退回付款金额（原路退款）并关闭当前售后单" style="margin"></el-alert>
    <el-form :rules="rules" ref="dataForm" :model="temp" size="small" label-position="right" label-width="120px">

      <el-form-item label="售后类型" prop="orderNo">
        <el-tag>{{ temp.strRefundType}}</el-tag>
      </el-form-item>
      <el-form-item label="订单付款的总金额" prop="status">
        {{temp.amountExpectRefund}}
      </el-form-item>
      <el-form-item label="退款金额(元)" prop="amountRealRefund">
        <el-input type="number" v-model="temp.amountRealRefund"></el-input>
        <p class="extra">请输入退款金额，最多{{ temp.amountExpectRefund}}元，最多不能大于订单实际付款的总金额</p>
      </el-form-item>

    </el-form>
    <div slot="footer">
      <el-button size="mini" @click="visible = false">取消</el-button>
      <el-button size="mini" type="primary" @click="handleSubmit">确认</el-button>
    </div>
  </el-dialog>
</template>

<script>
import { assignment } from '@/utils/util'
import * as Api from '@/api/order/orderRefundSku'
import { AuditStatusEnum, RefundTypeEnum } from '@/common/enum/order/refund'

export default {
  data() {
    return {
      // 对话框标题
      title: '确认收货并退款',
      // 标签布局属性
      labelCol: { span: 8 },
      // 输入框布局属性
      wrapperCol: { span: 13 },
      // modal(对话框)是否可见
      visible: false,
      // modal(对话框)确定按钮 loading
      isLoading: false,
      // 当前表单元素
      form: this.$form.createForm(this),
      rules: {
        amountRealRefund: [
          {
            required: true,
            message: "退款金额不能为空",
            trigger: "change",
          },
        ],
      },
      // 当前记录
      temp: {}
    }
  },
  beforeCreate() {
    // 批量给当前实例赋值
    assignment(this, { AuditStatusEnum, RefundTypeEnum })
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
      this.temp.amountRealRefund = this.temp.amountExpectRefund
      // 更新form元素
      this.$nextTick(() => {
        this.$forceUpdate();
      });
    },

    /**
     * 确认按钮
     */
    handleSubmit(e) {

      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.isLoading = true
          const tempData = Object.assign({}, this.temp)
          tempData.orderRefundSkuId = this.temp.id;

          Api.refundMoney(tempData)
            .then((result) => {
              // 显示成功
              this.$message.success(result.message, 1.5)
              // 关闭对话框事件
              this.handleCancel()
              // 通知父端组件提交完成了
              this.$emit("handleSubmit");
            })
            .finally(() => {
              this.isLoading = false
            })
        }
      })
    },

    /**
     * 关闭对话框事件
     */
    handleCancel() {
      this.visible = false
      this.form.resetFields()
    },

    /**
    * 提交到后端api
    */
    onFormSubmit(values) {

    }

  }
}
</script>
<style lang="less" scoped>
.extra {
  color: rgba(0, 0, 0, 0.45);
  font-size: 12.5px !important;
  margin-bottom: 4px !important;
}
</style>
