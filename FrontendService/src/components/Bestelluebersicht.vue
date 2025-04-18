<template>
  <div class="p-4">
    <Card>
      <CardHeader>
        <CardTitle>Meine Bestellungen</CardTitle>
      </CardHeader>
      <CardContent>
        <ul v-if="orders.length > 0" class="space-y-4">
          <li v-for="order in orders" :key="order.id" class="p-4 border rounded-xl shadow-sm">
            <div class="flex justify-between">
              <span class="font-medium">Bestellung #{{ order.id }}</span>
              <span class="text-sm text-gray-500">{{ formatDate(order.orderDate) }}</span>
            </div>

            <div class="mt-2">
              <div><strong>Name:</strong> {{ order.firstName }} {{ order.lastName }}</div>
              <div>
                <strong>Adresse:</strong> {{ order.street }} {{ order.houseNumber }},
                {{ order.city }} {{ order.postalCode }}
              </div>
              <div><strong>Gesamtpreis:</strong> {{ order.totalPrice }} €</div>
              <div><strong>Zahlungsart:</strong> {{ order.paymentMethod }}</div>
              <div><strong>Status:</strong> {{ order.orderStatus }}</div>
            </div>

            <!-- Hier wird die Produktliste nur angezeigt, wenn die Bestellung ausgewählt ist -->
            <div
              v-if="selectedOrder && selectedOrder.id === order.id"
              class="mt-4 p-4 border rounded-lg bg-gray-50"
            >
              <h3 class="text-lg font-semibold mb-2">Produkte</h3>
              <ul v-if="order.products.length > 0">
                <li
                  v-for="product in order.products"
                  :key="product.id"
                  class="flex justify-between mb-3"
                >
                  <div>
                    <strong>{{ product.name }}</strong
                    ><br />
                    Menge: {{ product.quantity }}<br />
                    Preis: {{ product.unitPrice }} €
                  </div>
                  <span class="text-right">{{ product.totalPrice }} €</span>
                </li>
              </ul>

              <!-- Gesamtkosten der Produkte anzeigen -->
              <div class="mt-2 text-right font-semibold">
                <strong>Gesamtkosten der Produkte:</strong>
                {{ calculateTotalPrice(order.products) }} €
              </div>
            </div>

            <!-- Klicken auf die Bestellung zeigt die Produktliste -->
            <button @click="selectOrder(order)" class="mt-2 text-blue-500">
              {{
                selectedOrder && selectedOrder.id === order.id
                  ? 'Verstecke Produkte'
                  : 'Zeige Produkte'
              }}
            </button>
          </li>
        </ul>
        <div v-else class="text-gray-500 text-center py-8">Keine Bestellungen gefunden.</div>
      </CardContent>
    </Card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card'
import keycloak from '@/keycloak'

const orders = ref([])
const selectedOrder = ref(null) // Zustand für die ausgewählte Bestellung

onMounted(async () => {
  const token = keycloak.token // Hier wird das Token von Keycloak geholt

  try {
    const response = await fetch('/api/orders', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}` // Bearer-Token im Header
      }
    })

    if (response.ok) {
      const data = await response.json()
      orders.value = data
      console.log(orders.value)
    } else {
      console.error('Fehler beim Laden der Bestellungen:', response.statusText)
    }
  } catch (error) {
    console.error('Fehler beim Abrufen der Bestellungen:', error)
  }
})

// Hilfsfunktion zur Formatierung des Datums
function formatDate(dateString) {
  return new Date(dateString).toLocaleDateString('de-DE', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

// Funktion zum Auswählen einer Bestellung, um ihre Produkte anzuzeigen
function selectOrder(order) {
  if (selectedOrder.value && selectedOrder.value.id === order.id) {
    selectedOrder.value = null // Wenn die Bestellung bereits ausgewählt ist, wird sie abgewählt
  } else {
    selectedOrder.value = order // Auswahl der Bestellung
  }
}

// Funktion zur Berechnung der Gesamtkosten der Produkte
function calculateTotalPrice(products) {
  return products.reduce((sum, product) => sum + product.totalPrice, 0).toFixed(2)
}
</script>
