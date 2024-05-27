<template>
  <el-row>
    <every
      ref="everys"
      :type="type_"
      :tag="tag_"
      :time-unit="timeUnit"
      @type-changed="changeType"
      @tag-changed="changeTag"/>
    <period
      ref="periods"
      :type="type_"
      :tag="tag_"
      :size="size"
      :time-unit="timeUnit"
      :start-config="startConfig"
      :cycle-config="cycleConfig"
      @type-changed="changeType"
      @tag-changed="changeTag"/>
    <range
      ref="ranges"
      :type="type_"
      :tag="tag_"
      :size="size"
      :time-unit="timeUnit"
      :lower-config="lowerConfig"
      :upper-config="upperConfig"
      @type-changed="changeType"
      @tag-changed="changeTag"/>
    <fixed
      ref="fixeds"
      :type="type_"
      :tag="tag_"
      :size="size"
      :time-unit="timeUnit"
      :nums="nums"
      @type-changed="changeType"
      @tag-changed="changeTag"/>
  </el-row>
</template>

<script>
import Every from '@/components/cron/config/common/every'
import Period from '@/components/cron/config/common/period'
import Range from '@/components/cron/config/common/range'
import Fixed from '@/components/cron/config/common/fixed'
import { BASE_SYMBOL, EVERY, MONTHS } from '@/components/cron/constant/filed'
import watchTime from '@/components/cron/mixins/watchTime'

// 1 months
const LENGTH = 12
const LOWER_LIMIT = 1
const STEP = 1

export default {
  components: {
    Every,
    Period,
    Range,
    Fixed
  },
  mixins: [watchTime],
  props: {
    tag: {
      type: String,
      default: EVERY
    },
    size: {
      type: String,
      default: 'mini'
    }
  },
  data() {
    return {
      type_: EVERY,
      // expression of second
      tag_: null,
      timeUnit: this.$t('month.title'),
      symbol: BASE_SYMBOL,
      val: this.$t('month.val'),
      nums: [],
      startConfig: {
        min: LOWER_LIMIT,
        step: STEP,
        max: LENGTH
      },
      cycleConfig: {
        min: STEP,
        step: STEP,
        max: LENGTH
      },
      lowerConfig: {
        min: LOWER_LIMIT,
        step: STEP,
        max: LENGTH
      },
      upperConfig: {
        min: LOWER_LIMIT,
        step: STEP,
        max: LENGTH
      }
    }
  },
  methods: {
    // 12 months like [ {label: '0', value: 0},{label: '1', value: 1}...{label: '59', value: 59} ]
    initNums() {
      for (let i = 0; i < LENGTH; i++) {
        const item = {
          label: (i + 1).toString(),
          value: i + 1
        }
        this.nums.push(item)
      }
    },
    // change type
    changeType(type) {
      this.type_ = type
      this.changeSiblingType(type)
    },
    // change tag
    changeTag(tag) {
      this.tag_ = tag
      this.$emit('month-change', this.tag_)
    },
    changeSiblingType(type) {
      this.$refs.everys.type_ =
        this.$refs.periods.type_ =
          this.$refs.ranges.type_ =
            this.$refs.fixeds.type_ = type
    },
    resolveCustom(val) {
      for (let i = 0; i < MONTHS.length; i++) {
        const item = MONTHS[i]
        if (val.indexOf(item) !== -1) {
          val = val.replace(item, i + 1)
        }
      }
      return val
    }
  }
}
</script>
