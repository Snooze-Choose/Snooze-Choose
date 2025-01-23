<script setup lang="ts">
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

const props = defineProps({
  id: {
    type: String
  },
  name: {
    type: String
  },
  description: {
    type: String
  },
  short_description: {
    type: String
  },
  price: {
    type: Number
  },
  imageUrl: {
    type: String
  }
})
</script>

<template>
  <Card class="w-[350px]">
    <CardHeader>
      <CardTitle>{{ name }}</CardTitle>
      <CardDescription>{{ short_description }}</CardDescription>
    </CardHeader>
    <CardContent>
      <img src="../assets/product.jpeg" alt="Produktbild" class="w-full h-auto mb-4" />
      <p class="text-sm text-muted-foreground">
        {{ description }}
      </p>
      <div class="flex items-center space-x-2 mt-4">
        <Input :id="id" type="number" default-value="1" min="1" class="w-16" />
      </div>
    </CardContent>
    <CardFooter class="flex justify-between items-center">
      <span class="text-lg font-bold">Preis: {{ price }}</span>
      <Button>In den Warenkorb</Button>
    </CardFooter>
  </Card>
</template>
