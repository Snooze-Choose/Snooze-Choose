<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { ref, onMounted } from 'vue'
import { Button } from '@/components/ui/button'
import { Card, CardHeader, CardContent } from '@/components/ui/card'
import { CheckCircleIcon, XCircleIcon, HomeIcon } from 'lucide-vue-next'
import OrderSummary from '@/components/OrderSummary.vue'

const route = useRoute()
const router = useRouter()

const order = ref<any>(null)
const isLoading = ref(true)
const errorMessage = ref('')

// API-Call zum Abrufen der Bestelldaten
onMounted(async () => {
  const orderId = route.query.orderId

  if (!orderId) {
    errorMessage.value = 'Keine Bestellnummer gefunden!'
    isLoading.value = false
    return
  }

  try {
    const response = await fetch(`/api/orders/${orderId}`)
    if (!response.ok) throw new Error('Bestellung konnte nicht geladen werden.')

    order.value = await response.json()
  } catch (error) {
    errorMessage.value = 'Fehler beim Laden der Bestellung.'
    console.error(error)
  } finally {
    isLoading.value = false
  }
})
</script>

<template>
  <div class="max-w-4xl mx-auto p-8">
    <div class="text-center mb-6">
      <template v-if="errorMessage">
        <XCircleIcon class="w-16 h-16 text-red-500 mx-auto" />
        <h1 class="text-3xl font-bold text-red-600 mt-4">Bestellung fehlgeschlagen!</h1>
        <p class="text-red-500 mt-2">{{ errorMessage }}</p>
      </template>
      <template v-else>
        <CheckCircleIcon class="w-16 h-16 text-green-500 mx-auto" />
        <h1 class="text-3xl font-bold text-gray-800 mt-4">Bestellung erfolgreich!</h1>
        <p class="text-gray-600 mt-2">
          Vielen Dank für deine Bestellung. Eine Bestätigungs-E-Mail wurde versendet.
        </p>
      </template>
    </div>

    <div v-if="isLoading" class="text-center text-gray-600">Lädt...</div>

    <div v-else-if="!errorMessage">
      <!-- Bestellinformationen -->
      <Card class="mb-6 shadow-lg">
        <CardHeader>
          <h2 class="text-lg font-bold text-gray-700">Bestellinformationen</h2>
        </CardHeader>
        <CardContent class="text-gray-700">
          <p class="mb-2"><strong>Bestellnummer:</strong> {{ order.id }}</p>
          <p class="mb-2">
            <strong>Bestelldatum:</strong> {{ new Date(order.orderDate).toLocaleDateString() }}
          </p>
        </CardContent>
      </Card>

      <!-- Versandadresse & Zahlungsmethode -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card class="shadow-lg">
          <CardHeader>
            <h3 class="text-lg font-bold text-gray-700">Versandadresse</h3>
          </CardHeader>
          <CardContent class="text-gray-700">
            <p>{{ order.firstName }} {{ order.lastName }}</p>
            <p>{{ order.street }} {{ order.houseNumber }}</p>
            <p>{{ order.postalCode }} {{ order.city }}</p>
          </CardContent>
        </Card>

        <Card class="shadow-lg">
          <CardHeader>
            <h3 class="text-lg font-bold text-gray-700">Zahlungsmethode</h3>
          </CardHeader>
          <CardContent class="text-gray-700">
            <p>{{ order.paymentMethod }}</p>
          </CardContent>
        </Card>
      </div>

      <OrderSummary class="mt-6" :orderData="order" />
    </div>
    <div class="text-center mt-6 flex justify-end">
      <Button @click="router.push('/')" class="w-full lg:w-auto flex items-end gap-2">
        <HomeIcon class="w-5 h-5" />
        Zur Startseite
      </Button>
    </div>
  </div>
</template>
