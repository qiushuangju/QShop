<template>
  <!--店铺推荐-->
  		<block v-if="listRecommend.length>0">
		<view class="vdivider">
			<u-divider :fontSize="30" :bold="true" width="50%"  color="#333">
				<text class="divTitle">{{title}}</text>
			</u-divider>	
		</view>
  			
  			
  			<!-- 商品列表 -->
  			<view class="goods-list clearfix" :class="'column-2'">
  				<view class="goods-item" v-for="(item, index) in listRecommend" :key="index" @click="onTargetDetail(item.id)">
  		
  				<view class="">
  				  <!-- 商品图片 -->
  				  <view class="goods-image">
  				    <image class="image" mode="aspectFill" :src="item.urlImageMain"></image>
  				  </view>
  				  <view class="detail">
  				    <!-- 商品标题 -->
  				    <view class="goods-name">
  				      <text class="twoline-hide">{{ item.goodsName }}</text>
  				    </view>
  				    <!-- 商品价格 -->
  				    <view class="detail-price oneline-hide">
  				      <text class="goods-price f-30 col-m">￥{{ item.salePrice }}</text>
  				      <text v-if="item.linePrice > 0" class="line-price col-9 f-24">￥{{ item.linePrice }}</text>
  				    </view>
  				  </view>
  				</view>
  				  </view>
  				</view>
  				
  		</block>
  		

</template>

<script>
     import * as GoodsApi from '@/api/goods/goods'

  export default {
    components: {
      
    },
    props: {
      // 分割线名称
      title: {
        type: String,
        default: "店铺推荐"
      },
      
    },
    data() {
      return {
        // 推荐商品
        listRecommend: [],
        // 评价总数量
        total: 0
      }
    },

    created() {		
      // 推荐商品
      this.listGoodsRecommend()
    },

    methods: {

     // 获取页面数据
     listGoodsRecommend() {
      const app = this
      return new Promise((resolve, reject) => {
        GoodsApi.listByWhere({isRecommend:true})
          .then(res => {
            app.listRecommend = res.result;
            resolve(app.listGoodsRecommend)
          }).catch(reject)
      })
     },

    }
  }
</script>

<style lang="scss" scoped>
  
  .vdivider{
	  padding-top: 15rpx;
  }
  .divTitle{
	  font-weight: 600;
  }
  
  // 平铺显示
  .goods-list.column-2 {
    .goods-item {
      width: 50%;
    }
  }
  
  .goods-item {
    float: left;
    box-sizing: border-box;
    padding: 6rpx;
  
    .goods-image {
      position: relative;
      width: 100%;
      height: 0;
      padding-bottom: 100%;
      overflow: hidden;
      background: #fff;
  
      &:after {
        content: '';
        display: block;
        margin-top: 100%;
      }
  
      .image {
        position: absolute;
        width: 100%;
        height: 100%;
        top: 0;
        left: 0;
        -o-object-fit: cover;
        object-fit: cover;
      }
    }
  
    .detail {
      padding: 8rpx;
      background: #fff;
  
      .goods-name {
        min-height: 68rpx;
        line-height: 32rpx;
        white-space: normal;
        color: #484848;
        font-size: 26rpx;
      }
  
      .detail-price {
        .goods-price {
          margin-right: 8rpx;
        }
  
        .line-price {
          text-decoration: line-through;
        }
      }
    }
  }
</style>
