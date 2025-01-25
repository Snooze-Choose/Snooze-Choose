<script setup lang="ts">
import { onMounted, ref, h } from 'vue'
import type { ColumnDef, SortingState } from '@tanstack/vue-table'

import { Button } from '@/components/ui/button'
import { Checkbox } from './ui/checkbox'
import {
  DropdownMenu,
  DropdownMenuCheckboxItem,
  DropdownMenuContent,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
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

import { ArrowUpDown, ChevronDown } from 'lucide-vue-next'

interface Product {
  id: number
  name: string
  price: number
  shortDescription: string
  longDescription: string
}

const products = ref<Product[]>([]) // Speichert die Produktdaten
const loading = ref(true)
const error = ref<string | null>(null)

// API-Daten laden
onMounted(async () => {
  try {
    const response = await fetch('/api/products')
    if (!response.ok) throw new Error('Failed to fetch products')
    products.value = await response.json()
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Unknown error occurred'
  } finally {
    loading.value = false
  }
})

// Tabellen-Spalten
const columns: ColumnDef<Product>[] = [
  {
    accessorKey: 'id',
    header: 'ID'
  },
  {
    accessorKey: 'name',
    header: ({ column }) =>
      h(
        Button,
        {
          variant: 'ghost',
          onClick: () => column.toggleSorting(column.getIsSorted() === 'asc')
        },
        () => ['Name', h(ArrowUpDown, { class: 'ml-2 h-4 w-4' })]
      )
  },
  {
    accessorKey: 'price',
    header: 'Price',
    cell: ({ row }) => {
      const price = row.getValue('price')
      return h('div', {}, `$${price}`)
    }
  },
  {
    accessorKey: 'shortDescription',
    header: 'Short Description'
  }
]

// Vue Table Einstellungen
const sorting = ref<SortingState>([])
const table = useVueTable({
  data: products,
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
</script>

<template>
  <div class="w-full">
    <div class="flex gap-2 items-center py-4">
      <Input
        class="max-w-sm"
        placeholder="Filter products..."
        :model-value="table.getColumn('name')?.getFilterValue() as string"
        @update:model-value="table.getColumn('name')?.setFilterValue($event)"
      />
    </div>

    <div v-if="loading" class="text-center py-4">Loading products...</div>

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
              <TableRow :data-state="row.getIsSelected() && 'selected'">
                <TableCell v-for="cell in row.getVisibleCells()" :key="cell.id">
                  <FlexRender :render="cell.column.columnDef.cell" :props="cell.getContext()" />
                </TableCell>
              </TableRow>
            </template>
          </template>

          <TableRow v-else>
            <TableCell :colspan="columns.length" class="h-24 text-center"> No results. </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  </div>
</template>
