<template>
  <div class="p-6" v-if="order">
    <h1 class="text-2xl font-bold mb-4">Bestellung #{{ order.id }}</h1>
    <div class="mb-4">📍 Versandadresse: {{ order.address }}</div>
    <div class="mb-4">🚚 Lieferstatus: {{ order.status }}</div>

    <h2 class="text-xl font-semibold mb-2">🛒 Artikel</h2>
    <ul class="space-y-2">
      <li
        v-for="(item, index) in order.items"
        :key="index"
        class="border p-2 rounded"
      >
        {{ item.quantity }}× {{ item.name }} – {{
          (item.quantity * item.price).toFixed(2)
        }} €
      </li>
    </ul>

    <button
      @click="goBack"
      class="mt-4 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
    >
      Zurück zur Übersicht
    </button>
  </div>
</template>

<script setup>
import { computed } from "vue";
import { defineProps } from "vue";

import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const orderId = route.params.orderId

const props = defineProps({
  orderId: String,
});

const mockOrderDetails = {
  "123": {
    id: "123",
    address: "Musterstraße 123, 12345 Berlin",
    status: "Versandt",
    items: [
      { name: "T-Shirt", quantity: 2, price: 19.99 },
      { name: "Hose", quantity: 1, price: 39.99 },
    ],
  },
  "124": {
    id: "124",
    address: "Beispielweg 5, 54321 Hamburg",
    status: "In Bearbeitung",
    items: [{ name: "Schuhe", quantity: 1, price: 49.99 }],
  },
};

const goBack = () => {
  router.push('/orders')
}

const order = computed(() => mockOrderDetails[props.orderId] || null);
</script>