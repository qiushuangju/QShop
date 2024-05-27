<template>
  <a-card :bordered="false">
    <!-- <div class="card-title">{{ $route.meta.title }}</div> -->
    <a-spin :spinning="isLoading">
      <a-form :selfUpdate="true">
        <a-tabs :activeKey="tabKey" :tabBarStyle="{marginBottom: '30px'}" @change="handleTabs">
          <a-tab-pane :key="0" tab="日汇总"></a-tab-pane>
          <a-tab-pane :key="1" tab="月汇总"></a-tab-pane>
          <a-tab-pane :key="2" tab="年汇总"></a-tab-pane>
        </a-tabs>
        <div class="tabs-content">
          <!-- 日汇总 -->
          <div class="tab-pane" v-show="tabKey == 0">
            <sticky :className="'sub-navbar'">
              <div class="filter-container">
                <el-date-picker size="mini" style="width: 200px;" value-format="yyyy-MM-dd" v-model="listQueryByDay.dateTime" type="daterange" align="right" unlink-panels range-separator="-" start-placeholder="开始日期" end-placeholder="结束日期"
                  :picker-options="pickerOptionsByDay">
                </el-date-picker>
                <el-button class="filter-item" size="mini" v-waves icon="el-icon-search" @click="handleFilterByDay">搜索</el-button>
              </div>
            </sticky>
            <div class="app-container">
              <div class="bg-white">
                <el-table ref="mainTable" :key="tableKey" :data="listByDay" v-loading="listLoading" border fit style="width: 100%" :height="autoTbHeight">
                  <!-- <el-table-column type="selection" align="center" width="55"></el-table-column> -->
                  <el-table-column label="日期" show-overflow-tooltip min-width="200">
                    <template slot-scope="item">
                      {{conversionTime(item.row.createTime)}}
                    </template>
                  </el-table-column>
                  <el-table-column align="center" prop="orderCount" label="订单数" show-overflow-tooltip min-width="170"></el-table-column>
                  <el-table-column align="center" prop="totalPayPrice" label="交易额" show-overflow-tooltip min-width="200"></el-table-column>
                  <el-table-column align="center" prop="totalRefundPrice" label="退款总额" show-overflow-tooltip min-width="200"></el-table-column>
                  <!-- <el-table-column align="center"  label="操作" show-overflow-tooltip>
                                        <template slot-scope="scope">
                                            <el-button type="text" class="c-p" @click="openOrderList(scope.row)">明细</el-button>
                                        </template>
                                    </el-table-column> -->
                </el-table>
                <pagination v-show="totalByDay > 0" :total="totalByDay" :page.sync="listQueryByDay.page" :limit.sync="listQueryByDay.limit" @pagination="handleCurrentChangeByDay" />
              </div>
            </div>
          </div>
          <!-- 月汇总 -->
          <div class="tab-pane" v-show="tabKey == 1">
            <sticky :className="'sub-navbar'" :height="'42'">
              <div class="filter-container">
                <el-date-picker size="mini" v-model="listQueryByMonth.dateTime" type="monthrange" style="width: 200px;" format="yyyy-MM" value-format="yyyy-MM" align="right" unlink-panels range-separator="-" start-placeholder="开始月份"
                  end-placeholder="结束月份" :picker-options="pickerOptionsByMonth">
                </el-date-picker>
                <el-button class="filter-item" size="mini" v-waves icon="el-icon-search" @click="handleFilterByMonth">搜索</el-button>
              </div>
            </sticky>
            <div class="app-container">
              <div class="bg-white">
                <el-table ref="mainTable" :key="tableKey" :data="listByMonth" v-loading="listLoading" border fit style="width: 100%" :height="autoTbHeight">
                  <!-- <el-table-column type="selection" align="center" width="55"></el-table-column> -->
                  <el-table-column prop="createTime" label="日期" show-overflow-tooltip min-width="200">
                  </el-table-column>
                  <el-table-column align="center" prop="orderCount" label="订单数" show-overflow-tooltip min-width="170"></el-table-column>
                  <el-table-column align="center" prop="totalPayPrice" label="交易额" show-overflow-tooltip min-width="200"></el-table-column>
                  <el-table-column align="center" prop="totalRefundPrice" label="退款总额" show-overflow-tooltip min-width="200"></el-table-column>
                  <!-- <el-table-column align="center"  label="操作" show-overflow-tooltip>
                                        <template slot-scope="scope">
                                            <el-button type="text" class="c-p" @click="openOrderList(scope.row)">明细</el-button>
                                        </template>
                                    </el-table-column> -->
                </el-table>
                <pagination v-show="totalByMonth > 0" :total="totalByMonth" :page.sync="listQueryByMonth.page" :limit.sync="listQueryByMonth.limit" @pagination="handleCurrentChangeByMonth" />
              </div>

            </div>
          </div>
          <!-- 年汇总 -->
          <div class="tab-pane" v-show="tabKey == 2">
            <sticky :className="'sub-navbar'">
              <div class="filter-container">
                <el-date-picker size="mini" v-model="listQueryByYear.dateTime" type="monthrange" format="yyyy-MM" value-format="yyyy-MM" align="right" unlink-panels range-separator="-" start-placeholder="开始年份" end-placeholder="结束年份"
                  :picker-options="pickerOptionsByYear">
                </el-date-picker>
                <el-button class="filter-item" size="mini" v-waves icon="el-icon-search" @click="handleFilterByYear">搜索</el-button>
              </div>
            </sticky>
            <div class="app-container">
              <div class="bg-white">
                <el-table ref="mainTable" :key="tableKey" :data="listByYear" v-loading="listLoading" border fit style="width: 100%" :height="autoTbHeight">
                  <!-- <el-table-column type="selection" align="center" width="55"></el-table-column> -->
                  <el-table-column prop="createTime" label="年份" show-overflow-tooltip min-width="200">
                  </el-table-column>
                  <el-table-column align="center" prop="orderCount" label="订单数" show-overflow-tooltip min-width="170"></el-table-column>
                  <el-table-column align="center" prop="totalPayPrice" label="交易额" show-overflow-tooltip min-width="200"></el-table-column>
                  <el-table-column align="center" prop="totalRefundPrice" label="退款总额" show-overflow-tooltip min-width="200"></el-table-column>
                  <!-- <el-table-column align="center"  label="操作" show-overflow-tooltip>
                                        <template slot-scope="scope">
                                            <el-button type="text" class="c-p" @click="openAfterSalesList(scope.row)">明细</el-button>
                                        </template>
                                    </el-table-column> -->
                </el-table>
                <pagination v-show="totalByYear > 0" :total="totalByYear" :page.sync="listQueryByYear.page" :limit.sync="listQueryByYear.limit" @pagination="handleCurrentChangeByYear" />
              </div>
            </div>
          </div>
        </div>
      </a-form>
    </a-spin>
  </a-card>
