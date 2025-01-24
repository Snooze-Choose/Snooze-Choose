import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import Home from './views/Home.vue'
import Settings from './views/Settings.vue'
import Warenkorb from './views/Warenkorb.vue'
import Profile from './components/Profile.vue'
import keycloak from './keycloak'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/settings',
    name: 'Settings',
    component: Settings,
    redirect: '/settings/profile',
    meta: { requiresAuth: true },
    children: [
      {
        path: 'profile',
        name: 'Profile',
        component: Profile,
        meta: { requiresAuth: true }
      }
    ]
  },
  {
    path: '/warenkorb',
    name: 'Warenkorb',
    component: Warenkorb
  }
]

// Create and configure the router
const router = createRouter({
  history: createWebHistory(),
  routes // Short for `routes: routes`
})

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth) {
    if (keycloak.authenticated) {
      next() // Allow access
    } else {
      keycloak.login() // Redirect to Keycloak login
    }
  } else {
    next() // Allow access to non-restricted routes
  }
})

export default router
