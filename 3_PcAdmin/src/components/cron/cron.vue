<template>
  <div class="cron-wrap">
    <el-row class="cron-row">
      <el-row :gutter="2">
        <el-col :span="3">
          <el-input ref="input1" v-model="tag.second" :size="size" @focus="activeTabName='1'"/>
        </el-col>
        <el-col :span="4">
          <el-input ref="input2" v-model="tag.minute" :size="size" @focus="activeTabName='2'"/>
        </el-col>
        <el-col :span="4">
          <el-input ref="input3" v-model="tag.hour" :size="size" @focus="activeTabName='3'"/>
        </el-col>
        <el-col :span="4">
          <el-input ref="input4" v-model="tag.dayOfMonth" :size="size" @focus="activeTabName='4'"/>
        </el-col>
        <el-col :span="3">
          <el-input ref="input5" v-model="tag.month" :size="size" @focus="activeTabName='5'"/>
        </el-col>
        <el-col :span="3">
          <el-input ref="input6" v-model="tag.dayOfWeek" :size="size" @focus="activeTabName='6'"/>
        </el-col>
        <el-col :span="3">
          <el-input ref="input7" v-model="tag.year" :size="size" @focus="activeTabName='7'"/>
        </el-col>
      </el-row>
    </el-row>
    <el-row class="cron-row">
      <el-tabs v-model="activeTabName" type="border-card">
        <el-tab-pane name="1">
          <span slot="label">{{ $t('second.title') }}</span>
          <second
            :tag="tag.second"
            :size="size"
            @second-change="changeSecond"/>
        </el-tab-pane>
        <el-tab-pane name="2">
          <span slot="label">{{ $t('minute.title') }}</span>
          <minute
            :tag="tag.minute"
            :size="size"
            @minute-change="changeMinute"/>
        </el-tab-pane>
        <el-tab-pane name="3">
          <span slot="label">{{ $t('hour.title') }}</span>
          <hour
            :tag="tag.hour"
            :size="size"
            @hour-change="changeHour"/>
        </el-tab-pane>
        <el-tab-pane name="4">
          <span slot="label">{{ $t('dayOfMonth.title') }}</span>
          <day-of-month
            :tag="tag.dayOfMonth"
            :size="size"
            @day-of-month-change="changeDayOfMonth"/>
        </el-tab-pane>
        <el-tab-pane name="5">
          <span slot="label">{{ $t('month.title') }}</span>
          <month
            :tag="tag.month"
            :size="size"
            @month-change="changeMonth"/>
        </el-tab-pane>
        <el-tab-pane name="6">
          <span slot="label">{{ $t('dayOfWeek.title') }}</span>
          <day-of-week
            :tag="tag.dayOfWeek"
            :size="size"
            @day-of-week-change="changeDayOfWeek"/>
        </el-tab-pane>
        <el-tab-pane name="7">
          <span slot="label">{{ $t('year.title') }}</span>
          <year
            :tag="tag.year"
            :size="size"
            @year-change="changeYear"/>
        </el-tab-pane>
        <el-tab-pane name="8">
          <span slot="label">{{ $t('common.help') }}</span>
          <div class="cell-div">
            <span style="margin-right: 10px;">
              <el-button :disabled="!sample || sample.trim().length < 11" :size="size" type="primary" @click="changeTime(sample)">{{ $t('common.use') }}</el-button>
            </span>
            <el-select
              v-model="sample"
              :size="size"
              :placeholder="$t('common.placeholder')"
              :filter-method="filterCase"
              style="min-width: 320px;"
              filterable>
              <el-option
                v-for="item in cases"
                :key="item.value"
                :label="item.label"
                :value="item.value">
                <span style="float: left">{{ item.label }}</span>
                <span style="float: right; color: #8492a6; font-size: 13px">{{ item.value }}</span>
              </el-option>
            </el-select>
            <span style="margin-left: 5px;">
              {{ sample }}
            </span>
          </div>
          <div v-for="(item, index) in timeUnits" :key="index">
            {{ item }}:{{ $t('common.valTip') }}<strong>{{ vals[index] }}</strong>
            {{ $t('common.symbolTip') }}<strong>{{ symbols[index] }}</strong>
          </div>
        </el-tab-pane>
      </el-tabs>
    </el-row>
  </div>
</template>

