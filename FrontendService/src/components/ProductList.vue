<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import ProductCard from './ProductCard.vue'

interface Product {
  id: string
  name: string
  shortDescription: string
  longDescription: string
  price: number
  rating: number
  imageUrl: string
  category: 'Verzehr' | 'Technik' | 'Alltag'
}

const products = ref<Product[]>([])
const searchQuery = ref('')
const selectedCategory = ref('Alle')

onMounted(async () => {
  try {
    const response = await fetch('/api/products')
    if (!response.ok) throw new Error('Failed to fetch products')
    products.value = await response.json()
  } catch (error) {
    console.error('Error loading products:', error)
  }
})

const filteredProducts = computed(() => {
  const query = searchQuery.value.trim().toLowerCase()
  const category = selectedCategory.value

  const matchesCategory = (product: Product) =>
    category === 'Alle' || product.category === category

  const exactMatches: Product[] = []
  const partialMatches: Product[] = []
  const descriptionMatches: Product[] = []

  products.value.forEach(product => {
    if (!matchesCategory(product)) return

    const name = product.name.toLowerCase()
    const shortDesc = product.shortDescription.toLowerCase()
    const longDesc = product.longDescription.toLowerCase()

    if (name === query) {
      exactMatches.push(product)
    } else if (name.includes(query)) {
      partialMatches.push(product)
    } else if (shortDesc.includes(query) || longDesc.includes(query)) {
      descriptionMatches.push(product)
    }
  })

  return {
    exactMatches,
    partialMatches,
    descriptionMatches
  }
})
</script>

<template>
  <div>
    <div class="flex flex-col sm:flex-row sm:items-center gap-4 mb-4">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Produkt suchen..."
        class="w-full sm:w-2/3 p-2 border rounded"
      />
      <select
        v-model="selectedCategory"
        class="w-full sm:w-1/3 p-2 border rounded"
      >
        <option value="Alle">Alle Kategorien</option>
        <option value="Verzehr">Verzehr</option>
        <option value="Technik">Technik</option>
        <option value="Alltag">Alltag</option>
      </select>
    </div>

    <div v-if="filteredProducts.exactMatches.length || filteredProducts.partialMatches.length">
      <div class="flex flex-wrap justify-start gap-4">
        <ProductCard
          v-for="product in [...filteredProducts.exactMatches, ...filteredProducts.partialMatches]"
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
    </div>

    <div v-if="filteredProducts.descriptionMatches.length">
      <h3 class="mt-6 mb-2 text-lg font-semibold">Weitere Artikel</h3>
      <div class="flex flex-wrap justify-start gap-4">
        <ProductCard
          v-for="product in filteredProducts.descriptionMatches"
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
    </div>
  </div>
</template>
