<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      <span class="cell-symbol">{{ tag_ }}</span>
      {{ $t('common.from') }}
      <el-select
        v-model="start"
        :size="size"
        :placeholder="$t('common.placeholder')"
        style="width: 100px;"
        filterable>
        <el-option
          v-for="item in nums"
          :key="item.value"
          :label="item.label"
          :value="item.value"/>
      </el-select>
      {{ $t('common.start') }}{{ $t('common.every') }}
      <el-input-number v-model="cycle" :precision="0" :min="cycleConfig.min" :step="cycleConfig.step" :max="cycleConfig.max" :size="size"/>
      {{ timeUnit }}
    </el-radio>
  </div>
</template>

<script>
import { PERIOD } from '@/components/cron/constant/filed'
import watchValue from '@/components/cron/mixins/watchValue'
import { isNumber } from '@/components/cron/util/tools'

export default {
  mixins: [watchValue],
  props: {
    nums: {
      type: Array,
      default: null
    },
    startConfig: {
      type: Object,
      default: (val) => {
        return val || { min: 1, max: 7 }
      }
    },
    cycleConfig: {
      type: Object,
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
    type: {
      type: String,
      default: PERIOD
    },
    tag: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      label: PERIOD,
      type_: this.type,
      start: 1,
      cycle: 1
    }
  },
  computed: {
    tag_: {
      get() {
        return this.start + PERIOD + this.cycle
      },
      set(newValue) {
        if (this.type_ !== PERIOD) {
          return
        }
        const arr = newValue.split(PERIOD)
        if (arr.length !== 2) {
          this.$message.error(this.$t('common.tagError') + ':' + newValue)
          return
        }
        if (!isNumber(arr[0]) || parseInt(arr[0]) < this.startConfig.min || parseInt(arr[0]) > this.startConfig.max) {
          this.$message.error(this.$t('period.startError') + ':' + arr[0])
          return
        }
        if (!isNumber(arr[1]) || parseInt(arr[1]) < this.cycleConfig.min || parseInt(arr[1]) > this.cycleConfig.max) {
          this.$message.error(this.$t('period.cycleError') + ':' + arr[1])
          return
        }
        this.start = parseInt(arr[0])
        this.cycle = parseInt(arr[1])
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
