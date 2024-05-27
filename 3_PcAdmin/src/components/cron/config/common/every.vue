<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      <span class="cell-symbol">{{ tag_ }}</span> {{ $t('common.every') }}{{ timeUnit }}
    </el-radio>
  </div>
</template>

<script>
import { EVERY } from '@/components/cron/constant/filed'
import watchValue from '@/components/cron/mixins/watchValue'

export default {
  mixins: [watchValue],
  props: {
    size: {
      type: String,
      default: 'mini'
    },
    timeUnit: {
      type: String,
      default: null
    },
    symbol: {
      type: String,
      default: null
    },
    type: {
      type: String,
      default: EVERY
    },
    tag: {
      type: String,
      default: EVERY
    }
  },
  data() {
    return {
      label: EVERY,
      type_: this.type,
      proxy: this.tag
    }
  },
  computed: {
    tag_: {
      get() {
        return EVERY
      },
      set() {
        if (this.type_ !== EVERY) {
          return
        }
        this.proxy = EVERY
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
