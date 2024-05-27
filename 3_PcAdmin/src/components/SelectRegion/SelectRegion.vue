<template>
  <a-cascader v-model="sValue" :options="options" :placeholder="placeholder" @change="onChange" />
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import RegionModel from '@/common/model/Region'

// 省市区级联选择器组件
export default {
  name: 'SelectRegion',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    // v-model 指定选中项
    value: PropTypes.array.def(),
    // 元素的尺寸(宽)
    placeholder: PropTypes.string.def('请选择省市区'),
  },
  data() {
    return {
      // 选中项
      sValue: [],
      // 级联选择器数据
      options: [],
    }
  },
  watch: {
    value(val) {
      this.sValue = val
    },
  },
  created() {
    // 获取地区数据
    RegionModel.getTreeData().then((regions) => {
      this.options = regions

      // this.getOptions(regions)
    })
  },
  methods: {
    // 触发change事件
    onChange(value) {
      this.$emit('change', value)
    },

    // /**
    //  * 格式化级联选择器数据
    //  * @param {*} regions 地区数据
    //  */
    // getOptions(regions) {
    //     const { getOptions, getChildren } = this
    //     const options = []
    //     for (const index in regions) {
    //         const item = regions[index]
    //         const children = getChildren(item)
    //         const optionItem = {
    //             value: item.id,
    //             label: item.name,
    //         }
    //         if (children !== false) {
    //             optionItem.children = getOptions(children)
    //         }
    //         options.push(optionItem)
    //     }
    //     return options
    // },

    // // 获取子集地区
    // getChildren(item) {
    //     if (item.city) {
    //         return item.city
    //     }
    //     if (item.region) {
    //         return item.region
    //     }
    //     return false
    // },
  },
}
</script>

<style lang="less" scoped>
</style>
