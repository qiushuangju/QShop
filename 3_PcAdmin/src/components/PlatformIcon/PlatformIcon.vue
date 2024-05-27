<template>
  <span v-if="PlatformIcons[name]" class="platform-icon">
    <a-tooltip placement="bottom">
      <template v-if="showTips" slot="title">
        <span class="f-12">{{ tipsPrefix }}{{ PlatformName[name] }}</span>
      </template>
      <a-icon class="icon" :class="[name]" :component="PlatformIcons[name]" />
    </a-tooltip>
  </span>
</template>

<script>
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import { mpweixin, h5, app } from '@/core/icons'

// 客户端图标
const PlatformIcons = {
  'MP-WEIXIN': mpweixin,
  'H5': h5,
  'APP': app
}

// 客户端名称
const PlatformName = {
  'MP-WEIXIN': '微信小程序',
  'H5': 'H5网页',
  'APP': '手机APP'
}

export default {
  name: 'PlatformIcon',
  props: {
    // 指定的客户端 (APP、H5、小程序等)
    name: PropTypes.string.def(''),
    // 是否显示文字提示
    showTips: PropTypes.bool.def(false),
    // 文字提示前缀
    tipsPrefix: PropTypes.string.def(''),
  },
  data () {
    return {
      PlatformIcons,
      PlatformName
    }
  },
  methods: {
    fetchNotice () {
      if (!this.visible) {
        this.loading = true
        setTimeout(() => {
          this.loading = false
        }, 2000)
      } else {
        this.loading = false
      }
      this.visible = !this.visible
    }
  }
}
</script>

<style lang="less">
// 客户端图标
.platform-icon {
  font-size: 16px;

  .icon {
    margin-right: 5px;

    &:last-child {
      margin-right: 0;
    }
  }

  .MP-WEIXIN {
    color: #04be02;
  }

  .H5 {
    color: #e44c27;
  }
}
</style>
