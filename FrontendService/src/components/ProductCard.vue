<script setup lang="ts">
import { defineProps, ref } from 'vue'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { toast } from 'vue-sonner' 
import { useCartStore } from '@/store/cart' 

const props = defineProps({
  id: { type: String, required: true },
  name: { type: String, required: true },
  description: { type: String, default: 'Keine Beschreibung verfügbar' },
  short_description: { type: String, default: '' },
  price: { type: Number, required: true },
  imageUrl: { type: String, default: '../assets/product.jpeg' },
  quantityLabel: { type: String, default: 'Menge' }
})

function getFullImageUrl() {
  return import.meta.env.services__productservice__https__0 + props.imageUrl
}

const quantity = ref(1) 
const cartStore = useCartStore() 

const addToCart = () => {
  console.log('Produkt wird hinzugefügt:', {
    id: props.id,
    name: props.name,
    price: props.price,
    quantity: quantity.value,
    imageUrl: props.imageUrl
  })
  cartStore.addProductToCart({
    id: props.id,
    name: props.name,
    price: props.price,
    quantity: quantity.value,
    imageUrl: props.imageUrl
  })
  console.log('Warenkorb-Inhalt nach dem Hinzufügen:', cartStore.items)

  toast.success(`${props.name} wurde dem Warenkorb hinzugefügt!`)
}
</script>

<template>
  <Card class="w-[350px]">
    <CardHeader>
      <CardTitle>{{ name }}</CardTitle>
      <CardDescription>{{ short_description }}</CardDescription>
    </CardHeader>
    <CardContent>
      <img :src="getFullImageUrl()" alt="Produktbild" class="w-full h-auto mb-4" />
      <p class="text-sm text-muted-foreground">
        {{ description }}
      </p>
      <div class="flex items-center space-x-2 mt-4">
        <Label for="quantity">{{ quantityLabel }}</Label>
        <Input id="quantity" type="number" v-model.number="quantity" min="1" class="w-16" />
      </div>
    </CardContent>
    <CardFooter class="flex justify-between items-center">
      <span class="text-lg font-bold">Preis: {{ price.toFixed(2) }} €</span>
      <Button @click="addToCart">In den Warenkorb</Button>
    </CardFooter>
  </Card>
</template>
