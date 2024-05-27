<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      <span class="cell-symbol">{{ tag_ }}</span>
      {{ $t('common.current') }}{{ targetTimeUnit }}{{ $t('custom.lastOne') }}
      <el-select
        v-model="lastNum"
        :size="size"
        :placeholder="$t('common.placeholder')"
        filterable>
        <el-option
          v-for="item in nums"
          :key="item.value"
          :label="item.label"
          :value="item.value"/>
      </el-select>
    </el-radio>
  </div>
</template>

<script>
import { LAST } from '@/components/cron/constant/filed'
import watchValue from '@/components/cron/mixins/watchValue'
import { isNumber } from '@/components/cron/util/tools'

export default {
  mixins: [watchValue],
  props: {
    nums: {
      type: Array,
      default: null
    },
    size: {
      type: String,
      default: 'mini'
    },
    timeUnit: {
      type: String,
      default: null
    },
    targetTimeUnit: {
      type: String,
      default: null
    },
    type: {
      type: String,
      default: LAST
    },
    tag: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      label: LAST,
      type_: this.type,
      lastNum: 7
    }
  },
  computed: {
    tag_: {
      get() {
        return (this.lastNum >= 1 && this.lastNum < 7 ? this.lastNum : '') + LAST
      },
      set(newValue) {
        if (this.type_ !== LAST) {
          return
        }
        if (newValue === LAST) {
          this.lastNum = 7
          return
        }
        const numStr = newValue.substring(0, newValue.length - 1)
        if (!isNumber(numStr) || parseInt(numStr) < this.nums[0].value || parseInt(numStr) > this.nums[this.nums.length - 1].value) {
          this.$message.error(this.$t('common.numError') + ':' + numStr)
          return
        }
        this.lastNum = parseInt(numStr)
      }
    }
  },
  methods: {
    change() {
      this.$emit('type-changed', this.type_)
      this.$emit('tag-changed', this.tag_)
    }
  }
}
</script>
