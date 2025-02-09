<script setup lang="ts">
import Topbar from './components/Topbar.vue';
import Navbar from './components/Navbar.vue';
import { Toaster } from './components/ui/sonner';
</script>

<template>
  <div class="app-container">
    <Toaster rich-colors />
    <Topbar></Topbar>
    <Navbar></Navbar>
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
