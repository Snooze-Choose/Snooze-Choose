<script setup lang="ts">
import { ref, computed, watch, onMounted } from 'vue'
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
import CheckoutSteps from '@/components/ui/CheckoutSteps.vue'
import OrderSummary from '@/components/ui/OrderSummary.vue'

// Interfaces für Adressen und Zahlungsmethoden
interface Address {
  id: string
  street: string
  city: string
  postal_code: string
  country: string
}

interface PaymentMethod {
  id: string
  type: string
  card_holder_name?: string
  card_number?: string
  expiry_date?: string
}

// Vue States mit korrektem Typ
const router = useRouter()
const cartStore = useCartStore()
const selectedPaymentMethod = ref<string | undefined>(undefined)
const addresses = ref<Address[]>([])
const paymentMethods = ref<PaymentMethod[]>([])
const progress = ref(0)

// Reaktive Inputs für Versandadresse
const firstName = ref('')
const lastName = ref('')
const street = ref('')
const houseNumber = ref('')
const city = ref('')
const postalCode = ref('')

// Kreditkarten-Eingabefelder
const cardHolderName = ref('')
const cardNumber = ref('')
const expiryDate = ref('')
const cvv = ref('')

// API-Daten laden
onMounted(async () => {
  try {
    const resAddresses = await fetch('/api/checkout/addresses')
    addresses.value = await resAddresses.json()

    const resPayment = await fetch('/api/checkout/payment-methods')
    paymentMethods.value = await resPayment.json()

    // Kreditkarte als Option hinzufügen, falls nicht in der API
    if (!paymentMethods.value.find((method) => method.id === 'credit-card')) {
      paymentMethods.value.push({ id: 'credit-card', type: 'Kreditkarte' })
    }
  } catch (error) {
    console.error('Fehler beim Laden der Daten:', error)
  }
})

// Watcher: Prüft, ob alle Versandadressfelder ausgefüllt sind
watch([firstName, lastName, street, houseNumber, city, postalCode], () => {
  if (
    firstName.value &&
    lastName.value &&
    street.value &&
    houseNumber.value &&
    city.value &&
    postalCode.value
  ) {
    progress.value = 30 // Fortschritt auf 30% setzen
  } else {
    progress.value = 0 // Falls ein Feld fehlt, wieder auf 0% setzen
  }
})

// Berechneter Wert für die Progress Bar
const computedProgress = computed(() => progress.value)

// Bestellung abschließen
const placeOrder = async () => {
  if (
    !firstName.value ||
    !lastName.value ||
    !street.value ||
    !houseNumber.value ||
    !city.value ||
    !postalCode.value
  ) {
    alert('Bitte fülle die Versandadresse aus.')
    return
  }

  if (!selectedPaymentMethod.value) {
    alert('Bitte wähle eine Zahlungsmethode aus.')
    return
  }

  let paymentDetails: Record<string, any> = { paymentMethodId: selectedPaymentMethod.value }

  // Falls Kreditkarte ausgewählt ist, füge Zahlungsdetails hinzu
  if (selectedPaymentMethod.value === 'credit-card') {
    if (!cardHolderName.value || !cardNumber.value || !expiryDate.value || !cvv.value) {
      alert('Bitte fülle alle Kreditkartenfelder aus.')
      return
    }

    paymentDetails = {
      ...paymentDetails,
      cardHolderName: cardHolderName.value,
      cardNumber: cardNumber.value,
      expiryDate: expiryDate.value,
      cvv: cvv.value
    }
  }

  const response = await fetch('/api/checkout/place-order', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      address: {
        firstName: firstName.value,
        lastName: lastName.value,
        street: street.value,
        houseNumber: houseNumber.value,
        city: city.value,
        postalCode: postalCode.value
      },
      payment: paymentDetails,
      totalPrice: cartStore.cartTotal,
      items: cartStore.items
    })
  })

  if (response.ok) {
    alert('Bestellung erfolgreich!')
    cartStore.clearCart()
    router.push('/')
  } else {
    alert('Fehler beim Abschluss der Bestellung')
  }
}

const formatExpiryDate = (event: Event) => {
  let value = (event.target as HTMLInputElement).value;
  
  // Nur Zahlen und "/" erlauben
  value = value.replace(/[^\d/]/g, '');

  // Automatische Formatierung: MM/YY
  if (value.length > 2 && !value.includes('/')) {
    value = value.slice(0, 2) + '/' + value.slice(2);
  }

  // Maximal 5 Zeichen (MM/YY)
  expiryDate.value = value.slice(0, 5);
};

</script>

<template>
  <div class="max-w-5xl mx-auto p-6">
    <h1 class="text-2xl font-bold mb-6">Checkout</h1>

    <!-- Checkout Steps mit dynamischem Fortschritt -->
    <CheckoutSteps :progressValue="computedProgress" class="mb-6" />

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <!-- Linke Spalte: Versandadresse & Zahlungsmethode -->
      <div class="lg:col-span-2 space-y-6">
        <!-- Versandadresse -->
        <Card>
          <CardHeader>
            <h2 class="text-lg font-bold">Versandadresse</h2>
          </CardHeader>
          <CardContent>
            <Input v-model="firstName" class="mb-3" type="text" placeholder="Vorname" />
            <Input v-model="lastName" class="mb-3" type="text" placeholder="Nachname" />
            <Input v-model="street" class="mb-3" type="text" placeholder="Straße" />
            <Input v-model="houseNumber" class="mb-3" type="number" placeholder="Hausnummer" />
            <Input v-model="city" class="mb-3" type="text" placeholder="Stadt" />
            <Input v-model="postalCode" class="mb-3" type="number" placeholder="Postleitzahl" />
          </CardContent>
        </Card>

        <!-- Zahlungsmethode -->
        <Card class="mb-6">
          <CardHeader>
            <h2 class="text-lg font-bold">Zahlungsmethode</h2>
          </CardHeader>
          <CardContent>
            <Select v-model="selectedPaymentMethod">
              <SelectTrigger class="w-full">
                <SelectValue placeholder="Wähle eine Zahlungsmethode" />
              </SelectTrigger>
              <SelectContent>
                <SelectGroup>
                  <SelectLabel>Zahlungsmethoden</SelectLabel>
                  <SelectItem value="credit-card">💳 Kreditkarte</SelectItem>
                </SelectGroup>
                <SelectGroup label="Gespeicherte Zahlungsmethoden">
                  <SelectItem v-for="method in paymentMethods" :key="method.id" :value="method.id">
                    {{ method.card_holder_name }} - **** {{ method.card_number?.slice(-4) }}
                  </SelectItem>
                </SelectGroup>
              </SelectContent>
            </Select>

            <!-- Kreditkartenfelder nur anzeigen, wenn Kreditkarte ausgewählt wurde -->
            <div v-if="selectedPaymentMethod === 'credit-card'" class="mt-4">
              <Input
                v-model="cardHolderName"
                class="mb-3"
                type="text"
                placeholder="Name auf der Karte"
              />
              <Input v-model="cardNumber" class="mb-3" type="number" placeholder="Kartennummer" />
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

      <!-- Rechte Spalte: Bestellübersicht -->
      <div>
        <OrderSummary />
        <Separator class="my-4" />
        <Button @click="placeOrder" class="w-full">Bestellung abschließen</Button>
      </div>
    </div>
  </div>
</template>
