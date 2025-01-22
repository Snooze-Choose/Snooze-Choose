<template>
  <div>
    <h1>Willkommen</h1>
    <p v-if="keycloak.authenticated">
      Angemeldet als: {{ keycloak.tokenParsed?.preferred_username }}
    </p>
    <p v-else>Du bist nicht angemeldet. Bitte melde dich an.</p>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { getKeycloak } from './keycloak'

export default defineComponent({
  name: 'Home',
  computed: {
    keycloak() {
      return getKeycloak()
    }
  }
})
</script>
