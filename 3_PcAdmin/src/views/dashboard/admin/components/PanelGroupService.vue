<template>
  <el-row class="panel-group-service" :gutter="40">
    <!-- <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
      <div class="card-panel" @click="handleSetLineChartData('messages')">
        <div class="card-panel-icon-wrapper icon-message">
          <span
            class="icon iconfont icon-weixin"
            style="font-size: xxx-large"
          ></span>
        </div>
        <div class="card-panel-description">
          <div class="card-panel-text">消息</div>
          <count-to
            class="card-panel-num"
            :startVal="0"
            :endVal="81212"
            :duration="2000"
          ></count-to>
        </div>
      </div>
    </el-col> -->

    <!-- <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
      <div class="card-panel" @click="handleSetLineChartData('newVisitis')">
        <div class="card-panel-icon-wrapper icon-people">
          <span
            class="icon iconfont icon-yonghu_zhanghao_wode"
            style="font-size: xxx-large"
          ></span>
        </div>
        <div class="card-panel-description">
          <div class="card-panel-text">用户(七日)</div>
          <count-to
            class="card-panel-num"
            :startVal="0"
            :endVal="data.userCount==null?0:data.userCount"
            :duration="2000"
          ></count-to>
        </div>
      </div>
    </el-col> -->

    <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
      <div class="card-panel" @click="handleSetLineChartData('purchases')">
        <div class="card-panel-icon-wrapper icon-money">
          <span
            class="icon iconfont icon-B"
            style="font-size: xxx-large"
          ></span>
        </div>
        <div class="card-panel-description">
          <div class="card-panel-text">交易金额(七日)</div>
          <count-to
            class="card-panel-num"
            :startVal="0"
            :decimals="2"
            :endVal="data.payOrderPrice==null?0:data.payOrderPrice"
            :duration="2000"
          ></count-to>
        </div>
      </div>
    </el-col>
    <!-- <el-col :xs="12" :sm="12" :lg="6" class="card-panel-col">
      <div class="card-panel" @click="handleSetLineChartData('shoppings')">
        <div class="card-panel-icon-wrapper icon-shoppingCard">
          <span
            class="icon iconfont icon-dingdan"
            style="font-size: xxx-large"
          ></span>
        </div>
        <div class="card-panel-description">
          <div class="card-panel-text">销量(七日)</div>
          <count-to
            class="card-panel-num"
            :startVal="0"
            :endVal="data.salesGoodsCount==null?0:data.salesGoodsCount"
            :duration="2000"
          ></count-to>
        </div>
      </div>
    </el-col> -->
  </el-row>
</template>
<script>
import PropTypes from "ant-design-vue/es/_util/vue-types";
import CountTo from "vue-count-to";
import * as Api from "@/api/commApi";
export default {
  components: {
    CountTo,
  },
  props: {
        // 商品信息
        data: PropTypes.object.def({
            userCount: 0,
            payOrderPrice: 0,
            salesGoodsCount: 0,
        }),
    },
  data() {
    return {
      temp: {},
    };
  },
  created() {
    //this.getData();
  },
  methods: {
    handleSetLineChartData(type) {
      this.$emit("handleSetLineChartData", type);
    },
    getData() {
      Api.getPcHomeData().then((res) => {
        this.temp = res.result;
      });
    },
  },
};
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
.panel-group-service {
  div,
  span,
  p {
    caret-color: transparent;
  }
  margin-top: 18px;
  .card-panel-col {
    margin-bottom: 32px;
    border-radius: 10px;
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
    box-shadow: 4px 4px 40px rgba(0, 0, 0, 0.05);
    border-color: rgba(0, 0, 0, 0.05);
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
    .card-panel-icon-wrapper {
      float: left;
      margin: 14px 0 0 14px;
      padding: 16px;
      transition: all 0.38s ease-out;
      border-radius: 6px;
    }
    .card-panel-icon {
      float: left;
      font-size: 48px;
    }
    .card-panel-description {
      //float: right;
      font-weight: bold;
      margin: 26px;
      margin-left: 0px;
      .card-panel-text {
        line-height: 18px;
        color: rgba(0, 0, 0, 0.45);
        font-size: 16px;
        margin-bottom: 12px;
      }
      .card-panel-num {
        font-size: 20px;
      }
    }
  }
}
</style>