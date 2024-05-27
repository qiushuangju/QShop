<template>
  <view class="container">
    <mescroll-body ref="mescrollRef" :sticky="true" @init="mescrollInit" :down="{ use: false }" :up="upOption"
      @up="upCallback">

      <!-- tab栏 -->
      <u-tabs :list="tabs" :is-scroll="false" :current="curTab" active-color="#FA2209" :duration="0.2"
        @change="onChangeTab" />

	<empty v-if="!list.data.length" :isLoading="isLoading" ></empty>
	
      <!-- 优惠券列表 -->
      <view class="coupon-list">
        <view class="coupon-item" v-for="(item, index) in list.data" :key="index">
          <view class="item-wrapper" :class="['color-' + (item.status ? color[index % color.length] : 'gray')]">
            <view class="coupon-type">{{item.strCouponType}}</view>
            <view class="tip dis-flex flex-dir-column flex-x-center">
             <!-- <view v-if="item.coupon_type == CouponTypeEnum.FULL_DISCOUNT.value">
                <text class="f-30">￥</text>
                <text class="money">{{ item.reducePrice }}</text>
              </view> -->
			  <view >
			    <text class="f-30">￥</text>
			    <text class="money">{{ item.reducePrice }}</text>
			  </view>
              <!-- <text class="money" v-if="item.coupon_type == CouponTypeEnum.DISCOUNT.value">{{ item.discount }}折</text> -->
              <text class="pay-line">满{{ item.minPrice }}元可用</text>
            </view>
            <view class="split-line"></view>
            <view class="content dis-flex flex-dir-column flex-x-between">
              <view class="title">{{ item.name }}</view>
              <view class="bottom dis-flex flex-y-center">
                <view class="time flex-box">
                  <block v-if="item.startTime === item.endTime">{{ item.startTime }} 当天有效</block>
                  <block v-else>{{ item.startTime }}~{{ item.endTime }}</block>
                </view>
                <view class="receive status">
                  <text>{{ item.strStatus }}</text>
                </view>
              </view>
            </view>
          </view>
        </view>
      </view>
    </mescroll-body>
  </view>
</template>

