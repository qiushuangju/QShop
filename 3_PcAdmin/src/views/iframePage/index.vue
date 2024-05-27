<template>
  <div class="app-container">
    <div class="block" v-if="url">
      <iframe :src="url" width="100%" height="768px" frameborder="0" scrolling="auto"></iframe>
    </div>
  </div>
</template>
<script>
import { mapGetters } from 'vuex'
export default {
  name: 'iframePage',
  computed: {
    ...mapGetters(['iframeViews'])
  },
  data() {
    return {
      url: ''
    }
  },
  mounted() {
    this.url = this.iframeViews[this.$route.params.code].url
  }
}
</script>

