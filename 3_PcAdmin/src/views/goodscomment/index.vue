<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="dvLeftFloat" style="">
        <span>评价类型：</span>
        <el-radio-group v-model="commentTypeText" size="mini" @change="clickCommentType">
          <el-radio-button :label=item.text v-for="item in listCommentType" :key="item.key" size="mini"></el-radio-button>
        </el-radio-group>
      </div>
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
        </el-input>

        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('GoodsComment', 'btnAdd')" icon="el-icon-plus" @click="onBtnClicked('btnAdd')">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('GoodsComment', 'btnDel')" icon="el-icon-delete" @click="onBtnClicked('btnDel')">删除</el-button>
      </div>
    </sticky>
    <div class="app-container ">

      <div class="bg-white">
        <el-table ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">

          <el-table-column fixed min-width="220" :label="'商品信息'">
            <template slot-scope="scope">
              <div class="goods">
                <el-image style="width: 50px;height: 50px;" class="goodsImg" :src="scope.row.goodsUrlImageMain"></el-image>
                <div style="width:80%;display: inline-block;">
                  <span class="goodsName">{{ scope.row.goodsName }}</span><br />
                </div>
              </div>
            </template>
          </el-table-column>
          <el-table-column label="买家" width="200" show-overflow-tooltip>
            <template slot-scope="scope">
              <el-avatar :size="50" :src="scope.row.urlAvater"></el-avatar>

              <div style="width:150;display: inline-block;">
                <span class="goodsName">{{ scope.row.nickName }}</span><br />
              </div>
            </template>
          </el-table-column>
          <el-table-column align="center" class="c-p" prop="status" label="显示状态" show-overflow-tooltip width="100">
            <template slot-scope="scope">
              <el-switch v-model="scope.row.status" active-color="#13ce66" inactive-color="#ff4949" :active-value=10 :inactive-value=-10 @change="setStatus(scope.row)">
              </el-switch>
            </template>
          </el-table-column>

          <el-table-column prop="score" label="评分" width="80" show-overflow-tooltip></el-table-column>
          <el-table-column prop="content" label="评论内容" show-overflow-tooltip></el-table-column>

          <el-table-column prop="createTime" label="创建时间" width="170" show-overflow-tooltip></el-table-column>

          <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('GoodsComment', 'btnEdit')" @click="openEdit(scope.row)">编辑</el-button>
              <el-button type="text" v-if="checkPermission('GoodsComment', 'btnDel')" @click="handleDelete([scope.row])">删除</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>

      <el-dialog v-el-drag-dialog class="dialog-mini" width="500px" :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
        <el-form :rules="rules" ref="dataForm" :model="temp" label-position="right" label-width="100px">

          <el-form-item size="small" label="Score">
            <el-select class="filter-item" v-model="temp.score" placeholder="Please select">
              <el-option v-for="item in  statusOptions" :key="item.key" :label="item.display_name" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
          <el-form-item size="small" label="Content" prop="content">
            <el-input v-model="temp.content"></el-input>
          </el-form-item>
          <el-form-item size="small" label="IsPicture">
            <el-select class="filter-item" v-model="temp.isPicture" placeholder="Please select">
              <el-option v-for="item in  statusOptions" :key="item.key" :label="item.display_name" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
          <el-form-item size="small" label="Status">
            <el-select class="filter-item" v-model="temp.status" placeholder="Please select">
              <el-option v-for="item in  statusOptions" :key="item.key" :label="item.display_name" :value="item.key">
              </el-option>
            </el-select>
          </el-form-item>
          <el-form-item size="small" label="Sort">
            <el-input-number v-model="temp.sort" :min="0" :max="10"></el-input-number>
          </el-form-item>
          <el-form-item size="small" label="UserId" prop="userId">
            <el-input v-model="temp.userId"></el-input>
          </el-form-item>
          <el-form-item size="small" label="OrderId" prop="orderId">
            <el-input v-model="temp.orderId"></el-input>
          </el-form-item>
          <el-form-item size="small" label="GoodsId" prop="goodsId">
            <el-input v-model="temp.goodsId"></el-input>
          </el-form-item>
          <el-form-item size="small" label="OrderGoodsId" prop="orderGoodsId">
            <el-input v-model="temp.orderGoodsId"></el-input>
          </el-form-item>
          <el-form-item size="small" label="StoreId" prop="storeId">
            <el-input v-model="temp.storeId"></el-input>
          </el-form-item>
          <el-form-item size="small" label="IsDelete" prop="isDelete">
            <el-input v-model="temp.isDelete"></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer">
          <el-button size="mini" @click="dialogFormVisible = false">取消</el-button>
          <el-button size="mini" type="primary" @click="saveData">确认</el-button>
        </div>
      </el-dialog>
    </div>
  </div>