<script>
  import MescrollBody from '@/components/mescroll-uni/mescroll-body.vue'
  import MescrollMixin from '@/components/mescroll-uni/mescroll-mixins'
  import { getEmptyPaginateObj, getMoreListData } from '@/core/app'
  import * as UserCouponApi from '@/api/user/userCoupon'
  import { CouponTypeEnum } from '@/common/enum/coupon'

  const color = ['red', 'blue', 'violet', 'yellow']
  const pageSize = 15
  const tabs = [{
    name: `未使用`,
    value: '10'
  }, {
    name: `已使用`,
    value: '-20'
  }, {
    name: `已过期`,
    value: '-10'
  }]

  export default {
    components: {
      MescrollBody
    },
    mixins: [MescrollMixin],
    data() {
      return {
        // 枚举类
        CouponTypeEnum,
        // 颜色组
        color,
        // 标签栏数据
        tabs,
        // 当前标签索引
        curTab: 0,
        // 优惠券列表数据
        list: getEmptyPaginateObj(),
        // 上拉加载配置
        upOption: {
          // 首次自动执行
          auto: true,
          // 每页数据的数量; 默认10
          page: { size: pageSize },
          // 数量要大于4条才显示无更多数据
          noMoreSize: 4,
          // 空布局
          empty: {
            tip: '亲，暂无相关优惠券'
          }
        }
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {

    },

    methods: {

      /**
       * 上拉加载的回调 (页面初始化时也会执行一次)
       * 其中page.num:当前页 从1开始, page.size:每页数据条数,默认10
       * @param {Object} page
       */
      upCallback(page) {
        const app = this
        // 设置列表数据
        app.getCouponList(page.num)
          .then(list => {
            const curlimit = list.data.length
            app.mescroll.endBySize(curlimit, list.count)
          })
          .catch(() => app.mescroll.endErr())
      },

      /**
       * 获取优惠券列表
       */
      getCouponList(pageNo = 1) {
        const app = this
        return new Promise((resolve, reject) => {
          UserCouponApi.listByWhere({ status: app.getTabValue(), page: pageNo }, { load: false })
            .then(res => {
			   // 合并新数据
			   const newList = res.result
			   app.list.count=res.count
			   app.list.data = getMoreListData(newList, app.list, pageNo)
			   resolve(app.list)
            })
        })
      },

      // 评分类型
      getTabValue() {
        return this.tabs[this.curTab].value
      },

      // 切换标签项
      onChangeTab(index) {
        const app = this
        // 设置当前选中的标签
        app.curTab = index
        // 刷新优惠券列表
        app.onRefreshList()
      },

      // 刷新优惠券列表
      onRefreshList() {
        this.list = getEmptyPaginateObj()
        setTimeout(() => {
          this.mescroll.resetUpScroll()
        }, 120)
      },

    }
  }
</script>

<style lang="scss" scoped>
  .coupon-list {
    padding: 20rpx;
  }


  .coupon-item {
    position: relative;
    overflow: hidden;
    margin-bottom: 22rpx;
  }

  .item-wrapper {
    width: 100%;
    display: flex;
    background: #fff;
    border-radius: 8rpx;
    color: #fff;
    height: 180rpx;

    .coupon-type {
      position: absolute;
      top: 0;
      right: 0;
      z-index: 10;
      width: 128rpx;
      padding: 6rpx 0;
      background: #a771ff;
      font-size: 20rpx;
      text-align: center;
      color: #ffffff;
      transform: rotate(45deg);
      transform-origin: 64rpx 64rpx;
    }

    &.color-blue {
      background: linear-gradient(-125deg, #57bdbf, #2f9de2);
    }

    &.color-red {
      background: linear-gradient(-128deg, #ff6d6d, #ff3636);
    }

    &.color-violet {
      background: linear-gradient(-113deg, #ef86ff, #b66ff5);

      .coupon-type {
        background: #55b5ff;
      }
    }

    &.color-yellow {
      background: linear-gradient(-141deg, #f7d059, #fdb054);
    }

    &.color-gray {
      background: linear-gradient(-113deg, #bdbdbd, #a2a1a2);

      .coupon-type {
        background: #9e9e9e;
      }
    }

    .content {
      flex: 1;
      padding: 30rpx 20rpx;
      border-radius: 16rpx 0 0 16rpx;

      .title {
        font-size: 32rpx;
      }

      .bottom {
        .time {
          font-size: 24rpx;
        }

        .receive {
          height: 46rpx;
          width: 122rpx;
          line-height: 46rpx;
          text-align: center;
          border: 1rpx solid #fff;
          border-radius: 30rpx;
          color: #fff;
          font-size: 24rpx;

          &.status {
            border: none;
          }
        }
      }
    }

    .tip {
      position: relative;
      flex: 0 0 32%;
      text-align: center;
      border-radius: 0 16rpx 16rpx 0;

      .money {
        font-weight: bold;
        font-size: 52rpx;
      }

      .pay-line {
        font-size: 22rpx;
      }
    }

    .split-line {
      position: relative;
      flex: 0 0 0;
      border-left: 4rpx solid #fff;
      margin: 0 10rpx 0 6rpx;
      background: #fff;

      &:before,
        {
        border-radius: 0 0 16rpx 16rpx;
        top: 0;
      }

      &:after {
        border-radius: 16rpx 16rpx 0 0;
        bottom: 0;
      }

      &:before,
      &:after {
        content: '';
        position: absolute;
        width: 24rpx;
        height: 12rpx;
        background: #f7f7f7;
        left: -14rpx;
        z-index: 1;
      }


    }
  }
</style>
