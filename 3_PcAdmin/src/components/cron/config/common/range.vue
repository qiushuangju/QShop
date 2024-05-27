<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      <span class="cell-symbol">{{ tag_ }}</span>
      {{ $t('common.between') }}
      <el-input-number v-model="lower" :precision="0" :min="lowerConfig.min" :step="lowerConfig.step" :max="upper_" :size="size"/>
      {{ timeUnit }}{{ $t('common.and') }}
      <el-input-number v-model="upper_" :precision="0" :min="lower" :step="upperConfig.step" :max="upperConfig.max" :size="size"/>
      {{ $t('common.end') }}{{ $t('common.every') }}{{ timeUnit }}
    </el-radio>
  </div>
</template>

<script>
import { RANGE } from '@/components/cron/constant/filed'
import watchValue from '@/components/cron/mixins/watchValue'
import { isNumber } from '@/components/cron/util/tools'

export default {
  mixins: [watchValue],
  props: {
    upper: {
      type: Number,
      default: 1
    },
    lowerConfig: {
      type: Object,
      default: null
    },
    upperConfig: {
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
      default: RANGE
    },
    tag: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      label: RANGE,
      type_: this.type,
      lower: 0,
      upper_: this.upper
    }
  },
  computed: {
    tag_: {
      get() {
        return this.lower + RANGE + this.upper_
      },
      set(newValue) {
        if (this.type_ !== RANGE) {
          return
        }
        const arr = newValue.split(RANGE)
        if (arr.length !== 2) {
          this.$message.error(this.$t('common.tagError') + ':' + newValue)
          return
        }
        if (!isNumber(arr[0]) || parseInt(arr[0]) < this.lowerConfig.min || parseInt(arr[0]) > this.lowerConfig.max) {
          this.$message.error(this.$t('range.lowerError') + ':' + arr[0])
          return
        }
        if (!isNumber(arr[1]) || parseInt(arr[1]) < this.upperConfig.min || parseInt(arr[1]) > this.upperConfig.max) {
          this.$message.error(this.$t('range.upperError') + ':' + arr[1])
          return
        }
        if (parseInt(arr[0]) > parseInt(arr[1])) {
          this.$message.error(this.$t('range.lowerBiggerThanUpperError') + ':' + arr[0] + '>' + arr[1])
          return
        }
        this.lower = parseInt(arr[0])
        this.upper_ = parseInt(arr[1])
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
