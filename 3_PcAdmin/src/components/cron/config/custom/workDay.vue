<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      <span class="cell-symbol">{{ tag_ }}</span>
      {{ $t('common.every') }}{{ targetTimeUnit }}
      <el-input-number v-model="startDate" :precision="0" :min="startDateConfig.min" :step="startDateConfig.step" :max="startDateConfig.max" :size="size"/>
      {{ timeUnit }}{{ $t('common.nearest') }}{{ $t('custom.workDay') }}
    </el-radio>
  </div>
</template>

<script>
import { WORK_DAY } from '@/components/cron/constant/filed'
import watchValue from '@/components/cron/mixins/watchValue'
import { isNumber } from '@/components/cron/util/tools'

export default {
  mixins: [watchValue],
  props: {
    startDateConfig: {
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
      default: WORK_DAY
    },
    tag: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      label: WORK_DAY,
      type_: this.type,
      startDate: 1
    }
  },
  computed: {
    tag_: {
      get() {
        return this.startDate + WORK_DAY
      },
      set(newValue) {
        if (this.type_ !== WORK_DAY) {
          return
        }
        const num = newValue.substring(0, newValue.length - WORK_DAY.length)
        if (!isNumber(num) || parseInt(num) < this.startDateConfig.min || parseInt(num) > this.startDateConfig.max) {
          this.$message.error(this.$t('common.numError') + ':' + num)
          return
        }
        this.startDate = num
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
