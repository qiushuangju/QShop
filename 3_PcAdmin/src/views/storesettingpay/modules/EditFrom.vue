<template>
  <el-dialog v-el-drag-dialog class="dialog-mini" width="600px" title="新增/编辑" :visible.sync="visible">
    <el-form :rules="rules" ref="dataForm" :model="temp" label-position="right" label-width="150px">
      <el-form-item size="small" label="编号" prop="code">
        <el-input v-model="temp.code" disabled="disabled"></el-input>
      </el-form-item>
      <el-form-item size="small" label="名字" prop="name">
        <el-input v-model="temp.name"></el-input>
      </el-form-item>
      <el-form-item size="small" label="排序">
        <el-input-number v-model="temp.sortNo" :min="0" :max="10"></el-input-number>
      </el-form-item>
      <div v-if="temp.code=='WxPay'">
        <el-form-item label="是否服务商支付" size="small">
          <el-switch v-model="temp.settingObj.servicePay" active-value="1" :inactive-value="0" />
          <p class="help">确定是服务商支付才开启，一般不要开启</p>
        </el-form-item>
        <el-form-item label="微信支付商户号" size="small">
          <el-input v-model="temp.settingObj.mchid" placeholder="请输入微信支付商户号" />
          <div class="help">财付通微信支付商户号</div>
        </el-form-item>
        <el-form-item label="微信支付密钥" size="small">
          <el-input v-model="temp.settingObj.signkey" auto-complete="off" :maxlength="32" clearable placeholder="请输入微信支付密钥" />
          <div class="help">财付通商户权限密钥</div>
        </el-form-item>
        <el-form-item label="子商户号" v-if="temp.settingObj.servicePay=='1'" size="small">
          <el-input v-model="temp.settingObj.subMchId" placeholder="请输入子商户号" />
          <div class="help">服务商子商户号</div>
        </el-form-item>
        <el-form-item label="apiclient_cert.pem 证书" size="small">
          <!-- <ImagesUpload size="small" is_local="1" file-type="file" :file.sync="temp.settingObj.certPath" /> -->
          <el-upload class="upload-demo" :action="baseURL + '/FileUpload/UploadCart'" name="files" :headers="headers" :before-remove="beforeRemoveCert" :before-upload="bandleBeforeUpload" multiple :limit="1" :on-exceed="handleExceed"
            :on-success="handleSuccessCert" :file-list="temp.certPath">
            <el-button size="small" type="primary">点击上传</el-button>
            <div slot="tip" class="help">只能上传pem文件</div>
          </el-upload>
        </el-form-item>
        <el-form-item label="apiclient_key.pem 证书" size="small">
          <!-- <ImagesUpload size="small" is_local="1" file-type="file" :file.sync="temp.settingObj.keyPath" /> -->
          <el-upload class="upload-demo" :action="baseURL + '/FileUpload/UploadCart'" name="files" :headers="headers" :before-remove="beforeRemoveKey" :before-upload="bandleBeforeUpload" multiple :limit="1" :on-exceed="handleExceed"
            :on-success="handleSuccessKey" :file-list="temp.keyPath">
            <el-button size="small" type="primary">点击上传</el-button>
            <div slot="tip" class="help">只能上传pem文件</div>
          </el-upload>
        </el-form-item>
      </div>
    </el-form>
    <div slot="footer">
      <el-button size="mini" @click="visible = false">取消</el-button>
      <el-button size="mini" type="primary" @click="saveData">确认</el-button>
    </div>
  </el-dialog>
</template>

<script>
import * as ApiStoreSettingPay from '@/api/storesettingpay'
import * as ApiFileUpload from '@/api/uploadFiles'
import elDragDialog from '@/directive/el-dragDialog'
import { getToken } from "@/utils/auth";
export default {
  components: {
    // ImagesUpload 
  },
  directives: {
    elDragDialog
  },
  data() {
    return {
      temp: {},
      visible: false,
      baseURL: process.env.VUE_APP_BASE_API, // api的base_url
      headers: {
        "X-Token": getToken(),
        "platform": "WebStore"
      },
      rules: {
        name: [{ required: true, message: '名称不能为空', trigger: 'blur' }],
        mchid: [{ required: true, message: '不能为空', trigger: 'blur' }],
        signkey: [{ required: true, message: '不能为空', trigger: 'blur' }],
        servicePay: [{ required: true, message: '不能为空', trigger: 'blur' }]
      },
    }
  },
  created() {
    this.getDetail();
  },
  methods: {
    //显示对话框
    show(record) {
      Promise.all([this.getDetail(record)])
        .then(res => {
          // 显示窗口
          this.visible = true;
        })
    },
    getDetail(record) { //获取详情
      ApiStoreSettingPay.getDetail({ id: record.id }).then((res) => {
        this.temp = res.result;
        this.temp.certPath = [{ name: this.temp.settingObj.certPath, url: this.temp.settingObj.certPath }];
        this.temp.keyPath = [{ name: this.temp.settingObj.keyPath, url: this.temp.settingObj.keyPath }]
      })
    },
    saveData() { // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          ApiStoreSettingPay.addOrUpdate(tempData).then((res) => {
            // 显示成功
            this.$message.success(res.message, 1.5);
            // 关闭对话框事件
            this.handleCancel();
            // 通知父端组件提交完成了
            this.$emit("handleSubmit");
          })
        }
      })
    },
    //  关闭对话框事件
    handleCancel() {
      this.visible = false;
      this.form.resetFields();
    },
    getFileType(name) {
      let startIndex = name.lastIndexOf('.')
      if (startIndex !== -1) {
        return name.slice(startIndex + 1).toLowerCase()
      } else {
        return ''
      }
    },
    bandleBeforeUpload(file) {
      let suffix = this.getFileType(file.name) //获取文件后缀名
      let suffixArray = ['pem'] //限制的文件类型，根据情况自己定义
      if (suffixArray.indexOf(suffix) === -1) {
        this.$message({
          message: '文件格式错误',
          type: 'error',
          duration: 2000
        })
        return false;
      }
    },

    handleExceed(files, listFile) {
      this.$message.warning(`当前限制选择 1 个文件，本次选择了 ${files.length} 个文件，共选择了 ${files.length + listFile.length} 个文件`);
    },
    handleSuccessCert(response, file, fileList) {
      var file = response.result[0];
      this.temp.settingObj.certPath = file.filePath;
    },
    handleSuccessKey(response, file, fileList) {
      var file = response.result[0];
      this.temp.settingObj.keyPath = file.filePath;
    },
    beforeRemoveCert(file, listFile) {
      this.temp.settingObj.certPath = "";
    },
    beforeRemoveKey(file, listFile) {
      this.temp.settingObj.keyPath = "";
    },


  }
}
</script>
<style scoped>
</style>
