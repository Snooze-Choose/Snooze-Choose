<script setup lang="ts">
import { onMounted, ref } from 'vue'
import keycloak from '../keycloak'

const userInfo = ref<Record<string, any> | null>(null)

onMounted(() => {
  if (keycloak.tokenParsed) {
    userInfo.value = keycloak.tokenParsed

    console.log(userInfo.value)
  }
})
</script>

<template>Profile</template>
