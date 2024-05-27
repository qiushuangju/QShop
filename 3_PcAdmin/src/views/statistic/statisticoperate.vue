<template>
  <!-- 运营管理-代理商经销商统计 -->
  <div class="flex-column">
    <sticky :className="'sub-navbar'">
      <div class="filter-container">
        <!-- <el-input @keyup.enter.native="handleQuery" size="mini" prefix-icon="el-icon-search" style="width: 200px;margin-bottom: 0;" class="filter-item" :placeholder="'关键字'"
                    v-model="listQuery.key">
                </el-input> -->

        <el-date-picker size="mini" style="width: 200px;" value-format="yyyy-MM-dd" v-model="inDate" type="daterange" align="right" unlink-panels range-separator="-" start-placeholder="开始日期" end-placeholder="结束日期"
          :picker-options="pickerOptions">
        </el-date-picker>
        <el-button class="filter-item" size="mini" v-waves icon="el-icon-search" @click="handleQuery">搜索</el-button>

      </div>
    </sticky>
    <div class="app-container flex-item">
      <!-- data数据 必须是树形结构 -->
      <!-- columns 设置属性 -->
      <!-- border边框 -->
      <!-- show-row-hover="false" 高亮显示为false -->
      <!-- selection-type="false 有>箭头，可以展开 -->
      <!-- expand-type="false 有复选框 可以选中 -->
      <tree-table class="tb-cate" index-text="序号" show-index stripe border :data="listDataTree" :columns="columns" :expand-type="false" :selection-type="false">

        <template slot="userType" slot-scope="scope">
          <span class="goodsName">{{ scope.row.name }}</span><br />
          <el-tag type="primary" effect="plain" size="mini" v-if="scope.row.userType == 100">
            用户
          </el-tag>
          <el-tag type="warning" effect="plain" size="mini" v-else-if="scope.row.userType == 110">
            经销商
          </el-tag>
          <el-tag type="danger" effect="plain" size="mini" v-else-if="scope.row.userType == 120">
            合伙人
          </el-tag>
        </template>

        <template slot="sourceCount" slot-scope="scope">
          <span class="goodsName">下级人数: {{ scope.row.sonCount }}</span><br />
          <span class="goodsName">团队人数: {{ scope.row.teamCount }}</span><br />
        </template>
        <template slot="recommendCount" slot-scope="scope">
          <!-- <span class="goodsName">推荐代理: {{ scope.row.recommendAgent }}</span><br /> -->
          <span class="goodsName">推荐经销商: {{ scope.row.recommendDealers }}</span><br />
          <span class="goodsName">推荐会员: {{ scope.row.recommendCustomer }}</span><br />
        </template>

        <template slot="teamOrderInfo" slot-scope="scope">
          <span class="goodsName">购物金额: {{ scope.row.teamGoodsPrice }}</span><br />
        </template>

        <!-- 操作 -->
        <template slot="opt" slot-scope="scope">
          <el-button type="primary" icon="el-icon-edit" size="mini" @click="openedit(scope.row)">
            编辑
          </el-button>
          <el-button type="danger" icon="el-icon-delete" size="mini" @click="opendel(scope.row)">
            删除
          </el-button>
        </template>
      </tree-table>

    </div>
  </div>

</template>

<script>
import TreeTable from 'vue-table-with-tree-grid'
// import treeTable from "@/components/TreeTable";

import moment from 'moment'
import { listToTreeSelect } from '@/utils'

