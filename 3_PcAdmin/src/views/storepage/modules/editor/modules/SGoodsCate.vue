<template>
  <a-spin :spinning="isLoading">
    <a-tree-select
      v-model="selectedId"
      @change="onChange"
      :treeData="categoryListTree"
      :dropdownStyle="{ maxHeight: '500px', overflow: 'auto' }"
      allowClear
    ></a-tree-select>
  </a-spin>
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import CategoryModel from '@/common/model/Category'

// 商品分类选择器组件
export default {
  name: 'SGoodsCate',
  components: {
  },
  model: {
    prop: 'value',
    event: 'change'
  },
  props: {
    value: PropTypes.integer.def(-1)
  },
  data () {
    return {
      // 正在加载
      isLoading: false,
      // 分类列表
      categoryListTree: [],
      // 选择的分类ID
      selectedId: -1
    }
  },
  watch: {
    value: {
      // 首次加载的时候执行函数
      immediate: true,
      handler (val) {
        this.selectedId = val
      }
    }
  },
  created () {
    // 获取分类列表
    this.getCategoryList()
  },
  methods: {

    // 获取分类列表
    getCategoryList () {
      this.isLoading = true
      CategoryModel.getListFromScreen()
        .then(selectList => this.categoryListTree = selectList)
        .finally(result => this.isLoading = false)
    },

    // 触发change事件
    onChange (value) {
      this.$emit('change', value)
    }

  }
}
</script>

<style lang="less" scoped>
.ant-select {
  width: 220px;
}
</style>
