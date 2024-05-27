<template>
  <!-- 商品评价 -->
  <view v-if="!isLoading && list.length" class="goods-comment m-top20">
    <view class="item-title dis-flex">
      <view class="block-left flex-box">
        商品评价 (<text class="total">{{ total }}条</text>)
      </view>
      <view class="block-right">
        <text @click="onTargetToComment" class="show-more col-9">查看更多</text>
        <text class="iconfont icon-arrow-right"></text>
      </view>
    </view>
    <!-- 评论列表 -->
    <view class="comment-list">
      <view class="comment-item" v-for="(item, index) in list" :key="index">
        <view class="comment-item_row dis-flex flex-y-center">
          <view class="user-info dis-flex flex-y-center">
            <avatar-image class="user-avatar" :url="item.urlAvater" :width="50" />
            <text class="user-name">{{ item.nickName }}</text>
          </view>
          <!-- 评星 -->
          <view class="star-rating">
			<uni-rate disabled="true"  disabledColor="#f4a213"  :size="20" v-model="item.score" />
          </view>
		  
        </view>
        <view class="item-content m-top20">
          <text class="f-26 twoline-hide">{{ item.content }}</text>
        </view>
        <view class="comment-time">{{ item.createTime }}</view>
      </view>
    </view>
  </view>

</template>

<script>
  import AvatarImage from '@/components/avatar-image'
  import * as CommentApi from '@/api/goods/comment'

  export default {
    components: {
      AvatarImage
    },
    props: {
      // 商品ID
      goodsId: {
        type: String,
        default: ""
      },
      // 加载多少条记录 默认2条
      limit: {
        type: Number,
        default: 2
      }
    },
    data() {
      return {
        // 正在加载
        isLoading: true,
        // 评星数据转换
        rates: { 10: 5, 20: 3, 30: 1 },
        // 评价列表数据
        list: [],
        // 评价总数量
        total: 0
      }
    },

    created() {		
      // 加载评价列表数据
      this.getCommentList()
    },

    methods: {

      // 加载评价列表数据
      getCommentList() {
        const app = this
        app.isLoading = true
        CommentApi.load(app.goodsId, app.limit)
          .then(res => {
            app.list = res.result
            app.total = res.count
          })
          .catch(err => err)
          .finally(() => app.isLoading = false)
      },

      // 跳转到评论列表页
      onTargetToComment() {
        const app = this
        app.$navTo('pages/comment/index', { goodsId: app.goodsId })
      }

    }
  }
</script>

<style lang="scss" scoped>
  .goods-comment {
    padding: 20rpx 30rpx;
    background-color: #fff;
  }

  .item-title {
    font-size: 28rpx;
    margin-bottom: 25rpx;

    .total {
      margin: 0 4rpx;
    }

    .show-more {
      margin-right: 8rpx;
      font-size: 24rpx;
    }
  }

  .comment-item {
    padding: 15rpx 5rpx;
    margin-bottom: 10rpx;
    border-bottom: 1rpx solid #f5f5f5;

    &:last-child {
      margin-bottom: 0;
      border-bottom: none;
    }

    .comment-item_row {
      margin-bottom: 10rpx;
    }
  }

  .user-info {
    margin-right: 15rpx;

    .user-avatar {
      width: 50rpx;
      height: 50rpx;
      border-radius: 50%;
      margin-right: 10rpx;
    }

    .user-name {
      font-size: 24rpx;
    }

  }

  .item-content {
    color: #333;
    margin: 16rpx 0;
    max-height: 76rpx;
    line-height: 38rpx;
  }

  .comment-time {
    font-size: 24rpx;
    color: #999;
    margin-top: 10rpx;
  }
</style>
