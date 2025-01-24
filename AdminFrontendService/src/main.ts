import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
// import keycloak from './keycloak'

const app = createApp(App)

app.use(router)
app.mount('#app')

// keycloak
//   .init({ onLoad: 'check-sso' })
//   .then((authenticated) => {
//     app.use(router)
//     app.mount('#app')
//   })
//   .catch((error) => {
//     console.error(error)
//   })
