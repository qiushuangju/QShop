<template>
  <div>
    <sticky :className="'sub-navbar'">
      <div class="dvLeftFloat" style="">
        <el-radio-group v-model="searchStatusText" size="mini" @change="clickStatus">
          <el-radio-button :label=item.text v-for="item in listStatus" :key="item.key" size="mini">{{ item.text }}</el-radio-button>
        </el-radio-group>
      </div>
      <div class="filter-container">
        <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px" :placeholder="'商品名称'" v-model="listQuery.key"></el-input>
        <el-button size="mini" icon="el-icon-search" @click="handleFilter">搜索</el-button>
        <el-button type="primary" size="mini" v-if="checkPermission('goods','btnAdd')" icon="el-icon-plus" @click="handleUpdate()">新增</el-button>
        <el-button type="danger" size="mini" v-if="checkPermission('goods','btnDel')" icon="el-icon-delete" @click="handleFilter">删除</el-button>

      </div>
    </sticky>
    <div class="app-container">
      <div class="bg-white">
        <el-table ref="mainTable" :key="tableKey" :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%" :height="autoTbHeight" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column fixed min-width="300" :label="'商品信息'">
            <template slot-scope="scope">
              <div class="goods">
                <el-image style="width: 50px;height: 50px;" class="goodsImg" :src="scope.row.urlImageMain"></el-image>
                <div style="width:80%;display: inline-block;">
                  <span class=" goodsName">{{ scope.row.goodsName }}</span><br />
                </div>
              </div>
            </template>
          </el-table-column>
          <el-table-column align="center" class="c-p" prop="status" label="上下架" show-overflow-tooltip width="150">
            <template slot-scope="scope">
              <el-switch v-model="scope.row.status" active-color="#13ce66" inactive-color="#ff4949" :active-value="10" :inactive-value="-10" @change="setStatus(scope.row)">
              </el-switch>
            </template>
          </el-table-column>
          <el-table-column align="center" class="c-p" prop="status" label="推荐" show-overflow-tooltip width="150">
            <template slot-scope="scope">
              <el-switch v-model="scope.row.isRecommend" active-color="#13ce66" inactive-color="#ff4949" :active-value=true :inactive-value=false @change="setRecommend(scope.row)">
              </el-switch>
            </template>
          </el-table-column>
          <el-table-column width="120" :label="'价格'">
            <template slot-scope="scope">
              <div class="goods">
                <el-row v-if="scope.row.linePrice>0">
                  <el-col>
                    划线价: <span class="c-p">{{scope.row.linePrice}}</span>
                  </el-col>
                </el-row>
                <el-row>
                  <el-col>
                    售价: <span class="c-p">{{scope.row.salePrice}}</span>
                  </el-col>
                </el-row>
                <el-row v-if="scope.row.purchasePrice>0">
                  <el-col>
                    采购价：<span class="c-p">{{scope.row.purchasePrice}}
                    </span>
                  </el-col>
                </el-row>
              </div>
            </template>
          </el-table-column>
          <el-table-column :label="'库存'" prop="stockTotal" min-width="80"> </el-table-column>
          <el-table-column width="100" :label="'销量'">
            <template slot-scope="scope">
              <el-row>
                <el-col>
                  初始销量:<span class="c-p">{{ scope.row.salesInitial }}</span>
                </el-col>
              </el-row>
              <el-row>
                <el-col>
                  实际销量:<span class="c-p">{{ scope.row.salesActual }}</span>
                </el-col>
              </el-row>
            </template>
          </el-table-column>

          <el-table-column align="center" prop="sortNo" label="排序" show-overflow-tooltip min-width="80"></el-table-column>

          <el-table-column min-width="200" :label="'创建时间'">
            <template slot-scope="scope">
              <el-row>
                创建时间:{{ scope.row.createTime }}
              </el-row>
              <el-row v-if="scope.row.stopTime!=null">
                下架时间:{{ scope.row.stopTime }}
              </el-row>
            </template>
          </el-table-column>

          <el-table-column fixed="right" align="center" label="操作" width="130" class-name="small-padding fixed-width">
            <template slot-scope="scope">
              <el-button type="text" v-if="checkPermission('goods','btnEdit')" @click="handleUpdate(scope.row.id)">编辑</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination v-show="total > 0" :total="total" :page-sizes="[10, 20, 30, 50, 100,500]" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
      </div>

    </div>
  </div>
