
<template>
  <a-card :bordered="false">
    <div class="card-title"></div>
    <a-spin :spinning="isLoading">
      <a-form>
        <div class="tabs-content">
          <!-- 打印区 -->
          <div id="printarea" class="printInfo">
            <div class="details">
              <div class="box">
                <el-row>
                  <!-- 头像 -->
                  <el-col :span="8">
                    <el-avatar :size="80" :src="temp.urlAvater"></el-avatar>
                  </el-col>
                  <el-col :span="8">昵称：{{temp.name}}</el-col>
                  <el-col :span="8">手机号：{{temp.phone}}</el-col>
                </el-row>
                <el-row>
                  <el-col :span="8">用户类型：
                    <el-tag type="primary" effect="plain" size="mini" v-if="temp.userType == 100">
                      用户
                    </el-tag>
                    <el-tag type="warning" effect="plain" size="mini" v-else-if="temp.userType == 110">
                      经销商
                    </el-tag>
                    <el-tag type="danger" effect="plain" size="mini" v-else-if="temp.userType == 120">
                      合伙人
                    </el-tag></el-col>
                  <el-col :span="8">状态：{{temp.strStatus}}</el-col>
                  <el-col :span="8">剩余积分：{{temp.balance}}</el-col>

                </el-row>
                <el-row>
                  <el-col :span="8">直接上级：<el-button type="text" @click="handleDetails(temp.parent)">{{temp.parent==null?"":temp.parent.name}}</el-button></el-col>
                  <el-col :span="8">间接上级：<el-button type="text" @click="handleDetails(temp.grandParent)">{{temp.grandParent==null?"":temp.grandParent.name}}</el-button></el-col>
                  <el-col :span="8">已消费金额：{{temp.expendMoney}}</el-col>
                </el-row>
              </div>
            </div>
            <div class="details">
              <div class="title">团队列表</div>
              <div class="box">
                <div class="filter-container">
                  <el-date-picker size="mini" style="width: 200px;" value-format="yyyy-MM-dd" v-model="inDate" type="daterange" align="right" unlink-panels range-separator="-" start-placeholder="开始日期" end-placeholder="结束日期"
                    :picker-options="pickerOptions">
                  </el-date-picker>
                  <el-button class="filter-item" size="mini" icon="el-icon-search" @click="getListTree">搜索</el-button>
                </div>
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
          </div>
        </div>
      </a-form>
    </a-spin>
  </a-card>
</template>
<script>
import _ from 'lodash'
import Layout from '@/views/layout/Layout'
import enums from '@/api/enumList'
import { debuglog, debug } from 'util'
import * as user from '@/api/user/memberUser'
import moment from 'moment'
import * as statistic from '@/api/statistic'
import TreeTable from 'vue-table-with-tree-grid'
import { listToTreeSelect } from '@/utils'

// import Pagination from '@/components/Pagination'

export default {
  components: {
    // Pagination,
    TreeTable,
  },
  data() {
    return {
      // loading状态
      isLoading: false,
      tabKey: 0,
      isBtnLoading: false,
      userId: null,
      labelCol: { span: 10 },
      wrapperCol: { span: 10 },
      listQuery: {
        // 查询条件
        page: 1,
        limit: 20,
        lastSourceUserId: undefined,
        minUserType: 15,
        maxUserType: 17,
      },
      inDate: [], //
      listDataTree: [],
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
        {
          label: '团队下单',
          type: 'template',
          template: 'teamOrderInfo',
        },
      ],

      temp: {
        urlAvater: '',
        name: '',
        phone: '',
        userType: '',
        strUserType: '',
        strStatus: '',
        status: '',
        vipEndDate: '',
        balance: '',
        points: '',
        payMoney: '',
        expendMoney: '',
        incomeMoney: '',
        parent: null,
        grandParent: null,
      },
      tableList: [], //下级列表
      total: 0,
    }
  },
  created() {
    this.userId = this.$route.params && this.$route.params.id
    this.listQuery.lastSourceUserId = this.userId
    if (this.userId != null && this.userId.trim() != '') {
      this.loadData()
      this.loadList()
    }
    this.addRoutes()
    this.initPicker()
  },
  methods: {
    //动态路由
    addRoutes() {
      var addRouter = [
        {
          path: '/memberuser',
          component: Layout,
          redirect: 'noredirect',
          name: 'memberuser',
          meta: {
            title: 'memberuser',
            icon: 'eye',
          },
          children: [
            {
              path: 'userdetails',
              component: () =>
                import('@/views/memberuser/userdetails'),
              name: 'userDetails',
              hidden: true,
              meta: {
                notauth: true,
                title: '用户详情',
                noCache: true,
                icon: 'list',
              },
            },
          ],
        },
      ]
      this.$router.addRoutes(addRouter)
    },
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
    //加载数据
    loadData() {
      this.isLoading = true
      user.get({ id: this.userId }).then((res) => {
        this.$nextTick(() => {
          this.temp = res.result
          this.isLoading = false
          this.getListTree()
        })
      })
    },
    loadList() {
      user.load(this.listQuery).then((res) => {
        this.tableList = res.result
        this.total = res.count
        this.listLoading = false
      })
    },
    load(row, treeNode, resolve) {
      setTimeout(() => {
        const query = {
          page: 1,
          limit: 10000,
          lastSourceUserId: row.id,
        }
        user.load(query).then((res) => {
          resolve(res.result)
        })
      }, 1000)
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
          parentUserId: this.userId,
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
    //
    handleCurrentChange(val) {
      this.listQuery.page = val.page
      this.listQuery.limit = val.limit
      this.loadList()
    },
    //用户详情
    handleDetails(row) {
      var id = row.id
      this.$router.push({
        path: '/memberuser/userdetails',
        params: { id: id },
      })
      // this.$router.push({ name: 'userDetails', params: { id: id } })
    },
  },
}
</script>

<style>
.box {
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
  padding: 10px;
}
.c-a {
  color: #0076c8;
}
.el-row {
  margin: 10px 0px;
}
.title {
  padding: 20px 0px;
}
.mt-20 {
  padding: 20px 0px;
}
.el-input__inner {
  height: 30px !important;
}
.el-image__inner {
  width: 50px;
  height: 50px;
}
.tb-cate {
  margin-top: 10px;
}
</style>