import extend from '@/extensions/delRows.js'
import * as modules from '@/api/modules'
import * as statistic from '@/api/statistic'
import * as login from '@/api/login'
import enums from '@/api/enumList'
import waves from '@/directive/waves' // 水波纹指令
import Sticky from '@/components/Sticky'
import elDragDialog from '@/directive/el-dragDialog'
import iconData from '@/assets/public/css/comIconfont/iconfont.json'
import qs from 'qs'
export default {
  name: 'module',
  components: {
    Sticky,
    // Treeselect,
    // treeTable,
    TreeTable,
  },
  mixins: [extend],
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      iconData: iconData,
      inDate: [], //
      classBtns: [
        {
          value: 'primary',
        },
        {
          value: 'danger',
        },
        {
          value: 'success',
        },
        {
          value: 'info',
        },
        {
          value: 'warning',
        },
      ], // 按钮样式
      normalizer(node) {
        // treeselect定义字段
        return {
          label: node.name,
          id: node.id,
          children: node.children,
        }
      },
      baseURL: process.env.VUE_APP_BASE_API,
      columns: [
        {
          label: '手机号',
          prop: 'phone',
          width: 180,
        },

        {
          label: '用户类型',
          type: 'template',
          template: 'userType',
          width: 180,
        },
        {
          label: '归属人数',
          type: 'template',
          template: 'sourceCount',
        },
        {
          label: '本期推荐人数',
          type: 'template',
          template: 'recommendCount',
        },
        // {
        //     label: '下级下单',
        //     type: 'template',
        //     template: 'sonOrderInfo',
        // },
        {
          label: '团队下单',
          type: 'template',
          template: 'teamOrderInfo',
        },
        // {
        //     label: '下级结单',
        //     type: 'template',
        //     template: 'sonDoneOrderInfo',
        // },
        // {
        //     label: '团队结单',
        //     type: 'template',
        //     template: 'teamDoneOrderInfo',
        // },
      ],

      selectMenus: [], // 菜单列表选中的值
      tableKey: 0,
      list: [], // 菜单列表
      total: 0,
      currentModule: null, // 左边模块treetable当前选中的项
      listQuery: {
        // 查询条件
        page: 1,
        limit: 20,
        key: undefined,
      },

      showDescription: false,
      listData: [], // 用户可访问到的模块列表
      listDataTree: [], // 用户可访问到的所有模块组成的树
      temp: {
        // 模块临时值
        id: undefined,
        cascadeId: '',
        url: '',
        code: '',
        sortNo: 0,
        iconName: '',
        parentId: null,
        name: '',
        status: 0,
        isSys: false,
      },

      lastParentId: '', //最后一次选中的上级模块Id
      dialogFormVisible: false, // 模块编辑框
      dialogStatus: '',
      dialogMenuVisible: false, // 菜单编辑框
      dialogMenuStatus: '',

      chkRoot: false, // 根节点是否选中
      treeDisabled: false, // 树选择框时候可用
      textMap: {
        update: '编辑',
        create: '添加',
      },
      rules: {
        name: [
          {
            required: true,
            message: '名称不能为空',
            trigger: 'blur',
          },
        ],
        buttonType: [
          {
            required: true,
            message: '按钮类型不能为空',
            trigger: 'blur',
          },
        ],
      },
      downloadLoading: false,
    }
  },
  computed: {
    isRoot: {
      get() {
        return this.chkRoot
      },
      set(v) {
        this.chkRoot = v
        if (v) {
          this.temp.parentName = '根节点'
          this.temp.parentId = null
        }
      },
    },
    currentPageMenus: {
      get() {
        var start = (this.listQuery.page - 1) * this.listQuery.limit
        var end = this.listQuery.page * this.listQuery.limit
        var result = this.list.slice(start, end)
        return result
      },
    },
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        0: 'info',
        1: 'danger',
      }
      return statusMap[status]
    },
  },
  created() {
    this.initPicker()
  },
  mounted() {
    this.getListTree()
  },
  methods: {
    initPicker() {
      let day = moment().startOf('month')
      const start = day.startOf('month').format('YYYY-MM-DD')
      const end = day.endOf('month').format('YYYY-MM-DD')

      this.inDate = [start, end]
      var _that = this
      this.pickerOptions = {
        disabledDate(time) {
          return time.getTime() < Date.now()
        },
        shortcuts: [
          {
            text: '本月',
            onClick(picker) {
              picker.$emit('pick', [start, end])
            },
          },
          {
            text: '上月',
            onClick(picker) {
              let day = moment()
                .startOf('month')
                .subtract(1, 'month')
              const start = day
                .startOf('month')
                .format('YYYY-MM-DD')
              const end = day.endOf('month').format('YYYY-MM-DD')
              picker.$emit('pick', [start, end])
            },
          },
          {
            text: '上上月',
            onClick(picker) {
              let day = moment()
                .startOf('month')
                .subtract(2, 'month')
              const start = day
                .startOf('month')
                .format('YYYY-MM-DD')
              const end = day.endOf('month').format('YYYY-MM-DD')
              picker.$emit('pick', [start, end])
            },
          },
        ],
      }
    },
    handleQuery() {
      this.getListTree()
    },
    onBtnClicked: function (btnCode) {

      switch (btnCode) {
        case 'btnExport':
          this.exportExcel()
          break
      }
    },
    getListTree() {
      const loading = this.$loading({
        lock: true,
        text: 'Loading',
        background: 'rgba(0, 0, 0, 0.4)',
      })

      var _this = this // 记录vuecomponent
      statistic
        .StatisticOperate({
          key: this.listQuery.key,
          startDate: this.inDate[0],
          endDate: this.inDate[1],
        })
        .then((res) => {
          loading.close()
          _this.listData = res.result.map(function (item) {
            return {
              id: item.userId,
              name: item.name,
              phone: item.phone,
              userType: item.userType,
              parentId: item.parentId || null,

              sonCount: item.sonCount,
              teamCount: item.teamCount,

              recommendAgent: item.recommendAgent,
              recommendAgentDealer: item.recommendAgentDealer,
              recommendVip: item.recommendVip,

              sonGoodsPrice: item.sonGoodsPrice,
              sonTotalFreightPrice: item.sonTotalFreightPrice,
              sonOrderPrice: item.sonOrderPrice,

              sonDoneGoodsPrice: item.sonDoneGoodsPrice,
              sonDoneTotalFreightPrice:
                item.sonDoneTotalFreightPrice,
              sonDoneOrderPrice: item.sonDoneOrderPrice,

              teamGoodsPrice: item.teamGoodsPrice,
              teamTotalFreightPrice: item.teamTotalFreightPrice,
              teamOrderPrice: item.teamOrderPrice,

              teamDoneGoodsPrice: item.teamDoneGoodsPrice,
              teamDoneTotalFreightPrice:
                item.teamDoneTotalFreightPrice,
              teamDoneOrderPrice: item.teamDoneOrderPrice,
            }
          })
          var listTmp = JSON.parse(JSON.stringify(_this.listData))
          _this.listDataTree = listToTreeSelect(listTmp)
        })
    },
    exportExcel() {
      var strParams = ''
      strParams = qs.stringify(this.listQuery, {
        arrayFormat: 'repeat',
      })
      window.open(`${this.baseURL}/Statistic/ExportOperate?` + strParams)
    },
    handleFilter() {
      //回车查询
      this.listQuery.page = 1
      // this.getList();
    },
    handleSizeChange(val) {
      this.listQuery.limit = val
      // this.getList();
    },
    handleCurrentChange(val) {
      this.listQuery.page = val.page
      this.listQuery.limit = val.limit
      // this.getList();
    },
  },
}
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