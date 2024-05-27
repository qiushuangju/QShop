<template>
  <div class="cell-div">
    <el-radio v-model="type_" :label="label" @change="change">
      <el-tooltip effect="dark" placement="top">
        <div slot="content">{{ tag_ }}</div>
        <span class="cell-symbol">,</span>
      </el-tooltip>
      {{ $t('common.specified') }}
      <el-select
        v-model="numArray"
        :collapse-tags="collapsed"
        :size="size"
        :placeholder="$t('common.placeholderMulti')"
        filterable
        multiple
        style="width: 100%;">
        <el-option
          v-for="item in nums"
          :key="item.value"
          :label="item.label"
          :value="item.value"/>
      </el-select>
      {{ timeUnit }}
    </el-radio>
  </div>
</template>

<script>
import { sortNum, isNumber } from '@/components/cron/util/tools'
import { FIXED } from '@/components/cron/constant/filed'
import watchValue from '@/components/cron/mixins/watchValue'

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
      default: FIXED
    },
    tag: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      label: FIXED,
      type_: this.type,
      numArray: [],
      collapsed: false
    }
  },
  computed: {
    tag_: {
      get() {
        let tag = ''
        const self = this
        if (this.numArray && this.numArray.length) {
          self.numArray.sort(sortNum)
          for (let i = 0; i < this.numArray.length; i++) {
            tag += this.numArray[i] + FIXED
          }
          tag = tag.substring(0, tag.length - 1)
        }
        return tag
      },
      set(newValue) {
        if (this.type_ !== FIXED) {
          return
        }
        const arr = newValue.split(FIXED)
        const tempNumArr = []
        arr.forEach(num => {
          if (!isNumber(num) || parseInt(num) < this.nums[0].value || parseInt(num) > this.nums[this.nums.length - 1].value) {
            this.$message.error(this.$t('common.numError') + ':' + num)
            return
          }
          tempNumArr.push(parseInt(num))
        })
        tempNumArr.sort(sortNum)
        this.numArray = tempNumArr
      }
    }
  },
  watch: {
    'numArray'(curVal) {
      let labelLength = 0
      this.nums.forEach(num => {
        if (curVal.indexOf(num.value) !== -1) {
          labelLength += num.label.length
        }
      })
      this.collapsed = (labelLength > 6)
    },
    type_(curVal) {
      if (curVal === FIXED) {
        this.protectNumArray()
      }
    }
  },
  methods: {
    change() {
      this.$emit('type-changed', this.type_)
      this.$emit('tag-changed', this.tag_)
    },
    protectNumArray() {
      if (this.numArray.length === 0) {
        this.numArray.push(this.nums[0].value)
      }
    }
  }
}
</script>
