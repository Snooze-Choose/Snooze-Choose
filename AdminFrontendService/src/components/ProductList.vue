<script setup lang="ts">
import { ref, onMounted } from 'vue'

const products = ref([])

onMounted(async () => {
  try {
    const response = await fetch('/api/products')
    if (!response.ok) throw new Error('Failed to fetch products')
    products.value = await response.json()
  } catch (error) {
    console.error('Error loading products:', error)
  }
})
</script>

<template>
  <div class="flex flex-wrap justify-start gap-4">Hier sind die Produdkte</div>
</template>
