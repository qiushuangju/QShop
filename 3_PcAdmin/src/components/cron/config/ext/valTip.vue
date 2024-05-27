<template>
  <div class="cell-div">
    {{ $t('common.valTip') }}<strong>{{ val }}</strong>
  </div>
</template>

<script>
export default {
  props: {
    val: {
      type: String,
      default: null
    }
  }
}
</script>
