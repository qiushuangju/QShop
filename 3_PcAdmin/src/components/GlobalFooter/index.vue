<template>
  <global-footer v-if="visibility" class="footer custom-render">
    <template v-slot:links>
      <!-- <a href="https://www.github.com/vueComponent/pro-layout" target="_blank">Pro Layout</a>
      <a href="https://www.github.com/vueComponent/ant-design-vue-pro" target="_blank">Github</a>
      <a href="https://www.github.com/sendya/" target="_blank">@Sendya</a>-->
    </template>
    <template v-slot:copyright>
      <span style="margin-right: 6px">{{ copyright }}</span>
      <a :href="link.url" target="_blank">{{ link.text }}</a>
    </template>
  </global-footer>
</template>

<script>
import { GlobalFooter } from '@/layouts/ProLayout'

const uncompileStr = (code) => {
  code = unescape(code)
  var c = String.fromCharCode(code.charCodeAt(0) - code.length)
  for (var i = 1; i < code.length; i++) {
    c += String.fromCharCode(code.charCodeAt(i) - c.charCodeAt(i - 1))
  }
  return c
}

export default {
  name: 'ProGlobalFooter',
  components: {
    GlobalFooter
  },
  data () {
    return {
      visibility: true,
      copyright: uncompileStr('%5D%B2%DF%E9%EB%DB%D0%CF%DC%94%C9%C9RbbdR%u8444%uF48F%uC5B1%uAD14%u5800%60%5EP%9C'),
      link: {
        text: uncompileStr('b%A2%98%A5%A5%7Dq%92%9C'),
        url: uncompileStr('%7D%DC%E8%E4%E3%ADi%5E%A6%EE%EE%A5%A7%E2%D8%E5%E5%9D%91%D2%DC')
      }
    }
  },
  methods: {

  }
}
</script>
