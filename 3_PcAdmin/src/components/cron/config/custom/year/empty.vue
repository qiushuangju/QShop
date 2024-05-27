<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      &nbsp;{{ $t('custom.empty') }}
    </el-radio>
  </div>
</template>

<script>
import { EMPTY } from '@/components/cron/constant/filed'
import watchValue from '@/components/cron/mixins/watchValue'

export default {
  mixins: [watchValue],
  props: {
    type: {
      type: String,
      default: EMPTY
    },
    tag: {
      type: String,
      default: EMPTY
    }
  },
  data() {
    return {
      label: EMPTY,
      type_: this.type,
      proxy: this.tag
    }
  },
  computed: {
    tag_: {
      get() {
        return EMPTY
      },
      set() {
        if (this.type_ !== EMPTY) {
          return
        }
        this.proxy = EMPTY
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
