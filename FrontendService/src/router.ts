import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import Home from './views/Home.vue'
import Settings from './views/Settings.vue'
import Warenkorb from './views/Warenkorb.vue'
import Profile from './components/Profile.vue'

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
    children: [
      {
        path: 'profile',
        name: 'Profile',
        component: Profile
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

export default router
