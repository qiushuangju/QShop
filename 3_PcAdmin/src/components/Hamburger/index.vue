<template>
  <span @click="toggleClick"
    :class="isActive === true ? 'icon iconfont icon-suoxiao' : 'icon iconfont icon-fangda'"></span>
</template>

<script>
  export default {
    name: 'hamburger',
    props: {
      isActive: {
        type: Boolean,
        default: false
      },
      toggleClick: {
        type: Function,
        default: null
      }
    }
  }
</script>

<style scoped>
  .hamburger {
    cursor: pointer;
    /* width: 20px;
	height: 20px; */
    font-size: 20px;
    transform: rotate(90deg);
    transition: 0.38s;
    transform-origin: 50% 50%;
    vertical-align: middle;
  }

  .hamburger.svg-icon {
    vertical-align: middle;
  }

  .hamburger.is-active {
    transform: rotate(0deg);
  }
</style>