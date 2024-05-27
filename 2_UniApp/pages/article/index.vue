<template>
  <mescroll-body ref="mescrollRef" :sticky="true" @init="mescrollInit" :down="{ use: false }" :up="upOption"
    @up="upCallback">

    <!-- tab栏 -->
    <u-tabs :list="tabList" :is-scroll="true" :current="curTab" active-color="#fd4a5f" :duration="0.2"
      @change="onChangeTab" />

    <!-- 文章列表 -->
    <view class="article-list">
      <view class="article-item" :class="[`show-type__${item.show_type}`]" v-for="(item, index) in articleList.data"
        :key="index" @click="onTargetDetail(item.article_id)">
        <!-- 小图模式 -->
        <block v-if="item.show_type == 10">
          <view class="article-item__left flex-box">
            <view class="article-item__title">
              <text class="twoline-hide">{{ item.title }}</text>
            </view>
            <view class="article-item__footer m-top10">
              <text class="article-views f-24 col-8">{{ item.show_views }}次浏览</text>
            </view>
          </view>
          <view class="article-item__image">
            <image class="image" mode="widthFix" :src="item.image_url"></image>
          </view>
        </block>
        <!-- 大图模式 -->
        <block v-if="item.show_type == 20">
          <view class="article-item__title">
            <text class="twoline-hide">{{ item.title }}</text>
          </view>
          <view class="article-item__image m-top20">
            <image class="image" mode="widthFix" :src="item.image_url"></image>
          </view>
          <view class="article-item__footer m-top10">
            <text class="article-views f-24 col-8">{{ item.show_views }}次浏览</text>
          </view>
        </block>
      </view>
    </view>
  </mescroll-body>
</template>

<script>
  import MescrollBody from '@/components/mescroll-uni/mescroll-body.vue'
  import MescrollMixin from '@/components/mescroll-uni/mescroll-mixins'
  import * as ArticleApi from '@/api/article'
  import * as CategoryApi from '@/api/article/category'
  import { getEmptyPaginateObj, getMoreListData } from '@/core/app'

  const pageSize = 15

  export default {
    components: {
      MescrollBody
    },
    mixins: [MescrollMixin],
    data() {
      return {
        // 选项卡列表
        tabList: [],
        // 当前选项
        curTab: 0,
        // 文章列表
        articleList: getEmptyPaginateObj(),
        // 上拉加载配置
        upOption: {
          // 首次自动执行
          auto: true,
          // 每页数据的数量; 默认10
          page: { size: pageSize },
          // 数量要大于3条才显示无更多数据
          noMoreSize: 3,
        }
      }
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {
      const app = this
      // 获取文章分类数据
      app.getCategoryList(options.categoryId)
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
        app.getArticleList(page.num)
          .then(list => {
			const curlimit = list.data.length
			app.mescroll.endBySize(curlimit, list.count)
          })
          .catch(() => app.mescroll.endErr())
      },

      // 获取文章分类数据
      getCategoryList(categoryId) {
        CategoryApi.list().then(result => {
          this.setTabList(result.data.list, categoryId)
        })
      },

      // 设置选项卡数据
      setTabList(categoryList, categoryId) {
        const app = this
        app.tabList = [{ value: 0, name: '全部' }]
        categoryList.forEach(item => {
          app.tabList.push({ value: item.category_id, name: item.name })
        })
        if (categoryId > 0) {
          const findIndex = app.tabList.findIndex(item => item.value == categoryId)
          app.curTab = findIndex > -1 ? findIndex : 0
        }
      },

      /**
       * 获取文章列表
       * @param {Number} pageNo 页码
       */
      getArticleList(pageNo = 1) {
        const app = this
        return new Promise((resolve, reject) => {
          ArticleApi.list({ categoryId: app.getTabValue(), page: pageNo }, { load: false })
            .then(result => {
			   // 合并新数据
			   const newList = res.result
			   app.list.count=res.count
			   app.list.data = getMoreListData(newList, app.list, pageNo)
              resolve(app.list)
            })
            .catch(reject)
        })
      },

      // 切换标签项
      onChangeTab(index) {
        // 设置当前选中的标签
        this.curTab = index
        // 刷新订单列表
        this.onRefreshList()
      },

      // 获取当前标签项的值
      getTabValue() {
        const app = this
        return app.tabList.length ? app.tabList[app.curTab].value : 0
      },

      // 刷新列表数据
      onRefreshList() {
        this.articleList = getEmptyPaginateObj()
        setTimeout(() => this.mescroll.resetUpScroll(), 120)
      },

      // 跳转文章详情页
      onTargetDetail(articleId) {
        this.$navTo('pages/article/detail', { articleId })
      }

    },

    /**
     * 分享当前页面
     */
    onShareAppMessage() {
      return {
        title: '文章首页',
        path: "/pages/article/index?" + this.$getShareUrlParams()
      }
    },

    /**
     * 分享到朋友圈
     * 本接口为 Beta 版本，暂只在 Android 平台支持，详见分享到朋友圈 (Beta)
     * https://developers.weixin.qq.com/miniprogram/dev/framework/open-ability/share-timeline.html
     */
    onShareTimeline() {
      return {
        title: '文章首页',
        path: "/pages/article/index?" + this.$getShareUrlParams()
      }
    }

  }
</script>

<style lang="scss" scoped>
  .container {
    min-height: 100vh;
  }

  // 文章列表
  .article-list {
    padding-top: 20rpx;
    line-height: 1;
    background: #f7f7f7;
  }


  .article-item {
    margin-bottom: 20rpx;
    padding: 30rpx;
    background: #fff;

    &:last-child {
      margin-bottom: 0;
    }

    .article-item__title {
      max-height: 74rpx;
      font-size: 28rpx;
      line-height: 38rpx;
      color: #333;
    }

    .article-item__image .image {
      display: block;
    }
  }

  // 小图模式
  .show-type__10 {
    display: flex;

    .article-item__left {
      padding-right: 20rpx;
    }

    .article-item__title {
      // min-height: 72rpx;
    }

    .article-item__image .image {
      width: 240rpx;
    }
  }

  // 大图模式
  .show-type__20 .article-item__image .image {
    width: 100%;
  }
</style>
