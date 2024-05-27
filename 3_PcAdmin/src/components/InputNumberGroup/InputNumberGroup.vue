<template>
  <div class="input-number-group">
    <span class="ant-input-group-wrapper">
      <span class="ant-input-wrapper ant-input-group">
        <span v-if="addonBefore" class="ant-input-group-addon">{{ addonBefore }}</span>
        <a-input-number v-bind="inputProps" v-model="sValue" @change="onChange" />
        <span v-if="addonAfter" class="ant-input-group-addon">{{ addonAfter }}</span>
      </span>
    </span>
  </div>
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'

// 数字输入框组
export default {
  name: 'InputNumberGroup',
  model: {
    prop: 'value',
    event: 'change'
  },
  props: {
    // v-model 当前输入框值
    value: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
    // 设置前置标签
    addonBefore: PropTypes.string.def(''),
    // 设置后置标签
    addonAfter: PropTypes.string.def(''),
    // input-number props属性
    inputProps: PropTypes.object.def({})
  },
  data () {
    return {
      // 当前值
      sValue: ''
    }
  },
  watch: {
    value: {
      // 首次加载的时候执行函数
      immediate: true,
      handler (val) {
        this.sValue = val
      }
    }
  },
  methods: {

    // 触发change事件
    onChange (value) {
      this.$emit('change', value)
    }

  }
}
</script>

<style lang="less" scoped>
.ant-input-group-wrapper {
  width: auto !important;
}
</style>
