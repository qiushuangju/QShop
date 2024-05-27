<template>
  <a-card :bordered="false">
    <a-spin :spinning="isLoading">
      <a-form :selfUpdate="true">
        <div class="dashboard-editor-container" style="padding:5px">
          <!-- 订单信息 -->
          <el-card style="height: 100px; width: 100%" shadow="hover">
            <el-row class="panel-group" :gutter="50">
              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-panel">
                  <router-link :to="{ path: '/Order/Index' ,query: { strStatus: '待发货' }}">
                    <div class="card-panel-only-description">
                      <div class="card-panel-text">待发货</div>
                      <div class="card-panel-num">{{this.data.total.totalOrderWaitDeliver}}</div>
                    </div>
                  </router-link>

                </div>
              </el-col>
              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-panel">
                  <router-link :to="{ path: '/AfterSales/Index' }">
                    <div class="card-panel-only-description">
                      <div class="card-panel-text">售后单</div>
                      <div class="card-panel-num">{{this.data.total.totalOrdertWaitRefund}}</div>
                    </div>
                  </router-link>
                </div>
              </el-col>

              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-panel">
                  <router-link :to="{ path: '/Order/Index' ,query: { strStatus: '待付款' }}">
                    <div class="card-panel-only-description">
                      <div class="card-panel-text">待付款订单</div>
                      <div class="card-panel-num">{{this.data.total.totalOrdertWaitPay}}</div>
                    </div>
                  </router-link>
                </div>
              </el-col>

              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-panel">
                  <div class="card-panel-only-description">
                    <div class="card-panel-text">已售罄</div>
                    <div class="card-panel-num">{{this.data.total.totalWarningStock}}</div>
                  </div>
                </div>
              </el-col>

            </el-row>
          </el-card>

          <!-- 今日概况 -->
          <el-card v-if="this.data.cardShow.isShowCardProfile" style="height: 160px; width: 100%;margin-top:2px" shadow="hover">
            <div slot="header" class="clearfix">
              <span class="spTitle">今日概况</span>
            </div>
            <el-row class="panel-group" :gutter="50">
              <el-col :xs="10" :sm="10" :lg="6" class="card-panel-col">
                <div class="card-panel">
                  <div class="card-panel-round-wrapper icon-round">
                    <span class="icon iconfont icon-jine" style="font-size: 30px"></span>
                  </div>
                  <div class="card-panel-description">
                    <div class="card-panel-text">成交金额(元)</div>
                    <div class="card-panel-num">{{this.data.today.totalAmountGoods}}</div>
                  </div>
                </div>
              </el-col>

              <el-col :xs="10" :sm="10" :lg="6" class="card-panel-col">
                <div class="card-panel">
                  <div class="card-panel-round-wrapper icon-round">
                    <span class="icon iconfont icon-zoushitu" style="font-size: 30px"></span>
                  </div>
                  <div class="card-panel-description">
                    <div class="card-panel-text">支付订单数</div>
                    <div class="card-panel-num">{{this.data.today.totalOrder}}</div>
                  </div>
                </div>
              </el-col>

              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-panel">
                  <div class="card-panel-round-wrapper icon-round">
                    <span class="icon iconfont icon-zoushitu" style="font-size: 30px"></span>
                  </div>
                  <div class="card-panel-description">
                    <div class="card-panel-text">总用户</div>
                    <div class="card-panel-num">{{this.data.total.totalCustomer}}</div>
                  </div>
                </div>
              </el-col>

              <el-col :xs="10" :sm="10" :lg="6" class="card-panel-col">
                <div class="card-panel">
                  <div class="card-panel-round-wrapper icon-round">
                    <span class="icon iconfont icon-zoushitu" style="font-size: 30px"></span>
                  </div>
                  <div class="card-panel-description">
                    <div class="card-panel-text">新增用户</div>
                    <div class="card-panel-num">{{this.data.today.totalNewCustomer}}</div>
                  </div>
                </div>
              </el-col>

            </el-row>
            <el-row class="panel-group" :gutter="50">

            </el-row>
          </el-card>

          <!-- 常用功能 -->
          <el-card style="height: 180px; width: 100%; margin-top:2px" shadow="hover">
            <div slot="header" class="clearfix">
              <span class="spTitle">常用功能</span>
            </div>
            <el-row :gutter="50">
              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-const">
                  <div class="card-const-wrapper icon-const" style="background-color: rgb(0, 191, 87);">
                    <span class="icon iconfont icon-shangpin-copy-copy"></span>
                  </div>
                  <div class="card-const-description">
                    <router-link :to="{ path: '/Goods/Index' }">
                      <div class="card-panel-text">商品管理</div>
                    </router-link>

                  </div>
                </div>
              </el-col>

              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-const">
                  <div class="card-const-wrapper icon-const" style="background-color: rgb(19, 157, 255);">
                    <span class="icon iconfont icon-yonghu-copy"></span>
                  </div>
                  <div class="card-const-description">

                    <router-link :to="{ path: '/MemberUser/Index' }">
                      <div class="card-panel-text">会员管理</div>
                    </router-link>

                  </div>
                </div>
              </el-col>

              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-const">
                  <div class="card-const-wrapper icon-const" style="background-color: rgb(19, 157, 255);">
                    <span class="icon iconfont icon-dingdan-copy"></span>
                  </div>
                  <div class="card-const-description">
                    <router-link :to="{ path: '/Order/Index' }">
                      <div class="card-panel-text"> 订单管理</div>
                    </router-link>

                  </div>
                </div>
              </el-col>
              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-const">
                  <div class="card-const-wrapper icon-const" style="background-color:  rgb(241, 88, 88);">
                    <span class="icon iconfont icon-shouhou"></span>
                  </div>
                  <div class="card-const-description">
                    <router-link :to="{ path: '/aftersales/index' }">
                      <div class="card-panel-text">售后订单</div>
                    </router-link>
                  </div>
                </div>
              </el-col>

            </el-row>
            <el-row :gutter="50">
              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-const">
                  <div class="card-const-wrapper icon-const" style="background-color: rgb(19, 157, 255);">
                    <span class="icon iconfont icon-dingdan-copy"></span>
                  </div>
                  <div class="card-const-description">
                    <router-link :to="{ path: '/StorePage/Index' }">
                      <div class="card-panel-text">店铺装修</div>
                    </router-link>
                  </div>
                </div>
              </el-col>
              <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
                <div class="card-const">
                  <div class="card-const-wrapper icon-const" style="background-color: rgb(241, 88, 88);">
                    <span class="icon iconfont icon-youhuiquan1"></span>
                  </div>
                  <div class="card-const-description">

                    <router-link :to="{ path: '/marketmanage/coupon/Index' }">
                      <div class="card-panel-text">优惠券</div>
                    </router-link>

                  </div>
                </div>
              </el-col>
            </el-row>
          </el-card>

          <el-card style="height:520px; width: 100%; margin-top: 2px ;margin-bottom: 50px" shadow="hover">
            <div slot="header" class="clearfix">
              <span class="spTitle">近七日交易走势</span>
            </div>
            <div id="myChart" class="homeChart"></div>
          </el-card>
          <!-- </div> -->
        </div>
      </a-form>
    </a-spin>
  </a-card>

