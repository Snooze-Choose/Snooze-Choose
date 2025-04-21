<template>
  <div class="p-6">
    <h1 class="text-2xl font-bold mb-4">Meine Bestellungen</h1>
    <ul class="space-y-4">
      <li
        v-for="order in orders"
        :key="order.id"
        class="border p-4 rounded shadow hover:bg-gray-50 cursor-pointer"
        @click="goToDetails(order.id)"
      >
        <div>📅 {{ order.date }}</div>
        <div>💶 Gesamt: {{ order.total.toFixed(2) }} €</div>
        <div>🚚 Status: {{ order.status }}</div>
        <span class="text-blue-600 hover:underline">Details anzeigen</span>
      </li>
    </ul>
  </div>
</template>

<script setup lang = ts>
import { onMounted, ref } from 'vue'
const userInfo = ref<Record<string, any> | null>(null)
import { useRouter } from 'vue-router'
const router = useRouter()

const orders = [
  {
    id: "123",
    date: "2025-04-01",
    total: 79.99,
    status: "Versandt",
  },
  {
    id: "124",
    date: "2025-04-05",
    total: 49.99,
    status: "In Bearbeitung",
  },
];

const goToDetails = (orderId: any) => {
  router.push(`/orders/${orderId}`)
}

</script>
