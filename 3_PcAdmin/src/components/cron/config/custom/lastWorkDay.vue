<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      <span class="cell-symbol">{{ tag_ }}</span>
      {{ $t('common.current') }}{{ targetTimeUnit }}{{ $t('custom.latestWorkday') }}
    </el-radio>
  </div>
</template>

<script>
import { LAST_WORK_DAY } from '@/components/cron/constant/filed'
import watchValue from '@/components/cron/mixins/watchValue'

export default {
  mixins: [watchValue],
  props: {
    lastWorkDayConfig: {
      type: Object,
      default: null
    },
    size: {
      type: String,
      default: 'mini'
    },
    targetTimeUnit: {
      type: String,
      default: null
    },
    type: {
      type: String,
      default: LAST_WORK_DAY
    },
    tag: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      label: LAST_WORK_DAY,
      type_: this.type,
      proxy: this.tag
    }
  },
  computed: {
    tag_: {
      get() {
        return LAST_WORK_DAY
      },
      set() {
        if (this.type_ !== LAST_WORK_DAY) {
          return
        }
        this.proxy = LAST_WORK_DAY
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
