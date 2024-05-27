<template>
  <div class="container">
    <a-spin :spinning="isLoading">
      <!-- 工作区 -->
      <div class="work-content">
        <!-- 组件库 -->
        <Components @handleClickItem="onAddItem" />
        <!--手机容器-->
        <Phone v-if="!isLoading" :data="data" :selectedIndex="selectedIndex" @onEditer="onEditer" @onDeleleItem="onDeleleItem" />
        <!-- 编辑器 -->
        <Editor v-if="!isLoading" :defaultData="defaultData" :data="data" :selectedIndex="selectedIndex" :curItem="curItem" />
      </div>
      <!-- 操作栏 -->
      <div class="footer">
        <div class="footer-content">
          <a-button type="primary" :loading="isLoading" @click="onFormSubmit">保存</a-button>
        </div>
      </div>
    </a-spin>
  </div>
</template>

<script>
import _ from 'lodash'
import { inArray } from '@/utils/util'
import * as Api from '@/api/store/storepage'

import { Components, Phone, Editor } from './modules'
import { PageTypeEnum } from '@/common/enum/page'

export default {
  components: {
    // SelectImage,
    // draggable,
    Components,
    Phone,
    Editor,
  },
  data() {
    return {
      // loading状态
      isLoading: false,
      // 页面装修默认数据
      defaultData: {},
      // 当前页面数据
      data: { page: {}, items: [] },
      // 当前选中的元素索引
      selectedIndex: 'page',
      // 当前选中的元素
      curItem: {},
      // 当前页面ID
      id: null,
    }
  },
  // 初始化数据
  created() {
    // 记录页面ID
    this.id = this.$route.params && this.$route.params.id
    // 获取初始化数据
    this.initData()
  },
  methods: {
    // 获取初始化数据
    initData() {
      this.isLoading = true
      Promise.all([
        // 获取默认数据
        this.getDefaultData(),
        // 获取当前页面数据
        this.getDetail(),
      ]).then(() => {
        this.isLoading = false
      })
    },

    // 获取默认数据
    getDefaultData() {
      return new Promise((resolve, reject) => {
        Api.defaultData().then((res) => {
          this.defaultData = res.result
          resolve()
        })
      })
    },

    // 获取当前页面数据
    getDetail() {
      return new Promise((resolve, reject) => {
        Api.getDetail({ id: this.id }).then((res) => {
          this.data = res.result.pageDataObj
          resolve()
        })
      })
    },

    /**
     * 新增Diy组件
     * @param type
     */
    onAddItem(type) {
      // 验证新增Diy组件
      if (!this.onCheckAddItem(type)) {
        return false
      }
      const { defaultData, data } = this
      // 复制默认diy组件数据
      const newItem = _.cloneDeep(defaultData.items[type])
      // 新增到diy列表数据
      data.items.push(newItem)
      // 编辑当前选中的元素
      this.onEditer(data.items.length - 1)
    },

    /**
     * 验证新增Diy组件
     * @param type
     */
    onCheckAddItem(type) {
      const { data } = this
      // 验证xx组件只能存在一个
      if (type === 'xxx') {
        const itemsTypes = data.items.map((item) => item.type)
        if (inArray(type, itemsTypes)) {
          this.$message.warning('该组件最多存在一个')
          return false
        }
      }
      return true
    },

    /**
     * 编辑当前选中的Diy元素
     * @param index
     */
    onEditer(index) {
      const { data } = this
      // 记录当前选中元素的索引
      this.selectedIndex = index
      // 当前选中的元素数据
      var curItem = index === 'page' ? data.page : data.items[index]

      this.curItem = curItem
    },

    /**
     * 删除diy元素
     * @param index
     */
    onDeleleItem(index) {
      const {
        data: { items },
      } = this
      items.splice(index, 1)
      this.selectedIndex = -1
    },

    /**
     * 编辑器：重置颜色
     * @param holder
     * @param attribute
     * @param color
     */
    onEditorResetColor(holder, attribute, color) {
      holder[attribute] = color
    },

    // 提交到后端api
    onFormSubmit() {
      this.isLoading = true
      const { data, $message } = this

      console.log('pageType.HOME.value,', PageTypeEnum.HOME.value)
      const id = this.$route.params && this.$route.params.id
      Api.addOrUpdate({
        id: id,
        page: data.page,
        items: data.items,
      })
        .then((result) => {
          // 显示成功
          $message.success(result.message, 1.5)
          // 跳转到列表页
          //   setTimeout(() => {
          //     this.$router.push('/storepage/index')
          //   }, 1200)
        })
        .finally(() => {
          setTimeout(() => {
            this.isLoading = false
          }, 1500)
        })
    },
  },
}
</script>
<style lang="less" scoped>
@import "./style.less";
</style>