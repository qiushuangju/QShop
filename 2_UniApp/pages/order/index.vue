<template>
  <view class="container">
    <mescroll-body ref="mescrollRef" :sticky="true" @init="mescrollInit" :down="{ native: true }" @down="downCallback" :up="upOption" @up="upCallback">

      <!-- tab栏 -->
      <u-tabs :list="tabs" :is-scroll="false" :current="curTab" active-color="#FA2209" :duration="0.2" @change="onChangeTab" />
      <!-- 订单列表 -->
      <view class="order-list">
        <view class="order-item" v-for="(item, index) in list.data" :key="index">
          <view class="item-top">
            <view class="item-top-left">
              <text class="order-time">{{ item.createTime }}</text>
            </view>
            <view class="item-top-right">
              <text class="state-text">{{ item.strOrderStatus }}</text>
            </view>
          </view>
          <!-- 商品列表 -->
          <view class="goods-list" @click="handleTargetDetail(item.id)">
            <view class="goods-item" v-for="(goods, idx) in item.listSku" :key="idx">
              <!-- 商品图片 -->
              <view class="goods-image">
                <image class="image" :src="goods.skuImageUrl" mode="scaleToFill"></image>
              </view>
              <!-- 商品信息 -->
              <view class="goods-content">
                <view class="goods-title"><text class="twoline-hide">{{ goods.goodsName }}</text></view>
                <view class="goods-props clearfix">
                  <view class="goods-props-item">
                    <text>{{ goods.skuName }}</text>
                  </view>
                </view>
              </view>
              <!-- 交易信息 -->
              <view class="goods-trade">
                <view class="goods-price">
                  <text class="unit">￥</text>
                  <!-- <text class="value">{{ goods.is_user_grade ? goods.grade_goods_price : goods.goods_price }}</text> -->
				   <text class="value">{{ goods.salePrice }}</text>
                </view>
                <view class="goods-num">
                  <text>×{{ goods.quantity }}</text>
                </view>
              </view>
            </view>
          </view>
          <!-- 订单合计 -->
          <view class="order-total">
            <text>共{{ item.goodsTotalNum }}件商品，实付款</text>
            <text class="unit">￥</text>
            <text class="money">{{ item.payPrice }}</text>
          </view>
          <!-- 订单操作 -->
          <view v-if="item.orderStatus != OrderStatusEnum.Cancel.value" class="order-handle">
            <view class="btn-group clearfix">
              <!-- 未支付 已支付未发货订单 取消订单 -->
              <block v-if="item.orderStatus == OrderStatusEnum.NotPaid.value ">
                <view class="btn-item" @click="onCancelOrder(item.id)">取消</view>
              </block>
              <!-- 已支付未发货订单 -->
              <block v-if="item.orderStatus == OrderStatusEnum.WaitDeliver.value">
                  <view class="btn-item" @click="onApplyCancelOrder(item.id)">申请取消</view>
              </block>
              <!-- 已申请取消 -->
              <!-- <view v-else class="f-28 col-8">取消申请中</view> -->
              <!-- 未支付的订单 -->
              <block v-if="item.orderStatus == OrderStatusEnum.NotPaid.value">
                <view class="btn-item active" @click="onPay(item.id)">去支付</view>
              </block>
              <!-- 确认收货 -->
              <block v-if="item.orderStatus == OrderStatusEnum.WaitReceiving.value">
                <view class="btn-item active" @click="onReceipt(item.id)">确认收货</view>
              </block>
              <!-- 订单评价 -->
              <block v-if="item.orderStatus >= OrderStatusEnum.Done.value&&item.isComment==-10 ">
                <view class="btn-item" @click="handleTargetComment(item.id)">评价</view>
              </block>
            </view>
          </view>

        </view>
      </view>
    </mescroll-body>

    <!-- 支付方式弹窗 -->
    <u-popup v-model="showPayPopup" mode="bottom" border-radius="26" :closeable="true">
      <view class="pay-popup">
        <view class="title">请选择支付方式</view>
        <view class="pop-content">
          <!-- 微信支付 -->
          <!-- #ifdef MP-WEIXIN -->
          <view class="pay-item dis-flex flex-x-between" @click="onSelectPayType(PayTypeEnum.WeChat.value)">
            <view class="item-left dis-flex flex-y-center">
              <view class="item-left_icon wechat">
                <text class="iconfont icon-weixinzhifu"></text>
              </view>
              <view class="item-left_text">
                <text>{{ PayTypeEnum.WeChat.name }}</text>
              </view>
            </view>
			
			<view class="item-right col-m" v-if="selectedPayType==PayTypeEnum.WeChat.value">
			    <text class="iconfont icon-xuanzhong"></text>
			</view>
          </view>
          <!-- #endif -->
          <!-- 余额支付 -->
          <view class="pay-item dis-flex flex-x-between" @click="onSelectPayType(PayTypeEnum.Balance.value)">
            <view class="item-left dis-flex flex-y-center">
              <view class="item-left_icon balance">
                <text class="iconfont icon-yuezhifu"></text>
              </view>
              <view class="item-left_text">
                <text>{{ PayTypeEnum.Balance.name }}</text>
              </view>
            </view>
			
			<view class="item-right col-m" v-if="selectedPayType==PayTypeEnum.Balance.value">
			    <text class="iconfont icon-xuanzhong"></text>
			</view>
          </view>
		  
        </view>
      </view>
    </u-popup>

  </view>