</template>

<script>
import Layout from '@/views/layout/Layout'
import enums from '@/api/enumList'
import { listToTreeSelect } from '@/utils'
import * as goods from '@/api/goods/goods'
import * as apiSku from '@/api/goods/goodsSku'
import * as goodsCate from '@/api/goods/goodscate'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
export default {
  // name: 'goods',
  components: { Sticky, Pagination },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      searchStatusText: '全部',
      GoodsStatusList: [], //商品状态
      HomeRecommendTypeList: [], //首页推荐类型
      DeductStockTypeList: [], //库存计算方式
      autoTbHeight: '500px',
      typeSearch: 0,
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      list: null,
      listSku: [],
      listStatus: [
        { text: '全部', value: 0 },
        { text: '出售中', value: 10 },
        { text: '已下架', value: -10 },
        { text: '已售罄', value: -80 },
      ],
      total: 0,
      listLoading: true,
      listQuery: {
        // 查询条件
        page: 1,
        limit: 10,
        status: '0',
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '添加',
      },
      temp: {
        id: '',
        listurlImageMain: [],
      },
      rules: {
        sortNo: [
          {
            required: true,
            message: '不能为空',
            trigger: 'blur',
          },
          {
            validator: this.$validater.validateDigit,
            trigger: 'blur',
          },
        ],
        goodsName: [
          {
            required: true,
            message: '不能为空',
            trigger: 'blur',
          },
        ],
      },
      downloadLoading: false,
      listStore: [],
    }
  },
  filters: {},
  watch: {},
  activated() {
    this.getList()
  },
  created() {
    new Promise((resolve, reject) => {
      Promise.all([
        enums.getGoodsStatusList().then((res) => {
          this.GoodsStatusList = res
        }),
        enums.getHomeRecommendTypeList().then((res) => {
          this.HomeRecommendTypeList = res
        }),
        enums.getDeductStockTypeList().then((res) => {
          this.DeductStockTypeList = res
        }),
      ]).then(() => {
        this.getList()
        this.getTbHeight()
        this.addRoutes()
        this.getListStore()
      })
    })
  },
  methods: {
    addRoutes() {
      var addRouter = [
        {
          path: '/goods',
          component: Layout,
          redirect: 'noredirect',
          name: 'goods',
          meta: {
            title: '商品',
            icon: 'eye',
          },
          children: [
            {
              path: 'edit',
              component: () => import('@/views/goods/edit'),
              name: 'goodsEdit',
              hidden: true,
              meta: {
                notauth: true,
                title: '新增/编辑商品',
                noCache: true,
                icon: 'list',
              },
            },
          ],
        },
      ]
      this.$router.addRoutes(addRouter)
    },
    //获取可选商户
    getListStore() { },
    //商品状态
    clickStatus() {
      var searchStatus = this.listStatus.filter((p) => p.text == this.searchStatusText).value
      this.listQuery.status = searchStatus
      if (this.searchStatus = 80)//已售罄
      {
        this.listQuery.status = 0
        this.listQuery.stockTotal = 0
      }
      this.getList()
    },

    getTbHeight() {
      this.autoTbHeight = parseInt(window.innerHeight) - 240 + 'px'
    },
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    setStatus(row) {
      const data = {
        id: row.id,
        status: row.status,
      }
      goods.setStatus(data).then((res) => {
        if (res.code === 200) {
          this.getList()
          this.$message.success('修改成功！')
        } else {
          this.$message.error(res.message)
        }
      })
    },
    setRecommend(row) {
      const data = {
        id: row.id,
        isRecommend: row.isRecommend,
      }
      goods.setRecommend(data).then((res) => {
        if (res.code === 200) {
          this.getList()
          this.$message.success('修改成功！')
        } else {
          this.$message.error(res.message)
        }
      })
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    resetTemp() {
      this.temp = { id: '', listurlImageMain: [] }
    },
    handleCreate() {
      // 弹出添加框
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    handleUpdate(id) {
      this.$router.push({ name: 'goodsEdit', params: { id: id } })
    },
    handleDetails(row) {
      var id = ''
      if (row != null) {
        id = row.id
      }
      this.$router.push({ name: 'goodsDetails', params: { id: id } })
    },
    openChangePrice(row) {
      this.listSku = []
      this.dialogFormVisible = true
      const params = { goodsId: row.id }
      apiSku.listByWhere(params).then((res) => {
        this.listSku = res.result
      })
    },

    savePriceData() {
      // 更新提交
      apiSku.changePrice({ list: this.listSku }).then(() => {
        this.getList()
        this.dialogFormVisible = false
        this.$notify({
          title: '成功',
          message: '保存成功',
          type: 'success',
          duration: 2000,
        })
      })
    },
    updateData() {
      // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          goods.addOrUpdate(tempData).then((res) => {
            if (res.code == 200) {
              this.temp.id = res.result
              this.dialogFormVisible = false
              this.$notify({
                title: '成功',
                message: '更新成功',
                type: 'success',
                duration: 2000,
              })
              this.getList()
            }
          })
        }
      })
    },
    handleClick() {
      this.listQuery.page = 1
      this.getList()
    },
    getList() {
      this.listLoading = true
      goods.load(this.listQuery).then((res) => {
        this.list = res.result
        this.total = res.count
        this.listLoading = false
      })
    },
    /**
     * 修改商品上架状态
     */
    changeStatus(data) {
      // return false;
      const params = {
        id: data.id,
        status: data.status,
      }
      goods.setStatus(params).then((res) => {
        if (res.code === 200) {
          this.getList()
          this.$message.success('修改成功！')
        } else {
          this.$message.error(res.message)
        }
      })
    },

    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    handleSizeChange(val) {
      this.listQuery.limit = val
      this.getList()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val.page
      this.listQuery.limit = val.limit
      this.getList()
    },
    handleDelete() {
      var rows = this.multipleSelection;
      if (rows.length < 1) {
        this.$message({
          message: '至少删除一个',
          type: 'error',
        })
        return
      }
      this.$confirm(`此操作将删除商品，请确认是否继续?`, `提示`, {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        beforeClose: (action, instance, done) => {
          if (action === 'confirm') {
            // 多行删除
            goods.del(rows.map((u) => u.id)).then((result) => {
              this.$message.success(result.message, 1.5)
              rows.forEach((row) => {
                const index = this.list.indexOf(row)
                this.list.splice(index, 1)
              })
            })
            done()
          } else {
            done()
          }
        },
      })
    },
  },
}
</script>
<style  scoped>
.dialog-mini .el-select {
  width: 100%;
}
.app-container {
  padding: 0 10px 10px 10px;
}
.dvLeftFloat {
  float: left;
  margin-left: 10px;
}
.rowOrderMin {
  top: 8px;
  margin-left: 10px;
}
.goodsImg {
  float: left;
  width: 58px;
  height: 58px;
  border-radius: 10px;
}
.goodsName {
  float: left;
  margin-left: 10px;
  color: #2656bd;
  font-style: 12px;
}
.c-p {
  color: #2656bd;
}
.c-s {
  color: #409167;
}
.c-a {
  margin: 5px;
}
.goodsNo {
  margin-left: 10px;
  color: #666666;
  font-weight: 400;
  font-size: 12px;
}
.divlabel span {
  border: 0.5px solid #ff0036;
  background-color: #ffeded;
  border-radius: 5px;
  padding: 0 2px;
}
</style>
