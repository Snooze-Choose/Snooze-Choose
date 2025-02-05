<script setup lang="ts">
import Topbar from './components/Topbar.vue'
</script>

<template>
  <div class="app-container">
    <Topbar></Topbar>
    <div class="content">
      <router-view />
    </div>
  </div>
</template>

<style scoped>
.app-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 16px;
  box-sizing: border-box;
}

.content {
  margin-top: 20px;
}
</style>
