<template>
  <div>
  </div>
</template>

<script>
  import {
    mapActions
  } from 'vuex'

  export default {
    name: 'OidcCallback',
    methods: {
      ...mapActions([
        'oidcSignInCallback'
      ])
    },
    created() {
      this.oidcSignInCallback()
        .then((redirectPath) => {
          this.$router.push(redirectPath)
        })
        .catch((err) => {
          console.error(err)
          this.$router.push('/signin-oidc-error') // Handle errors any way you want
        })
    }
  }

</script>
