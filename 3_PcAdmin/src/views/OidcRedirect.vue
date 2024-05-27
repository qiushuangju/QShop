<template>
  <div>
    跳转中，请稍等...
  </div>
</template>

<script>
export default {
  name: 'OidcRedirect',
  created() {
    if (this.$store.getters.oidcUser) {
      this.$router.push('/')
    }
  }
}

</script>
