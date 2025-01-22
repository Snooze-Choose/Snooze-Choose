<script setup lang="ts">
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
</script>

<template>
  <Card class="w-[350px]">
    <CardHeader>
      <CardTitle>Melone</CardTitle>
      <CardDescription>Kurzbeschreibung des Produkts</CardDescription>
    </CardHeader>
    <CardContent>
      <img src="../assets/product.jpeg" alt="Produktbild" class="w-full h-auto mb-4" />
      <p class="text-sm text-muted-foreground">
        Detaillierte Beschreibung des Produkts mit allen wichtigen Informationen.
      </p>
      <div class="flex items-center space-x-2 mt-4">
        <Label for="quantity">Menge</Label>
        <Input id="quantity" type="number" default-value="1" min="1" value="1" class="w-16" />
      </div>
    </CardContent>
    <CardFooter class="flex justify-between items-center">
      <span class="text-lg font-bold">Preis: 49,99€</span>
      <Button>In den Warenkorb</Button>
    </CardFooter>
  </Card>
</template>