</template>

<script>
import waves from "@/directive/waves"; // 水波纹指令
import Layout from '@/views/layout/Layout'
import moment from 'moment'
import * as api from "@/api/statistic";
import Pagination from "@/components/Pagination";
import Sticky from "@/components/Sticky";
import { SelectImage, SelectVideo } from '@/components'
import goodsIndex from '@/api/goods/Index'
import * as comm from '@/api/commApi'
import { quillEditor } from 'vue-quill-editor'
import { isEmptyObject } from '@/utils/util'
import FileTypeEnum from '@/common/enum/file/FileType'
import elDragDialog from "@/directive/el-dragDialog";
import '@riophae/vue-treeselect/dist/vue-treeselect.css'

export default {
  components: { Sticky, Pagination },
  components: {
    Sticky,
    Pagination
  },
  directives: {
    waves,
    elDragDialog,
  },
  data() {
    return {
      tabKey: 0,
      // loading状态
      isLoading: false,
      autoTbHeight: "500px",
      //日汇总日期选择
      pickerOptionsByDay: {
        shortcuts: [
          {
            text: "上周",
            onClick(picker) {

              const end = moment().subtract(7, "days").day("Monday").subtract(-6, "days").format('YYYY-MM-DD 23:59:59');
              const start = moment().subtract(7, "days").day("Monday").format('YYYY-MM-DD 00:00:00');
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "本周",
            onClick(picker) {
              const end = moment().endOf('day').format('YYYY-MM-DD HH:mm:ss');
              const start = moment().day("Monday").format('YYYY-MM-DD 00:00:00');
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "上月",
            onClick(picker) {
              let day = moment().startOf('month').subtract(1, "days")
              const end = day.endOf('month').format('YYYY-MM-DD HH:mm:ss');
              const start = day.startOf('month').format('YYYY-MM-DD HH:mm:ss');
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "本月",
            onClick(picker) {
              let day = moment().startOf('month')
              const end = day.endOf('month').format('YYYY-MM-DD HH:mm:ss');
              const start = day.startOf('month').format('YYYY-MM-DD HH:mm:ss');
              picker.$emit("pick", [start, end]);
            },
          },
          // {
          //     text: "最近三个月",
          //     onClick(picker) {
          //         const end = new Date();
          //         const start = new Date();
          //         start.setTime(
          //             start.getTime() - 3600 * 1000 * 24 * 90
          //         );
          //         picker.$emit("pick", [start, end]);
          //     },
          // },
        ],
      },
      //月汇总日期选择
      pickerOptionsByMonth: {
        shortcuts: [
          {
            text: '上月',
            onClick(picker) {
              let month = moment().startOf('month').subtract(1, "month")
              const end = month.endOf('month').format('YYYY-MM-DD HH:mm:ss');
              const start = month.startOf('month').format('YYYY-MM-DD HH:mm:ss');
              picker.$emit('pick', [start, end]);
            }
          },
          {
            text: '本月',
            onClick(picker) {
              const end = moment().endOf('month').format('YYYY-MM-DD HH:mm:ss')
              const start = moment().startOf('month').format('YYYY-MM-DD HH:mm:ss')
              picker.$emit('pick', [start, end]);
            }
          },
          {
            text: '今年至今',
            onClick(picker) {
              const end = new Date();
              const start = new Date(new Date().getFullYear(), 0);
              picker.$emit('pick', [start, end]);
            }
          },
          {
            text: '最近六个月',
            onClick(picker) {
              const end = moment().endOf('month').format('YYYY-MM-DD HH:mm:ss');
              const start = moment().startOf('month').format('YYYY-MM-DD HH:mm:ss');
              start.setMonth(start.getMonth() - 6);
              picker.$emit('pick', [start, end]);
            }
          }]
      },
      //年汇总日期选择
      pickerOptionsByYear: {
        shortcuts: [
          {
            text: '上一年',
            onClick(picker) {
              let year = moment().startOf('year').subtract(1, "year")
              const end = year.endOf('year').format('YYYY-MM-DD HH:mm:ss');
              const start = year.startOf('year').format('YYYY-MM-DD HH:mm:ss');
              picker.$emit('pick', [start, end]);
            }
          },
          {
            text: '今年至今',
            onClick(picker) {
              const end = new Date();
              const start = new Date(new Date().getFullYear(), 0);
              picker.$emit('pick', [start, end]);
            }
          }]
      },
      multipleSelection: [], // 列表checkbox选中的值
      tableKey: 0,
      listByDay: [],//日汇总
      listByMonth: [],//月汇总
      listByYear: [],//年汇总
      totalByDay: 0,//日汇总总数
      totalByMonth: 0,//月汇总总数
      totalByYear: 0,//年汇总总数
      listLoading: true,
      //日汇总查询条件
      listQueryByDay: {
        // 查询条件
        page: 1,
        limit: 10,
        key: undefined,
        appId: undefined,
        status: "10",
        dateTime: [moment(moment().month(moment().month()).startOf('month').valueOf()).format('YYYY-MM-DD'), moment(new Date()).format('YYYY-MM-DD')],
        startDate: undefined,
        endDate: undefined,
      },
      //月汇总查询条件
      listQueryByMonth: {
        // 查询条件
        page: 1,
        limit: 10,
        key: undefined,
        appId: undefined,
        status: "10",
        dateTime: [moment(new Date(new Date().getFullYear(), 0)).format('YYYY-MM'), moment(new Date()).format('YYYY-MM')],
        startDate: undefined,
        endDate: undefined,
      },
      //年汇总查询条件
      listQueryByYear: {
        // 查询条件
        page: 1,
        limit: 10,
        key: undefined,
        appId: undefined,
        status: "10",
        dateTime: [moment(new Date(new Date().getFullYear(), 0)).format('YYYY-MM'), moment(new Date()).format('YYYY-MM')],
        startDate: undefined,
        endDate: undefined,
      }
    };
  },
  created() {
    this.getListByDay();
    this.getTbHeight();
    this.addRoutes()
  },
  methods: {
    // 切换tab选项卡
    handleTabs(key) {
      this.tabKey = key
      if (this.tabKey == 0 && this.listByDay.length == 0) {
        this.getListByDay()
      } else if (this.tabKey == 1 && this.listByMonth.length == 0) {
        this.getListByMonth()
      } else if (this.tabKey == 2 && this.listByYear.length == 0) {
        this.getListByYear()
      }
    },
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
    getTbHeight() {
      console.log("height", parseInt(window.outerHeight));
      this.autoTbHeight = parseInt(window.innerHeight) - 240 + "px";
    },
    onBtnClicked: function (btnCode) {
      console.log("you click:" + btnCode);
      switch (btnCode) {
        default:
          break;
      }
    },
    //日期转换，显示年月日
    conversionTime(time) {
      const nowDate = new Date(time)
      const date = {
        year: nowDate.getFullYear(),
        month: nowDate.getMonth() + 1,
        date: nowDate.getDate(),
      };
      const newmonth = date.month >= 10 ? date.month : "0" + date.month;
      const day = date.date >= 10 ? date.date : "0" + date.date;
      return date.year + "-" + newmonth + "-" + day;
    },
    //获取日汇总
    getListByDay() {
      if (this.listQueryByDay.dateTime != undefined) {
        this.listQueryByDay.startDate = moment(new Date(this.listQueryByDay.dateTime[0])).startOf('day').format("YYYY-MM-DD HH:mm:ss")
        this.listQueryByDay.endDate = moment(new Date(this.listQueryByDay.dateTime[1])).endOf('day').format("YYYY-MM-DD HH:mm:ss")
      }
      else {
        this.listQueryByDay.startDate = undefined
        this.listQueryByDay.endDate = undefined
      }
      this.listLoading = true;
      api.loadOrderByDay(this.listQueryByDay).then((res) => {
        this.listByDay = res.result;
        this.totalByDay = res.count;
        this.listLoading = false;
      });
    },
    //获取月汇总
    getListByMonth() {
      if (this.listQueryByMonth.dateTime != undefined) {
        this.listQueryByMonth.startDate = moment(new Date(this.listQueryByMonth.dateTime[0])).startOf('month').format("YYYY-MM-DD HH:mm:ss")
        this.listQueryByMonth.endDate = moment(new Date(this.listQueryByMonth.dateTime[1])).endOf('month').format("YYYY-MM-DD HH:mm:ss")
      }
      else {
        this.listQueryByMonth.startDate = undefined
        this.listQueryByMonth.endDate = undefined
      }
      this.listLoading = true;
      api.loadOrderByMonth(this.listQueryByMonth).then((res) => {
        this.listByMonth = res.result;
        this.totalByMonth = res.count;
        this.listLoading = false;
      });
    },
    //获取年汇总
    getListByYear() {
      if (this.listQueryByYear.dateTime != undefined) {
        this.listQueryByYear.startDate = moment(new Date(this.listQueryByYear.dateTime[0])).startOf('month').format("YYYY-MM-DD HH:mm:ss")
        this.listQueryByYear.endDate = moment(new Date(this.listQueryByYear.dateTime[1])).endOf('month').format("YYYY-MM-DD HH:mm:ss")
      }
      else {
        this.listQueryByYear.startDate = undefined
        this.listQueryByYear.endDate = undefined
      }
      this.listLoading = true;
      api.loadOrderByYear(this.listQueryByYear).then((res) => {
        this.listByYear = res.result;
        this.totalByYear = res.count;
        this.listLoading = false;
      });
    },
    //日汇总搜索
    handleFilterByDay() {
      this.listQueryByDay.page = 1;
      this.getListByDay();
    },
    handleSizeChangeByDay(val) {
      this.listQueryByDay.limit = val;
      this.getListByDay();
    },
    //日汇总更改当前页/每页显示条数
    handleCurrentChangeByDay(val) {
      this.listQueryByDay.page = val.page;
      this.listQueryByDay.limit = val.limit;
      this.getListByDay();
    },
    //月汇总搜索
    handleFilterByMonth() {
      this.listQueryByMonth.page = 1;
      this.getListByMonth();
    },
    handleSizeChangeByMonth(val) {
      this.listQueryByMonth.limit = val;
      this.getListByMonth();
    },
    //月汇总更改当前页/每页显示条数
    handleCurrentChangeByMonth(val) {
      this.listQueryByMonth.page = val.page;
      this.listQueryByMonth.limit = val.limit;
      this.getListByMonth();
    },
    //月汇总搜索
    handleFilterByYear() {
      this.listQueryByYear.page = 1;
      this.getListByYear();
    },
    handleSizeChangeByYear(val) {
      this.listQueryByYear.limit = val;
      this.getListByYear();
    },
    //年汇总更改当前页/每页显示条数
    handleCurrentChangeByYear(val) {
      this.listQueryByYear.page = val.page;
      this.listQueryByYear.limit = val.limit;
      this.getListByYear();
    },
    //转到明细
    openOrderList(row) {
      const start = moment(new Date(row.createTime)).startOf('day').format("YYYY-MM-DD HH:mm:ss")
      const end = moment(new Date(row.createTime)).endOf('day').format("YYYY-MM-DD HH:mm:ss")
      this.$router.push({ name: 'statisticOrderList', query: { start: start, end: end } })
    }
  },
}
</script>
<style lang="less" scoped>
</style>
