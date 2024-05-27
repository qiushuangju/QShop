import { EMPTY, EVERY, FIXED, LAST, LAST_WORK_DAY, PERIOD, RANGE, UNFIXED, WEEK_DAY, WORK_DAY } from '@/components/cron/constant/filed'

export default {
  watch: {
    tag(val) {
      this.resolveTag(val)
    }
  },
  created() {
    this.initNums()
  },
  mounted() {
    this.resolveTag(this.tag)
  },
  methods: {
    resolveTag(val) {
      if (val == null || val === undefined) {
        val = EMPTY
      }
      let temp = null
      val = this.resolveCustom(val)
      // equals
      if (val === EMPTY) {
        temp = EMPTY
      } else if (val === UNFIXED) {
        temp = UNFIXED
      } else if (val === EVERY) {
        temp = EVERY
      } else if (val === LAST_WORK_DAY) {
        temp = LAST_WORK_DAY
      }
      // contains
      if (temp == null) {
        if (val.startsWith(LAST + '-')) {
          temp = LAST
        } else if (val.endsWith(LAST)) {
          temp = LAST
        } else if (val.endsWith(WORK_DAY) && val.length > WORK_DAY.length) {
          temp = WORK_DAY
        } else if (val.indexOf(WEEK_DAY) > 0) {
          temp = WEEK_DAY
        } else if (val.indexOf(PERIOD) > 0) {
          temp = PERIOD
        } else if (val.indexOf(RANGE) > 0) {
          temp = RANGE
        } else {
          temp = FIXED
        }
      }
      this.type_ = temp
      this.changeSiblingType(this.type_)
      this.tag_ = val
    },
    resolveCustom(val) {
      return val
    }
  }
}
