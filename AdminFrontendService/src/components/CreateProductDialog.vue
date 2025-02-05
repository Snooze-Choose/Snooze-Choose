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

const emit = defineEmits<{ (event: 'productCreated'): void }>()

const formSchema = toTypedSchema(
  z.object({
    name: z.string().min(2).max(50),
    price: z.number(),
    shortDescription: z.string().min(2).max(300),
    longDescription: z.string().min(2).max(500),
    imageUrl: z.any()
  })
)

const form = useForm({
  validationSchema: formSchema
})

const isDialogOpen = ref(false)
const imageFile = ref<File | null>(null)

const onFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement
  if (target.files && target.files[0]) {
    imageFile.value = target.files[0]
  }
}

const uploadImage = async (productId: number) => {
  if (!imageFile.value) return null

  const formData = new FormData()
  formData.append('file', imageFile.value)

  const token = keycloak.token
  const response = await fetch(`/api/products/${productId}/upload`, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${token}`
    },
    body: formData
  })

  if (!response.ok) {
    console.error('Fehler beim Hochladen des Bildes')
    return null
  }

  const data = await response.json()
  return data.imageUrl
}

const onSubmit = form.handleSubmit(async (values) => {
  try {
    const token = keycloak.token

    const productResponse = await fetch('/api/products', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(values)
    })

    if (!productResponse.ok) {
      throw new Error('Fehler beim Erstellen des Produkts')
    }

    const product = await productResponse.json()

    if (imageFile.value) {
      const imageUrl = await uploadImage(product.id)
      if (imageUrl) {
        values.imageUrl = imageUrl
      }
    }

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

        <FormField v-slot="{ componentField }" name="imageUrl">
          <FormItem>
            <FormLabel>Bild hochladen</FormLabel>
            <FormControl>
              <Input type="file" @change="onFileChange" accept="image/*" />
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
