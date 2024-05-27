<template>
  <view v-if="list.length > 0" class="secondary">
    <!-- 二级分类 20 -->
    <view class="cate-content">
      <!-- 左侧 一级分类 -->
      <scroll-view class="cate-left" :scroll-y="true" :style="{ height: `${scrollHeight}px` }">
        <text class="type-nav" :class="{ selected: curIndex == index }" v-for="(item, index) in list" :key="index"
          @click="handleSelectNav(index)">{{ item.name }}</text>
      </scroll-view>
      <!-- 右侧 二级分类 -->
      <scroll-view class="cate-right" :scroll-top="scrollTop" :scroll-y="true" :style="{ height: `${scrollHeight}px` }">
        <view v-if="list[curIndex]" class="cate-right-cont">
          <view class="cate-two-box">
            <view class="cate-cont-box">
              <view class="flex-three" v-for="(item, idx) in list[curIndex].children" :key="idx" @click="onTargetGoodsList(item.category_id)">
                <view class="cate-img-padding">
                  <view v-if="item.image" class="cate-img">
                    <image class="image" mode="scaleToFill" :src="item.image.preview_url"></image>
                  </view>
                </view>
                <text class="name oneline-hide">{{ item.name }}</text>
              </view>
            </view>
          </view>
        </view>
      </scroll-view>
    </view>
    <empty v-if="!list.length" />
  </view>
</template>

<script>
  import { PageCategoryStyleEnum } from '@/common/enum/store/page/category'
  import Empty from '@/components/empty'
  import { rpx2px } from '@/utils/util'

  export default {
    components: {
      Empty
    },
    props: {
      // 分类列表
      list: {
        type: Array,
        default: []
      },
    },
    data() {
      return {
        // 枚举类
        PageCategoryStyleEnum,
        // 列表高度
        scrollHeight: 0,
        // 一级分类：指针
        curIndex: 0,
        // 内容区竖向滚动条位置
        scrollTop: 0,
      }
    },
    created() {
      // 设置分类列表高度
      this.setListHeight()
    },
    methods: {

      // 设置列表内容的高度
      setListHeight() {
        const { windowHeight } = uni.getSystemInfoSync()
        this.scrollHeight = windowHeight - rpx2px(96)
      },

      // 一级分类：选中分类
      handleSelectNav(index) {
        this.curIndex = index
        this.scrollTop = 0
      },

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
    display: flex;
    z-index: 1;
    background: #fff;
    padding-top: 96rpx;
  }

  // 一级分类+二级分类 20
  .cate-left {
    height: 100%;
    display: flex;
    flex-direction: column;
    flex: 0 0 23%;
    background: #f8f8f8;
    color: #444;
  }

  .cate-right {
    display: flex;
    flex-direction: column;
    height: 100%;
    overflow: hidden;

    .cate-right-cont {
      width: 100%;
      display: flex;
      flex-flow: row wrap;
      align-content: flex-start;
      padding-top: 15rpx;

      .cate-two-box {
        width: 100%;
        padding: 0 10px;
      }
    }
  }

  // 左侧一级分类
  .type-nav {
    position: relative;
    height: 90rpx;
    z-index: 10;
    display: block;
    font-size: 26rpx;
    display: flex;
    justify-content: center;
    align-items: center;

    &.selected {
      background: #fff;
      color: #fa2209;
      border-right: none;
      font-size: 28rpx;
    }
  }

  // 右侧二级分类
  .cate-cont-box {
    margin-bottom: 30rpx;
    padding-bottom: 10rpx;
    background: #fff;
    overflow: hidden;

    .name {
      display: block;
      padding-bottom: 30rpx;
      text-align: center;
      font-size: 26rpx;
      color: #444444;
    }

    .cate-img-padding {
      padding: 16rpx 16rpx 4rpx 16rpx;
    }

    .cate-img {
      position: relative;
      width: 100%;
      padding-top: 100%;

      .image {
        width: 100%;
        height: 100%;
        position: absolute;
        top: 0;
        left: 0;
        border-radius: 10rpx;
      }
    }

  }
</style>
