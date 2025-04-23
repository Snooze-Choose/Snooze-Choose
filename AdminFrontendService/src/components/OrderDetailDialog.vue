<script setup lang="ts">
import { defineProps, defineEmits } from 'vue'
import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle
} from '@/components/ui/dialog'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { type Order } from './OrderList.vue'
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table'

const props = defineProps<{
  modelValue: boolean
  selectedOrder: Order | null
}>()

console.log(props.selectedOrder)

const emit = defineEmits(['update:modelValue'])
</script>

<template>
  <Dialog :open="modelValue" @update:open="emit('update:modelValue', $event)">
    <DialogContent class="sm:max-w-[425px]">
      <DialogHeader>
        <DialogTitle>Order Details</DialogTitle>
        <DialogDescription> Edit the order details and save changes. </DialogDescription>
      </DialogHeader>
      <div class="grid gap-4 py-4">
        <div class="grid grid-cols-4 items-center gap-4">
          <Label for="name" class="text-right"> Name </Label>
          <Input
            id="name"
            class="col-span-3"
            type="email"
            :model-value="props.selectedOrder?.firstName + ' ' + props.selectedOrder?.lastName"
            readonly
          />
        </div>
        <div class="grid grid-cols-4 items-center gap-4">
          <Label for="address" class="text-right"> Adresse </Label>
          <Input
            id="address"
            class="col-span-3"
            type="email"
            :model-value="
              props.selectedOrder?.street +
              ' ' +
              props.selectedOrder?.houseNumber +
              ', ' +
              props.selectedOrder?.city
            "
            readonly
          />
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead class="w-[100px]"> Id </TableHead>
              <TableHead>Name</TableHead>
              <TableHead>Menge</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            <TableRow v-for="product in props.selectedOrder?.products" :key="product.id">
              <TableCell>{{ product.id }} </TableCell>
              <TableCell>{{ product.name }}</TableCell>
              <TableCell>{{ product.quantity }}</TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </div>
      <DialogFooter>
        <Button @click="emit('update:modelValue', false)">Close</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
