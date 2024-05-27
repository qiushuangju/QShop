<template>
  <mescroll-body ref="mescrollRef" :sticky="true" @init="mescrollInit" :down="{ use: false }" :up="upOption" @up="upCallback">
    <!-- tab栏 -->
    <u-tabs :list="tabs" :is-scroll="false" :current="curTab" active-color="#FA2209" :duration="0.2" @change="onChangeTab" />

    <!-- 商品评价列表 -->
    <view class="comment-list">
      <view class="comment-item" v-for="(item, index) in list.data" :key="index">
        <view class="item-head">
          <!-- 用户信息 -->
          <view class="user-info">
            <avatar-image class="user-avatar" :url="item.urlAvater" :width="50" />
            <text class="user-name f-26">{{ item.nickName }}</text>
          </view>
          <!-- 评星 -->
		  <uni-rate disabled="true"  disabledColor="#f4a213"  :size="20" v-model="item.score" />
          <!-- 评价日期-->
          <view class="flex-box f-22 col-9 t-r">{{ item.createTime }}</view>
        </view>
        <!-- 评价内容 -->
        <view class="item-content m-top20">
          <text class="f-26">{{ item.content }}</text>
        </view>
        <!-- 评价图片 -->
        <view class="images-list clearfix" v-if="item.listImg.length">
          <view class="image-preview" v-for="(image, imgIdx) in item.listImg" :key="imgIdx">
            <image class="image" mode="aspectFill" :src="image.thumbnail" @click="onPreviewImages(index, imgIdx)"></image>
          </view>
        </view>
        <!-- 商品规格 -->
        <view class="goods-props clearfix">
          <view class="goods-props-item" >
            <text>{{ item.skuName}}</text>
          </view>
        </view>
      </view>
    </view>
  </mescroll-body>
</template>

<script>
  import MescrollBody from '@/components/mescroll-uni/mescroll-body.vue'
  import MescrollMixin from '@/components/mescroll-uni/mescroll-mixins'
    import AvatarImage from '@/components/avatar-image'
  import { getEmptyPaginateObj, getMoreListData } from '@/core/app'
  import * as CommentApi from '@/api/goods/comment'

  const pageSize = 15
  const tabs = [{
    name: `全部`,
    scoreType: -1
  }, {
    name: `好评`,
    scoreType: 10
  }, {
    name: `中评`,
    scoreType: 20
  }, {
    name: `差评`,
    scoreType: 30
  }]

  export default {
    components: {
      MescrollBody,
          AvatarImage
    },
    mixins: [MescrollMixin],
    data() {
      return {
        // 当前商品ID
        goodsId: null,
        // 当前标签索引
        curTab: 0,
        // 评价列表数据
        list: getEmptyPaginateObj(),
        // 评价总数量
        total: { all: 0, negative: 0, praise: 0, review: 0 },
        // 评星数据转换
        rates: { 10: 5, 20: 3, 30: 1 },
        // 标签栏数据
        tabs,
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
            tip: '亲，暂无相关商品评价'
          }
        },
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {
      // 记录属性值
      this.goodsId = options.goodsId
      // 获取指定评分总数
      this.getTotal()
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
        app.getCommentList(page.num)
          .then(list => {
			const curlimit = list.data.length
			app.mescroll.endBySize(curlimit, list.count)
          })
          .catch(() => app.mescroll.endErr())
      },

      // 加载评价列表数据
      getCommentList(pageNo = 1) {
        const app = this
        return new Promise((resolve, reject) => {
          CommentApi.load(app.goodsId, { commentType: app.getScoreType(), page: pageNo }, { load: false })
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
      getScoreType() {
        return this.tabs[this.curTab].scoreType
      },

      // 获取指定评分总数
      getTotal() {
        const app = this
        CommentApi.totalGroup(app.goodsId)
          .then(res => {
            // tab标签内容
            const total = res.result
            app.getTabs(total)
          })
      },

      // 获取tab标签内容
      getTabs(total) {
        const tabs = this.tabs
        tabs[0].name = `全部(${total.totalAll})`
        tabs[1].name = `好评(${total.totalPraise})`
        tabs[2].name = `中评(${total.totalMiddle})`
        tabs[3].name = `差评(${total.totalBad})`
      },

      // 切换标签项
      onChangeTab(index) {
        const app = this
        // 设置当前选中的标签
        app.curTab = index
        // 刷新评价列表
        app.onRefreshList()
      },

      // 刷新评价列表
      onRefreshList() {
        this.list = getEmptyPaginateObj()
        setTimeout(() => {
          this.mescroll.resetUpScroll()
        }, 120)
      },

      // 预览评价图片
      onPreviewImages(dataIdx, imgIndex) {
        const app = this
        const images = app.list.data[dataIdx].listImg
        const imageUrls = images.map(item => item.thumbnail)
        uni.previewImage({
          current: imageUrls[imgIndex],
          urls: imageUrls
        })
      }

    }
  }
</script>

<style lang="scss" scoped>
  .comment-item {
    padding: 30rpx;
    box-sizing: border-box;
    border-bottom: 1rpx solid #f7f7f7;
    background: #fff;
  }

  .item-head {
    display: flex;
    align-items: center;

    // 用户信息
    .user-info {
      margin-right: 15rpx;
      display: flex;
      align-items: center;

      .user-avatar {
        margin-right: 15rpx;
      }

      .user-name {
        color: #999;
      }
    }

  }

  // 评价内容
  .item-content {
    font-size: 30rpx;
    color: #333;
    margin: 16rpx 0;
  }

  // 评价图片
  .images-list {
    &::after {
      clear: both;
      content: " ";
      display: table;
    }

    .image-preview {
      float: left;
      margin-bottom: 15rpx;
      margin-right: 15rpx;

      &:nth-child(3n+0) {
        margin-right: 0;
      }

      .image {
        display: block;
        width: 220rpx;
        height: 220rpx;
      }
    }
  }

  // 商品规格
  .goods-props {
    font-size: 24rpx;
    color: #999;

    .goods-props-item {
      float: left;

      .group-name {
        margin-right: 6rpx;
      }
    }
  }
</style>
