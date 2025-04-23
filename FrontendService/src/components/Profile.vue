<script setup lang="ts">
import { onMounted, ref } from 'vue'
import keycloak from '../keycloak'
import Button from './ui/button/Button.vue'

const userInfo = ref<Record<string, any> | null>(null)

onMounted(() => {
  if (keycloak.tokenParsed) {
    userInfo.value = keycloak.tokenParsed
  }
})

const openAccountManagement = () => {
  if (keycloak) {
    const accountManagementUrl = keycloak.createAccountUrl()
    window.open(accountManagementUrl, '_blank')
  }
}
</script>

<template>
  <Button @click="openAccountManagement">Profil bearbeiten</Button>
</template>
