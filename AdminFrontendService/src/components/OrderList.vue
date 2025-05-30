<script setup lang="ts">
import { onMounted, ref, h } from 'vue'
import type { ColumnDef, SortingState } from '@tanstack/vue-table'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table'
import { valueUpdater } from '@/lib/valueUpdater'
import {
  FlexRender,
  getCoreRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  useVueTable
} from '@tanstack/vue-table'

import { ArrowUpDown } from 'lucide-vue-next'
import keycloak from '@/keycloak'
import OrderDetailDialog from './OrderDetailDialog.vue'

export interface Order {
  id: number
  firstName: string
  lastName: string
  street: string
  houseNumber: string
  city: string
  postalCode: string
  orderStatus: string
  orderDate: string
  totalPrice: number
  products: Product[]
}

interface Product {
  id: number
  name: string
  quantity: number
  unitPrice: number
  totalPrice: number
  imageUrl: string | null
}

const orders = ref<Order[]>([])
const loading = ref(true)
const error = ref<string | null>(null)
const isDialogOpen = ref(false)
const selectedOrder = ref<Order | null>(null)

const fetchOrders = async () => {
  try {
    loading.value = true
    const token = keycloak.token
    const response = await fetch('/api/orders/admin', {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    })
    if (!response.ok) throw new Error('Failed to fetch orders')
    orders.value = await response.json()
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Unknown error occurred'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchOrders()
})

const columns: ColumnDef<Order>[] = [
  {
    accessorKey: 'id',
    header: 'ID'
  },
  {
    id: 'customerName',
    header: ({ column }) =>
      h(
        Button,
        {
          variant: 'ghost',
          onClick: () => column.toggleSorting(column.getIsSorted() === 'asc')
        },
        () => ['Kunde', h(ArrowUpDown, { class: 'ml-2 h-4 w-4' })]
      ),
    cell: ({ row }) => `${row.original.firstName} ${row.original.lastName}`
  },
  {
    header: 'Adresse',
    cell: ({ row }) =>
      `${row.original.street} ${row.original.houseNumber}, ${row.original.postalCode} ${row.original.city}`
  },
  {
    accessorKey: 'orderStatus',
    header: 'Status'
  },
  {
    accessorKey: 'orderDate',
    header: 'Bestelldatum',
    cell: ({ row }) => new Date(row.original.orderDate).toLocaleDateString()
  },
  {
    accessorKey: 'totalPrice',
    header: 'Gesamtpreis',
    cell: ({ row }) => `${row.original.totalPrice.toFixed(2)} €`
  }
]

const sorting = ref<SortingState>([])
const table = useVueTable({
  data: orders,
  columns,
  getCoreRowModel: getCoreRowModel(),
  getPaginationRowModel: getPaginationRowModel(),
  getSortedRowModel: getSortedRowModel(),
  onSortingChange: (updaterOrValue) => valueUpdater(updaterOrValue, sorting),
  state: {
    get sorting() {
      return sorting.value
    }
  }
})

const openEditDialog = (order: Order) => {
  selectedOrder.value = { ...order }
  isDialogOpen.value = true
}
</script>

<template>
  <div class="w-full">
    <div class="flex gap-2 items-center py-4">
      <Input
        class="max-w-sm"
        placeholder="Suche nach Vornamen..."
        :model-value="table.getColumn('firstName')?.getFilterValue() as string"
        @update:model-value="table.getColumn('firstName')?.setFilterValue($event)"
      />
    </div>

    <div v-if="loading" class="text-center py-4">Bestellungen werden geladen...</div>

    <div v-else-if="error" class="text-center text-red-500 py-4">
      {{ error }}
    </div>

    <div v-else class="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow v-for="headerGroup in table.getHeaderGroups()" :key="headerGroup.id">
            <TableHead v-for="header in headerGroup.headers" :key="header.id">
              <FlexRender
                v-if="!header.isPlaceholder"
                :render="header.column.columnDef.header"
                :props="header.getContext()"
              />
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <template v-if="table.getRowModel().rows?.length">
            <template v-for="row in table.getRowModel().rows" :key="row.id">
              <TableRow
                :data-state="row.getIsSelected() && 'selected'"
                @click="openEditDialog(row.original)"
              >
                <TableCell v-for="cell in row.getVisibleCells()" :key="cell.id">
                  <FlexRender :render="cell.column.columnDef.cell" :props="cell.getContext()" />
                </TableCell>
              </TableRow>
            </template>
          </template>
          <TableRow v-else>
            <TableCell :colspan="columns.length" class="h-24 text-center">
              Keine Ergebnisse.
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>

    <OrderDetailDialog v-model="isDialogOpen" :selectedOrder="selectedOrder" />
  </div>
</template>