<script>
import Second from './time/second'
import Minute from './time/minute'
import Hour from './time/hour'
import DayOfMonth from './time/dayOfMonth'
import Month from './time/month'
import Year from './time/year'
import DayOfWeek from './time/dayOfWeek'
import ZHCN from './mixins/plugins/translate/zh-CN'
import {
  EMPTY,
  EVERY,
  UNFIXED,
  BASE_SYMBOL,
  DAY_OF_MONTH_SYMBOL,
  DAY_OF_WEEK_SYMBOL,
  DEFAULT_CRON_EXPRESSION
} from './constant/filed'

export default {
  name: 'Cron',
  components: {
    DayOfWeek,
    Year,
    Month,
    DayOfMonth,
    Hour,
    Minute,
    Second
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
      tag: {
        second: EVERY,
        minute: EVERY,
        hour: EVERY,
        dayOfMonth: EVERY,
        month: EVERY,
        dayOfWeek: UNFIXED,
        year: EMPTY
      },
      activeTabName: '1',
      timeUnits: [
        this.$t('second.title'), this.$t('minute.title'), this.$t('hour.title'), this.$t('dayOfMonth.title'),
        this.$t('month.title'), this.$t('dayOfWeek.title'), this.$t('year.title')
      ],
      vals: [
        this.$t('second.val'), this.$t('minute.val'), this.$t('hour.val'), this.$t('dayOfMonth.val'),
        this.$t('month.val'), this.$t('dayOfWeek.val'), this.$t('year.val')
      ],
      symbols: [
        BASE_SYMBOL, BASE_SYMBOL, BASE_SYMBOL, DAY_OF_MONTH_SYMBOL, BASE_SYMBOL, DAY_OF_WEEK_SYMBOL, BASE_SYMBOL
      ],
      sample: '',
      cases: [],
      bakCases: []
    }
  },
  watch: {
    value(val) {
      this.changeTime(val)
    },
    activeTabName(val) {
      const input_ = this.$refs['input' + val]
      if (input_) {
        input_.focus()
      }
    },
    tag: {
      handler() {
        this.changeCron()
      },
      deep: true
    }
  },
  created() {
    this.loadConst()
    this.changeTime(this.value)
  },
  methods: {
    changeSecond(tag) {
      this.tag.second = tag
    },
    changeMinute(tag) {
      this.tag.minute = tag
    },
    changeHour(tag) {
      this.tag.hour = tag
    },
    changeDayOfMonth(tag) {
      this.tag.dayOfMonth = tag
    },
    changeMonth(tag) {
      this.tag.month = tag
    },
    changeDayOfWeek(tag) {
      this.tag.dayOfWeek = tag
    },
    changeYear(tag) {
      this.tag.year = tag
    },
    changeCron() {
      const cron = (this.tag.second + ' ' + this.tag.minute + ' ' + this.tag.hour + ' ' + this.tag.dayOfMonth + ' ' +
          this.tag.month + ' ' + this.tag.dayOfWeek + ' ' + this.tag.year).trim()
      this.$emit('change', cron)
    },
    changeTime(newValue) {
      if (!newValue) {
        return
      }
      if (!newValue || newValue.trim().length < 11) {
        this.$message.error(this.$t('common.wordNumError'))
        return
      }
      const arr = newValue.trim().split(' ')
      if (arr.length !== 6 && arr.length !== 7) {
        this.$message.error(this.$t('common.wordNumError'))
        return
      }
      this.tag.second = arr[0]
      this.tag.minute = arr[1]
      this.tag.hour = arr[2]
      this.tag.dayOfMonth = arr[3]
      this.tag.month = arr[4]
      this.tag.dayOfWeek = arr[5]
      this.tag.year = arr.length === 7 ? arr[6] : ''
    },
    filterCase(query) {
      if (query !== '') {
        this.loading = true
        setTimeout(() => {
          this.loading = false
          this.cases = this.bakCases.filter(item => {
            return item.label.toLowerCase()
              .indexOf(query.toLowerCase()) !== -1 ||
            item.value.toLowerCase()
              .indexOf(query.toLowerCase()) !== -1
          })
        }, 100)
      } else {
        this.cases = this.bakCases
      }
    },
    loadConst() {
      this.bakCases = this.cases = ZHCN.cases
      // loadArray().then(array => {
      //   this.bakCases = this.cases = array.cases
      // })
    }
  }
}
</script>
<style lang="scss" scoped>
  @import '~@/components/cron/styles/index';
</style>
