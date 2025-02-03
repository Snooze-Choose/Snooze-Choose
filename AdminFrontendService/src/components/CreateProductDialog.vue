<script setup lang="ts">
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { Button } from '@/components/ui/button'

import {
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { toTypedSchema } from '@vee-validate/zod'
import { useForm } from 'vee-validate'
import { ref } from 'vue'
import * as z from 'zod'
import keycloak from '@/keycloak'

const emit = defineEmits<{
  (event: 'productCreated'): void
}>()

const formSchema = toTypedSchema(
  z.object({
    name: z.string().min(2).max(50),
    price: z.number(),
    shortDescription: z.string().min(2).max(300),
    longDescription: z.string().min(2).max(500)
  })
)

const form = useForm({
  validationSchema: formSchema
})

const isDialogOpen = ref(false)

const onSubmit = form.handleSubmit(async (values) => {
  try {
    const token = keycloak.token

    await fetch('/api/products', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(values)
    })

    isDialogOpen.value = false

    emit('productCreated')
  } catch (error) {
    console.error('Fehler beim Hinzufügen des Produkts:', error)
  }
})
</script>

<template>
  <Dialog v-model:open="isDialogOpen">
    <DialogTrigger as-child><Button variant="outline">Produkt hinzufügen</Button></DialogTrigger>
    <DialogContent>
      <DialogHeader>
        <DialogTitle>Produkt hinzufügen</DialogTitle>
        <DialogDescription>Füge hier das Produkt zu deinem Shop hinzu.</DialogDescription>
      </DialogHeader>

      <form @submit="onSubmit">
        <FormField v-slot="{ componentField }" name="name">
          <FormItem>
            <FormLabel>Name</FormLabel>
            <FormControl>
              <Input type="text" placeholder="Name des Produktes" v-bind="componentField" />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>

        <FormField v-slot="{ componentField }" name="price">
          <FormItem>
            <FormLabel>Preis</FormLabel>
            <FormControl>
              <Input type="number" placeholder="Preis des Produktes" v-bind="componentField" />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>

        <FormField v-slot="{ componentField }" name="shortDescription">
          <FormItem>
            <FormLabel>Kurze Beschreibung</FormLabel>
            <FormControl>
              <Input
                type="text"
                placeholder="Kurze Beschreibung des Produktes"
                v-bind="componentField"
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>

        <FormField v-slot="{ componentField }" name="longDescription">
          <FormItem>
            <FormLabel>Lange Beschreibung</FormLabel>
            <FormControl>
              <Input
                type="text"
                placeholder="Lange Beschreibung des Produktes"
                v-bind="componentField"
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>

        <br />
        <Button type="submit">Submit</Button>
      </form>
    </DialogContent>
  </Dialog>
</template>
