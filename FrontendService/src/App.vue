<script setup lang="ts">
import Topbar from './components/Topbar.vue';
import Navbar from './components/Navbar.vue';
</script>

<template>
  <Topbar></Topbar>
  <Navbar></Navbar>
  <router-view />
</template>

<style>
div {
  max-width: 1500px;
  margin: 0 auto;
}
</style>