import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import Home from './views/Home.vue'
// import keycloak from './keycloak'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  }
]

// Create and configure the router
const router = createRouter({
  history: createWebHistory(),
  routes // Short for `routes: routes`
})

// router.beforeEach((to, from, next) => {
//   if (to.meta.requiresAuth) {
//     if (keycloak.authenticated) {
//       next() // Allow access
//     } else {
//       keycloak.login() // Redirect to Keycloak login
//     }
//   } else {
//     next() // Allow access to non-restricted routes
//   }
// })

export default router
