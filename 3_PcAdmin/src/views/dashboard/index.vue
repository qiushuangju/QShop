<template>
  <div class="dashboard-container">
    <component :is="currentRole"></component>
  </div>
</template>

<script>
import adminDashboard from './admin'

export default {
  name: 'dashboard',
  components: { adminDashboard },
  data() {
    return {
      currentRole: 'adminDashboard'
    }
  }
}
</script>
