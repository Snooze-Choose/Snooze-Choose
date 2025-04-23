import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import Home from './views/Home.vue'
import Settings from './views/Settings.vue'
import Warenkorb from './views/Warenkorb.vue'
import Profile from './components/Profile.vue'
import keycloak from './keycloak'
import Checkout from './views/Checkout.vue'
import OrderConfirmation from './views/OrderConfirmation.vue'
import Haushalt from './views/Haushalt.vue'
import Technik from './views/Technik.vue'
import Nahrung from './views/Nahrung.vue'
import Bestelluebersicht from './components/Bestelluebersicht.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/haushalt',
    name: 'Haushalt',
    component: Haushalt
  },
  {
    path: '/technik',
    name: 'Technik',
    component: Technik
  },
  {
    path: '/nahrung',
    name: 'Nahrung',
    component: Nahrung
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
      },
      {
        path: 'bestelluebersicht',
        name: 'bestelluebersicht',
        component: Bestelluebersicht,
        meta: { requiresAuth: true }
      }
    ]
  },
  {
    path: '/warenkorb',
    name: 'Warenkorb',
    component: Warenkorb
  },
  {
    path: '/checkout',
    name: 'Checkout',
    component: Checkout
  },
  {
    path: '/order-confirmation',
    name: 'OrderConfirmation',
    component: OrderConfirmation
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth) {
    if (keycloak.authenticated) {
      next()
    } else {
      keycloak.login()
    }
  } else {
    next()
  }
})

export default router
