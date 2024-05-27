<template>
  <view class="container">
    <mescroll-body ref="mescrollRef" :sticky="true" @init="mescrollInit" :down="{ use: false }" :up="upOption"
      @up="upCallback">
      <view class="log-list">
        <view v-for="(item, index) in list.data" :key="index" class="log-item">
          <view class="item-left flex-box">
            <view class="rec-status">
              <text>{{ item.describe }}</text>
            </view>
            <view class="rec-time">
              <text>{{ item.createTime }}</text>
            </view>
          </view>
          <view class="item-right" :class="[item.points > 0 ? 'col-green' : 'col-6']">
            <text>{{ item.points > 0 ? '+' : '' }}{{ item.value }}</text>
          </view>
		  
		  <view class="item-right col-6" >
		    <text>{{ item.balancePoints  }}</text>
		  </view>
        </view>
      </view>
    </mescroll-body>
  </view>
</template>

<script>
  import MescrollBody from '@/components/mescroll-uni/mescroll-body.vue'
  import MescrollMixin from '@/components/mescroll-uni/mescroll-mixins'
  import * as UserPointsLogApi from '@/api/user/userPointsLog'
  import { getEmptyPaginateObj, getMoreListData } from '@/core/app'

  export default {
    components: {
      MescrollBody
    },
    mixins: [MescrollMixin],
    data() {
      return {
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
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {},

    methods: {

      /**
       * 上拉加载的回调 (页面初始化时也会执行一次)
       * 其中page.num:当前页 从1开始, page.size:每页数据条数,默认10
       * @param {Object} page
       */
      upCallback(page) {
        const app = this
       // 设置列表数据
       app.loadList(page.num).then(list => {
       	 const curlimit = list.data.length
       	 app.mescroll.endBySize(curlimit, list.count)
         }).catch(() => app.mescroll.endErr())
      },

      // 获取积分明细列表
      loadList(pageNo = 1) {
        const app = this
        return new Promise((resolve, reject) => {
			console.log("111111")
        	UserPointsLogApi.load({ page: pageNo,limit:app.list.limit }, { load: false }).then(res => {
        				// 合并新数据
        				const newList = res.result
        				app.list.count=res.count
        				app.list.data = getMoreListData(newList, app.list, pageNo)
        				resolve(app.list)
        			  }) 
        	  })
      }

    }
  }
</script>

<style lang="scss" scoped>
  page,
  .container {
    background: #fff;
  }

  .log-list {
    padding: 0 30rpx;
  }

  .log-item {
    font-size: 28rpx;
    padding: 20rpx 20rpx;
    line-height: 1.8;
    border-bottom: 1rpx solid rgb(238, 238, 238);
    display: flex;
    justify-content: center;
    align-items: center;
  }

  .rec-status {
    color: #333;

    .rec-time {
      color: rgb(160, 160, 160);
      font-size: 26rpx;
    }
  }
</style>
