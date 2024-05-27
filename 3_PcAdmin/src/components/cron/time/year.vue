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
      :upper="upper"
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
    <empty
      ref="emptys"
      :type="type_"
      :tag="tag_"
      @type-changed="changeType"
      @tag-changed="changeTag"/>
  </el-row>
</template>

<script>
import Every from '@/components/cron/config/common/every'
import Period from '@/components/cron/config/common/period'
import Range from '@/components/cron/config/common/range'
import Fixed from '@/components/cron/config/common/fixed'
import Empty from '@/components/cron/config/custom/year/empty'
import { BASE_SYMBOL, CUR_YEAR, EMPTY, UPPER_LIMIT_YEAR } from '@/components/cron/constant/filed'
import watchTime from '@/components/cron/mixins/watchTime'

// 2099 years
const LOWER_LIMIT = CUR_YEAR
const LENGTH = UPPER_LIMIT_YEAR
const STEP = 1

export default {
  components: {
    Every,
    Period,
    Range,
    Fixed,
    Empty
  },
  mixins: [watchTime],
  props: {
    tag: {
      type: String,
      default: EMPTY
    },
    size: {
      type: String,
      default: 'mini'
    }
  },
  data() {
    return {
      type_: EMPTY,
      // expression of second
      tag_: null,
      timeUnit: this.$t('year.title'),
      symbol: BASE_SYMBOL,
      val: this.$t('year.val'),
      nums: [],
      upper: LOWER_LIMIT,
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
        step: STEP
      },
      upperConfig: {
        step: STEP,
        max: LENGTH
      }
    }
  },
  methods: {
    // xxx years like [ {label: '2019', value: 2019},{label: '2020', value: 2020}...{label: '2099', value: 2099} ]
    initNums() {
      for (let i = LOWER_LIMIT; i <= LENGTH; i++) {
        const item = {
          label: i.toString(),
          value: i
        }
        this.nums.push(item)
      }
    },
    // change type
    changeType(type) {
      this.changeSiblingType(type)
      this.type_ = type
    },
    // change tag
    changeTag(tag) {
      this.tag_ = tag
      this.$emit('year-change', this.tag_)
    },
    changeSiblingType(type) {
      this.$refs.everys.type_ =
        this.$refs.periods.type_ =
          this.$refs.ranges.type_ =
            this.$refs.fixeds.type_ =
              this.$refs.emptys.type_ = type
    }
  }
}
</script>
