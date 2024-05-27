<template>
  <iframe :v-model="sValue" id="map" src="static/getpoint/index.html" width="915" height="610"></iframe>
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'

// 省市区级联选择器组件
export default {
  name: 'Getpoint',
  model: {
    prop: 'value',
    event: 'change'
  },
  props: {
    // v-model 指定选中项
    value: PropTypes.array.def(),
    address:PropTypes.string.def(),
  },
  data () {
    return {
      // 选中项
      sValue: [],
      sAddress:''
    }
  },
  watch: {
      value:{
        immediate: true,
        handler (val) {
          this.sValue = val
        }
      },
      address:{
        immediate: true,
        handler (val) {
          this.sAddress = val
        }
      }
  },
  created () {
    window.setCoordinate = this.setCoordinate
    window.setAddress=this.setAddress
  },
  methods: {
    setCoordinate (coordinate) {
      this.$emit('setCoordinate', coordinate)
    },
    setAddress(address){
      this.$emit('setAddress', address)
    },
    


  }
}
</script>

<style lang="less" scoped>
</style>
