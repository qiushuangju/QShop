<template>
  <view class="primary">
    <!-- 一级分类(大图) 10 -->
    <view v-if="list.length > 0 && display == PageCategoryStyleEnum.One_Level_Big.value" class="cate-content">
      <view class="cate-wrapper cate_style__10 clearfix">
        <view class="cate-item" v-for="(item, index) in list" :key="index" @click="onTargetGoodsList(item.category_id)">
          <image v-if="item.image" class="image" mode="widthFix" :src="item.image.preview_url"></image>
        </view>
      </view>
    </view>
    <!-- 一级分类(小图) 11 -->
    <view v-if="list.length > 0 && display == PageCategoryStyleEnum.One_Level_Small.value" class="cate-content">
      <view class="cate-wrapper cate_style__11 clearfix">
        <view class="cate-item" v-for="(item, index) in list" :key="index" @click="onTargetGoodsList(item.category_id)">
          <image v-if="item.image" class="image" mode="widthFix" :src="item.image.preview_url"></image>
          <view class="cate-name">{{ item.name }}</view>
        </view>
      </view>
    </view>
    <empty v-if="!list.length" :tips="'亲，暂无商品分类' + display" />
  </view>
</template>

<script>
  import { PageCategoryStyleEnum } from '@/common/enum/store/page/category'
  import Empty from '@/components/empty'

  export default {
    components: {
      Empty
    },
    props: {
      // 分类页样式
      display: {
        type: Number,
        default: 10
      },
      // 分类列表
      list: {
        type: Array,
        default: []
      }
    },
    data() {
      return {
        // 枚举类
        PageCategoryStyleEnum,
      }
    },

    methods: {

      // 跳转至商品列表页
      onTargetGoodsList(categoryId) {
        this.$navTo('pages/goods/list', { categoryId })
      }

    }
  }
</script>

<style lang="scss" scoped>
  // 分类内容
  .cate-content {
    z-index: 1;
    background: #fff;
    padding-top: 96rpx;

    .cate-wrapper {
      padding: 0 20rpx 20rpx 20rpx;
      box-sizing: border-box;
    }
  }

  // 一级分类(大图) 10
  .cate_style__10 .cate-item {
    margin-bottom: 20rpx;

    &:last-child {
      margin-bottom: 0;
    }

    .image {
      display: block;
      width: 100%;
      height: auto;
    }
  }

  // 一级分类(小图) 11
  .cate_style__11 .cate-item {
    float: left;
    padding: 25rpx;
    width: 33.3333%;
    text-align: center;
    box-sizing: border-box;

    .image {
      display: block;
      width: 100%;
      height: 33vw;
      margin-bottom: 12rpx;
    }

    .cate-name {
      font-size: 28rpx;
      color: #555;
    }
  }
</style>
