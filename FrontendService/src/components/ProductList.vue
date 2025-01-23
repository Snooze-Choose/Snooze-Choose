<script setup lang="ts">
import ProductCard from './ProductCard.vue';

// Beispielprodukte, die Props bereitstellen
const products = [
  {
    id: 1,
    title: 'Produkt 1',
    description: 'Eine kurze Beschreibung von Produkt 1.',
    price: 19.99,
    imageUrl: '../assets/product.jpeg',
  },
  {
    id: 2,
    title: 'Produkt 2',
    description: 'Eine kurze Beschreibung von Produkt 2.',
    price: 29.99,
    imageUrl: '../assets/product.jpeg',
  },
  {
    id: 3,
    title: 'Produkt 3',
    description: 'Eine kurze Beschreibung von Produkt 3.',
    price: 39.99,
    imageUrl: '../assets/product.jpeg',
  },
  {
    id: 4,
    title: 'Produkt 4',
    description: 'Eine kurze Beschreibung von Produkt 4.',
    price: 49.99,
    imageUrl: '../assets/product.jpeg',
  },
  {
    id: 5,
    title: 'Produkt 5',
    description: 'Eine kurze Beschreibung von Produkt 5.',
    price: 59.99,
    imageUrl: '../assets/product.jpeg',
  },
];
</script>

<template>
  <div class="flex flex-wrap justify-start gap-4">
    <ProductCard
      v-for="product in products"
      :key="product.id"
      :id="product.id"
      :title="product.title"
      :description="product.description"
      :price="product.price"
      :imageUrl="product.imageUrl"
    />
  </div>
</template>
