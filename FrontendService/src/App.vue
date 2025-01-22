<script setup lang="ts">
import ProductList from './components/ProductList.vue';
import Navbar from './components/Navbar.vue';
import Topbar from './components/Topbar.vue';
</script>

<template>
  <div class="flex flex-col gap-4">
    <Topbar></Topbar>
    <Navbar></Navbar>
    <ProductList></ProductList>
  </div>
  </template>
  

<style scoped>
/* Optional: You can set the max width of the container for more control */
div {
  max-width: 1500px; /* Adjust based on your layout needs */
  margin: 0 auto; /* Center the entire container */
}
</style>
