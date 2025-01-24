import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import Home from './views/Home.vue'
import ProductList from './components/ProductList.vue'

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
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
