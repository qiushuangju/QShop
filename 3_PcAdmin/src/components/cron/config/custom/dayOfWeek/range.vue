<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      <span class="cell-symbol">{{ tag_ }}</span>
      {{ $t('common.between') }}
      <el-select
        v-model="lower"
        :size="size"
        :placeholder="$t('common.placeholder')"
        style="width: 100px;"
        filterable>
        <el-option
          v-for="item in nums"
          :key="item.value"
          :label="item.label"
          :value="item.value"
          :disabled="item.value>upper"/>
      </el-select>
      {{ $t('common.and') }}
      <el-select
        v-model="upper"
        :size="size"
        :placeholder="$t('common.placeholder')"
        style="width: 100px;"
        filterable>
        <el-option
          v-for="item in nums"
          :key="item.value"
          :label="item.label"
          :value="item.value"
          :disabled="item.value<lower"/>
      </el-select>
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
      lower: 1,
      upper: 1
    }
  },
  computed: {
    tag_: {
      get() {
        return this.lower + RANGE + this.upper
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
        if (!isNumber(arr[0]) || parseInt(arr[0]) < this.nums[0].value || parseInt(arr[0]) > this.nums[this.nums.length - 1].value) {
          this.$message.error(this.$t('range.lowerError') + ':' + arr[0])
          return
        }
        if (!isNumber(arr[1]) || parseInt(arr[1]) < this.nums[0].value || parseInt(arr[1]) > this.nums[this.nums.length - 1].value) {
          this.$message.error(this.$t('range.upperError') + ':' + arr[1])
          return
        }
        if (parseInt(arr[0]) > parseInt(arr[1])) {
          this.$message.error(this.$t('range.lowerBiggerThanUpperError') + ':' + arr[0] + '>' + arr[1])
          return
        }
        this.lower = parseInt(arr[0])
        this.upper = parseInt(arr[1])
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
