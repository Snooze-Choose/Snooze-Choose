import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import { initializeKeycloak } from './keycloak'

initializeKeycloak()
  .then(() => {
    createApp(App).mount('#app')
  })
  .catch((error) => {
    console.error('Keycloak initialization failed', error)
  })

createApp(App).mount('#app')
