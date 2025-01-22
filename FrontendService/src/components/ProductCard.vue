<script setup lang="ts">
import { defineProps } from 'vue'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

// Props definieren
const props = defineProps({
  title: {
    type: String,
  },
  description: {
    type: String,
  },
  price: {
    type: String,
  },
  imageUrl: {
    type: String,
  },
  quantityLabel: {
    type: String,
  },
})

console.log(props.imageUrl);
</script>

<template>
  <Card class="w-[350px]">
    <CardHeader>
      <CardTitle>{{ title }}</CardTitle>
      <CardDescription>{{ description }}</CardDescription>
    </CardHeader>
    <CardContent>
      <img src="../assets/product.jpeg" alt="Produktbild" class="w-full h-auto mb-4" />
      <p class="text-sm text-muted-foreground">
        Detaillierte Beschreibung des Produkts mit allen wichtigen Informationen.
      </p>
      <div class="flex items-center space-x-2 mt-4">
        <Label for="quantity">{{ quantityLabel }}</Label>
        <Input id="quantity" type="number" default-value="1" min="1" class="w-16" />
      </div>
    </CardContent>
    <CardFooter class="flex justify-between items-center">
      <span class="text-lg font-bold">Preis: {{ price }}</span>
      <Button>In den Warenkorb</Button>
    </CardFooter>
  </Card>
</template>
