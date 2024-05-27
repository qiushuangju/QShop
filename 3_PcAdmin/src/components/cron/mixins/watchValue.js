export default {
  watch: {
    tag_(val) {
      if (this.type_ === this.label) {
        this.$emit('tag-changed', val)
      }
    },
    tag(val) {
      this.tag_ = val
    }
  }
}