</template>

<script>
import Layout from '@/views/layout/Layout'
import 'echarts/theme/fresh-cut'
import * as Api from '@/api/commApi'
import * as statistic from '@/api/statistic'
import * as Icons from './modules/icon'

export default {
  name: 'Index',
  components: {
    // PanelGroup
  },
  data() {
    return {
      Icons,
      tabKey: 0,
      // 正在提交
      isLoading: false,
      // 首页数据
      data: {
        total: {},
        cardShow: {},
        today: {},
      },
    }
  },
  created() {
    // 获取首页数据
    this.getData()
  },
  mounted() {
  },
  methods: {
    // 获取首页数据
    getData() {
      this.isLoading = true
      statistic
        .loadPcHomePageData()
        .then((res) => {
          this.data = res.result
          //组装订单数和交易额
          let arrDate = [] //日期
          let arrOrder = [] //订单数
          let arrPrice = [] //交易额
          this.data.listEchart.forEach((element) => {
            arrOrder.push(element.orderCount)
            arrPrice.push(element.payOrderPrice)
            arrDate.push(this.getTime(element.data))
          })

          var option = {
            tooltip: {
              trigger: 'axis',
            },
            legend: {
              data: ['订单数', '交易额'],
            },
            toolbox: {
              show: true,
              showTitle: false,
              feature: {
                mark: { show: true },
                magicType: { show: true, type: ['line', 'bar'] }
              }
            },
            calculable: true,
            xAxis: [
              {
                type: 'category',
                boundaryGap: false,
                data: arrDate,
              },
            ],
            yAxis: [
              {
                type: 'value',
              },
            ],
            series: [
              {
                name: '订单数',
                type: 'line',
                data: arrOrder,
              },
              {
                name: '交易额',
                type: 'line',
                data: arrPrice,
              },
            ],
          };

          this.$nextTick(() => {
            let myChart = this.$echarts.init(document.getElementById('myChart'))
            // 绘制图表
            myChart.setOption(option)
          })
        }).finally(() => {
          this.isLoading = false
        })
    },

    getTime(time) {
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
    test() {
      this.$router.push({ name: 'goodsIndex' })
    },
    handleTabs(key) {
      this.tabKey = key
      if (this.tabKey == 0) {
        this.getData()
      } else if (this.tabKey == 1) {
        this.getDataService()
      }
    },
  },
}

</script>
<style lang="less" scoped>
.spTitle {
  margin-left: 5px;
  font-size: 14px;
  //   font-weight: 600;
  color: #000;
}
.dashboard-container .el-card .el-card__header {
  border-bottom: 0px solid #ebeef5 !important;
}

.card-panel {
  height: 108px;
  cursor: pointer;
  font-size: 12px;
  position: relative;
  overflow: hidden;
  color: #666;
  background: #fff;
  border-radius: 8px;
  //   box-shadow: 4px 4px 40px rgba(0, 0, 0, 0.05);
  border-color: rgba(0, 0, 0, 0.05);
  div,
  span,
  p {
    caret-color: transparent;
  }

  .card-panel-icon-wrapper {
    float: left;
    margin: 0px 0 0 14px;
    padding: 16px;
    transition: all 0.38s ease-out;
    border-radius: 6px;
  }

  .card-panel-round-wrapper {
    float: left;
    margin: 12px 0 0 14px;
    padding: 15px;
    transition: all 0.38s ease-out;
    border-radius: 6px;
  }
  .icon-round {
    width: 60px;
    height: 60px;
    color: #36a3f7;
    background-color: rgba(54, 163, 247, 0.2);
    border-radius: 40px;
  }
  .icon-round .icon {
    font-size: 30px;
  }

  .card-const-wrapper {
    float: left;
    margin: 8px 0 0 8px;
    padding: 8px;
    transition: all 0.38s ease-out;
    border-radius: 5px;
  }
  .icon-const {
    width: 30px;
    height: 30px;
    color: #fff;
    background-color: rgba(54, 163, 247, 0.2);
    border-radius: 5px;
  }
  .icon-const .icon {
    font-size: 14px;
  }
  .card-const-description {
    float: left;
    margin: 15px 10px;
    margin-right: 0px;
    .card-panel-text {
      line-height: 14px;
      color: rgba(0, 0, 0, 0.6);
      font-size: 14px;
      font-weight: 600;
      // margin-bottom: 12px;
    }
  }
  .card-panel-icon {
    float: left;
    font-size: 48px;
  }
  .card-panel-description {
    float: left;

    margin: 16px 10px;
    margin-right: 0px;
    .card-panel-text {
      line-height: 18px;
      color: rgba(0, 0, 0, 0.6);
      font-size: 16px;
      margin-bottom: 12px;
    }
    .card-panel-num {
      font-size: 16px;
      font-weight: 600;
      color: rgba(0, 0, 0, 1);
    }
  }
  .card-panel-only-description {
    margin: 10px 0 0 40px;

    .card-panel-text {
      line-height: 18px;
      color: rgba(0, 0, 0, 0.6);
      font-size: 16px;
      margin-bottom: 12px;
    }
    .card-panel-num {
      font-size: 16px;
      font-weight: 600;
      color: rgba(255, 0, 0, 0.8);
    }
  }

  &:hover {
    .card-panel-icon-wrapper {
      color: #fff;
    }
    .icon-people {
      background: #40c9c6;
    }
    .icon-message {
      background: #36a3f7;
    }
    .icon-money {
      background: #f4516c;
    }
    .icon-shoppingCard {
      background: #34bfa3;
    }
  }
  .icon-people {
    color: #40c9c6;
  }
  .icon-message {
    color: #36a3f7;
  }
  .icon-money {
    color: #f4516c;
  }
  .icon-shoppingCard {
    color: #34bfa3;
  }
  .icon-xiaoshoue {
    color: #fff;
  }
}

.card-const {
  height: 60px;
  cursor: pointer;
  font-size: 12px;
  position: relative;
  overflow: hidden;
  color: #666;
  background: #fff;
  border-radius: 8px;
  //   box-shadow: 4px 4px 40px rgba(0, 0, 0, 0.05);
  border-color: rgba(0, 0, 0, 0.05);
  div,
  span,
  p {
    caret-color: transparent;
  }

  .card-const-wrapper {
    float: left;
    margin: 8px 0 0 8px;
    padding: 8px;
    transition: all 0.38s ease-out;
    border-radius: 5px;
  }
  .icon-const {
    width: 30px;
    height: 30px;
    color: #fff;
    background-color: rgba(54, 163, 247, 0.2);
    border-radius: 5px;
  }
  .icon-const .icon {
    font-size: 14px;
  }
  .card-const-description {
    float: left;
    margin: 15px 10px;
    margin-right: 0px;
    .card-panel-text {
      line-height: 14px;
      color: rgba(0, 0, 0, 0.6);
      font-size: 14px;
      font-weight: 600;
      // margin-bottom: 12px;
    }
  }
}

.dashboard-editor-container {
  padding: 15px;
  background-color: rgb(240, 242, 245);
  min-height: calc(100vh - 84px);
}
.divParentChart {
  padding-top: 15px;
}
.homeChart {
  width: 95%;
  height: 450px;

  background-color: #fff;
}

.card-title {
  font-size: 14px;
  font-weight: bold;
  margin-bottom: 15px;
}

.ant-card {
  .ant-card-body {
    height: 100%;
    padding: 0px !important;
  }
}

// 统计
.statistics {
  line-height: 1;

  .ant-card {
    .ant-card-body {
      height: 100%;
      padding: 30px 40px;
    }
  }

  .col-left {
    margin-right: 25px;
    font-size: 24px;
    color: #4799ff;

    .icon-body {
      width: 58px;
      height: 58px;
      border-radius: 50%;
      background-color: #eff6ff;
    }
  }

  .col-right {
    margin-top: -5px;

    .keynote {
      font-size: 26px;
    }
  }
}

// 待办事项
.pending {
  text-align: center;
  padding: 15px 0;

  .item {
    display: inline-block;
    padding: 0 20px;

    a {
      color: unset;

      &:hover {
        color: #1890ff;
      }
    }
  }

  .name {
    font-weight: bold;
    margin-bottom: 10px;
    font-size: 13.6px;
  }

  .value {
    font-weight: bold;
    color: red;
    font-size: 16px;
  }
}
</style>

