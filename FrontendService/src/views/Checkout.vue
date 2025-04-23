<script setup lang="ts">
import { ref, computed, watchEffect, onMounted } from 'vue'
import { useCartStore } from '@/store/cart'
import { Button } from '@/components/ui/button'
import { Card, CardHeader, CardContent } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import {
  Select,
  SelectTrigger,
  SelectContent,
  SelectGroup,
  SelectValue,
  SelectItem
} from '@/components/ui/select'
import { Separator } from '@/components/ui/separator'
import { useRouter } from 'vue-router'
import { toast } from 'vue-sonner'
import { Progress } from '@/components/ui/progress'
import OrderSummary from '@/components/OrderSummary.vue'
import keycloak from '@/keycloak'

const router = useRouter()
const cartStore = useCartStore()

const selectedPaymentMethod = ref<string | undefined>(undefined)
const progress = ref(0)

const firstName = ref('')
const lastName = ref('')
const street = ref('')
const houseNumber = ref('')
const city = ref('')
const postalCode = ref('')

const cardHolderName = ref('')
const cardNumber = ref('')
const expiryDate = ref('')
const cvv = ref('')

watchEffect(() => {
  const totalAddressFields = 6
  let filledFields = 0

  if (firstName.value) filledFields++
  if (lastName.value) filledFields++
  if (street.value) filledFields++
  if (houseNumber.value) filledFields++
  if (city.value) filledFields++
  if (postalCode.value) filledFields++

  let maxProgress = 50
  let currentProgress = (filledFields / totalAddressFields) * 50

  if (selectedPaymentMethod.value === 'credit-card') {
    const totalPaymentFields = 4
    let filledPaymentFields = 0

    if (cardHolderName.value) filledPaymentFields++
    if (cardNumber.value.length === 16) filledPaymentFields++
    if (expiryDate.value.length === 5) filledPaymentFields++
    if (cvv.value.length >= 3) filledPaymentFields++

    currentProgress += (filledPaymentFields / totalPaymentFields) * 50
    maxProgress = 100
  } else if (selectedPaymentMethod.value) {
    currentProgress = 100
  }

  progress.value = Math.min(Math.round(currentProgress), maxProgress)
})

const isFormValid = computed(() => {
  const addressValid =
    firstName.value &&
    lastName.value &&
    street.value &&
    houseNumber.value &&
    city.value &&
    postalCode.value

  if (selectedPaymentMethod.value === 'credit-card') {
    return (
      addressValid &&
      cardHolderName.value &&
      cardNumber.value.length === 16 &&
      expiryDate.value.length === 5 &&
      cvv.value.length >= 3
    )
  }

  return addressValid && selectedPaymentMethod.value
})

