<script setup lang="ts">
import { defineProps, ref } from 'vue'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { useCartStore } from '@/store/cart' // Import des Warenkorb-Stores

// Props definieren
const props = defineProps({
  id: {
    type: Number,
    required: true,
  },
  title: {
    type: String,
    required: true,
  },
  description: {
    type: String,
    required: true,
  },
  price: {
    type: Number,
    required: true,
  },
  imageUrl: {
    type: String,
    required: true,
  },
  quantityLabel: {
    type: String,
    default: 'Menge',
  },
})

// State für die Menge
const quantity = ref(1) // Standardmäßig 1
const cartStore = useCartStore() // Zugriff auf den Warenkorb-Store

// Funktion: Produkt in den Warenkorb legen
const addToCart = () => {
  cartStore.addProductToCart({
    id: props.id,
    name: props.title,
    price: props.price,
    quantity: quantity.value,
    imageUrl: props.imageUrl,
  })
}
</script>

<template>
  <Card class="w-[350px]">
    <CardHeader>
      <CardTitle>{{ title }}</CardTitle>
      <CardDescription>{{ description }}</CardDescription>
    </CardHeader>
    <CardContent>
      <img :src="imageUrl" alt="Produktbild" class="w-full h-auto mb-4" />
      <p class="text-sm text-muted-foreground">
        Detaillierte Beschreibung des Produkts mit allen wichtigen Informationen.
      </p>
      <div class="flex items-center space-x-2 mt-4">
        <Label for="quantity">{{ quantityLabel }}</Label>
        <Input
          id="quantity"
          type="number"
          v-model.number="quantity"
          min="1"
          class="w-16"
        />
      </div>
    </CardContent>
    <CardFooter class="flex justify-between items-center">
      <span class="text-lg font-bold">Preis: {{ price.toFixed(2) }} €</span>
      <Button @click="addToCart">In den Warenkorb</Button>
    </CardFooter>
  </Card>
</template>