</template>

<script>
  import {
    DeliveryStatusEnum,
    DeliveryTypeEnum,
    OrderStatusEnum,
    PayTypeEnum,
  } from '@/common/enum/order'
  import MescrollBody from '@/components/mescroll-uni/mescroll-body.vue'
  import MescrollMixin from '@/components/mescroll-uni/mescroll-mixins'
  import { getEmptyPaginateObj, getMoreListData } from '@/core/app'
  import * as OrderApi from '@/api/order/order'
  import { wxPayment } from '@/core/app'


  // tab栏数据
  const tabs = [{
    name: `全部`,
    value: 0
  }, {
    name: `待支付`,
    value: 10
  }, {
    name: `待发货`,
    value: 20
  }, {
    name: `待收货`,
    value: 30
  }, {
    name: `已完成`,
    value: 80
  }, {
    name: `待评价`,
    value: 40
  }]

  export default {
    components: {
      MescrollBody
    },
    mixins: [MescrollMixin],
    data() {
      return {
        // 枚举类
        DeliveryStatusEnum,
        DeliveryTypeEnum,
        OrderStatusEnum,
        PayTypeEnum,
		bigOrderStatus:0,
        // 当前页面参数
        options: { bigOrderStatus: 0},
        // tab栏数据
        tabs,
        // 当前标签索引
        curTab: 0,
        // 订单列表数据
        list: getEmptyPaginateObj(),
        // 上拉加载配置
        upOption: {
          // 首次自动执行
          auto: true,
          // 每页数据的数量; 默认10
          page: { size: getEmptyPaginateObj().limit },
          // 数量要大于4条才显示无更多数据
          noMoreSize: getEmptyPaginateObj().limit,
        },
        // 控制onShow事件是否刷新订单列表
        canReset: false,
		selectedPayType:10,
        // 支付方式弹窗
        showPayPopup: false
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {
      // 初始化当前选中的标签
      this.initCurTab(options)
      // 注册全局事件订阅: 是否刷新订单列表
      uni.$on('syncRefresh', canReset => {
        this.canReset = canReset
      })
    },

    /**
     * 生命周期函数--监听页面显示
     */
    onShow() {
      this.canReset && this.onRefreshList()
      this.canReset = false
    },

    /**
     * 生命周期函数--监听页面的卸载
     */
    onUnload() {
      // 卸载全局事件订阅
      uni.$off('syncRefresh')
    },

    methods: {
      // 初始化当前选中的标签
      initCurTab(options) {
        const app = this
		console.log("bbbbb",options.bigOrderStatus)
        if (options.bigOrderStatus!=null&& options.bigOrderStatus!=undefined) {
          const index = app.tabs.findIndex(item => item.value == options.bigOrderStatus)
          app.curTab = index > -1 ? index : 0
        }
      },

      /**
       * 上拉加载的回调 (页面初始化时也会执行一次)
       * 其中page.num:当前页 从1开始, page.size:每页数据条数,默认10
       * @param {Object} page
       */
      upCallback(page) {
        const app = this
        // 设置列表数据
        app.loadList(page.num)
          .then(list => {
            const curlimit = list.data.length
			console.log("QQQQQQQQQQQ",list.count)
            app.mescroll.endBySize(curlimit, list.count)
          })
          .catch(() => app.mescroll.endErr())
      },
      // 分页列表
      loadList(pageNo = 1) {
        const app = this
        return new Promise((resolve, reject) => {
          OrderApi.load({ bigOrderStatus: app.getTabValue(),page: pageNo,limit:app.list.limit }, { load: false })
            .then(res => {
            // 合并新数据
            const newList = res.result
            app.list.count=res.count
            app.list.data = getMoreListData(newList, app.list, pageNo)
            resolve(app.list)
            })
        })
      },

 

      // 获取当前标签项的值
      getTabValue() {
        return this.tabs[this.curTab].value
      },

      // 切换标签项
      onChangeTab(index) {
        const app = this
        // 设置当前选中的标签
        app.curTab = index
        // 刷新订单列表
        app.onRefreshList()
      },

      // 刷新订单列表
      onRefreshList() {
        this.list = getEmptyPaginateObj()
        setTimeout(() => {
          this.mescroll.resetUpScroll()
        }, 120)
      },

      // 取消订单
      onCancelOrder(orderId) {
        const app = this
        uni.showModal({
          title: '友情提示',
          content: '确认要取消该订单吗？',
          success(o) {
            if (o.confirm) {
              OrderApi.cancelOrder(orderId)
                .then(result => {
                  // 显示成功信息
                  app.$toast(result.message)
                  // 刷新订单列表
                  app.onRefreshList()
                })
            }
          }
        });
      },
	  // 申请消订单
	  onApplyCancelOrder(orderId) {
	    const app = this
	    uni.showModal({
	      title: '友情提示',
	      content: '确认要申请取消该订单吗？',
	      success(o) {
	        if (o.confirm) {
	          OrderApi.applyCancelOrder(orderId)
	            .then(result => {
	              // 显示成功信息
	              app.$toast(result.message)
	              // 刷新订单列表
	              app.onRefreshList()
	            })
	        }
	      }
	    });
	  },

      // 确认收货
      onReceipt(orderId) {
        const app = this
        uni.showModal({
          title: '友情提示',
          content: '确认收到商品了吗？',
          success(o) {
            if (o.confirm) {
              OrderApi.receipt(orderId)
                .then(result => {
                  // 显示成功信息
                  app.$success(result.message)
                  // 刷新订单列表
                  app.onRefreshList()
                })
            }
          }
        });
      },

      // 点击去支付
      onPay(orderId) {
        // 记录订单id
        this.payOrderId = orderId
        // 显示支付方式弹窗
        this.showPayPopup = true
      },

      // 选择支付方式
      onSelectPayType(payType) {
        const app = this
        // 隐藏支付方式弹窗
        this.showPayPopup = false
		app.selectedPayType=payType
        // 发起支付请求
        OrderApi.payOrderBefore(app.payOrderId, payType)
          .then(res => app.onSubmitCallback(res))
      },

      // 订单提交成功后回调
      onSubmitCallback(res) {
        const app = this
        // 发起微信支付
        if (app.selectedPayType == PayTypeEnum.WeChat.value) {
          wxPayment(res.wxPayParams)
            .then(() => {
              app.$success('支付成功')
              setTimeout(() => {
                app.onRefreshList()
              }, 1500)
            })
            .catch(err => {
              app.$error('订单未支付')
            })
            .finally(() => {
              app.disabled = false
            })
        }
        // 余额支付
        if (app.selectedPayType == PayTypeEnum.Balance.value) {
          app.$success('支付成功')
          app.disabled = false
          setTimeout(() => {
            app.onRefreshList()
          }, 1500)
        }
      },

      // 跳转到订单详情页
      handleTargetDetail(orderId) {
        this.$navTo('pages/order/detail', { orderId })
      },

      // 跳转到订单评价页
      handleTargetComment(orderId) {
        this.$navTo('pages/order/comment/index', { orderId })
      }

    },

  }
</script>

<style lang="scss" scoped>
  // 项目内容
  .order-item {
    margin: 20rpx auto 20rpx auto;
    padding: 30rpx 30rpx;
    width: 94%;
    box-shadow: 0 1rpx 5rpx 0px rgba(0, 0, 0, 0.05);
    border-radius: 16rpx;
    background: #fff;
  }

  // 项目顶部
  .item-top {
    display: flex;
    justify-content: space-between;
    font-size: 26rpx;
    margin-bottom: 40rpx;

    .order-time {
      color: #777;
    }

    .state-text {
      color: $uni-text-color-active;
    }
  }

  // 商品列表
  .goods-list {

    // 商品项
    .goods-item {
      display: flex;
      margin-bottom: 40rpx;

      // 商品图片
      .goods-image {
        width: 180rpx;
        height: 180rpx;

        .image {
          display: block;
          width: 100%;
          height: 100%;
          border-radius: 8rpx;
        }
      }

      // 商品内容
      .goods-content {
        flex: 1;
        padding-left: 16rpx;
        padding-top: 16rpx;

        .goods-title {
          font-size: 26rpx;
          max-height: 76rpx;
        }

        .goods-props {
          margin-top: 14rpx;
          height: 40rpx;
          color: #ababab;
          font-size: 24rpx;
          overflow: hidden;

          .goods-props-item {
            display: inline-block;
            margin-right: 14rpx;
            padding: 4rpx 16rpx;
            border-radius: 12rpx;
            background-color: #F5F5F5;
            width: auto;
          }
        }


      }

      // 交易信息
      .goods-trade {
        padding-top: 16rpx;
        width: 150rpx;
        text-align: right;
        color: $uni-text-color-grey;
        font-size: 26rpx;

        .goods-price {
          vertical-align: bottom;
          margin-bottom: 16rpx;

          .unit {
            margin-right: -2rpx;
            font-size: 24rpx;
          }
        }
      }

    }

  }

  // 订单合计
  .order-total {
    font-size: 26rpx;
    vertical-align: bottom;
    text-align: right;
    height: 40rpx;
    margin-bottom: 30rpx;

    .unit {
      margin-left: 8rpx;
      margin-right: -2rpx;
      font-size: 26rpx;
    }

    .money {
      font-size: 28rpx;
    }
  }

  // 订单操作
  .order-handle {
    .btn-group {

      .btn-item {
        border-radius: 10rpx;
        padding: 6rpx 20rpx;
        margin-left: 15rpx;
        font-size: 28rpx;
        float: right;
        color: #383838;
        border: 1rpx solid #a8a8a8;

        &:last-child {
          margin-left: 0;
        }

        &.active {
          color: $uni-text-color-active;
          border: 1rpx solid $uni-text-color-active;
        }
      }

    }

  }

  // 弹出层-支付方式
  .pay-popup {
    padding: 24rpx;

    .title {
      font-size: 30rpx;
      margin-bottom: 40rpx;
      font-weight: bold;
      text-align: center;
    }

    .pop-content {
      min-height: 260rpx;
      padding: 0 10rpx;

      .pay-item {
        padding: 24rpx 35rpx;
        font-size: 28rpx;
        border-bottom: 1rpx solid #f1f1f1;

        &:last-child {
          border-bottom: none;
        }

        .item-left_icon {
          margin-right: 20rpx;
          font-size: 32rpx;

          &.wechat {
            color: #00c800;
          }

          &.balance {
            color: #ff9700;
          }
        }
      }
    }
  }
</style>