</template>

<script>
import * as goodsComment from '@/api/goodscomment'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import enums from '@/api/enumList'
export default {
  components: { Sticky, Pagination },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      commentTypeText: '全部',
      listCommentType: [], //评价类型
      listQuery: {
        // 查询条件
        page: 1,
        limit: 20,
        isShowHide: true,
        commentType: 0,
        key: undefined,
        appId: undefined,
      },
      temp: {
        id: '', // Id
        score: '', // Score
        content: '', // Content
        isPicture: '', // IsPicture
        status: '', // Status
        sort: '', // Sort
        userId: '', // UserId
        orderId: '', // OrderId
        goodsId: '', // GoodsId
        orderGoodsId: '', // OrderGoodsId
        storeId: '', // StoreId
        isDelete: '', // IsDelete
        extendInfo: '', // 其他信息,防止最后加逗号，可以删除
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '添加',
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        appId: [
          {
            required: true,
            message: '必须选择一个应用',
            trigger: 'change',
          },
        ],
        name: [
          {
            required: true,
            message: '名称不能为空',
            trigger: 'blur',
          },
        ],
      },
      downloadLoading: false,
    }
  },
  created() {
    new Promise((resolve, reject) => {
      Promise.all([
        enums.listCommentType(true).then((data) => {
          this.listCommentType = data
        }),
      ]).then(() => {
        this.loadList()
      })
    })
  },
  methods: {
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    onBtnClicked: function (btnCode) {

      switch (btnCode) {
        case 'btnAdd':
          this.openEdit()
          break
        case 'btnEdit':
          if (this.multipleSelection.length !== 1) {
            this.$message({
              message: '只能选中一个进行编辑',
              type: 'error',
            })
            return
          }
          this.openEdit(this.multipleSelection[0])
          break
        case 'btnDel':

          if (this.multipleSelection.length < 1) {
            this.$message({
              message: '至少删除一个',
              type: 'error',
            })
            return
          }
          this.handleDelete(this.multipleSelection)
          break
        default:
          break
      }
    },
    //选择订单状态
    clickCommentType() {
      var commentType = this.listCommentType.filter(
        (p) => p.text == this.commentTypeText
      ).value
      this.listQuery.commentType = commentType
      this.loadList()
    },
    loadList() {
      this.listLoading = true
      goodsComment.load(this.listQuery).then((res) => {
        this.list = res.result
        this.total = res.count
        this.listLoading = false
      })
    },
    handleFilter() {
      this.listQuery.page = 1
      this.loadList()
    },
    handleSizeChange(val) {
      this.listQuery.limit = val
      this.loadList()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val.page
      this.listQuery.limit = val.limit
      this.loadList()
    },
    handleModifyStatus(row, disable) {
      // 模拟修改状态
      this.$message({
        message: '操作成功',
        type: 'success',
      })
      row.disable = disable
    },
    resetTemp() {
      this.temp = {
        id: '',
        score: '',
        content: '',
        isPicture: '',
        status: '',
        sort: '',
        userId: '',
        orderId: '',
        goodsId: '',
        orderGoodsId: '',
        storeId: '',
        isDelete: '',
        createTime: '',
        updateTime: '',
        extendInfo: '',
      }
    },
    openEdit(row) {
      // 弹出编辑框
      if (row == null) {
        this.resetTemp()
        this.dialogStatus = 'create'
      } else {
        this.temp = Object.assign({}, row) // copy obj
        this.dialogStatus = 'update'
      }
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    setStatus(row) {
      const data = {
        id: row.id,
        status: row.status,
      }
      goodsComment.setStatus(data).then((res) => {
        if (res.code === 200) {
          this.loadList()
          this.$message.success('修改成功！')
        } else {
          this.$message.error(res.message)
        }
      })
    },
    saveData() {
      // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          goodsComment.addOrUpdate(tempData).then(() => {
            this.loadList()
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '保存成功',
              type: 'success',
              duration: 2000,
            })
          })
        }
      })
    },
    handleDelete(rows) {
      // 多行删除
      goodsComment.del(rows.map((u) => u.id)).then(() => {
        this.$notify({
          title: '成功',
          message: '删除成功',
          type: 'success',
          duration: 2000,
        })
        rows.forEach((row) => {
          const index = this.list.indexOf(row)
          this.list.splice(index, 1)
        })
      })
    },
  },
}
</script>
<style>
.dialog-mini .el-select {
  width: 100%;
}
.goodsName {
  float: left;
  margin-left: 10px;
  color: #2656bd;
  font-style: 12px;
}
</style>
