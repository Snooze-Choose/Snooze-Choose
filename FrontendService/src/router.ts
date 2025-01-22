// src/router.ts
import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import { initializeKeycloak, getKeycloak } from './keycloak'
import Home from './Home.vue'
import ProtectedView from './ProtectedView.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home // Diese Route ist öffentlich zugänglich
  },
  {
    path: '/protected',
    name: 'Protected',
    component: ProtectedView,
    beforeEnter: async (to, from, next) => {
      const keycloak = getKeycloak()

      if (keycloak && keycloak.authenticated) {
        next()
      } else {
        // Wenn der Benutzer nicht authentifiziert ist, zur Login-Seite weiterleiten
        try {
          await initializeKeycloak() // Initialisiere Keycloak
          if (keycloak.authenticated) {
            next()
          } else {
            next('/') // Zurück zur Home-Seite, wenn die Authentifizierung fehlschlägt
          }
        } catch (error) {
          console.error(error)
          // Fehlerhafte Initialisierung, zur Home-Seite weiterleiten
        }
      }
    }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