const placeOrder = async () => {
  if (!isFormValid.value) {
    toast.error('Bitte fülle alle Felder korrekt aus.')
    return
  }

  const orderData = {
    firstName: firstName.value,
    lastName: lastName.value,
    street: street.value,
    houseNumber: houseNumber.value,
    city: city.value,
    postalCode: postalCode.value,
    products: cartStore.items.map((item) => ({
      name: item.name,
      quantity: item.quantity,
      unitPrice: item.price,
      totalPrice: (item.price * item.quantity).toFixed(2),
      productImageUrl: item.imageUrl
    })),
    totalPrice: cartStore.cartTotal.toFixed(2),
    paymentMethod: selectedPaymentMethod.value,
    ...(selectedPaymentMethod.value === 'credit-card' && {
      cardHolderName: cardHolderName.value,
      cardNumber: cardNumber.value,
      expiryDate: expiryDate.value,
      cvv: cvv.value
    })
  }
  try {
    let response

    if (keycloak.authenticated) {
      const token = keycloak.token
      console.log(orderData)
      response = await fetch('/api/orders/logged', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${token}` },
        body: JSON.stringify(orderData)
      })
    } else {
      response = await fetch('/api/orders', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(orderData)
      })
    }

    if (!response.ok) throw new Error('Bestellung konnte nicht gespeichert werden.')

    const responseData = await response.json()

    cartStore.clearCart()
    router.push({
      path: '/order-confirmation',
      query: {
        orderId: responseData.id,
        totalPrice: cartStore.cartTotal.toFixed(2)
      }
    })

    toast.success('Bestellung wurde erfolgreich abgeschlossen! 🎉')
  } catch (error) {
    toast.error('Fehler beim Speichern der Bestellung!')
    console.error('Bestellfehler:', error)
  }
}

const formatExpiryDate = (event: Event) => {
  let value = (event.target as HTMLInputElement).value.replace(/[^\d/]/g, '')
  if (value.length > 2 && !value.includes('/')) {
    value = value.slice(0, 2) + '/' + value.slice(2)
  }
  expiryDate.value = value.slice(0, 5)
}
</script>

<template>
  <div class="max-w-6xl mx-auto p-6">
    <h1 class="text-2xl font-bold mb-6">Checkout</h1>

    <div class="mb-6 w-full">
      <p class="text-sm font-medium text-gray-700 mb-2">Checkout-Fortschritt: {{ progress }}%</p>
      <Progress
        :modelValue="progress"
        class="w-full h-2 bg-gray-200 transition-all duration-300 rounded-md"
      />
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <div class="lg:col-span-2 space-y-6">
        <Card>
          <CardHeader><h2 class="text-lg font-bold">Versandadresse</h2></CardHeader>
          <CardContent>
            <Input v-model="firstName" class="mb-3" type="text" placeholder="Vorname" />
            <Input v-model="lastName" class="mb-3" type="text" placeholder="Nachname" />
            <Input v-model="street" class="mb-3" type="text" placeholder="Straße" />
            <Input v-model="houseNumber" class="mb-3" type="text" placeholder="Hausnummer" />
            <Input v-model="postalCode" class="mb-3" type="text" placeholder="Postleitzahl" />
            <Input v-model="city" class="mb-3" type="text" placeholder="Stadt" />
          </CardContent>
        </Card>

        <Card>
          <CardHeader><h2 class="text-lg font-bold">Zahlungsmethode</h2></CardHeader>
          <CardContent>
            <Select v-model="selectedPaymentMethod">
              <SelectTrigger class="w-full"
                ><SelectValue placeholder="Wähle eine Zahlungsmethode"
              /></SelectTrigger>
              <SelectContent>
                <SelectGroup>
                  <SelectItem value="credit-card">💳 Kreditkarte</SelectItem>
                </SelectGroup>
              </SelectContent>
            </Select>

            <div v-if="selectedPaymentMethod === 'credit-card'" class="mt-4">
              <Input
                v-model="cardHolderName"
                class="mb-3"
                type="text"
                placeholder="Name auf der Karte"
              />
              <Input
                v-model="cardNumber"
                class="mb-3"
                type="text"
                placeholder="Kartennummer (16-stellig)"
                maxlength="16"
                @input="cardNumber = cardNumber.replace(/\D/g, '').slice(0, 16)"
              />
              <Input
                v-model="expiryDate"
                class="mb-3"
                type="text"
                placeholder="Ablaufdatum (MM/YY)"
                maxlength="5"
                @input="formatExpiryDate"
              />
              <Input
                v-model="cvv"
                class="mb-3"
                type="text"
                placeholder="CVV"
                maxlength="4"
                @input="cvv = cvv.replace(/\D/g, '').slice(0, 4)"
              />
            </div>
          </CardContent>
        </Card>
      </div>

      <div>
        <OrderSummary />
        <Separator class="my-4" />
        <Button
          @click="placeOrder"
          class="w-full"
          :disabled="!isFormValid"
          :class="{ 'opacity-50': !isFormValid }"
        >
          Bestellung abschließen
        </Button>
      </div>
    </div>
  </div>
</template>
