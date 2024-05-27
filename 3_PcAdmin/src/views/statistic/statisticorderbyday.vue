<template>
  <!-- 订单日汇总 -->
  <div>

    <sticky :className="'sub-navbar'">

      <div class="filter-container">

        <el-date-picker size="mini" style="width: 200px;" value-format="yyyy-MM-dd" v-model="listQuery.dateTime" type="daterange" align="right" unlink-panels range-separator="-" start-placeholder="开始日期" end-placeholder="结束日期"
          :picker-options="pickerOptions">
        </el-date-picker>

        <!-- <el-input @keyup.enter.native="handleFilter" size="mini" style="width: 200px;" class="filter-item" :placeholder="'名称'" v-model="listQuery.key">
                </el-input> -->

        <el-button class="filter-item" size="mini" v-waves icon="el-icon-search" @click="handleFilter">搜索</el-button>
      </div>
    </sticky>
    <div class="app-container">
      <div class="bg-white">
        <el-table ref="mainTable" :key="tableKey" :data="list" v-loading="listLoading" border fit highlight-current-row style="width: 100%" :height="autoTbHeight" @row-click="rowClick" @selection-change="handleSelectionChange">
          <el-table-column type="selection" align="center" width="55"></el-table-column>
          <el-table-column label="日期" show-overflow-tooltip min-width="200">
            <template slot-scope="item">
              {{conversionTime(item.row.createTime)}}
            </template>
          </el-table-column>
          <el-table-column align="center" prop="orderCount" label="订单数" show-overflow-tooltip min-width="170"></el-table-column>
          <el-table-column align="center" prop="totalPayPrice" label="交易额" show-overflow-tooltip min-width="200"></el-table-column>
          <el-table-column align="center" label="操作" show-overflow-tooltip>
            <template slot-scope="scope">
              <el-button type="text" class="c-p" @click="openOrderList(scope.row)">明细</el-button>
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
import { listToTreeSelect } from '@/utils'
import * as api from '@/api/statistic'
import moment from 'moment'
import imgSelect from '@/utils/imgSelect'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
export default {
  name: 'StatisticOrderByDay',
  components: { Sticky, Pagination },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      autoTbHeight: '500px',
      pickerOptions: {
        shortcuts: [
          {
            text: '最近一周',
            onClick(picker) {
              const end = new Date()
              const start = new Date()
              start.setTime(
                start.getTime() - 3600 * 1000 * 24 * 7
              )
              picker.$emit('pick', [start, end])
            },
          },
          {
            text: '最近一个月',
            onClick(picker) {
              const end = new Date()
              const start = new Date()
              start.setTime(
                start.getTime() - 3600 * 1000 * 24 * 30
              )
              picker.$emit('pick', [start, end])
            },
          },
          {
            text: '最近三个月',
            onClick(picker) {
              const end = new Date()
              const start = new Date()
              start.setTime(
                start.getTime() - 3600 * 1000 * 24 * 90
              )
              picker.$emit('pick', [start, end])
            },
          },
        ],
      },
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        // 查询条件
        page: 1,
        limit: 10,
        key: undefined,
        appId: undefined,
        status: '10',
        dateTime: [
          moment(
            moment()
              .month(moment().month())
              .startOf('month')
              .valueOf()
          ).format('YYYY-MM-DD'),
          moment(new Date()).format('YYYY-MM-DD'),
        ],
        startDate: undefined,
        endDate: undefined,
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
      cateTree: [], // 商品分类
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
    }
  },
  filters: {},
  watch: {
    'temp.goodsName'() {
      console.log(this.temp.goodsName.split('#')[1])
      this.temp.sortNo = this.temp.goodsName.split('#')[1]
    },
  },
  created() {
    this.getList()
    this.getTbHeight()
    this.addRoutes()
  },
  methods: {
    addRoutes() {
      var addRouter = [
        {
          path: '/statistic',
          component: Layout,
          redirect: 'noredirect',
          name: 'statistic',
          meta: {
            title: 'statistic',
            icon: 'eye',
          },
        
        },
      ]
      this.$router.addRoutes(addRouter)
    },
    searchStatus(status) {
      this.listLoading = true
      this.listQuery.status = status
      this.getList()
    },
    getTbHeight() {
      this.autoTbHeight = parseInt(window.innerHeight) - 240 + 'px'
    },
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    onBtnClicked: function (btnCode) {

      switch (btnCode) {
        default:
          break
      }
    },
    resetTemp() {
      this.temp = { id: '', listurlImageMain: [] }
    },
    conversionTime(time) {
      const nowDate = new Date(time)
      const date = {
        year: nowDate.getFullYear(),
        month: nowDate.getMonth() + 1,
        date: nowDate.getDate(),
      }
      const newmonth = date.month >= 10 ? date.month : '0' + date.month
      const day = date.date >= 10 ? date.date : '0' + date.date
      return date.year + '-' + newmonth + '-' + day
    },
    updateData() {
      // 更新提交
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
        }
      })
    },
    handleClick() {
      this.listQuery.page = 1
      this.getList()
    },
    getList() {
      if (this.listQuery.dateTime != undefined) {
        this.listQuery.startDate = moment(
          new Date(this.listQuery.dateTime[0])
        )
          .startOf('day')
          .format('YYYY-MM-DD HH:mm:ss')
        this.listQuery.endDate = moment(
          new Date(this.listQuery.dateTime[1])
        )
          .endOf('day')
          .format('YYYY-MM-DD HH:mm:ss')
      } else {
        this.listQuery.startDate = undefined
        this.listQuery.endDate = undefined
      }
      this.listLoading = true
      api.loadOrderByDay(this.listQuery).then((res) => {
        this.list = res.result
        this.total = res.count
        this.listLoading = false
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
    //转到明细
    openOrderList(row) {
      const start = moment(new Date(row.createTime))
        .startOf('day')
        .format('YYYY-MM-DD HH:mm:ss')
      const end = moment(new Date(row.createTime))
        .endOf('day')
        .format('YYYY-MM-DD HH:mm:ss')
      this.$router.push({
        name: 'statisticOrderList',
        query: { start: start, end: end },
      })
    },
  },
}
</script>
<style  scoped>
</style>
