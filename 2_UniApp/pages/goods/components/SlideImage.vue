<template>
  <!-- 商品图片 -->
  <view class="images-swiper">
    <swiper class="swiper-box" :autoplay="autoplay" :duration="duration" :indicator-dots="indicatorDots"
      :interval="interval" :circular="true" @change="setCurrent">
      <!-- 主图视频 -->
      <swiper-item v-if="video">
        <view class="slide-video">
          <video id="myVideo" class="video" :poster="videoCover.preview_url" :src="video.external_url" controls
            x5-playsinline playsinline webkit-playsinline webkit-playsinline x5-video-player-type="h5"
            x5-video-player-fullscreen x5-video-orientation="portrait" :enable-progress-gesture="false"
            @play="onVideoPlay"></video>
        </view>
      </swiper-item>
      <!-- 轮播图片 -->
      <swiper-item v-for="(item, index) in images" :key="index" @click="onPreviewImages(index)">
        <view class="slide-image">
          <image class="image" :draggable="false" :src="item.filePath"></image>
        </view>
      </swiper-item>
    </swiper>
    <view class="swiper-count">
      <text>{{ currentIndex }}</text>
      <text>/</text>
      <text>{{ images.length + (video ? 1 : 0) }}</text>
    </view>
  </view>
</template>

<script>
  export default {
    props: {
      // 主图视频
      video: {
        type: Object,
        default () {
          return null
        }
      },
      // 主图视频封面
      videoCover: {
        type: Object,
        default () {
          return null
        }
      },
      // 图片轮播
      images: {
        type: Array,
        default: []
      }
    },
    data() {
      return {
        indicatorDots: true, // 是否显示面板指示点
        autoplay: true, // 是否自动切换
        interval: 4000, // 自动切换时间间隔
        duration: 800, // 滑动动画时长
        currentIndex: 1, // 轮播图指针
      }
    },

    methods: {

      // 事件：视频开始播放
      onVideoPlay(e) {
        this.autoplay = false
      },

      // 设置轮播图当前指针 数字
      setCurrent({ detail }) {
        const app = this
        app.currentIndex = detail.current + 1
      },

      // 浏览商品图片
      onPreviewImages(index) {
        const app = this
        const imageUrls = []
        app.images.forEach(item => {
          imageUrls.push(item.preview_url);
        });
        uni.previewImage({
          current: imageUrls[index],
          urls: imageUrls
        })
      }

    }
  }
</script>

<style lang="scss" scoped>
  // swiper组件
  .images-swiper {
    position: relative;
  }

  .swiper-box {
    width: 100%;
    height: 100vw;

    /* #ifdef H5 */
    max-width: 480px;
    max-height: 480px;
    margin: 0 auto;
    /* #endif */

    // 主图视频
    .slide-video {
      width: 100%;
      height: 100%;

      .video {
        display: block;
        width: 100%;
        height: 100%;
      }
    }

    // 图片轮播
    .slide-image {
      position: relative;
      width: 100%;
      height: 100%;

      .image {
        display: block;
        width: 100%;
        height: 100%;
      }
    }
  }

  // swiper计数
  .swiper-count {
    position: absolute;
    right: 36rpx;
    bottom: 72rpx;
    padding: 2rpx 18rpx;
    background: rgba(0, 0, 0, 0.363);
    border-radius: 50rpx;
    color: #fff;
    font-size: 26rpx;
  }
</style>
