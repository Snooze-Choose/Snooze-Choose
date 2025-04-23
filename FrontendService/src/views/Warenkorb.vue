<script setup lang="ts">
import { Table, TableBody, TableCell, TableHeader, TableRow } from '@/components/ui/table'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Separator } from '@/components/ui/separator'
import { Alert, AlertTitle } from '@/components/ui/alert'
import { InfoCircledIcon } from '@radix-icons/vue'
import { useCartStore } from '@/store/cart'
import { computed } from 'vue'
import router from '@/router'

const cartStore = useCartStore()
const cartItems = computed(() => {
  return cartStore.items
})
const cartTotal = computed(() => {
  return cartStore.cartTotal
})

const cartQuantityTotal = computed(() => {
  return cartStore.items.reduce((total, item) => total + item.quantity, 0)
})

const removeFromCart = (id: string) => {
  cartStore.removeProductFromCart(id)
}

const clearCart = () => {
  cartStore.clearCart()
}

const goToCheckout = () => {
  router.push({ name: 'Checkout' })
}

function getFullImageUrl(imageUrl: String) {
  return import.meta.env.services__productservice__https__0 + imageUrl
}
</script>

<template>
  <div class="p-6">
    <h1 class="text-2xl font-bold mb-4">Warenkorb</h1>
    <div v-if="cartItems.length === 0" class="text-center">
      <Alert>
        <InfoCircledIcon class="w-6 h-6 text-blue-500" />
        <AlertTitle>Dein Warenkorb ist leer.</AlertTitle>
      </Alert>
      <Button class="mt-4" @click="$router.push({ name: 'Home' })">Zurück zu den Produkten</Button>
    </div>
    <div v-else>
      <Table>
        <TableHeader class="bg-gray-100">
          <TableRow>
            <TableCell class="font-medium">Produkt</TableCell>
            <TableCell class="font-medium">Preis</TableCell>
            <TableCell class="font-medium">Menge</TableCell>
            <TableCell class="font-medium">Gesamt</TableCell>
            <TableCell class="font-medium">Aktionen</TableCell>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow v-for="item in cartItems" :key="item.id">
            <TableCell class="flex flex-col items-start">
              <span class="font-semibold">{{ item.name }}</span>
              <img
                :src="getFullImageUrl(item.imageUrl)"
                alt="Produktbild"
                class="w-24 h-24 object-cover rounded mt-2"
              />
            </TableCell>
            <TableCell>{{ item.price.toFixed(2) }} €</TableCell>
            <TableCell>
              <Input type="number" v-model.number="item.quantity" min="1" class="w-16" />
            </TableCell>
            <TableCell>{{ (item.price * item.quantity).toFixed(2) }} €</TableCell>
            <TableCell>
              <Button variant="destructive" @click="removeFromCart(item.id)">Entfernen</Button>
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
      <Separator class="my-4" />
      <div class="flex justify-between mt-2">
        <p class="text-lg font-bold">Gesamtsumme:</p>
        <p class="text-lg font-bold">{{ cartTotal.toFixed(2) }} €</p>
      </div>
      <Separator class="my-4" />
      <div class="flex justify-between mt-4">
        <Button variant="secondary" @click="clearCart">Warenkorb leeren</Button>
        <Button variant="default" class="ml-auto" @click="goToCheckout">Zur Kasse</Button>
      </div>
    </div>
  </div>
</template>
