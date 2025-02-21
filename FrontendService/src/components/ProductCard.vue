<script setup lang="ts">
import { computed, defineProps, ref } from 'vue'
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
  rating: {type: Number, required: true},
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

const circleRating = computed(() => {
  const fullStars = Math.floor(props.rating)
  const hasHalfStar = props.rating % 1 >= 0.5
  const emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0)
  
  return '★'.repeat(fullStars) + (hasHalfStar ? '✬' : '') + '☆'.repeat(emptyStars)
})

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
      <div class="flex items-center mt-4">
        <Label for="quantity" class="mr-1" >{{ quantityLabel }}</Label>
        <Input id="quantity" type="number" v-model.number="quantity" min="1" class="w-16" />
        <Button @click="addToCart" class="ml-auto">In den Warenkorb</Button>
      </div>
    </CardContent>
    <CardFooter class="flex justify-between items-center">
      <span class="text-lg font-bold">Preis: {{ price.toFixed(2) }} €</span>
      <div class="flex flex-col items-end">
      <span class="text-sm text-gray-500">Bewertung: {{ rating }}/5</span>
      <div class="flex items-center text-yellow-500 text-xl mt-1">
        {{ circleRating }}
      </div>
    </div>
    </CardFooter>
  </Card>
</template>
