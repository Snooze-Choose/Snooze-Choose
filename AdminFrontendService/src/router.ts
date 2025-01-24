import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import Home from './views/Home.vue'
import ProductList from './components/ProductList.vue'
import OrderList from './components/OrderList.vue'
import Settings from './components/Settings.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    redirect: '/products',
    children: [
      {
        path: '/products',
        name: 'Products',
        component: ProductList,
        meta: { requiresAuth: true }
      },
      {
        path: '/orders',
        name: 'Orders',
        component: OrderList,
        meta: { requiresAuth: true }
      },
      {
        path: '/settings',
        name: 'Settings',
        component: Settings,
        meta: { requiresAuth: true }
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
