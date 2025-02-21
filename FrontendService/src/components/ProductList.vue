<script setup lang="ts">
import { ref, onMounted } from 'vue'
import ProductCard from './ProductCard.vue'
import { Rat } from 'lucide-vue-next'

interface Product {
  id: string
  name: string
  shortDescription: string
  longDescription: string
  price: number
  rating: number
  imageUrl: string
}

const products = ref<Product[]>([])

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
  <div class="flex flex-wrap justify-start gap-4">
    <ProductCard
      v-for="product in products"
      :key="product.id"
      :id="product.id"
      :name="product.name"
      :short_description="product.shortDescription"
      :description="product.longDescription"
      :price="product.price"
      :rating="product.rating"
      :imageUrl="product.imageUrl"
    />
  </div>
</template>
