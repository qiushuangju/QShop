<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      <span class="cell-symbol">{{ tag_ }}</span>
      {{ $t('common.current') }}{{ targetTimeUnit }}{{ $t('custom.lastTh') }}
      <el-input-number v-model="lastNum" :precision="0" :min="lastConfig.min" :step="lastConfig.step" :max="lastConfig.max" :size="size"/>
      {{ timeUnit }}
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
    lastConfig: {
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
      lastNum: 1
    }
  },
  computed: {
    tag_: {
      get() {
        return this.lastNum === 1 ? LAST : LAST + '-' + (this.lastNum - 1)
      },
      set(newValue) {
        if (this.type_ !== LAST) {
          return
        }
        if (newValue === LAST) {
          this.lastNum = 1
          return
        }
        const num = newValue.substring(2, newValue.length - 1)
        if (!isNumber(num) || parseInt(num) < this.lastConfig.min - 1 || parseInt(num) > this.lastConfig.max - 1) {
          this.$message.error(this.$t('common.numError') + ':' + num)
          return
        }
        this.lastNum = num + 1
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
