import './assets/main.css'
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate' // Plugin importieren
import App from './App.vue'
import router from './router'
import keycloak from './keycloak'

const app = createApp(App)

const pinia = createPinia()
pinia.use(piniaPluginPersistedstate) // Persistenz aktivieren
app.use(pinia)

keycloak
  .init({ onLoad: 'check-sso' })
  .then((authenticated) => {
    app.use(router)
    app.mount('#app')
  })
  .catch((error) => {
    console.error(error)
  })
