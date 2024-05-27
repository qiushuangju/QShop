<template>
  <div>
    <el-popover v-model="visible">
      <div style="text-align: right; margin: 0">
        <i class="el-icon-close" @click="visible = false" style="cursor:pointer;"></i>
      </div>
      <cron v-model="cron_" :size="size" @change="change"/>
      <el-input readonly @change="setCron" slot="reference" :value="value" @input="$emit('input', $event.target.value)" :placeholder="$t('common.inputPlaceholder')" :size="size">
        <el-button slot="append" icon="el-icon-refresh" @click="reset"/>
      </el-input>
    </el-popover>
  </div>
</template>

<script>
import Cron from '@/components/cron/cron'
import { DEFAULT_CRON_EXPRESSION } from '@/components/cron/constant/filed'
import Vue from 'vue'
import translate from '@/components/cron/mixins/plugins/translate'
Vue.use(translate)
export default {
  name: 'CronInput',
  components: {
    Cron
  },
  props: {
    value: {
      type: String,
      default: DEFAULT_CRON_EXPRESSION
    },
    size: {
      type: String,
      default: 'mini'
    }
  },
  data() {
    return {
      cron_: '',
      visible: false
    }
  },
  watch: {
    value(val) {
      this.setCron(val || DEFAULT_CRON_EXPRESSION)
    }
  },
  created() {
    this.setCron(this.value || DEFAULT_CRON_EXPRESSION)
  },
  methods: {
    setCron(newValue) {
      this.cron_ = newValue
    },
    change(cron) {
      this.cron_ = cron
      this.$emit('input', cron)
    },
    reset() {
      this.$emit('input', DEFAULT_CRON_EXPRESSION)
    }
  }
}
</script>
