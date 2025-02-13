<script setup lang="ts">
import { useCartStore } from '@/store/cart';
import { Card, CardHeader, CardContent, CardFooter } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';

const cartStore = useCartStore();

function getFullImageUrl(imageUrl: String) {
  return import.meta.env.services__productservice__https__0 + imageUrl
}
</script>

<template>
  <Card class="shadow-lg border border-gray-200">
    <CardHeader class="bg-gray-100 p-4 rounded-t-lg">
      <h2 class="text-xl font-semibold text-gray-700">🛒 Bestellübersicht</h2>
    </CardHeader>
    <CardContent class="p-4">
      <ul class="divide-y divide-gray-300">
        <li 
          v-for="item in cartStore.items" 
          :key="item.id" 
          class="flex items-center gap-4 py-3"
        >
          <img 
            :src="getFullImageUrl(item.imageUrl)" 
            alt="Produktbild" 
            class="w-16 h-16 object-cover rounded-md border"
          />
          <div class="flex-1">
            <p class="font-medium text-gray-800">{{ item.name }}</p>
            <p class="text-sm text-gray-500">Menge: {{ item.quantity }}x</p>
          </div>
          <p class="text-md font-semibold text-gray-700">
            {{ (item.price * item.quantity).toFixed(2) }} €
          </p>
        </li>
      </ul>
      
      <Separator class="my-4" />

      <div class="flex justify-between text-lg font-semibold text-gray-800">
        <span>Gesamtsumme:</span>
        <span>{{ cartStore.cartTotal.toFixed(2) }} €</span>
      </div>
    </CardContent>
    <CardFooter class="p-4 bg-gray-50 rounded-b-lg text-center text-gray-500 text-sm">
      Alle Preise inkl. MwSt. & Versandkosten.
    </CardFooter>
  </Card>
</template>
