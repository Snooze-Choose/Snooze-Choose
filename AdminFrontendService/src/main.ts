import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import keycloak from './keycloak'

const app = createApp(App)

keycloak
  .init({ onLoad: 'login-required' })
  .then((authenticated) => {
    app.use(router)
    app.mount('#app')
  })
  .catch((error) => {
    console.error(error)
  })
