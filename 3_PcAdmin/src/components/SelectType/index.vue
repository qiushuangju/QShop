<template>
  <div>
    <span v-if="!isEdit">{{labelName}}</span>
    <el-select v-else :size="size" :disabled="disabled" :value="value" @change="handleChange">
      <el-option v-for="(item, index) in typeDatas" :key="index" :value="item[defaultProps.value]" :label="item[defaultProps.label]"></el-option>
    </el-select>
  </div>
</template>

<script>
  import * as categorys from '@/api/categorys'
  import { mapGetters, mapActions } from 'vuex'

  export default {
    name: 'select-type',
    components: {},
    props: {
      typeId: [String, Number],
      value: {
        type: [String, Number],
        default: ''
      },
      disabled: {
        type: Boolean,
        default: false
      },
      isEdit: {
        type: Boolean,
        default: true
      },
      size: {
        type: String,
        default: 'mini'
      },
      defaultProps: {
        type: Object,
        default() {
          return {
            label: 'name',
            value: 'dtCode'
          }
        }
      }
    },
    data() {
      return {
      }
    },
    destroyed() {
      this.clearTypeDatas()
    },
    computed: {
      ...mapGetters({
        typeDataLists: 'typeDataLists',
        typeIds: 'typeIds'
      }),
      typeDatas() {
        const object = this.typeDataLists.length > 0 && this.typeDataLists.find(item => item.typeId === this.typeId)
        return object && object.typeDatas || []
      },
      labelName() {
        const item = this.typeDatas.find(item => item[this.defaultProps.value] === this.value)
        return item && item[this.defaultProps.label] || this.value
      }
    },
    watch: {
      typeId() {
        this.getList()
      }
    },
    created() {
      this.initView()
    },
    methods: {
      ...mapActions({
        saveTypeDataLists: 'saveTypeDataLists',
        saveTypeIds: 'saveTypeIds',
        clearTypeDatas: 'clearTypeDatas'
      }),
      initView() {
        const type = this.typeIds.find(item => item === this.typeId)
        this.saveTypeIds(this.typeId)
        if (type) {
          return
        }
        this.getList()
      },
      getList() {
        const listQuery = {
          page: 1,
          limit: 9999,
          TypeId: this.typeId
        }
        return categorys.getList(listQuery).then(res => {
          const obj = {
            typeId: this.typeId,
            typeDatas: res.data
          }
          this.saveTypeDataLists(obj)
        })
      },
      handleChange(val) {
        this.$emit('input', val)
      }
    }
  }

</script>

<style scoped>
</style>
